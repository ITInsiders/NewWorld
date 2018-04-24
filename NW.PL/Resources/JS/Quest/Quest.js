var map = null;

ymaps.ready(initMap);
function initMap() {
    map = new Map();
}

class Map {
    constructor() {
        this.data = {};
        this.data.user = null;

        this.data.map = new ymaps.Map('Map', {
            center: [0, 0],
            zoom: 10,
            controls: []
        });

        this.goGeolocation();
    }

    goGeolocation() {
        var self = this;
        ymaps.geolocation.get({
            mapStateAutoApply: true
        }).then(function (result) {
            var position = result.geoObjects.position;
            self.data.map.setCenter(position, 14, { duration: 300 });
        });
    }
}

$(document).ready(function () {
    initHashChange();
});

function getPageByHash(link) {
    if (typeof (link) != "undefined") {
        if (link != "") {
            $.ajax({
                type: "POST",
                cache: false,
                async: false,
                url: link,
                success: function (data) {
                    setPage(data);
                }
            });
        }
    }
}

var setPage = function (data) {
    $(".InfoBlock .MinPage").html(data);
    $(".InfoBlock .Header .Title").text(PageData.text);
}

function initHashChange() {
    $("a").click(function () {
        if ($(this).attr("href").substr(0, 1) == "/") {
            if (getNameBrouser() == "gecko") {
                window.history.pushState("", "", $(this).attr("href"));
                window.history.replaceState("", "", $(this).attr("href"));
                getPageByHash($(this).attr("href"));
            } else {
                window.location.hash = $(this).attr("href");
            }
            return false;
        }
    });
}

function getNameBrouser() {
    var userAgent = navigator.userAgent.toLowerCase();
    // Определим Internet Explorer
    if (userAgent.indexOf("msie") != -1 && userAgent.indexOf("opera") == -1 && userAgent.indexOf("webtv") == -1) {
        return "msie";
    }
    // Opera
    if (userAgent.indexOf("opera") != -1) {
        return "opera";
    }
    // Gecko = Mozilla + Firefox + Netscape
    if (userAgent.indexOf("gecko") != -1) {
        return "gecko";
    }
    // Safari, используется в MAC OS
    if (userAgent.indexOf("safari") != -1) {
        return "safari";
    }
    // Konqueror, используется в UNIX-системах
    if (userAgent.indexOf("konqueror") != -1) {
        return "konqueror";
    }

    return "unknown";
}