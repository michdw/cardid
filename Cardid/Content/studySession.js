
function viewFront() {
    $('.back-view').addClass('hidden');
    $('.front-view').removeClass('hidden');
}

function viewBack() {
    $('.front-view').addClass('hidden');
    $('.back-view').removeClass('hidden');
}


function flipCard() {
    if ($('.back-view').hasClass('hidden')) {
        viewFront();
    } else {
        viewBack();
    }
    $('.flip').addClass('hidden');
    $('.mark-frame').removeClass('hidden');
}


function nextCard() {
    if ($('.front-view').hasClass('hidden')) {
        viewBack();
    } else {
        viewFront();
    }
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
    var toRedo = '';

    updateScore(totalCorrect, totalViewed);

    $('.studycard').first().removeClass('new-card').addClass('active-card');

    $('.flip').click(function () {
        flipCard();
        flipped = true;
    });

    $('.mark-right').click(function () {
        totalCorrect++;
    });

    $('.mark-wrong').click(function () {
        var cardID = $('.active-card .card-ID').text().trim();
        toRedo += cardID + ',';
    });

    $('.mark-right, .mark-wrong').click(function () {
        totalViewed++;

        if (totalViewed == cardCount) {
            $('#totalScore').val(totalCorrect);
            $('#possibleScore').val(cardCount);
            $('#toRedo').val(toRedo.slice(0, -1));
            $('#complete').submit();
        }

        else {
            updateScore(totalCorrect, totalViewed);
            flipped = false;
            nextCard();
        }
    });

});
