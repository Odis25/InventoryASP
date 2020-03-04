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
    let model = $('.modal form').serialize();
    $.post('/Account/Login', model).done(function (result) {

        // Код обработки результата
        $('#modalWindow-content-sm').html(result);
        var isValid = $('.modal-body').find('[name="IsValid"]').val() == 'True';
        if (isValid) {
            window.location.href = '/Home/Index';
        }
    })
}

function ChangePassword() {
    let model = $('.modal form').serialize();
    $.post('/Account/ChangePassword', model).done(function (result) {

        // Код обработки результата
        $('#modalWindow-content-sm').html(result);
        var isValid = $('.modal-body').find('[name="IsValid"]').val() == 'True';
        if (isValid) {
            window.location.href = '/Home/Index';
        }
    })
}

// Создать нового сотрудника
function CreateEmployee() {
    CreateOrUpdate({ type: 'Create', object: 'Employee', size: 'lg' });
}

// Изменить данные сотрудника
function UpdateEmployee() {
    CreateOrUpdate({ type: 'Update', object: 'Employee', size: 'lg' });
}

// Создать новое оборудование
function CreateDevice() {
    CreateOrUpdate({ type: 'Create', object: 'Device', size: 'lg' });
}

// Изменить данные оборудования
function UpdateDevice() {
    CreateOrUpdate({ type: 'Update', object: 'Device', size: 'lg' });
}

// Создать или изменить объект
function CreateOrUpdate(operation) {

    let model = $('.modal form').serialize();
    $.post(`/${operation.object}/${operation.type}`, model).done(function (result) {

        // Код обработки результата
        $(`#modalWindow-content-${operation.size}`).html(result);
        let isValid = $('.modal-body [name="IsValid"]').val() == 'True';
        if (isValid) {
            window.location.href = `/${operation.object}/Index`;
        }
    });
}