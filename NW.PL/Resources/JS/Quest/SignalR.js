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
    Answer.data("json", JSON.stringify(Obj));
    Answer.data("id", Obj.id);
    Answer.data("userid", Obj.UserId);

    if (Obj.id == You.id)
        Answer.addClass("Right");
    else
        Answer.addClass("Left");

    var user = User(Obj.UserId);
    if (user != null)
        Obj.Login = user.Login;

    Answer.find(".Title .Name").text(Obj.Login);
    Answer.find(".Title .Date").text(Obj.DateString);
    Answer.find(".Task span").text(Obj.Ask);
    Answer.find(".Answer span").text(Obj.Answer);
    Answer.find(".UserAnswer span").text(Obj.UserAnswer);

    if (Obj.isTrue != null) {

        if (Obj.isTrue) {
            var place = Place(Obj.id);

            if (place == null)
                Places.push({ Id: Obj.Id, Address: Obj.Answer, Position: Obj.Position });
            
            UpdateUserPosition();

            Answer.find(".Answer").removeClass("hidden");
        }
        
        Answer.find(".Creator button").remove();
        var color = Obj.isTrue ? "green" : "red";
        Answer.find(".Title").css("background-color", color);
        Answer.find(".Creator").css("background-color", color);

        if (You.isCreator) {
            Answer.find(".Title").removeClass("hidden");
            Answer.find(".Answer").removeClass("hidden");
        } else {
            $("form.MessageInputBlock").removeClass("hidden");
        }

        Answer.find(".UserAnswer").removeClass("hidden");
        Answer.find(".Creator").removeClass("hidden");
    } else if (You.isCreator) {
        Answer.find(".Title").removeClass("hidden");
        Answer.find(".Answer").removeClass("hidden");
        Answer.find(".UserAnswer").removeClass("hidden");
        Answer.find(".Creator").removeClass("hidden");
    } else if (Obj.UserAnswer != null && Obj.UserAnswer != "") {
        Answer.find(".UserAnswer").removeClass("hidden");
        $("form.MessageInputBlock").addClass("hidden");
    } else if (Obj.UserAnswer == null || Obj.UserAnswer == "") {
        $("form.MessageInputBlock").removeClass("hidden");
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

        if (Task != null) {
            Json = JSON.parse(Task.data("json"));

            Json.UserAnswer = Message;
            Hub.server.sendAnswer(JSON.stringify(Json));
        }
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
        if (!You.isCreator)
            You.Position = [51.667836, 39.173176];
        
        UpdateUserPosition();

        if (You.Lives === -1) {
            noty({
                text: "Вы проиграли!",
                type: 'information',
                dismissQueue: true,
                theme: 'defaultTheme',
                layout: 'center'
            });

            $("form.MessageInputBlock").addClass("hidden");
        }
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

            ChangeUser.addClass("Left");
            
            if (isAdd === true) {
                if (UserObj === null) Users.push(v);

                ChangeUser.find(".Delete").remove();
            } else {
                if (UserObj != null) Users.splice(UserObj.index, 1);
                
                ChangeUser.find(".Add").remove();
            }

            ChangeUser.find(".Title span").text(v.Login);

            Messages.append(ChangeUser);
        });
        UpdateUserPosition();
    },
    AddAnswers = function (answers) {
        answers.forEach(function (v, i) {
            var Answer = Data.Answer.clone();
            Answer.addClass("i" + v.Id + "u" + v.UserId);
            Messages.append(UpdateAnswer(Answer, v));
        });
    },
    ServerUpdateAnswer = function (answer) {
        var Answer = Messages.find(".i" + answer.Id + "u" + answer.UserId + ":last-child");
        UpdateAnswer(Answer, answer);
    },
    Reload = function () {
        location.reload();
    },
    CheckPosition = function () {
        Hub.server.addPosition(You.Position);

        UpdateUserPosition();
        /*ymaps.geolocation.get({
            provider: 'browser'
        }).then(function (result) {
            You.Position = result.geoObjects.position;
            Hub.server.addPosition(You.Position);

            UpdateUserPosition();
        }, function (error) {
            console.log(error);
        });*/
    },
    UserPosition = function (user) {
        var Search = User(user.id);

        if (Search == null) Users.push(user);
        else Users[Search.index] = user;

        UpdateUserPosition();
    },
    Win = function (wins) {
        if (!You.isCreator) {
            console.log(wins);
            noty({
                text: "Победа: У вас " + wins.length + " место",
                type: 'information',
                dismissQueue: true,
                theme: 'defaultTheme',
                layout: 'center'
            });
        }
        
        $("form.MessageInputBlock").addClass("hidden");
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
    });
}