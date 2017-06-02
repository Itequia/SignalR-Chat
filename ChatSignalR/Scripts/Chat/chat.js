$(function () {

    $.ajax({
        url: "/signalr/hubs",
        dataType: "script",
        async: false
    });

    function encodeHTML(s) {
        return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/"/g, '&quot;');
    }






    //Chat logic:
    var chat = $.connection.chatHub;

    chat.client.addNewMessageToPage = function (username, message) {
        $('#discussion').append('<li><strong>' + username + '</strong>' + message + '</li>');
    };

    chat.client.hello = function (username) {
        $('#users').append('<li>' + username + '</li>');
    };

    chat.client.update = function (users) {
        $('#users2').html(users);
    };

    $('#username').val(prompt('Tu nick de messenger:', ''));
    $('#message').focus();

    $.connection.hub.start().done(function () {

        chat.server.hello($('#username').val());
        chat.server.update($('#username').val());

        $('#send').click(function () {
            chat.server.send($('#username').val(),encodeHTML($('#message').val()));
            $('#message').val('').focus();
        })
    });

    
});