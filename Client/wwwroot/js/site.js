// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function diesRoll() {
    var imageArr = new Array();

    imageArr[0] = "images/dice/dice1.png";
    imageArr[1] = "images/dice/dice2.png";
    imageArr[2] = "images/dice/dice3.png";
    imageArr[3] = "images/dice/dice4.png";
    imageArr[4] = "images/dice/dice5.png";
    imageArr[5] = "images/dice/dice6.png";

    var num = Math.floor(Math.random() * imageArr.length);

    return document.getElementById('dice-img').innerHTML = '<img src="' + imageArr[num] + '"/>';
}