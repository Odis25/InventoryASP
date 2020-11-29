
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
        console.log(data);
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

    let button = document.querySelector('#login-button');

    button.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Авторизация...'

    let model = $('.modal form').serialize();
    $.post('/Authorization/Login', model).done(function (result) {

        button.innerHTML = 'Войти';

        // Код обработки результата
        $('#modalWindow-content-sm').html(result);
        var isValid = $('.modal-body').find('[name="IsValid"]').val() == 'True';
        if (isValid) {
            window.location.href = '/Home/Index';
        }
    }).fail(function () {
        button.innerHTML = 'Войти';
    });
}

// Создать нового сотрудника
function CreateEmployee() {
    CreateOrUpdate({ action: 'Create', controller: 'Employee', modalWindowSize: 'lg' });
}

// Изменить данные сотрудника
function UpdateEmployee() {
    CreateOrUpdate({ action: 'Update', controller: 'Employee', modalWindowSize: 'lg' });
}

// Создать новое оборудование
function CreateDevice() {
    CreateOrUpdate({ action: 'Create', controller: 'Device', modalWindowSize: 'lg' });
}

// Изменить данные оборудования
function UpdateDevice() {
    CreateOrUpdate({ action: 'Update', controller: 'Device', modalWindowSize: 'lg' });
}

// Создать или изменить объект
function CreateOrUpdate(operation) {

    let model = $('.modal form').serialize();

    $.post(`/${operation.controller}/${operation.action}`, model).done(function (result) {

        // Код обработки результата
        $(`#modalWindow-content-${operation.modalWindowSize}`).html(result);

        let isValid = $('.modal-body [name="IsValid"]').val() == 'True';

        if (isValid) {
            window.location.href = `/${operation.controller}/Index`;
        }
    });
}