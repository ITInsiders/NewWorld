$(document).ready(Ready)

var Users = [],
    Hub = null;

var Auth = function (mes) {
    noty({
        text: mes,
        type: 'information',
        dismissQueue: true,
        theme: 'defaultTheme',
        layout: 'center'
    });
},
    SetGame = function (game) {
        $(".InfoBlock .Title").text(game.Name);
    },
    NewUser = function (user) {
        var Message = Data.Message.clone();
        Message.text("Добавился к игре: " + user.Login);
        Message.addClass("System");
        Users.push(user);
        Messages.append(Message);
    },
    OutUser = function (user) {
        var Message = Data.Message.clone();
        Message.text("Вышел из игры: " + user.Login);
        Message.addClass("System");
        Messages.append(Message);
    }
    AddMessage = function (mes) {
        var Message = Data.Message.clone();
        Message.text(mes);
        Message.addClass("Creator");
        Message.addClass("Left");
        Messages.append(Message);
    }
    SetMessage = function (mes, user = null) {
        if (user === null) {
            console.log(typeof mes);
            if (typeof mes === "string") AddMessage(mes);
            else $.each(mes, function (i, v) { AddMessage(v); });
        }
        else {
            AddMessage(user.Login + ": " + mes);
        }
    },
    Answer = function (mes) {
        console.log(mes);
    },
    CheckPosition = function () {
        console.log("Check");
    },
    UserPosition = function (UserId, Position) {
        console.log(UserId);
        console.log(Position);
    };

function SendMessageServer(message) {
    Hub.server.sendMessage(message, 0);
};

function Ready() {
    Hub = $.connection.chatQuestHub;

    Hub.client.Auth = Auth;
    Hub.client.SetGame = SetGame;
    Hub.client.NewUser = NewUser;
    Hub.client.OutUser = OutUser;
    Hub.client.SetMessage = SetMessage;
    Hub.client.Answer = Answer;
    Hub.client.CheckPosition = CheckPosition;
    Hub.client.UserPosition = UserPosition;

    var QuestId = $("#IdQuest").val();
    $.connection.hub.start().done(function () {
        Hub.server.connect(QuestId);
    });
}