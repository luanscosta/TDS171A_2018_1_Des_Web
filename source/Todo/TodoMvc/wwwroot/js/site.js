$(document).ready(function(){
    $('#add-item-button').on('click', addItems);
});

function addItems() {
    $('#add-item-error').hide();
    var newTitle = $('#add-item-title').val();

    var data = {
        title: newTitle
    };

    $.post("todo/AddItem",data, function() {
        window.location = '/todo';
    }).fail(function(err) {
        if(err && err.responseJSON) {
            var firstError = err.responseJSON[Object.keys(err.responseJSON)[0]];
            $('#add-item-error').text(firstError).show();
        }
    });
}