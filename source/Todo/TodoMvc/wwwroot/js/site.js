$(document).ready(function(){
    $('#add-item-button').on('click', addItems);

    $('.done-checkbox').on('click', function (e) {
        markCompleted(e.target);
    });
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

function markCompleted(checkbox) {
    checkbox.disabled = true;

    var data = {
        id: checkbox.name
    };

    $.post('/Todo/MarkDone', data, function () {
        var row = checkbox.parentElement.parentElement;
        $(row).addClass('done');
    });
}