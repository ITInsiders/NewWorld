var myMap = null;
ymaps.ready(init);
function init() {
    var position = [51.661535, 39.200287];

    ymaps.geolocation.get({
        provider: 'browser'
    }).then(function (result) {
        position = result.geoObjects.position;

        if (myMap != null)
            myMap.setCenter(position);
    });

    myMap = new ymaps.Map('Map', {
        center: position,
        zoom: 10,
        controls: []
    });
}