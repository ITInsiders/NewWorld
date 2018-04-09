var myMap = null,
    Clusterer = null;

ymaps.ready(init);
function init() {
    myMap = new ymaps.Map('map', {
        center: [55, 34],
        zoom: 10,
        controls: []
    });

    Clusterer = new ymaps.Clusterer({
        clusterDisableClickZoom: true,
    });

    myMap.geoObjects.add(Clusterer);
}

$(document).ready(Ready);

function Ready() {
    $("#Search").keyup(SearchLines).click(SearchLines);
}

function AddPointToMap(object) {
    var objectMap = new ymaps.Placemark(object.position, {
        balloonContentHeader: "<div class=\"balloonHeader\">" + object.Name + "</div>",
        balloonContentBody: "<div class=\"balloonBody\">" + object.Address + "</div>",
        balloonContentFooter: object.Tags,
        hintContent: object.Name,
        iconCaption: object.Name
    });
    Clusterer.add(objectMap);
}

function SearchLines() {
    var Search = $(this).val(),
        List = $("#SearchList");

    var Data = null;
    $.ajax({
        type: "POST",
        url: "/API/SearchLines",
        data: { Search: Search },
        dataType: "json",
        timeout: 5000,
        async: false,
        success: function (data) {
            Data = data;
        },
        error: function () {
        }
    });

    List.empty();
    $.each(Data, function (i, v) {
        List.append('<option value="' + v.Value + '" label="' + v.Type + '">');
    });
}

function SearchBlocks() {
    var Search = $("#Search").val(),
        SearchResult = $("#SearchResult"),
        Block = $("#Templates .Block");

    var Data = null;
    $.ajax({
        type: "POST",
        url: "/API/SearchBlocks",
        data: { Search: Search },
        dataType: "json",
        timeout: 5000,
        async: false,
        success: function (data) {
            Data = data;
        },
        error: function () {
        }
    });
    
    SearchResult.empty();
    Clusterer.removeAll();
    $.each(Data, function (i, v) {
        v.position = [v.Longitude, v.Latitude];

        setTimeout(AddPointToMap(v), 0);

        var BlockClone = Block.clone(),
            BlockData = BlockClone.find(".Data");

        BlockData.find(".Name").text(v.Name);
        BlockData.find(".Info.Category").text(v.Tags);
        BlockData.find(".Info.Adress").text(v.Address);
        BlockData.find(".Info.Time").text(v.WorkingHour);
        BlockData.find(".Distance").text("");

        BlockData.data("position", JSON.stringify(v.position));

        var mainPhoto = null;

        $.each(v.photos, function (index, element) {
            if (element.Main) mainPhoto = element.SRC;
        });

        BlockClone.find(".Photo").css("background-image", "url(\"" + mainPhoto + "\")");

        SearchResult.append(BlockClone);
    });
    myMap.setBounds(Clusterer.getBounds(), {
        checkZoomRange: true
    });
}