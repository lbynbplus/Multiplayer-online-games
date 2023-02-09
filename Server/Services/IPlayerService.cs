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

        /// <summary>
        /// 添加玩家到玩家列表
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        Task<Player> AddPlayerToPlayerListAsync(Player player);
        /// <summary>
        /// 更新用户网络状态
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        Task<Player> UpdatePlayerNetworkStatusAsync(Player player);

        /// <summary>
        /// 获取当前进入游戏的用户
        /// </summary>
        /// <returns></returns>
        Task<List<Player>> GetAllPlayersAsync();
    }
}
