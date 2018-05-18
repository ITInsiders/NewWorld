var Data = {};

$(document).ready(function () {

    Data.Addresses = $("#Address");
    Data.Addresses.Adr = Data.Addresses.find(".Address").clone();
    Data.Addresses.find(".Address").remove();

    Data.Prizes = $("#Place");
    Data.Prizes.Prz = Data.Prizes.find(".Place").clone();
    Data.Prizes.find(".Place").remove();
    

    Data.PictureLoad = $("#PictureLoad");
    Data.PictureLoad.change(PictureLoad);

    $("#AddPointMap").click(AddressAdd);
    AddressAdd();

    $("#AddPrize").click(PrizeAdd);
    PrizeAdd();

    $("#openMapForInput").click(ShowMap);

    AddressAddInit();

    PrizeAddInit();

    Data.SearchAddress = $("#SearchAddress");
})

function matchRegex(reg, str) {
    var result = [];
    while ((match = reg.exec(str)) !== null) {
        result.push(parseInt(match[1]));
    }
    return result;
}

function UpdateIndex(element) {
    const regex = /\[(\d+)\]/gim;

    var element = $(element),
        className = "." + element.attr("class"),
        elements = $(className),
        indexes = [],
        inputs = element.find("input, textarea"),
        index = 0;      

    elements.each((ind, elem) => {
        var name = $(elem).find("input, textarea").attr("name"),
            index = matchRegex(regex, name);
        indexes.push(index[0]);
    });

    while (indexes.indexOf(index) != -1) index += 1;

    element.attr("id", "Address_" + index)
    inputs.each((ind, elem) => {
        var name = $(elem).attr("name").replace(regex, "[" + index + "]");
        $(elem).attr("name", name);
    });

    return element;
}

function ShowMap() {
    var map = $(this).closest(".form-group").find(".map");
    if (map.is(":hidden"))
        map.addClass("active");
    else
        map.removeClass("active");
}

function AddressAddInit() {
    Data.Addresses.find(".remove").click(function () {
        $(this).closest(".Address").remove();
    });
}

function AddressAdd() {
    var clone = Data.Addresses.Adr.clone();
    Data.Addresses.append(UpdateIndex(clone));
    AddressAddInit();
}

function PrizeAddInit() {
    Data.Prizes.find(".remove").click(function () { $(this).closest(".Place").remove(); });
}

function PrizeAdd() {
    var clone = Data.Prizes.Prz.clone();
    Data.Prizes.append(UpdateIndex(clone));
    PrizeAddInit();
}
$('.Study').click(function () {
    if ($('#Study').is(":visible")) $('#Study').hide();
    else $('#Study').show();
})


//-------------------------------------------------------------------------------------------
var SearchMap = null,
    SearchList = null,
    Clusterer = null,
    Points = [];

function PointProcessing(point) {

}

function AddPointToMap(marker) {
    var Placemark = new ymaps.Placemark(marker.position, {
        iconLayout: 'default#image',
        preset: 'islands#greenDotIconWithCaption',
        iconImageClipRect: [[0, 0], [64, 64]],
        iconImageHref: 'http://гмпотолки.рф/images/point.png',
        iconImageSize: [32, 32],
        iconImageOffset: [-16, -32],
        draggable: true
    });

    Placemark.id = marker.id;

    Placemark.events.add("dragend", (event) => {
        updateInputs(this.geometry.getCoordinates());
    });

    Placemark.id = marker.id;

    Clusterer.add(Placemark);
}

function addMarkers(markers) {
    Clusterer.removeAll();

    markers.each((key, value) => {
        AddPointToMap(value)
    });

    SearchMap.setBounds(Clusterer.getBounds(), {
        checkZoomRange: true,
        zoomMargin: 50
    });

    SearchMap.geoObjects.add(Clusterer);
}

function geocode(element, markers) {
    var HelpMessage = element.siblings(".HelpMessage"),
        Id = element.data("id");

    ymaps.geocode(element.val()).then(function (res) {
        var obj = res.geoObjects.get(0),
            hint;

        if (obj) {
            switch (obj.properties.get('metaDataProperty.GeocoderMetaData.precision')) {
                case 'exact':
                    break;
                case 'number':
                case 'near':
                case 'range':
                    hint = 'Уточните номер дома';
                    break;
                case 'street':
                    hint = 'Уточните номер дома';
                    break;
                case 'other':
                default:
                    hint = 'Уточните адрес';
            }
        } else {
            hint = 'Уточните адрес';
        }

        if (hint) {
            HelpMessage.text(hint);
            HelpMessage.addClass("Show");
        } else {
            HelpMessage.removeClass("Show");
        }

        SearchAddress.val(obj.getAddressLine());

        var Position = obj.geometry.getCoordinates();
        markers.push({ position: Position, id: Id });

    }, function (err) {
        console.log(err);
    });
}

