
const uri = 'api/duckfeeding';
const uri2 = 'api/login'
let records = null;


$(document).ready(function () {


});

function login(){
    const user = {
        'username' : $('#username').val(),
        'password' : $('#password').val()
    };
    $.ajax({
    type: 'POST',
    accepts: 'application/json',
    url: uri2,
    contentType: 'application/json',
    data: JSON.stringify(user),
    error: function (jqXHR, textStatus, errorThrown) {
        alert('Some Error Happened in JS login!');
    },
    success: function (result) {

        getData();
        $('#username').val('');
        $('#password').val('');
    }
    });
}

function getData() {
    $.ajax({
        type: 'GET',
        url: uri,
        success: function (data) {
            $('#records').empty();
            $.each(data, function (key, item) {                
                const checked = item.repeat ? 'checked = "checked"' : '';

                $('<tr><td><input diabled="true" readonly="readonly" type="text" value="' + item.time.toString() + '"></td> '+
                    '<td><input diabled="true" readonly="readonly" type="text" value="' + item.food + '"></td> '+
                    '<td><input diabled="true" readonly="readonly" type="text" value="' + item.location + '"></td> '+
                    '<td><input diabled="true" readonly="readonly" type="text" value="' + item.count + '"></td> '+
                    '<td><input diabled="true" readonly="readonly" type="text" value="' + item.foodType + '"></td> '+
                    '<td><input diabled="true" readonly="readonly" type="text" value="' + item.foodAmount + '"></td> '+
                ' <td><input disabled="true" type="checkbox" value="' + checked + '"></td>' +
                    '<td><input diabled="true" type="text" value="' + item.period + '"></td>' +
                    '</tr>').appendTo($('#records'));
            });
            records = data;

        }
    });

}


function addItem() {
    var x=$("#repeat").is(":checked");
    const item = {
        'time': $('#time').val(),
        'food': $('#food').val(),
        'location': $('#location').val(),
        'count': $('#count').val(),
        'foodtype': $('#foodtype').val(),
        'foodamount': $('#foodamount').val(),
        'repeat': x,
        'period': $('#period').val()
    };

    $.ajax({
        type: 'POST',
        accepts: 'application/json',
        url: uri,
        contentType: 'application/json',
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Some Error Happened in JS file!');
        },
        success: function (result) {
        
            $('#time').val('');
            $('#food').val('');
            $('#location').val('');
            $('#count').val('');
            $('#foodtype').val('');
            $('#foodamount').val('');
            $('#repeat').prop('checked',false);
            $('#period').val('');
        }
    });
}

