using Server.Domain;
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
        /// <summary>
        /// 更新当前投骰子的玩家索引
        /// </summary>
        /// <returns></returns>
        int UpdateCurrentPlayerIndex();
        /// <summary>
        /// 获取当前轮到投骰子的玩家索引
        /// </summary>
        /// <returns></returns>
        int GetCurrentPlayerIndex();

        /// <summary>
        /// 计算当前用户索引
        /// </summary>
        /// <returns></returns>
        int ComputeCurrentPlayerIndex(string playerName);
        /// <summary>
        /// 更新玩家卡牌数量
        /// </summary>
        /// <param name="playerName">玩家名称</param>
        /// <param name="cardColor">卡牌颜色</param>
        /// <returns></returns>
        Task<Player> UpdatePlayerCardCountAsync(string playerName, CardColor cardColor);
    }
}
