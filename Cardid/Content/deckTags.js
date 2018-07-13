
function switchView() {

    if ($('.tags-pop').hasClass('hidden')) {

        $('.tagview-pop').addClass('bold').removeClass('pointer');
        $('.tagview-alph').removeClass('bold').addClass('pointer');
        $('.tags-alph').addClass('hidden');
        $('.toggle-right').addClass('hidden');
        $('.tags-pop').removeClass('hidden');
        $('.toggle-left').removeClass('hidden');

    } else {
  
        $('.tagview-alph').addClass('bold').removeClass('pointer');
        $('.tagview-pop').removeClass('bold').addClass('pointer');
        $('.tags-pop').addClass('hidden');
        $('.toggle-left').addClass('hidden');
        $('.tags-alph').removeClass('hidden');
        $('.toggle-right').removeClass('hidden');
    }
}


$(document).ready(function () {

    $('.tagview-pop, .tagview-alph, .toggle-left, .toggle-right').on('click', function () {
        switchView();
    });

});