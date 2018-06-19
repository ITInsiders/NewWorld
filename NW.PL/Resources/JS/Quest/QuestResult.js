var PageData = {};
PageData.text = "Поиск";

initHashChange();

$(document).ready(function () {
    ReadyMap();
});

function ReadyMap() {
    if (myMap === null) setTimeout(ReadyMap, 500);
    else {
        $.each(mapspoint, function (i, e) {
            var placemark = new ymaps.Placemark(e);
            myMap.geoObjects.add(placemark);
        });  

        myMap.setBounds(Bounds, {
            preciseZoom: true,
            checkZoomRange: true
        }).then(function () {
            myMap.setCenter(ArgPoint, myMap.getZoom());
        }, function (err) {
            console.log(err);
        }, this);

    }
}