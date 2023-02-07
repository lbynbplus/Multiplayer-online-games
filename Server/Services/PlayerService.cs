using LiteDB;
using Server.Models;

namespace Server.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly LiteDatabase _liteDatabase;
        public PlayerService(string dbDataPath = "gameData.db")
        {
            _liteDatabase = new LiteDatabase(dbDataPath);
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
    }
}
