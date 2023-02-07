using Server.Models;

namespace Server.Services
{
    public interface IPlayerService
    {
        /// <summary>
        /// 添加玩家到数据库
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        Task<Player> AddPlayerToDbAsync(Player player);

        /// <summary>
        /// 更新玩家信息
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        Task<Player> UpdatePlayerToDbAsync(Player player);
    }
}
