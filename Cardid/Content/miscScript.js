
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

    //double-clicking on card fields in EditDeck opens EditCard view
    $('.card-field').dblclick(function () {
        $(this).closest('.card-bothsides').find('.edit-link')[0].click();
    });

    //show and hide text search results
    $('.show-results').on('click', function () {
        $(this).hide();
        var currentSection = $(this).closest('.results-container').find('.results-section');
        $('.results-section').not(currentSection).find('.hide-results').click();
        currentSection.removeClass('hidden');
    });

    $('.hide-results').on('click', function () {
        $(this).closest('.results-container').find('.results-section').addClass('hidden');
        $(this).closest('.results-container').find('.show-results').show();
    });

    //show and hide study sessions on account info page
    $('#show-sessions').on('click', function () {
        $(this).removeClass('btn').addClass('hidden');
        $('#hide-sessions').addClass('btn').removeClass('hidden');
        $('.study-list-top').addClass('hidden');
        $('.study-list-full').removeClass('hidden');
    });

    $('#hide-sessions').on('click', function () {
        $(this).removeClass('btn').addClass('hidden');
        $('#show-sessions').addClass('btn').removeClass('hidden');
        $('.study-list-full').addClass('hidden');
        $('.study-list-top').removeClass('hidden');
    });

    //delete decks or account - confirm or cancel
    $('.delete-init').on('click', function () {
        $(this).addClass('hidden').removeClass('btn');
        $('.delete-submit').removeClass('hidden');
    });

    $('.cancel-btn').on('click', function () {
        $('.delete-submit').addClass('hidden');
        $('.delete-init').addClass('btn').removeClass('hidden');
    });

    $('.userinfo-change').on('click', function () {
        $(this).closest('.userinfo-inactive').addClass('hidden');
        $(this).closest('form').find('.userinfo-active').removeClass('hidden');
    });

});