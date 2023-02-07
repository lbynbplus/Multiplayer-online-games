using Microsoft.AspNetCore.SignalR;
using Server.Domain;
using Server.Models;
using Server.Services;

namespace Server.Hubs
{
    public class ChatHub : Hub
    {
        #region snippet_SendMessage

        private readonly IPlayerService _playerService;
        private readonly IGameService _gameService;
        public ChatHub(IPlayerService playerService, IGameService gameService)
        {
            _playerService = playerService;
            _gameService = gameService;
        }

        /// <summary>
        /// 示例广播代码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        /// <summary>
        /// 添加用户到数据库和游戏用户列表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task AddUser(string user, string message)
        {
            if (string.IsNullOrEmpty(user))
            {
                await Clients.All.SendAsync("ReceiveMessage", user, $"用户名不能为空");
                return;
            }
            try
            {
                var player = new Player
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = user
                };
                await _playerService.AddPlayerToDbAsync(player);
            }
            catch (Exception ex)
            {
                await Clients.All.SendAsync("ReceiveMessage", user, $"出现错误--{ex.Message}");
            }


            await Clients.All.SendAsync("ReceiveMessage", user, $"加入游戏--{message}");
        }

        public async Task RollDice(string user, string message)
        {
            var data = _gameService.RollDice(user);

            //判断卡片逻辑 如果存在颜色卡片暂停游戏进程进行卡片游戏

            if (data.CardColor != CardColor.None)
            {
                await Clients.All.SendAsync("ReceiveMessage", user, $"进入卡片游戏 卡片颜色--{data.CardColor}");
                return;
            }

            var player = new Player
            {
                Name = user,
                GameMatrix = data
            };

            try
            {
                var playerData = await _playerService.UpdatePlayerToDbAsync(player);
            }
            catch (Exception ex)
            {
                await Clients.All.SendAsync("ReceiveMessage", user, $"出现错误--{ex.Message}");
            }

            await Clients.All.SendAsync("ReceiveMessage", user, System.Text.Json.JsonSerializer.Serialize(data)); ;
        }
        #endregion
    }
}
