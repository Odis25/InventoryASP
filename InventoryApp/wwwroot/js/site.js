
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

// Распечатка таблицы оборудования или сотрудников
function PrintTable() {

    const table = document.querySelector('.indexTable');

    let css = '<style type = "text/css">';
    css += `
    table
    {
        font-family: Arial;
        font-size: 0.9em;
        border: 1px solid #333;
        border-collapse: collapse;
    }
    table a
    {
        text-decoration: none;
    }
    .controlCell
    {
        display: none;
    }
    table th span
    {
        display: none;
    }
    table th
    {
        font-weight: bold;
    }
    table th, table td
    {
        padding: 5px;
        border: 1px solid #333;
    }`; 
    css += '</style>';

    let html = table.outerHTML;

    window.frames["print_frame"].document.write(css);
    window.frames["print_frame"].document.write(html);
    
    window.frames["print_frame"].window.focus();
    window.frames["print_frame"].window.print();
    window.frames["print_frame"].document.close();
}

// Сортировка таблиц в диалоговых окнах
function SortTable(data) {

    const getCellValue = (tr, idx) => tr.children[idx].innerText || tr.children[idx].textContent;

    const comparer = (idx, asc) => (a, b) => ((v1, v2) =>
        v1 !== '' && v2 !== '' && !isNaN(v1) && !isNaN(v2) ? v1 - v2 : v1.toString().localeCompare(v2)
    )(getCellValue(asc ? a : b, idx), getCellValue(asc ? b : a, idx));

    const table = document.querySelector('.indexTable');
    const tbody = table.querySelector('tbody');

    Array.from(tbody.querySelectorAll('tr'))
        .sort(comparer(data, this.asc = !this.asc))
        .forEach(tr => tbody.appendChild(tr));
}

// Фильтрация таблицы в диалоговых окнах
function FilterTable() {

    const searchPattern = document.querySelector('#search-pattern').value.toUpperCase();

    const table = document.querySelector('.indexTable tbody');

    const rows = table.querySelectorAll('tr');

    let i = 1;

    for (let row of rows) {

        let rowVisible = true;

        for (let cell of row.cells) {

            if (cell.innerText.toUpperCase().includes(searchPattern)) {
                rowVisible = true;
                break;
            }
            rowVisible = false;
        }

        if (rowVisible) {
            row.style.display = "";
            row.cells[0].innerText = i++;
        } else {
            row.style.display = "none";
        }
    }


    let model = { employeeId: id, sortOrder: "", searchPattern: searchPattern };

    $.get('/Device/SelectDevices', model).done(function (result) {
        $('#modalWindow-content-lg').html(result);
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