$(document).ready(Ready)

var You = null,
    Users = [],
    Hub = null,
    GameId = null;

function User(Id) {
    var user = null;
    $.each(Users, function (i, v) {
        if (v.Id === Id) user = v;
    });
    return user;
}

var Auth = function () {
    noty({
        text: "Пожалуйста авторизуйтесь!",
        type: 'information',
        dismissQueue: true,
        theme: 'defaultTheme',
        layout: 'center'
    });
},
    isYou = function (user) {
        You = user;
    },
    AddGame = function (game) {
        $(".InfoBlock .Title").text(game.Name);
        GameId = game.Id;

        if (game.Users != null) {
            Users = game.Users;
        }
    },
    AddUsers = function (users) {
        $.each(users, function (i, v) {
            if (User(v.id) == null) {
                Users.push(v);

                var AddUser = Data.AddUser.clone();
                AddUser.addClass("Left");
                AddUser.fund(".Title span").text(v.Login);
            }
        });
    },
    AddAnswers = function (answers) {
        $.each(answers, function (i, v) {
            var Answer = Data.Answer.clone();

            if (v.id == You.id)
                Answer.addClass("Right");
            else
                Answer.addClass("Left");

            Answer.data("id", v.id);
            Answer.data("userid", v.UserId);
            v.Login = User(v.UserId).Login;
            Answer.find(".Title .Name").text(v.Login);
            Answer.find(".Title .Date").text(v.DateString);
            Answer.find(".Task span").text(v.Ask);
            Answer.find(".Answer span").text(v.Answer);
            Answer.find(".UserAnswer span").text(v.UserAnswer);

            if (v.isTrue != null) {
                Answer.find(".Creator button").remove();
                var color = v.isTrue ? "green" : "red";
                Answer.find(".Creator").css("background-color", color);

                Answer.find(".Title").removeClass("hidden");
                Answer.find(".Answer").removeClass("hidden");
                Answer.find(".UserAnswer").removeClass("hidden");
                Answer.find(".Creator").removeClass("hidden");
            } else if (You.isCreator) {
                Answer.find(".Title").removeClass("hidden");
                Answer.find(".Answer").removeClass("hidden");
                Answer.find(".UserAnswer").removeClass("hidden");
                Answer.find(".Creator").removeClass("hidden");
            }

            Messages.append(Answer);
        });
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