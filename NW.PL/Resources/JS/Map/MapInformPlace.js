var myMap = null,
    position = null;

ymaps.ready(init);
function init() {
    ymaps.geolocation.get({
        provider: 'browser',
        mapStateAutoApply: true
    }).then(function (result) {
        result.geoObjects.get(0).properties.set({
            balloonContentBody: 'Мое местоположение'
        });

        position = result.geoObjects.position;
        $("#Distance").css("display", "inline-block");
        route();

    }, function (error) {
        myMap = new ymaps.Map('Map', {
            center: [longitude,latitude],
            zoom: 17
        });
        myMap.geoObjects.add(new ymaps.Placemark([longitude, latitude], {
            balloonContent: '<strong>' + name + '</strong>'
        }, {
                preset: 'islands#dotIcon',
                iconColor: '#735184'
            }))
        
    });
}
//------------------------- постороение маршрута-------------------------
var multiRouteModel;

function route() {
    multiRouteModel = new ymaps.multiRouter.MultiRouteModel([
        position, [longitude, latitude]
    ], {});

    var routeTypeSelector = new ymaps.control.ListBox({
        data: {
            content: 'Как добраться'
        },
        items: [
            new ymaps.control.ListBoxItem({ data: { content: "На авто" }, state: { selected: true } }),
            new ymaps.control.ListBoxItem({ data: { content: "Общественным транспортом" } }),
            new ymaps.control.ListBoxItem({ data: { content: "Пешком" } })
        ],
        options: {
            itemSelectOnClick: false
        }
    });

    var autoRouteItem = routeTypeSelector.get(0),
        masstransitRouteItem = routeTypeSelector.get(1),
        pedestrianRouteItem = routeTypeSelector.get(2);

    autoRouteItem.events.add('click', function (e) { changeRoutingMode('auto', e.get('target')); });
    masstransitRouteItem.events.add('click', function (e) { changeRoutingMode('masstransit', e.get('target')); });
    pedestrianRouteItem.events.add('click', function (e) { changeRoutingMode('pedestrian', e.get('target')); });

    ymaps.modules.require([
        'MultiRouteCustomView'
    ], function (MultiRouteCustomView) {
        new MultiRouteCustomView(multiRouteModel);
    });

    myMap = new ymaps.Map('Map', {
        center: [(longitude + position[0]) / 2, (latitude + position[1]) / 2],
        zoom: 7,
        controls: [routeTypeSelector]
    }, {
            buttonMaxWidth: 300
        });

    multiRoute = new ymaps.multiRouter.MultiRoute(multiRouteModel, {});

    myMap.geoObjects.add(multiRoute);

    myMap.setBounds([position, [longitude, latitude]]);
    function changeRoutingMode(routingMode, targetItem) {
        multiRouteModel.setParams({ routingMode: routingMode }, true);

        // Отменяем выбор элементов.
        autoRouteItem.deselect();
        masstransitRouteItem.deselect();
        pedestrianRouteItem.deselect();

        // Выбираем элемент и закрываем список.
        targetItem.select();
        routeTypeSelector.collapse();
    }
}



var flagmes = true;
function message() {
    if (flagmes) $(".Message").css({ "top": 0, "opacity": 1 });
    else $(".Message").css({ "top": "-100vh", "opacity": 0 });
    flagmes = !flagmes;
}
//-----------------------------------------работа с лайками и дизлайками----------------------------
function RatingUpdate() { RatingTest(); RatingCount(); }

function Rating(R) {
    $.getJSON("/Map/AddRatingJson", { R: R, IdPlace: id })
        .done(function (result) { RatingUpdate(); })
        .fail(function (result) { RatingUpdate(); });
};

function RatingTest() {
    $.getJSON("/Map/TestRatingJson", { R: 2, IdPlace: id })
        .done(function (Result) {
            if (Result) { $("#ImageLike").css("background-image", "url('/Resources/Images/System/Icons/Like.png')"); }
            else { $("#ImageLike").css("background-image", "url('/Resources/Images/System/Icons/Like1.png')"); }
        });
    $.getJSON("/Map/TestRatingJson", { R: 1, IdPlace: id })
        .done(function (Result) {
            if (Result) { $("#ImageDislike").css("background-image", "url('/Resources/Images/System/Icons/Dislike.png')"); }
            else { $("#ImageDislike").css("background-image", "url('/Resources/Images/System/Icons/Dislike1.png')"); }
        });
    $.getJSON("/Map/TestRatingJson", { R: 3, IdPlace: id })
        .done(function (Result) {
            if (Result) { $("#ImageCheckins").css("background-image", "url('/Resources/Images/System/Icons/Checkin.png')"); }
            else { $("#ImageCheckins").css("background-image", "url('/Resources/Images/System/Icons/Checkin1.png')"); }
        });
}

function RatingCount() {
    $.getJSON("/Map/CountRatingJson", { R: 2, IdPlace: id })
        .done(function (Result) { $("#Like").html(Result); });
    $.getJSON("/Map/CountRatingJson", { R: 1, IdPlace: id })
        .done(function (Result) { $("#Dislike").html(Result); });
    $.getJSON("/Map/CountRatingJson", { R: 3, IdPlace: id })
        .done(function (Result) { $("#Checkins").html(Result); });
    $.getJSON("/Map/CountRatingJson", { R: 4, IdPlace: id })
        .done(function (Result) { $("#Rating").html(Result); });
}


