﻿using Microsoft.AspNetCore.SignalR;
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
                await Clients.Client(Context.ConnectionId).SendAsync("FirstPlayerJoin", "First time in the game");

                await Clients.All.SendAsync("ReceiveMessage", connectionId, $"The total number of players allowed is:{playerCount.ToString()}");
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("PlayerJoin", "Create a character to enter the game");
            }

            await Clients.All.SendAsync("ReceiveMessage", connectionId, $"The total number of players allowed is:{playerCount.ToString()}");

            var onlinePlayer = playerList.Where(p => p.IsOffLine == false)
                .Select(p => p.Name).ToList();

            if (onlinePlayer.Count > 0)
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ResetPlayerList", onlinePlayer);
            }
        }


        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);

            var connectionId = Context.ConnectionId;

            var player = new Player
            {
                ConnectionId = connectionId,
            };

            _ = await _playerService.UpdatePlayerNetworkStatusAsync(player);
        }
        /// <summary>
        /// 示例广播代码
        /// </summary>
        /// <param name="gameStartData"></param>
        /// <returns></returns>
        public async Task SendMessage(GameStartData gameStartData)
        {
            var playerCount = _gameConfig.Value.PlayerCount;

            var connectionId = Context.ConnectionId;

            await Clients.All.SendAsync("ReceiveMessage", gameStartData.PlayerName, gameStartData.GameName);
        }

        /// <summary>
        /// 聊天代码
        /// </summary>
        /// <param name="chatMsg"></param>
        /// <returns></returns>
        public async Task ChatMsg(ChatMsg chatMsg)
        {
            var playerCount = _gameConfig.Value.PlayerCount;

            var connectionId = Context.ConnectionId;

            await Clients.All.SendAsync("ReceiveMessage", chatMsg.PlayerName, chatMsg.MsgContent);
        }

        public async Task StartGame()
        {
            var playerList = await _playerService.GetAllPlayersAsync();

            if (playerList.Count <= 0)
            {
                await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, "Please add players");
            }
            else
            {
                var connectObj = playerList.FirstOrDefault();

                if (connectObj != null)
                {
                    await Clients.Client(connectObj.ConnectionId).SendAsync("ReceiveMessage", Context.ConnectionId, "Please throw the dice");
                }
                else
                {
                    await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, "Please add players");
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
                await Clients.All.SendAsync("ReceiveMessage", gameStartData.PlayerName, $"Username cannot be empty");
                return;
            }

            if (!string.IsNullOrEmpty(gameStartData.GameName))
            {
                _gameService.SetGameName(gameStartData.GameName);
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
                await Clients.All.SendAsync("ReceiveMessage", gameStartData.PlayerName, $"An error has occurred--{ex.Message}");
            }

            var gameName = _gameService.GetGameName();

            await Clients.All.SendAsync("SetGameName", gameName);

            await Clients.AllExcept(Context.ConnectionId).SendAsync("UpdatePlayerList", gameStartData);

            await Clients.Client(Context.ConnectionId).SendAsync("SetCurrentPlayer", gameStartData);

            await Clients.All.SendAsync("ReceiveMessage", gameStartData.PlayerName, $"Join the game--{gameStartData.GameName}");
        }

        public async Task RollDice(string user)
        {
            var data = _gameService.RollDice(user);

            //判断卡片逻辑 如果存在颜色卡片暂停游戏进程进行卡片游戏

            if (data.CardColor != CardColor.None)
            {
                await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveMessage", user, $"Go to Card Games Card Colors--{data.CardColor}");
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, $"You lead the card game Card Colors--{data.CardColor}");
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
                await Clients.All.SendAsync("ReceiveMessage", user, $"An error has occurred--{ex.Message}");
            }

            await Clients.All.SendAsync("ReceiveMessage", user, System.Text.Json.JsonSerializer.Serialize(data)); ;
        }
        #endregion
    }
}
