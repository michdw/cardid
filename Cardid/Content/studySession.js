
function viewFront() {
    $('.back-text').addClass('hidden');
    $('.front-text').removeClass('hidden');
}

function viewBack() {
    $('.front-text').addClass('hidden');
    $('.back-text').removeClass('hidden');
}


function flipCard() {
    $('.active-card').css('transform', 'rotateX(-180deg)');

    setTimeout(function () {
        $('.active-card p').css('transform', 'rotateX(-180deg)');
        if ($('.back-text').hasClass('hidden')) {
            viewBack();
        } else {
            viewFront();
        }
    }, 150);

    $('.flip').addClass('hidden');
    $('.mark-frame').removeClass('hidden');
}


function nextCard() {
    if ($('.front-text').hasClass('hidden')) {
        viewFront();
    } else {
        viewBack();
    }
    $('.flip').removeClass('hidden');
    $('.mark-frame').addClass('hidden');

    var thisCard = $('.active-card');
    var nextCard = thisCard.next();

    thisCard.css('transform', 'rotateX(-180deg) translateX(-100vw)').removeClass('active-card').addClass('old-card');
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

    $('.study-card').first().removeClass('new-card').addClass('active-card');


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
