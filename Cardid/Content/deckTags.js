$(document).ready(function () {

    $('.tagview-pop').click(function () {
        $(this).addClass('bold').removeClass('pointer');
        $('.tagview-alph').removeClass('bold').addClass('pointer');
        $('.tags-alph').addClass('hidden');
        $('.tags-pop').removeClass('hidden');
    });

    $('.tagview-alph').click(function () {
        $(this).addClass('bold').removeClass('pointer');
        $('.tagview-pop').removeClass('bold').addClass('pointer');
        $('.tags-pop').addClass('hidden');
        $('.tags-alph').removeClass('hidden');
    });

});