function SearchBlur(element) {
    var elements = $("[id='SearchAddress']"),
        element = $(element);


}

ymaps.ready(initMap);

function initMap() {
    SearchAddress = $("#SearchAddress");

    SearchAddress.focus(function () {
        FlagFocus = true;
    }).blur(function () {
        if (FlagFocus) {
            TextWrite = true;
            Search = SearchAddress.val();
            updateAddress();
        }
    });

    SearchCoordinates = $("#SearchCoordinates");
    HelpMessage = SearchAddress.closest(".InputHelp").find(".HelpMessage");

    SuggestView = new ymaps.SuggestView('SearchAddress');

    SuggestView.events.add(["select"], function (e) {
        Search = SearchAddress.val();
        updateAddress();
    });

    Marker = new ymaps
        .Placemark([51.661535, 39.200287], { balloonContent: null },
        {
            iconLayout: 'default#image',
            preset: 'islands#greenDotIconWithCaption',
            iconImageClipRect: [[0, 0], [64, 64]],
            iconImageHref: 'http://гмпотолки.рф/images/point.png',
            iconImageSize: [32, 32],
            iconImageOffset: [-16, -32],
            draggable: true
        });

    Marker.events
        .add("dragend", function (event) {
            Search = self.Marker.geometry.getCoordinates();
            updateAddress();
        });

    SearchMap = new ymaps.Map('AddressMap', {
        center: [51.661535, 39.200287],
        zoom: 16,
        controls: []
    }, { searchControlProvider: 'yandex#search' });

    SearchMap.events.add('click', function (e) {
        updateAddress();
    });

    SearchMap.geoObjects.add(Marker);
}

function updateAddress() {
    ymaps.geocode(Search).then(function (res) {
        var obj = res.geoObjects.get(0),
            hint;

        if (obj) {
            switch (obj.properties.get('metaDataProperty.GeocoderMetaData.precision')) {
                case 'exact':
                    break;
                case 'number':
                case 'near':
                case 'range':
                    hint = 'Уточните номер дома';
                    break;
                case 'street':
                    hint = 'Уточните номер дома';
                    break;
                case 'other':
                default:
                    hint = 'Уточните адрес';
            }
        } else {
            hint = 'Уточните адрес';
        }

        if (hint) {
            HelpMessage.text(hint);
            HelpMessage.addClass("Show");
        } else {
            HelpMessage.removeClass("Show");
        }

        if (Array.isArray(Search) || TextWrite)
            SearchAddress.val(obj.getAddressLine());

        var Position = obj.geometry.getCoordinates();
        SearchCoordinates.val(JSON.stringify(Position));
        GoCenter(Position);
        TextWrite = FlagFocus = false;
        SearchAddress.blur();
    }, function (err) {
        console.log(err);
    });
}

function GoCenter(Position) {
    Marker.geometry.setCoordinates(Position);
    SearchMap.geoObjects.add(Marker);
    SearchMap.setCenter(Position, SearchMap.getZoom(), { duration: 300 });
}

//------------------------------------------------------------------------------------------------

function PictureLoad() {

    var picture = $('<img class="Picture"/>'),
        block = $(this).siblings(".Pictures");
    $(".Pictures").empty();
    for (var i = 0; i < this.files.length; ++i) {
        var READER = new FileReader();

        READER.addEventListener("load", function (e) {
            var pic = picture.clone();
            pic.attr("src", e.target.result);
            block.append(pic);
        }, false);

        READER.readAsDataURL(this.files[i]);
    }
}

document.querySelector("input").addEventListener("change", function () {
    if (this.files[0]) {
        var fr = new FileReader();

        fr.addEventListener("load", function () {
            document.querySelector("label").style.backgroundImage = "url(" + fr.result + ")";
        }, false);

        fr.readAsDataURL(this.files[0]);
    }
});