$(document).ready(Ready)

var You = null,
    Places = [],
    Users = [],
    Hub = null,
    GameId = null;

function User(Id) {
    var user = null;
    $.each(Users, function (i, v) {
        if (v.Id === Id) {
            user = v;
            user.index = i;
        }
    });
    return user;
}
function Place(Id) {
    var place = null;
    $.each(Places, function (i, v) {
        if (v.Id === Id) {
            place = v;
            place.index = i;
        }
    });
    return place;
}

function UpdateAnswer(Answer, Obj) {
    Answer.attr("id", "i" + Obj.Id + "u" + Obj.UserId);
    Answer.data("json", JSON.stringify(Obj));

    if (Obj.id == You.id)
        Answer.addClass("Right");
    else
        Answer.addClass("Left");

    Answer.data("id", Obj.id);
    Answer.data("userid", Obj.UserId);
    Ans.Login = User(Obj.UserId).Login;
    Answer.find(".Title .Name").text(Obj.Login);
    Answer.find(".Title .Date").text(Obj.DateString);
    Answer.find(".Task span").text(Obj.Ask);
    Answer.find(".Answer span").text(Obj.Answer);
    Answer.find(".UserAnswer span").text(Obj.UserAnswer);

    if (You.isCreator) {
        Answer.addClass("TypeAnswer");

        Answer.find(".Title").removeClass("hidden");
        Answer.find(".Answer").removeClass("hidden");
        Answer.find(".UserAnswer").removeClass("hidden");
        Answer.find(".Creator").removeClass("hidden");
    } else if (Obj.isTrue != null) {
        Answer.addClass("TypeAnswer");

        Answer.find(".Creator button").remove();
        var color = Obj.isTrue ? "green" : "red";
        Answer.find(".Creator").css("background-color", color);

        Answer.find(".Title").removeClass("hidden");
        Answer.find(".Answer").removeClass("hidden");
        Answer.find(".UserAnswer").removeClass("hidden");
        Answer.find(".Creator").removeClass("hidden");
    } else if (Obj.UserAnswer != null && Obj.UserAnswer != null) {
        Answer.find("UserAnswer").removeClass("hidden");
    } else {
        $("form.MessageInputBlock").removeClass("hidden");
    }

    if (Obj.UserAnswer != null && Obj.UserAnswer != null) {
        var place = Place(Obj.Id);

        if (place === null) {
            place = {
                Id: Obj.Id,
                Position: Obj.Position,
                Address: Obj.Answer
            }
            Places.push(place);
        }

        UpdateUserPosition();
    }

    return Answer;
}

function SendAnswer(element = null) {
    var Task = null,
        Json = null;

    if (element === null) {
        $("form.MessageInputBlock").addClass("hidden");

        var Input = $("form.MessageInputBlock .MessageInput"),
            Message = Input.val();

        Task = Messages.fund(".Message.Task:not(.TypeAnswer):last-child");
        Json = JSON.parse(Task.data("json"));

        Json.UserAnswer = Message;
        Hub.server.SendAnswer(Json);
    } else {
        element = $(element);

        Task = element.closest(".Message");
        Json = JSON.parse(Task.data("json"));

        Json.isTrue = Task.val() === "true";
        Hub.server.SendAnswer(Json);
    }
    UpdateAnswer(Task, Json)
}

var
    Auth = function () {
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

        if (game.Places != null) {
            Places = game.Places;
        }

        UpdateUserPosition();
    },
    ChangeUsers = function (users, isAdd) {
        $.each(users, function (i, v) {
            var ChangeUser = Data.User.clone(),
                UserObj = User(v.id);

            if (isAdd) {
                if (UserObj == null) {
                    Users.push(v);

                    ChangeUser.addClass("Left");
                    ChangeUser.find(".Delete").remove();
                    ChangeUser.fund(".Title span").text(v.Login);

                    Messages.append(AddUser);
                }
                else
                    ChangeUser = null;
            } else {
                if (UserObj != null) {
                    Users.splice(UserObj.index, 1);

                    ChangeUser.addClass("Left");
                    ChangeUser.find(".Add").remove();
                    ChangeUser.fund(".Title span").text(v.Login);
                }
                else
                    ChangeUser = null;
            }
            Messages.append(ChangeUser);
        });
    },
    AddAnswers = function (answers) {
        $.each(answers, function (i, v) {
            var Answer = Data.Answer.clone();

            Answer.attr("id", "i" + v.Id + "u" + v.UserId);
            Messages.append(UpdateAnswer(Answer, v));
        });
    },
    UpdateAnswer = function (answer) {
        var Answer = Messages.find("#i" + answer.Id + "u" + answer.UserId);
        Answer = UpdateAnswer(Answer, answer);
    },
    Reload = function () {
        location.reload();
    },
    CheckPosition = function () {
        ymaps.geolocation.get({
            provider: 'browser'
        }).then(function (result) {
            isYou.Position = result.geoObjects.position;
            Hub.server.AddPosition(isYou.Position);
        }, function (error) {
            console.log(error);
        });
    },
    UserPosition = function (user) {
        var Search = User(user.id);
        Users[Search.index] = user;

        UpdateUserPosition();
    },
    Win = function (wins) {
        alert.log(wins);
    };

function Ready() {
    Hub = $.connection.chatQuestHub;

    Hub.client.Auth = Auth;
    Hub.client.isYou = isYou;
    Hub.client.AddGame = AddGame;
    Hub.client.ChangeUsers = ChangeUsers;
    Hub.client.AddAnswers = AddAnswers;
    Hub.client.UpdateAnswer = UpdateAnswer;
    Hub.client.Reload = Reload;
    Hub.client.CheckPosition = CheckPosition;
    Hub.client.UserPosition = UserPosition;
    Hub.client.Win = Win;
    
    $.connection.hub.start().done(function () {
        Hub.server.Connect($("#IdQuest").val());
        CheckPosition();
    });
}