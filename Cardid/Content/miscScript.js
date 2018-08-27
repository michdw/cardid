
$(document).ready(function () {

    //homepage leaderboards
    $('.leaderboard-expand').on('click', function () {
        $('.leaderboard-list-short').addClass('hidden');
        $('.leaderboard-list-long').removeClass('hidden');
    });
    $('.leaderboard-collapse').on('click', function () {
        $('.leaderboard-list-long').addClass('hidden');
        $('.leaderboard-list-short').removeClass('hidden');
    });
    $('.x-button').on('click', function () {
        $('.welcome-header').slideUp(250);
    });

    //show and hide text search results
    $('#show-results').on('click', function () {
        $(this).hide();
        var currentSection = $(this).closest('.results-container').find('.results-section');
        $('.results-section').not(currentSection).find('.hide-btn').click();
        currentSection.removeClass('hidden');
    });

    $('#hide-results').on('click', function () {
        $(this).closest('.results-container').find('.results-section').addClass('hidden');
        $(this).closest('.results-container').find('#show-results').show();
    });

    //show and hide study sessions on account info page
    $('#show-sessions').on('click', function () {
        $(this).css('display', 'none');
        $('.hide-btn').css('display', 'inline-block');
        $('.study-list-top').addClass('hidden');
        $('.study-list-full').removeClass('hidden');
    });

    $('#hide-sessions').on('click', function () {
        $(this).css('display', 'none');
        $('.show-btn').css('display', 'inline-block');
        $('.study-list-full').addClass('hidden');
        $('.study-list-top').removeClass('hidden');
    });

    //delete user account option
    $('.delete-init').on('click', function () {
        $(this).hide();
        $('.delete-submit').css('display', 'inline-block');
    });

    $('.cancel-btn').on('click', function () {
        $('.delete-submit').css('display', 'none');
        $('.delete-init').show();
    });

    $('.userinfo-change').on('click', function () {
        $(this).closest('.userinfo-inactive').addClass('hidden');
        $(this).closest('form').find('.userinfo-active').removeClass('hidden');
    });


});