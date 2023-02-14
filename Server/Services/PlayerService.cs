using LiteDB;
using Microsoft.Extensions.Options;
using Server.Domain;
using Server.Models;

namespace Server.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly LiteDatabase _liteDatabase;

        private static List<Player> PlayerList = new List<Player>();

        private readonly int _playerCount = 0;

        private int _currentOnlinePlayerIndex = 0;
        public PlayerService(IOptions<GameConfig> options, string dbDataPath = "gameData.db")
        {
            _liteDatabase = new LiteDatabase(dbDataPath);
            _playerCount = options.Value.PlayerCount;
        }
        public Task<Player> AddPlayerToDbAsync(Player player)
        {
            var players = _liteDatabase.GetCollection<Player>();

            var playerList = players.Query().Where(p => p.Name == player.Name).ToList();

            if (playerList.Count > 0)
            {
                throw new Exception("玩家已存在");
            }

            _ = players.Insert(player);

            return Task.FromResult(player);
        }

        public Task<Player> UpdatePlayerToDbAsync(Player player)
        {
            var players = _liteDatabase.GetCollection<Player>();

            var playerData = players.Query().Where(p => p.Name == player.Name).FirstOrDefault();

            if (playerData is null)
            {
                throw new Exception("玩家不存在");
            }

            playerData.GameMatrix = player.GameMatrix;
            playerData.Position = player.Position;
            playerData.CardG = player.CardG;
            playerData.CardB = player.CardB;
            playerData.CardY = player.CardY;

            _ = players.Update(playerData);

            return Task.FromResult(playerData);
        }

        public Task<Player> AddPlayerToPlayerListAsync(Player player)
        {
            if (PlayerList.Count >= _playerCount)
            {
                throw new Exception($"游戏最大支持人数为{_playerCount}");
            }

            PlayerList.Add(player);

            return Task.FromResult(player);
        }

        public Task<Player> UpdatePlayerNetworkStatusAsync(Player player)
        {
            var players = _liteDatabase.GetCollection<Player>();

            var playerData = players.Query().Where(p => p.ConnectionId == player.ConnectionId).FirstOrDefault();

            if (playerData is not null)
            {
                playerData.IsOffLine = true;

                _ = players.Update(playerData);
            }

            foreach (var playerItem in PlayerList)
            {
                if (playerItem.ConnectionId == player.ConnectionId)
                {
                    playerItem.IsOffLine = true;
                    return Task.FromResult(playerItem);
                }
            }
            return Task.FromResult(player);
        }

        public Task<List<Player>> GetAllPlayersAsync()
        {
            return Task.FromResult(PlayerList);
        }

        public int UpdateCurrentPlayerIndex()
        {
            _currentOnlinePlayerIndex += 1;
            if (PlayerList.Count > 0)
            {
                if (_currentOnlinePlayerIndex >= PlayerList.Count)
                {
                    _currentOnlinePlayerIndex = 0;
                }
            }

            return _currentOnlinePlayerIndex;
        }

        public int GetCurrentPlayerIndex()
        {
            return _currentOnlinePlayerIndex;
        }

        public int ComputeCurrentPlayerIndex(string playerName)
        {
            var onlinePlayers = PlayerList.Where(p => p.IsOffLine == false).ToList();

            var index = onlinePlayers.FindIndex(p => p.Name == playerName);

            return index;
        }

        public Task<Player> UpdatePlayerCardCountAsync(string playerName, CardColor cardColor)
        {
            var players = _liteDatabase.GetCollection<Player>();

            var playerData = players.Query().Where(p => p.Name == playerName).FirstOrDefault();

            if (playerData is not null)
            {
                if (cardColor == CardColor.Blue)
                {
                    playerData.CardB += 1;
                }
                else if (cardColor == CardColor.Green)
                {
                    playerData.CardG += 1;
                }
                else if (cardColor == CardColor.Yellow)
                {
                    playerData.CardY += 1;
                }

                if (playerData.CardY > 0 && playerData.CardG > 0 && playerData.CardB > 0)
                {
                    playerData.CardNumIsOk = true;
                }

                _ = players.Update(playerData);


                foreach (var playerItem in PlayerList)
                {
                    if (playerItem.Name == playerName)
                    {
                        if (cardColor == CardColor.Blue)
                        {
                            playerItem.CardB += 1;
                        }
                        else if (cardColor == CardColor.Green)
                        {
                            playerItem.CardG += 1;
                        }
                        else if (cardColor == CardColor.Yellow)
                        {
                            playerItem.CardY += 1;
                        }

                        if (playerItem.CardY > 0 && playerItem.CardG > 0 && playerItem.CardB > 0)
                        {
                            playerItem.CardNumIsOk = true;
                        }

                        return Task.FromResult(playerItem);
                    }
                }

            }
            return Task.FromResult(new Player());
        }
    }
}
