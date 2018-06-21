var PageData = {};
PageData.text = "Главная";

initHashChange();

$(window).resize(function () {
    changeMap();
});

$(document).ready(function () {
    changeMap();

    if ($("#IdUser").val() === $("#Creater").val()) {
        $(".Message.Standart").removeClass('Left');
        $(".Message.Standart").addClass('Right');
    }
})

function changeMap() {
    if ($(window).width() <= 768) {
        $("#miniMap").append($("#Map"));
    }
    else {
        $("section").before($("#Map"));
    }
}

function UpdateUserPosition() {
    if (myMap === null) {
        setTimeout(UpdateUserPosition, 500);
        return;
    };

    myMap.geoObjects.removeAll();
    console.log(Places);
    console.log(Users);

    $.each(Places, function (i, v) {
        var point = new ymaps.Placemark(v.Position, {
            balloonContent: v.Address
        }, {
                preset: 'islands#governmentCircleIcon',
                iconColor: '#3b5998'
            });
        myMap.geoObjects.add(point);
    });

    $.each(Users, function (i, v) {
        var point = new ymaps.GeoObject({
            geometry: {
                type: "Point",
                coordinates: v.Position
            },
            properties: {
                iconContent: v.Login,
                hintContent: v.Lives + ' жизней'
            }
        }, { preset: 'islands#blackStretchyIcon' });
        myMap.geoObjects.add(point);
    });
}