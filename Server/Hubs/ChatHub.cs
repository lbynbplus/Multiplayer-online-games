using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
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

        private readonly IOptions<GameConfig> _gameConfig;
        public ChatHub(IPlayerService playerService,
            IGameService gameService,
            IOptions<GameConfig> gameConfig)
        {
            _playerService = playerService;
            _gameService = gameService;
            _gameConfig = gameConfig;

        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();


            var playerCount = _gameConfig.Value.PlayerCount;

            var connectionId = Context.ConnectionId;

            var playerList = await _playerService.GetAllPlayersAsync();

            if (playerList.Count <= 0)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("FirstPlayerJoin", "首次进入游戏");

                await Clients.All.SendAsync("ReceiveMessage", connectionId, $"允许的总玩家数为：{playerCount.ToString()}");
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("PlayerJoin", "创建角色进入游戏");
            }

            await Clients.All.SendAsync("ReceiveMessage", connectionId, $"允许的总玩家数为：{playerCount.ToString()}");
        }


        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);

            var connectionId = Context.ConnectionId;

            var player = new Player
            {
                ConnectionId = connectionId,
            };

            var playerData = await _playerService.UpdatePlayerNetworkStatusAsync(player);
        }

        /// <summary>
        /// 示例广播代码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(GameStartData gameStartData)
        {
            var playerCount = _gameConfig.Value.PlayerCount;

            var connectionId = Context.ConnectionId;

            await Clients.All.SendAsync("ReceiveMessage", gameStartData.PlayerName, gameStartData.GameName);
        }

        public async Task StartGame()
        {
            var playerList = await _playerService.GetAllPlayersAsync();

            if (playerList.Count <= 0)
            {
                await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, "请添加玩家");
            }
            else
            {
                var connectObj = playerList.FirstOrDefault();

                if (connectObj != null)
                {
                    await Clients.Client(connectObj.ConnectionId).SendAsync("ReceiveMessage", Context.ConnectionId, "请投骰子");
                }
                else
                {
                    await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, "请添加玩家");
                }
            }
        }

        /// <summary>
        /// 添加用户到数据库和游戏用户列表
        /// </summary>
        /// <param name="gameStartData"></param>
        /// <returns></returns>
        public async Task AddUser(GameStartData gameStartData)
        {
            if (string.IsNullOrEmpty(gameStartData.PlayerName))
            {
                await Clients.All.SendAsync("ReceiveMessage", gameStartData.PlayerName, $"用户名不能为空");
                return;
            }

            try
            {
                var player = new Player
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = gameStartData.PlayerName,
                    ConnectionId = Context.ConnectionId,
                };

                var playerResult = await _playerService.AddPlayerToPlayerListAsync(player);

                await _playerService.AddPlayerToDbAsync(player);
            }
            catch (Exception ex)
            {
                await Clients.All.SendAsync("ReceiveMessage", gameStartData.PlayerName, $"出现错误--{ex.Message}");
            }


            await Clients.All.SendAsync("ReceiveMessage", gameStartData.PlayerName, $"加入游戏--{gameStartData.GameName}");
        }

        public async Task RollDice(string user, string message)
        {
            var data = _gameService.RollDice(user);

            //判断卡片逻辑 如果存在颜色卡片暂停游戏进程进行卡片游戏

            if (data.CardColor != CardColor.None)
            {
                await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveMessage", user, $"进入卡片游戏 卡片颜色--{data.CardColor}");
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, $"你主导卡片游戏 卡片颜色--{data.CardColor}");
                return;
            }
            else
            {
                await Clients.All.SendAsync("RollDiceResult", data);
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
