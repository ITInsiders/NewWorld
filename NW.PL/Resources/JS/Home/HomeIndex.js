var myMap = null,
    position = null;

ymaps.ready(init);
function init() {
    var geolocation = ymaps.geolocation;
    myMap = new ymaps.Map('map', {
            center: [51.6720400, 39.1843000],
            zoom: 12,
            controls: []
        }, {
                searchControlProvider: 'yandex#search'
            });
    geolocation.get({
        provider: 'browser',
        mapStateAutoApply: true
    }).then(function (result) {
        result.geoObjects.get(0).properties.set({
            balloonContentBody: 'Мое местоположение'
        });
        myMap.geoObjects.add(result.geoObjects);
        position = result.geoObjects.position;
        setTimeout(function () {
            myMap.setZoom(16, { duration: 300 });
        }, 500)
    });
}

$('#Location').click(function () {
    if (position !== null) myMap.setCenter(position, 16, { duration: 300 });
    else {
        new noty({
            type: 'success',
            layout: 'topRight',
            text: 'Для определения местоположения разрешите доступ к данным о геопозиции!'
        }).show();
    }
})
var flaguser = true;
function showuser() {
    if (flaguser) $("header .User .user").css({ "display": "inline-block" });
    else $("header .User .user").css({ "display": "none" });
    flaguser = !flaguser;
}