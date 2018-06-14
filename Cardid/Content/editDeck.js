
function deactivate(buttons) {
    buttons.prop('disabled', true);

    buttons.css({
        background: '#777',
        color: '#aaa',
        border: '1px solid #777'
    });

    buttons.hover(function () {
        $(this).css({
            opacity: '1',
            cursor: 'default'
        });
    });
}

function reactivate(buttons) {
    buttons.prop('disabled', true);

    otherButtons.prop('disabled', false);
    otherButtons.css({
        background: '#333',
        color: '#fff',
        border: '1px solid #fff'
    });

    otherButtons.hover(function () {
        $(this).css({
            opacity: '.7',
            cursor: 'pointer'
        });
    });
}


$(document).ready(function () {

    $('.delete-init').on('click', function () {
        $(this).hide();
        $("html, body").animate({ scrollTop: $(document).height() }, 400);
        $('.delete-submit').css('display', 'inline-block');
    });

    $('.cancel-btn').on('click', function () {
        $('.delete-submit').css('display', 'none');
        $('.delete-init').show();
    });
});
