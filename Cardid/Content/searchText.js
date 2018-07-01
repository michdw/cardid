
$(document).ready(function () {
    $('.show-btn').on('click', function () {
        $(this).hide();
        //$(this).closest('.results-container').find('.hide-btn').show();
        $(this).closest('.results-container').find('.results-section').removeClass('hidden');
    });

    $('.hide-btn').on('click', function () {
        //$(this).hide();
        $(this).closest('.results-container').find('.results-section').addClass('hidden');
        $(this).closest('.results-container').find('.show-btn').show();
    });
});