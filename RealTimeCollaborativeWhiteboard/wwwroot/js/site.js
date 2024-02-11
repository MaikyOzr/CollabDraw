// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

$(document).ready(function () {
    $('.fixed-size-image').on('click', function () {
        var imageUrl = $(this).attr('src');
        $('#modalImage').attr('src', imageUrl);
        $('#exampleModal').modal('show');
    });
});
$(document).ready(function () {
    // Клік на кнопку "Закрити"
    $('.close').on('click', function () {
        // Закриття модального вікна
        $('#exampleModal').modal('hide');
    });
});