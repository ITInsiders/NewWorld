var map = null;
var myMap = null;

ymaps.ready(initMap);
function initMap() {
    map = new Map();

    try {
        CreatePoligone();
    } catch { }
    try {
        CreatePoligoneOneQuest();
    } catch { }
}

class Map {
    constructor() {
        this.data = {};
        this.data.user = null;

        this.data.map = new ymaps.Map('Map', {
            center: [51.694272, 39.335960],
            zoom: 10,
            controls: []
        });
        myMap = this.data.map;
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
    loadCSS();
    loadJS();
    $(".InfoBlock .Header .Title").text(PageData.text);
    $("title").html(PageData.text);
}

var loadJS = function () {
    var JS = $("input#JS").val();
    $.ajax({
        type: "GET",
        url: JS,
        dataType: "script",
        cache: true,
        async: false
    });
}

var loadCSS = function () {
    var CSS = $("input#CSS").val();
    $("<link/>", {
        rel: "stylesheet",
        type: "text/css",
        href: CSS
    }).appendTo("head");
}

function initHashChange() {
    //$("a").click(function () {
    //    if ($(this).attr("href").substr(0, 7) == "/Quest/") {
    //        if (getNameBrouser() == "gecko") {
    //            window.history.pushState(null, null, $(this).attr("href"));
    //            //window.history.replaceState(null, null, $(this).attr("href"));
    //            getPageByHash($(this).attr("href"));
    //        } else {
    //            window.location.hash = $(this).attr("href");
    //        }
    //        return false;
    //    }
    //});
}

window.addEventListener("popstate", function (e) {
    getPageByHash(location.pathname);
});

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