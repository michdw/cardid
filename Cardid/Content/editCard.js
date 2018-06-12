
$(document).ready(function () {

    $('.submit-change').on('focus', function () {

        $('.btn').css({
            background: '#777',
            color: '#aaa',
            border: '1px solid #777'
        });
        $('.btn').hover(function () {
            $(this).css({
                opacity: '1',
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
