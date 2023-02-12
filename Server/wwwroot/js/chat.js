"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/ChatHub").build();

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} : ${message}`;
});

connection.on("FirstPlayerJoin", function (message) {
    console.log("进入首个用户进入界面")
    var myModal = new bootstrap.Modal(document.getElementById('exampleModal'), {
        keyboard: false
    });
    myModal.show();
});

connection.on("PlayerJoin", function (message) {
    var myModal = new bootstrap.Modal(document.getElementById('joinGameModal'), {
        keyboard: false
    });
    myModal.show();
});

connection.on("UpdatePlayerList", function (message) {
    console.log("更新玩家列表")
    console.log(message);

    var li = document.createElement("li");
    var playerName = message.playerName;
    document.getElementById("player-list").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${playerName}`;

});

connection.on("ResetPlayerList", function (message) {
    console.log("重置玩家列表")
    console.log(message);

    // 循环的数组元素为对象，会改变原数组的对象属性的值
    message.forEach((item) => {
        console.log(item)


        var li = document.createElement("li");
        var playerName = item;
        document.getElementById("player-list").appendChild(li);
        // We can assign user-supplied strings to an element's textContent because it
        // is not interpreted as markup. If you're assigning in any other way, you 
        // should be aware of possible script injection concerns.
        li.textContent = `${playerName}`;
    })
});

connection.on("SetGameName", function (message) {
    console.log("设置游戏名称")
    console.log(message);

    var gameName = document.getElementById("game-name");
    gameName.textContent = `${message}`;
});

connection.on("RollDiceResult", function (message) {
    console.log("投骰子的结果")
    console.log(message);

    var obj = document.getElementById("diceResult");

    var imgSrc = `images/dice/dice${message.diceValue}.png`;
    console.log(imgSrc);
    obj.setAttribute('src', imgSrc);

});

connection.on("SetCurrentPlayer", function (message) {
    console.log("设置当前的用户状态")
    console.log(message);
    var li = document.createElement("li");
    li.setAttribute('id', 'current-player');
    var playerName = message.playerName;
    document.getElementById("player-list").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${playerName}`;

});

connection.start().then(function () {
    //初始化一些数据逻辑
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("roll-button").addEventListener("click", function (event) {
    var playerName = document.getElementById("current-player").textContent;
    console.log(playerName);

    connection.invoke("RollDice", playerName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();

});


var exampleModal = document.getElementById('exampleModal')
exampleModal.addEventListener('show.bs.modal', function (event) {

})

document.getElementById("first-player-save-btn").addEventListener("click", function (event) {
    var gameName = document.getElementById("game-name").value;
    var playerName = document.getElementById("player-name").value;
    var data ={
        GameName :gameName,
        PlayerName:playerName
    }
    console.log(data);
    connection.invoke("AddUser", data).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("player-save-btn").addEventListener("click", function (event) {
    var playerName = document.getElementById("join-game-player-name").value;
    var data = {
        PlayerName: playerName
    }
    console.log(data);
    connection.invoke("AddUser", data).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendChatMsgButton").addEventListener("click", function (event) {
    var playerName = document.getElementById("current-player").textContent;
    console.log(playerName);

    var message = document.getElementById("messageInput").value;

    var data = {
        MsgContent: message,
        PlayerName: playerName
    }

    console.log(data);
    connection.invoke("ChatMsg", data).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});