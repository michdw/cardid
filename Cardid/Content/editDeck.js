
$(document).ready(function () {

    //expand and collapse tag view
    $('.expand').on('click', function () {
        $(this).addClass('hidden');
        $('.partial-tags').addClass('hidden');
        $('.collapse').removeClass('hidden');
        $('.full-tags').removeClass('hidden');
    });

    $('.collapse').on('click', function () {
        $(this).addClass('hidden');
        $('.partial-tags').removeClass('hidden');
        $('.full-tags').addClass('hidden');
        $('.expand').removeClass('hidden');
    });

    //show and hide delete option
    $('.delete-init').on('click', function () {
        $(this).hide();
        $('.delete-submit').css('display', 'inline-block');
    });

    $('.cancel-btn').on('click', function () {
        $('.delete-submit').css('display', 'none');
        $('.delete-init').show();
    });

    //show rename option
    $('.rename-init').on('click', function () {
        $(this).hide();
        $('.rename-submit').removeClass('hidden');
    });

});
