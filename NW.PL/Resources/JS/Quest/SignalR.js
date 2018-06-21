$(document).ready(Ready)

var You = null,
    Places = [],
    Users = [],
    Hub = null,
    GameId = null;

function User(Id) {
    var user = null;
    Users.forEach(function (v, i) {
        if (v.Id === Id) {
            user = v;
            user.index = i;
        }
    });
    return user;
}
function Place(Id) {
    var place = null;
    Places.forEach(function (v, i) {
        if (v.Id === Id) {
            place = v;
            place.index = i;
        }
    });
    return place;
}

function UpdateAnswer(Answer, Obj) {
    Answer.addClass(".i" + Obj.Id + "u" + Obj.UserId);
    Answer.data("json", JSON.stringify(Obj));

    if (Obj.id == You.id)
        Answer.addClass("Right");
    else
        Answer.addClass("Left");

    Answer.data("id", Obj.id);
    Answer.data("userid", Obj.UserId);

    var user = User(Obj.UserId);
    if (user != null)
        Obj.Login = user.Login;

    Answer.find(".Title .Name").text(Obj.Login);
    Answer.find(".Title .Date").text(Obj.DateString);
    Answer.find(".Task span").text(Obj.Ask);
    Answer.find(".Answer span").text(Obj.Answer);
    Answer.find(".UserAnswer span").text(Obj.UserAnswer);

    if (Obj.isTrue != null) {
        Answer.addClass("TypeAnswer");

        Answer.find(".Creator button").remove();
        var color = Obj.isTrue ? "green" : "red";
        Answer.find(".Creator").css("background-color", color);

        if (You.isCreator) {
            Answer.find(".Title").removeClass("hidden");
            Answer.find(".Answer").removeClass("hidden");
        } else {
            $("form.MessageInputBlock").removeClass("hidden");
        }

        if (Obj.isTrue) {
            Answer.find(".Answer").removeClass("hidden");
        }

        Answer.find(".UserAnswer").removeClass("hidden");
        Answer.find(".Creator").removeClass("hidden");
    } else if (You.isCreator) {
        Answer.addClass("TypeAnswer");

        Answer.find(".Title").removeClass("hidden");
        Answer.find(".Answer").removeClass("hidden");
        Answer.find(".UserAnswer").removeClass("hidden");
        Answer.find(".Creator").removeClass("hidden");
    } else if (Obj.UserAnswer != null && Obj.UserAnswer != null) {
        Answer.find(".UserAnswer").removeClass("hidden");
    } else if (Obj.UserAnswer == null) {
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

        Input.val("");

        Task = Messages.find(".Message.AddTask:not(.TypeAnswer):last-child");
        Json = JSON.parse(Task.data("json"));
        
        Json.UserAnswer = Message;
        Hub.server.sendAnswer(JSON.stringify(Json));
    } else {
        element = $(element);

        Task = element.closest(".Message");
        Json = JSON.parse(Task.data("json"));

        Json.isTrue = element.val() === "true";
        Hub.server.sendAnswer(JSON.stringify(Json));
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
            ChangeUsers(Users, true);
        }

        if (game.Places != null) {
            Places = game.Places;
        }

        UpdateUserPosition();
    },
    ChangeUsers = function (users, isAdd) {
        users.forEach(function (v, i) {
            var ChangeUser = Data.User.clone(),
                UserObj = User(v.Id);
            
            if (isAdd === true) {
                if (UserObj === null) {
                    Users.push(v);
                }

                ChangeUser.addClass("Left");
                ChangeUser.find(".Delete").remove();
                ChangeUser.find(".Title span").text(v.Login);
            } else {
                if (UserObj != null) {
                    Users.splice(UserObj.index, 2);
                }

                ChangeUser.addClass("Left");
                ChangeUser.find(".Add").remove();
                ChangeUser.find(".Title span").text(v.Login);
            }
            UpdateUserPosition();
            Messages.append(ChangeUser);
        });
    },
    AddAnswers = function (answers) {
        answers.forEach(function (v, i) {
            var Answer = Data.Answer.clone();
            
            Messages.append(UpdateAnswer(Answer, v));
        });
    },
    ServerUpdateAnswer = function (answer) {
        var Answer = Messages.find(".i" + answer.Id + "u" + answer.UserId + ":last-child");
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
            Hub.server.addPosition(isYou.Position);
        }, function (error) {
            console.log(error);
        });
    },
    UserPosition = function (user) {
        var Search = User(user.id);
        if (Search != null) {
            Users[Search.index] = user;

            UpdateUserPosition();
        }
    },
    Win = function (wins) {
        console.log(wins);
    };

function Ready() {
    Hub = $.connection.chatQuestHub;

    Hub.client.auth = Auth;
    Hub.client.isYou = isYou;
    Hub.client.addGame = AddGame;
    Hub.client.changeUsers = ChangeUsers;
    Hub.client.addAnswers = AddAnswers;
    Hub.client.updateAnswer = ServerUpdateAnswer;
    Hub.client.reload = Reload;
    Hub.client.checkPosition = CheckPosition;
    Hub.client.userPosition = UserPosition;
    Hub.client.win = Win;
    
    $.connection.hub.start().done(function () {
        Hub.server.connect($("#IdQuest").val());
        CheckPosition();
    });
}