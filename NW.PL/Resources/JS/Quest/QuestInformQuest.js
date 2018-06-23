var PageData = {};
PageData.text = "Поиск";

initHashChange();


function onZonesLoadQuest(data) {
        var maxRadius = 0,
            argPoint = [data.ArgLatitude, data.ArgLongtude];

        $.each(data.coordinates, function (i, v) {
            var Radius = ymaps.coordSystem.geo.getDistance(argPoint, v);
            maxRadius = (maxRadius < Radius) ? Radius : maxRadius;
        });

        var myCircle = new ymaps.Circle([
            argPoint,
            maxRadius + 100
        ], {
                balloonContent: data.Name
            }, {
                fillColor: data.fillColor
            });

        console.log(myCircle);
    
        myMap.geoObjects.add(myCircle);

        console.log(data.Bounds);
        console.log(argPoint);

        myMap.setBounds(data.Bounds, {
            preciseZoom: true,
            checkZoomRange: true
        }).then(function () {
            //myMap.setCenter(argPoint, myMap.getZoom() - 2);
            }, function (err) {
                console.log(err);
            }, this);

        ymaps.geocode(argPoint).then(function (res) {
            var firstGeoObject = res.geoObjects.get(0);
            console.log(firstGeoObject);
            $(".Title.Region").text("Область квеста в радиусе " + maxRadius.toFixed(2) + " м. от " + firstGeoObject.properties._data.name);
            $(".Title.Sity").text(firstGeoObject.getLocalities());
        });

}

var CreatePoligoneOneQuest = function () {
    $.ajax({
        type: "POST",
        url: "/Quest/OnZonesLoadOneQuest/" + $("#id").val(),
        dataType: "json",
        timeout: 5000,
        async: false,
        success: onZonesLoadQuest,
        error: function (err) { console.log(err); }
    });
}

$("#GameStart").click(function () {
    console.log(1);
    $.ajax({
        type: "POST",
        url: "/Quest/OnZonesLoadOneQuest/" + $("#id").val(),
        dataType: "json",
        timeout: 5000,
        async: false,
        success: LocationCheck,
        error: function (err) { console.log(err); }
    });
})

var position = null;
function LocationCheck(data) {
    console.log(2);
    var maxRadius = 0,
        argPoint = [data.ArgLatitude, data.ArgLongtude];
    console.log(3);
    $.each(data.coordinates, function (i, v) {
        var Radius = ymaps.coordSystem.geo.getDistance(argPoint, v);
        maxRadius = (maxRadius < Radius) ? Radius : maxRadius;
    });
    console.log(4 + " maxRadius" + maxRadius);
    ymaps.geolocation.get({
        provider: 'browser',
        mapStateAutoApply: true
    }).then(function (result) {
        console.log(5);
        //console.log(result.geoObjects.get(0).properties.get('metaDataProperty'));
        //result.geoObjects.get(0).properties.set({
        //    balloonContentBody: 'Мое местоположение'
        //});
        position = result.geoObjects.position;
        console.log(6);
        var RadiusPosition = ymaps.coordSystem.geo.getDistance(argPoint, position);
        if (RadiusPosition < maxRadius + 10000) {
            console.log("+");
            document.location.href = "/Quest/Game/" + $("#id").val();
        }
        else {
            console.log("-");
            var n = noty({
                text: "Вы находитесь вне зоны квеста",
                type: 'information',
                dismissQueue: true,
                theme: 'defaultTheme',
                layout: 'center'
            })

        }
        }, function (error) {
            console.log(error);
        });
        

}