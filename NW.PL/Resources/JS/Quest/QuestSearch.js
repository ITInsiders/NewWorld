var PageData = {};
PageData.text = "Поиск";

initHashChange();

$(document).ready(Ready);

function Ready() {
    SearchBlocks();
    $("#Search").keyup(SearchLines).click(SearchLines);

}

function SearchLines() {
    if (this === window) return

    var Search = $(this).val(),
        List = $("#SearchList");
    var id = $("#id").val();
    var Data = null;
    $.ajax({
        type: "POST",
        url: "/Quest/SearchLines",
        data: { Search: Search, id: id },
        dataType: "json",
        timeout: 5000,
        async: false,
        success: function (data) {
            Data = data;
        },
        error: function () {
        }
    });

    List.empty();
    $.each(Data, function (i, v) {
        List.append('<option value="' + v.Value + '" label="' + v.Type + '">');
    });
}

function SearchBlocks() {
    var Search = $("#Search").val(),
        SearchResult = $("#SearchResult"),
        Block = $("#Templates .Block.Empty"),
        BlockNull = $("#Templates .Block.Null");
    var id = $("#id").val();
    var Data = null;

    $.ajax({
        type: "POST",
        url: "/Quest/SearchBlocks",
        data: { Search: Search, id: id },
        dataType: "json",
        timeout: 5000,
        async: false,
        success: function (data) {
            Data = data;
        },
        error: function () {
        }
    });

    SearchResult.empty();
    if (Data.length === 0) {
        SearchResult.append(BlockNull.clone());
    }
    else {
        $.each(Data, function (i, v) {
            var BlockClone = Block.clone();

            BlockClone.attr("href", "/Quest/InformQuest/" + v.Id);
            BlockClone.find(".ImgBlock").css({ "background-image": "url('" + v.SRC + "')" });
            BlockClone.find(".ImgBlock .Title").text(v.Name);


            SearchResult.append(BlockClone);
        });
    }
}
//-------------------- КАРТЫ полигоны-------------------------------


//var myMap = null;
//var CreatePoligone = function () {
//    myMap = map.data.map;
//    var deliveryPoint = new ymaps.GeoObject({
//        geometry: { type: 'Point' },
//        properties: { iconCaption: 'Адрес' }
//    }, {
//            preset: 'islands#blackDotIconWithCaption',
//            draggable: true,
//            iconCaptionMaxWidth: '215'
//        });
//    myMap.geoObjects.add(deliveryPoint);

//    function onZonesLoad(json) {
//        // Добавляем зоны на карту.
//        var deliveryZones = ymaps.geoQuery(json).addToMap(myMap);
//        // Задаём цвет и контент балунов полигонов.
//        deliveryZones.each(function (obj) {
//            var color = obj.options.get('fillColor');
//            color = color.substring(0, color.length - 2);
//            obj.options.set({ fillColor: color, fillOpacity: 0.4 });
//            //obj.properties.set('balloonContent', obj.properties.get('name'));
//            obj.properties.set('balloonContentHeader', obj.properties.get('name'))
//        });

//        // При перемещении метки сбрасываем подпись, содержимое балуна и перекрашиваем метку.
//        deliveryPoint.events.add('dragstart', function () {
//            deliveryPoint.properties.set({ iconCaption: '', balloonContent: '' });
//            deliveryPoint.options.set('iconColor', 'black');
//        });

//        // По окончании перемещения метки вызываем функцию выделения зоны доставки.
//        deliveryPoint.events.add('dragend', function () {
//            highlightResult(deliveryPoint);
//        });

//        function highlightResult(obj) {
//            // Сохраняем координаты переданного объекта.
//            var coords = obj.geometry.getCoordinates(),
//                // Находим полигон, в который входят переданные координаты.
//                polygon = deliveryZones.searchContaining(coords).get(0);

