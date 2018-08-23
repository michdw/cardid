
$(document).ready(function () {
    $('.show-btn').on('click', function () {
        $(this).hide();
        var currentSection = $(this).closest('.results-container').find('.results-section');
        $('.results-section').not(currentSection).find('.hide-btn').click();
        currentSection.removeClass('hidden');
    });

    $('.hide-btn').on('click', function () {
        $(this).closest('.results-container').find('.results-section').addClass('hidden');
        $(this).closest('.results-container').find('.show-btn').show();
    });
});