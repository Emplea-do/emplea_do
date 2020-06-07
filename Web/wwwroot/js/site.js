// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $('.select2').each(function () {
        let placeholder = $(this).data('placeholder') || 'seleccione una opción';
        let allowClear = $(this).data('allowClear') || $(this).attr('data-allowClear');;

        $(this).select2({
            placeholder: placeholder,
            allowClear: allowClear
        });
    });

    $('.i-checks').iCheck({
        checkboxClass: 'icheckbox_square-aero',
        radioClass: 'iradio_square-aero'
    });
});