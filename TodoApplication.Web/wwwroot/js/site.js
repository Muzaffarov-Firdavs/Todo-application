// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var starbox = document.querySelector(".star-box");
var starimage = document.querySelector(".star-image");


starbox.addEventListener("click", function () {
    starimage.classList.toggle("star-active");
});