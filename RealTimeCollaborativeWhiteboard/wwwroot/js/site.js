$(document).ready(function () {
    // Handle click event on "Upload New Photo" button to show the modal
    $('#uploadNewPhotoBtn').on('click', function () {
        $('#uploadPhotoModal').modal('show');
    });

    // Handle click event on "Upload New Audio" button to show the modal
    $('#uploadNewAudioBtn').on('click', function () {
        $('#uploadAudioModal').modal('show');
    });

    // Handle click event on "Upload New Doc File" button to show the modal
    $('#uploadNewDocFileBtn').on('click', function () {
        $('#uploadDocFileModal').modal('show');
    });

    // Handle click event on "Delete" button to show the modal
    $('.deleteBtn').on('click', function () {
        var deskId = $(this).data('desk-id');
        $('#deskIdInput').val(deskId);
        $('#deleteModal').modal('show');
    });

    // Handle click event on "Close" button to close the modal
    $('.closeBtn').on('click', function () {
        $(this).closest('.modal').modal('hide');
    });

    // Handle click event on image to show modal with full image
    $('.card-img-top').on('click', function () {
        var imageUrl = $(this).attr('src');
        $('#modalImage').attr('src', imageUrl);
        $('#exampleModal').modal('show');
    });

    // Handle click event on "Download" button to download the image
    $('#downloadButton').on('click', function () {
        var imageUrl = $('#modalImage').attr('src');
        window.open(imageUrl, '_blank');
    });
});
