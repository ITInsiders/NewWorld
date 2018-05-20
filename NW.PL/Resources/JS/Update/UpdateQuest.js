var Data = {};

var SearchMap = null,
    Collection = null,
    Position = [51.661535, 39.200287],
    LastPointId = 0,
    ClickTimeout = null,
    StopGeocode = false;

ymaps.ready(() => {
    ymaps.geolocation.get({}).then((e) => {
        Position = e.geoObjects.geometry.getCoordinates()
        if (SearchMap != null) SearchMap.setCenter(Position);
    });

    Collection = new ymaps.GeoObjectCollection({});

    SearchMap = new ymaps.Map('AddressMap', {
        center: Position,
        zoom: 16,
        controls: []
    }, { searchControlProvider: 'yandex#search' });

    SearchMap.geoObjects.add(Collection);

    SearchMap.events.add(["boundschange"], (e) => { Position = SearchMap.getCenter(); });
    SearchMap.events.add(["click"], (e) => { ClickTimeout = setTimeout(() => { Geocode(Collection.get(LastPointId), e.get('coords')); }, 200) });
    SearchMap.events.add(["dblclick"], (e) => { clearTimeout(ClickTimeout); AddPlace(e.get('coords')); });
});

function AddPoint(point) {
    if (point.position == null) point.position = Position;

    var newpoint = new ymaps.Placemark(point.position, { balloonContent: null },
        {
            iconLayout: 'default#image',
            preset: 'islands#greenDotIconWithCaption',
            iconImageClipRect: [[0, 0], [64, 64]],
            iconImageHref: 'http://гмпотолки.рф/images/point.png',
            iconImageSize: [32, 32],
            iconImageOffset: [-16, -32],
            draggable: true
        });
    
    newpoint.address = point.address;
    newpoint.position = point.position;
    point = newpoint;

    point.events.add("dragend", (event) => { Geocode(point, point.geometry.getCoordinates()); });
    Collection.add(point);
    Geocode(point, point.position);
}

function RemovePoint(point) {
    if (typeof point === "number")
        point = Collection.get(point);

    Collection.remove(point);
    SearchMap.setBounds(Collection.getBounds(), {
        checkZoomRange: true,
        zoomMargin: 50
    });
}

function Geocode(point, search) {
    if (StopGeocode) {
        StopGeocode = false;
        return;
    }
    
    LastPointId = Collection.indexOf(point);;

    ymaps.geocode(search).then((res) => {
        var tempPoint = res.geoObjects.get(0), hint = null;

        if (tempPoint) {
            switch (tempPoint.properties.get('metaDataProperty.GeocoderMetaData.precision')) {
                case 'exact':
                    break;
                case 'number':
                case 'near':
                case 'range':
                    hint = 'Уточните номер дома';
                    break;
                case 'street':
                    hint = 'Уточните улицу';
                    break;
                case 'other':
                default:
                    hint = 'Уточните адрес';
                    break;
            }

            point.address = tempPoint.getCountry() + ", " + tempPoint.getAddressLine() + " ";
            point.position = tempPoint.geometry.getCoordinates();
            point.geometry.setCoordinates(point.position);
        }
        else {
            hint = 'Уточните адрес';
        }

        point.help = hint;

        PointProcessing(point);
    }, function (err) {
        console.log(err);
    });
}

function SearchBlur(element) {
    element = $(element);
    var id = parseInt(element.closest(".Address").data("id"));
    setTimeout(() => { Geocode(Collection.get(id), element.val()); }, 500);
}
function SearchFocus(element) {
    LastFocusId = parseInt($(element).closest(".Address").data("id"));
}

function PointProcessing(point) {
    SearchMap.setBounds(Collection.getBounds(), {
        checkZoomRange: true,
        zoomMargin: 50
    });

    var object = $("#Address_" + Collection.indexOf(point));

    object.find(".ILatitude").val(point.position[0]);
    object.find(".ILongitude").val(point.position[1]);

    var IAddress = object.find(".IAddress");
    if (IAddress.val() != point.address)
        IAddress.val(point.address);
        

    if (point.help)
        object.find(".IHint").text(point.help).addClass("Show");
    else {
        object.find(".IHint").removeClass("Show");
        var ITask = object.find(".ITask");
        setTimeout(() => { ITask.focus(); }, 500);
    }
}

function AddSuggest(element) {
    SuggestView = new ymaps.SuggestView(element.attr("id"));
}

//-------------------------------------------------------------------------------------------------------------------------

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
        inputs = element.find("input, textarea, datalist"),
        index = 0;      

    elements.each((ind, elem) => {
        var name = $(elem).find("input, textarea").attr("name"),
            index = matchRegex(regex, name);
        indexes.push(index[0]);
    });

    while (indexes.indexOf(index) != -1) index += 1;

    inputs.each((ind, elem) => {
        if ($(elem).attr("name")) {
            var name = $(elem).attr("name").replace(regex, "[" + index + "]");
            $(elem).attr("name", name);
        }

        if ($(elem).hasClass("IAddress"))
            $(elem).attr("id", "SearchAddress_" + index);

        if ($(elem).hasClass("IDataList"))
            $(elem).attr("id", "SearchDataList_" + index);
    });

    if (element.hasClass("Address")) {
        element.attr("id", "Address_" + index);
        element.data("id", index);
    }

    return element;
}

function ShowMap() {
    var map = $(this).closest(".form-group").find(".map");
    if (map.is(":hidden"))
        map.addClass("active");
    else
        map.removeClass("active");
}

function RemoveAddress(element) {
    var count = Collection.toArray().length;
    if (count > 1) {
        element = $(element).closest(".Address");
        var index = element.data("id");

        for (var i = index; i < count; ++i) {
            var temp = $("#Address_" + (i + 1));
            if (temp != null)
                setIndex(temp, i);
        }

        RemovePoint(index);
        element.remove();
    }
}

function setIndex(element, index) {
    const regex = /\[(\d+)\]/gim;
    var inputs = element.find("input, textarea, datalist");

    inputs.each((ind, elem) => {
        if ($(elem).attr("name")) {
            var name = $(elem).attr("name").replace(regex, "[" + index + "]");
            $(elem).attr("name", name);
        }

        if ($(elem).hasClass("IAddress"))
            $(elem).attr("id", "SearchAddress_" + index);

        if ($(elem).hasClass("IDataList"))
            $(elem).attr("id", "SearchDataList_" + index);
    });

    if (element.hasClass("Address")) {
        element.attr("id", "Address_" + index);
        element.data("id", index);
    }
}

function AddPlace(coord = null) {
    var clone = Data.Addresses.Adr.clone();
    var object = UpdateIndex(clone);
    Data.Addresses.append(object);

    AddSuggest(object.find(".IAddress"));
    AddPoint({ id: parseInt(object.data("id")), position: (coord === null ? Position : coord), address: "" });
}

function AddressAdd() {
    if (SearchMap === null) return setTimeout(() => { AddressAdd(); }, 100);
    else AddPlace();
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