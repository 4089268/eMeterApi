// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready( () => {
    $("#navbarDropdownMenuLink2").click(() => {
        $(".dropdown-menu.nav-user-dropdown").toggleClass("show");
    });
});