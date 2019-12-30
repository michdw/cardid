
//display tags alphabetically or by popularity
function switchView() {

    if ($('.tags-pop').hasClass('hidden')) {

        $('.tagview-pop').removeClass('show-pointer').addClass('noclick');
        $('.tagview-alph').addClass('show-pointer').removeClass('noclick');
        $('.tags-alph').addClass('hidden');
        $('.toggle-right').addClass('hidden');
        $('.tags-pop').removeClass('hidden');
        $('.toggle-left').removeClass('hidden');

    } else {

        $('.tagview-alph').removeClass('show-pointer').addClass('noclick');
        $('.tagview-pop').addClass('show-pointer').removeClass('noclick');
        $('.tags-pop').addClass('hidden');
        $('.toggle-left').addClass('hidden');
        $('.tags-alph').removeClass('hidden');
        $('.toggle-right').removeClass('hidden');
    }
}

//preserve scroll position on page refresh
$(window).scroll(function () {
    sessionStorage.scrollTop = $(this).scrollTop();
});


$(document).ready(function () {

    if (sessionStorage.scrollTop !== "undefined") {
        $(window).scrollTop(sessionStorage.scrollTop);
    }

    $('.deck-listing').on('click', function () {
        $(this).find('.deckname-listing')[0].click();
    });

    $('.submit-btn').on('click', function () {
        $(this).closest('form').submit();
    });

    $('.tagview-pop, .tagview-alph, .toggle-left, .toggle-right').on('click', function () {
        switchView();
    });

    $('.rename-init').on('click', function () {
        $(this).hide();
        $('.rename-submit').removeClass('hidden');
    });
});



