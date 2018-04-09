var map = null;
ymaps.ready(function () { map = new Map(); });

var Map = function () {
    var _this = this,
        Data = {};

    function init() {
        Data.GeoLocation = ymaps.geolocation;
        Data.Position = null;
        Data.Map = null;

        _this.updateGeoLocation('createMap');
    }

    this.createMap = function () {
        Data.Map = new ymaps.Map('Map', {
            center: Data.Position,
            zoom: 10,
            controls: []
        });
    }

    this.updateGeoLocation = function (func = null, param = null) {
        Data.GeoLocation.get()
            .then(function (result) {
                Data.Position = result.geoObjects.position;

                if (func != null)
                    _this[func](param);
            });
    }

    this.getPosition = function () {
        return Data.Position;
    }
    
    init();
};

$(document).ready(Ready);

function Ready() {

}