
$(document).ready(function () {


    $('.card-field').dblclick(function () {

        $(this).closest('.card-bothsides').find('#edit-link')[0].click();

    });

});