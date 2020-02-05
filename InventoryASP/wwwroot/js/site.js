// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $.ajaxSetup({ cache: false });

    // Открытие модальных окон
    $('.openModalBtn').on('click', openModal);
});

// Открытие модального окна
function openModal(e) {
    e.preventDefault();
    $.get(this.href, function (data) {
        $('#modalWindow-content').html(data);
        $('#modalWindow-background').modal('show');
    });
}