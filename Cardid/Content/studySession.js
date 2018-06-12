
function flipCard() {
    $('.front-view').addClass('hidden');
    $('.back-view').removeClass('hidden');
    $('.flip').addClass('hidden');
    $('.mark-frame').removeClass('hidden');
}


function nextCard() {
    $('.front-view').removeClass('hidden');
    $('.back-view').addClass('hidden');
    $('.flip').removeClass('hidden');
    $('.mark-frame').addClass('hidden');

    var thisCard = $('.active-card');
    var nextCard = thisCard.next();

    thisCard.removeClass('active-card').addClass('old-card');
    nextCard.removeClass('new-card').addClass('active-card');
}


function updateScore(totalCorrect, totalViewed) {
    $('.total-correct').text(totalCorrect);
    $('.total-viewed').text(totalViewed);
}


$(document).ready(function () {

    var cardCount = parseInt($('#cardCount').data('name'));
    var flipped = false;
    var totalCorrect = 0;
    var totalViewed = 0;

    updateScore(totalCorrect, totalViewed);

    $('.studycard').first().removeClass('new-card').addClass('active-card');

    $('.flip').click(function () {
        flipCard();
        flipped = true;
    });

    $('.mark-right').click(function () {
        totalCorrect++;
    });

    $('.mark-right, .mark-wrong').click(function () {

        totalViewed++;

        if (totalViewed == cardCount) {
            $('#totalScore').val(totalCorrect);
            $('#possibleScore').val(cardCount);
            $('#complete').submit();
        }

        else {
            updateScore(totalCorrect, totalViewed);
            flipped = false;
            nextCard();
        }
    });

});
