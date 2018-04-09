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

var scrollPage = true;
window.addEventListener("mousewheel", function (e) {
    e = e || window.event;
    if (scrollPage === true)
        ScrollPage(e);
});

function ScrollPage(e) {
    var $window = $(window),
        scrollTop = $window.scrollTop();
    $('html, body').animate({ scrollTop: (scrollTop + ((e.deltaY > 0) ? $window.height() : -$window.height())) }, 500);
    scrollPage = false;
    setTimeout(setScrollPage, 500);
}

function setScrollPage(scroll = true) {
    scrollPage = scroll;
}

$(window).scroll(function () {
    if ($(window).scrollTop() > 10)
        $("header").addClass("min");
    else
        $("header").removeClass("min");
});
$(document).ready(function () {
    if ($(window).scrollTop() > 10)
        $("header").addClass("min");
    else
        $("header").removeClass("min");
});

$(function () {
    $('a[href^="#"]').on('click', function (event) {
        event.preventDefault();
        var sc = $(this).attr("href"),
            dn = $(sc).offset().top;
        $('html, body').animate({ scrollTop: dn }, 500);
    });
});