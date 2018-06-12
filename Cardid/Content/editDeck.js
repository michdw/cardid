$('.delete-submit').hide();

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
        $('.delete-submit').show();
    });

    $('.cancel-btn').on('click', function () {
        $('.delete-submit').hide();
        $('.delete-init').show();
    });
});