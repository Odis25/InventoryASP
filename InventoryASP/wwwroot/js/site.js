// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $.ajaxSetup({ cache: false });
    $("#employee-selectDeviceBtn").click(function (e) {
        e.preventDefault();
        $.get(this.href, function (data) {
            $('#employee-modalContent').html(data);
            $('#employee-modal').modal('show');
        });
    });

    $('#device-newDeviceBtn').click(function (e) {
        e.preventDefault();
        $.get(this.href, function (data) {
            $('#device-modalContent').html(data);
            $('#device-modal').modal('show');
        });
    });
})