//-------------------------------------------- Для вывода данных о маршруте -------------------------------------------------------
ymaps.modules.define('MultiRouteCustomView', [
    'util.defineClass'
], function (provide, defineClass) {
    // Класс простого текстового отображения модели мультимаршрута.
    function CustomView(multiRouteModel) {
        this.multiRouteModel = multiRouteModel;
        // Объявляем начальное состояние.
        this.state = "init";
        this.stateChangeEvent = null;
        // Элемент, в который будет выводиться текст.
        this.outputElement = $('<div class="DescriptionRoute"></div>').appendTo('#lengthTime');

        this.rebuildOutput();

        // Подписываемся на события модели, чтобы
        // обновлять текстовое описание мультимаршрута.
        multiRouteModel.events
            .add(["requestsuccess", "requestfail", "requestsend"], this.onModelStateChange, this);
    }

    // Таблица соответствия идентификатора состояния имени его обработчика.
    CustomView.stateProcessors = {
        init: "processInit",
        requestsend: "processRequestSend",
        requestsuccess: "processSuccessRequest",
        requestfail: "processFailRequest"
    };

    // Таблица соответствия типа маршрута имени его обработчика.
    CustomView.routeProcessors = {
        "driving": "processDrivingRoute",
        "masstransit": "processMasstransitRoute",
        "pedestrian": "processPedestrianRoute"
    };

    defineClass(CustomView, {
        // Обработчик событий модели.
        onModelStateChange: function (e) {
            // Запоминаем состояние модели и перестраиваем текстовое описание.
            this.state = e.get("type");
            this.stateChangeEvent = e;
            this.rebuildOutput();
        },

        rebuildOutput: function () {
            // Берем из таблицы обработчик для текущего состояния и исполняем его.
            var processorName = CustomView.stateProcessors[this.state];
            this.outputElement.html(
                this[processorName](this.multiRouteModel, this.stateChangeEvent)
            );
        },

        processInit: function () {
            return "Инициализация ...";
        },

        processRequestSend: function () {
            return "Запрос данных ...";
        },

        processSuccessRequest: function (multiRouteModel, e) {
            var routes = multiRouteModel.getRoutes();
            $("#length").empty();

            if (routes.length) {
                var typeRoute;
                if (routes[0].properties.get("type") == "driving") typeRoute = "на авто";
                if (routes[0].properties.get("type") == "masstransit") typeRoute = "общ. транспортом";
                if (routes[0].properties.get("type") == "pedestrian") typeRoute = "пешком";
                result = ["До места: " + routes[0].properties.get("distance").text + " , " + routes[0].properties.get("duration").text + " (" + typeRoute + ")"];
                $("#length").append(result);
                $("#down").css("display", "inline-block");
                result = ["Подробное описание маршрутов:"];
                //result.push("Всего маршрутов: " + routes.length + ".");
                for (var i = 0, l = routes.length; i < l; i++) {
                    result.push(this.processRoute(i, routes[i]));
                }
            } else {
                result.push("Нет маршрутов.");
            }
            return result.join("<br/>");
        },

        processFailRequest: function (multiRouteModel, e) {
            return e.get("error").message;
        },

        processRoute: function (index, route) {
            // Берем из таблицы обработчик для данного типа маршрута и применяем его.
            var processorName = CustomView.routeProcessors[route.properties.get("type")];
            return (index + 1) + ". " + this[processorName](route);
        },

        processDrivingRoute: function (route) {
            var result = ["Автомобильный маршрут."];
            result.push(this.createCommonRouteOutput(route));
            return result.join("<br/>");
        },

        processMasstransitRoute: function (route) {
            var result = ["Маршрут на общественном транспорте."];
            result.push(this.createCommonRouteOutput(route));
            result.push("Описание маршута: <ul>" + this.createMasstransitRouteOutput(route) + "</ul>");
            return result.join("<br/>");
        },

        processPedestrianRoute: function (route) {
            var result = ["Пешеходный маршрут."];
            result.push(this.createCommonRouteOutput(route));
            return result.join("<br/>");
        },

        // Метод, формирующий общую часть описания для всех типов маршрутов.
        createCommonRouteOutput: function (route) {
            return "Протяженность маршрута: " + route.properties.get("distance").text + "<br/>" +
                "Время в пути: " + route.properties.get("duration").text;
        },

        // Метод строящий список текстовых описаний для
        // всех сегментов маршрута на общественном транспорте.
        createMasstransitRouteOutput: function (route) {
            var result = [];
            for (var i = 0, l = route.getPaths().length; i < l; i++) {
                var path = route.getPaths()[i];
                for (var j = 0, k = path.getSegments().length; j < k; j++) {
                    result.push("<li>" + path.getSegments()[j].properties.get("text") + "</li>");
                }
            }
            return result.join("");
        },

        destroy: function () {
            this.outputElement.remove();
            this.multiRouteModel.events
                .remove(["requestsuccess", "requestfail", "requestsend"], this.onModelStateChange, this);
        }
    });

    provide(CustomView);
});


$("#down").click(function () {
    if ($("#lengthTime").is(":visible")) {
        $("#lengthTime").css("display", "none");
        $("#down").removeClass("glyphicon-menu-up").addClass("glyphicon-menu-down");
    }
    else {
        $("#lengthTime").css("display", "inline-block");
        $("#down").removeClass("glyphicon-menu-down").addClass("glyphicon-menu-up");
    }
})

//---------------------------------------------------------------------