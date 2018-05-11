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
        synchAdd: true
    });
   /* readObjects();*/
    SearchBlocks();
}

/*function readObjects() {
    var objects = $(".jsonObject");
    for (var i = 0; i < 10 || i < objects.length; ++i) {
        var json = JSON.parse(objects[i].val());
        var objectMap = new ymaps.Placemark([json.Latitude, json.Longitude], {
            balloonContentHeader: "<div class=\"balloonHeader\">" + json.Name + "</div>",
            balloonContentBody: "<div class=\"balloonBody\">" + json.Address + "</div>",
            balloonContentFooter: json.Tags,
            hintContent: json.Name,
            iconCaption: json.Name
        });
        Clusterer.add(objectMap);
    }
}*/

$(document).ready(Ready);

function Ready() {
    $("#Search").keyup(SearchLines).click(SearchLines);
}

function AddPointToMap(object) {
    var objectMap = new ymaps.Placemark(object.position, {
        balloonContentHeader: "<div class=\"balloonHeader\">" + object.Name + "</div>",
        balloonContentBody: "<div class=\"balloonBody\">" + object.Description + "</div>",
        balloonContentFooter: "<div class=\"balloonFooter\" style=\"text-align: right; color: #777;\">" + object.Address + "</div>",
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
        url: "/Map/SearchLines",
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
        Block = $("#Templates .Block.Empty"),
        BlockNull = $("#Templates .Block.Null");

    var Data = null;
    $.ajax({
        type: "POST",
        url: "/Map/SearchBlocks",
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
    if (Data.length === 0) {
        SearchResult.append(BlockNull.clone());
    }
    else {
        $.each(Data, function (i, v) {
            v.position = [v.Longitude, v.Latitude];

            AddPointToMap(v);

            var BlockClone = Block.clone(),
                BlockData = BlockClone.find(".Data");

            BlockClone.attr("href", "InformPlace/" + v.Id);
            BlockClone.find(".Photo").css({ "background-image": "url('" + v.MainPhoto + "')"});
            BlockClone.find(".Photo .Ratings").text(v.Rating);

            BlockData.find(".Name").text(v.Name);
            BlockData.find(".Info.Category").text(v.Tags);
            BlockData.find(".Info.Adress").text(v.Address);
            BlockData.find(".Info.Time").text(v.WorkingHour);
            BlockData.find(".Distance").text("");

            BlockData.data("position", JSON.stringify(v.position));
            console.log(v.position);

            SearchResult.append(BlockClone);
        });
    }

    myMap.setBounds(Clusterer.getBounds(), {
        checkZoomRange: true,
        zoomMargin: 50
    });

    myMap.geoObjects.add(Clusterer);
}