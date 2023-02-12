// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Timer
const countDownTime = 120; // number of seconds to count down from
let countDown = countDownTime;

const timer = setInterval(function () {
    countDown--;
    document.getElementById("timer").innerHTML = countDown;

    if (countDown <= 0) {
        clearInterval(timer);
        alert("Time's up!");
    }
}, 1000);
