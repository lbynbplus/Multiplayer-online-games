using LiteDB;
using Microsoft.Extensions.Options;
using Server.Models;

namespace Server.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly LiteDatabase _liteDatabase;

        private static List<Player> PlayerList = new List<Player>();

        private readonly int _playerCount = 0;
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
    }
}
