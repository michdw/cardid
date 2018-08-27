
$(document).ready(function () {

    //double-clicking on card fields in EditDeck opens EditCard view
    $('.card-field').dblclick(function () {
        $(this).closest('.card-bothsides').find('#edit-link')[0].click();
    });

    //changing text in EditCard
    $('.submit-change').on('focus', function () {

        $('.btn').css({
            background: '#777',
            color: '#aaa',
            border: '1px solid #777'
        });
        $('.btn').hover(function () {
            $(this).css({
                opacity: '.9',
                cursor: 'default'
            });
        });
    });

    $('.submit-change').on('focusout', function () {
        $(this).closest('form').submit();

        $('.btn').css({
            background: '#333',
            color: '#fff',
            border: '1px solid #fff'
        });
        $('.btn').hover(function () {
            $(this).css({
                opacity: '.7',
                cursor: 'pointer'
            });
        }, function () {
            $(this).css({
                opacity: '1',
                cursor: 'default'
            });
        });
    });

});
