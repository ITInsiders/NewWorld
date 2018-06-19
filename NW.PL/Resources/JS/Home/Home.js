var myMap = null;
ymaps.ready(init);
function init() {
    var position = [51.661535, 39.200287];

    ymaps.geolocation.get({
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

function setScrollPage(scroll = true) {
    scrollPage = scroll;
}

$(window).scroll(function () {
    ScrollTop();
});

$(document).ready(function () {
    ScrollTop();
});

function ScrollTop() {
    if ($(window).scrollTop() > 10)
        $("header").addClass("min");
    else
        $("header").removeClass("min");
}

$(function () {
    $('a[href^="#"]').on('click', function (event) {
        event.preventDefault();
        var sc = $(this).attr("href"),
            dn = $(sc).offset().top;
        $('html, body').animate({ scrollTop: dn }, 500);
    });
});
var flaguser = true;
function showuser() {
    if (flaguser) $("header .Info").addClass("show");
    else $("header .Info").removeClass("show");
    flaguser = !flaguser;
}
$(document).ready(function () {
    var message = $("#message").val();
    if (message != "") {
        noty({
            text: message,
            type: 'information',
            dismissQueue: true,
            theme: 'defaultTheme',
            layout: 'center'
        });
    }
})