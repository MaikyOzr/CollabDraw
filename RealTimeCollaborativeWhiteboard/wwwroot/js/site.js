// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.
$('.fixed-size-image').on('click', function () {
    var imageUrl = $(this).attr('src');
    var deskId = $(this).data('desk-id');
    $('#modalImage').attr('src', imageUrl);
    $('#exampleModal').modal('show');
    $('#downloadButton')
        .attr('href', imageUrl)
        .attr('download', 'image.jpg');
    $('#deskIdInput').val(deskId);
    $('#deleteForm').attr('action', '/ViewFiles/DeletePhoto');
});
$('.close').on('click', function () {
    $('#exampleModal').modal('hide');
});