//            if (polygon) {
//                // Уменьшаем прозрачность всех полигонов, кроме того, в который входят переданные координаты.
//                deliveryZones.setOptions('fillOpacity', 0.4);
//                polygon.options.set('fillOpacity', 0.8);
//                // Перемещаем метку с подписью в переданные координаты и перекрашиваем её в цвет полигона.
//                deliveryPoint.geometry.setCoordinates(coords);
//                deliveryPoint.options.set('iconColor', polygon.options.get('fillColor'));
//                // Задаем подпись для метки.
//                if (typeof (obj.getThoroughfare) === 'function') {
//                    setData(obj);
//                } else {
//                    // Если вы не хотите, чтобы при каждом перемещении метки отправлялся запрос к геокодеру,
//                    // закомментируйте код ниже.
//                    ymaps.geocode(coords, { results: 1 }).then(function (res) {
//                        var obj = res.geoObjects.get(0);
//                        setData(obj);
//                    });
//                }
//            } else {
//                // Если переданные координаты не попадают в полигон, то задаём стандартную прозрачность полигонов.
//                deliveryZones.setOptions('fillOpacity', 0.4);
//                // Перемещаем метку по переданным координатам.
//                deliveryPoint.geometry.setCoordinates(coords);
//                // Задаём контент балуна и метки.
//                deliveryPoint.properties.set({
//                    iconCaption: 'Доставка транспортной компанией',
//                    balloonContent: 'Cвяжитесь с оператором',
//                    balloonContentHeader: ''
//                });
//                // Перекрашиваем метку в чёрный цвет.
//                deliveryPoint.options.set('iconColor', 'black');
//            }

//            function setData(obj) {
//                var address = [obj.getThoroughfare(), obj.getPremiseNumber(), obj.getPremise()].join(' ');
//                if (address.trim() === '') {
//                    address = obj.getAddressLine();
//                }
//                deliveryPoint.properties.set({
//                    iconCaption: address,
//                    balloonContent: address,
//                    balloonContentHeader: '<b>Стоимость доставки: ' + polygon.properties.get('dateTime') + ' р.</b>'
//                });
//            }
//        }
//    }

//    $.ajax({
//        type: "POST",
//        url: "/Quest/OnZonesLoad",
//        data: { Search: $(Search).val(), id: $(id).val() },
//        dataType: "json",
//        timeout: 5000,
//        async: false,
//        success: onZonesLoad,
//        error: function () { }
//    });
//}

function onZonesLoad(data) {
    $.each(data, function (index, value) {
        var maxRadius = 0,
            argPoint = [value.ArgLatitude, value.ArgLongtude];

        $.each(value.coordinates, function (i, v) {
            var Radius = ymaps.coordSystem.geo.getDistance(argPoint, v);
            maxRadius = (maxRadius < Radius) ? Radius : maxRadius;
        });

        var myCircle = new ymaps.Circle([
            argPoint,
            maxRadius + 100
        ], {
                balloonContent: value.Name
            }, {
                fillColor: value.fillColor
        });

        console.log(myCircle);

        myMap.geoObjects.add(myCircle);
    });
}

var CreatePoligone = function () {
    $.ajax({
        type: "POST",
        url: "/Quest/OnZonesLoad",
        data: { Search: $(Search).val(), id: $(id).val() },
        dataType: "json",
        timeout: 5000,
        async: false,
        success: onZonesLoad,
        error: function (err) { console.log(err); }
    });
}

// Создаем карту.
//var myMap = new ymaps.Map("map", {
//    center: [55.76, 37.64],
//    zoom: 10
//}, {
//        searchControlProvider: 'yandex#search'
//    });

// Создаем круг.

//var myCircle = new ymaps.Circle([
//    // Координаты центра круга.
//    [55.76, 37.60],
//    // Радиус круга в метрах.
//    10000
//], {
//        // Описываем свойства круга.
//        // Содержимое балуна.
//        balloonContent: "Радиус круга - 10 км",
//        // Содержимое хинта.
//        hintContent: "Подвинь меня"
//    }, {
//        // Задаем опции круга.
//        // Включаем возможность перетаскивания круга.
//        draggable: true,
//        // Цвет заливки.
//        // Последний байт (77) определяет прозрачность.
//        // Прозрачность заливки также можно задать используя опцию "fillOpacity".
//        fillColor: "#DB709377",
//        // Цвет обводки.
//        strokeColor: "#990066",
//        // Прозрачность обводки.
//        strokeOpacity: 0.8,
//        // Ширина обводки в пикселях.
//        strokeWidth: 5
//    });

//// Добавляем круг на карту.
//myMap.geoObjects.add(myCircle);
//}