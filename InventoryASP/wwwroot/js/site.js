// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $.ajaxSetup({ cache: false });

    // Открытие модальных окон
    $('.openModalBtn-sm').on('click', { size: 'sm' }, openModal);
    $('.openModalBtn-m').on('click', { size: 'm' }, openModal);
    $('.openModalBtn-lg').on('click', { size: 'lg' }, openModal);
});

// Открытие модального окна
function openModal(e) {
    e.preventDefault();
    $.get(this.href).done(function (data) {
        switch (e.data.size) {
            case 'sm':
                $('#modalWindow-content-sm').html(data);
                $('.bgModal-sm').modal('show');
                break;
            case 'm':
                $('#modalWindow-content-m').html(data);
                $('.bgModal-m').modal('show');
                break;
            case 'lg':
                $('#modalWindow-content-lg').html(data);
                $('.bgModal-lg').modal('show');
                break;
        }        
    });
}

// Логин пользователя
function Login() {

    var model = $("#loginForm").serialize();
    $.ajax({
        type: "post",
        data: model,
        url: "/Account/Login",
        success: function (result) {

            // Код обработки результата
            $("#modalWindow-content-sm").html(result);

            var isValid = $("#loginIsValid").val() == "True";

            if (isValid) {
                window.location.href = "/Home/Index";
            }
        }
    })
}

// Создать нового сотрудника
function CreateEmployee() {

    var model = $("#createEmployeeForm").serialize();

    $.post("/Employee/AddEmployee", model).done(function (result) {

        alert('test');

        //$("#modalWindow-content-lg").html(result);
        //alert(result);
        //var isValid = $("#createEmployeeForm").find("name=[IsValid]").val() == 'True';

        //alert(isValid);

        //if (isValid) {
        //    window.location.href = "/Employee/Index";
        //}
    });
}