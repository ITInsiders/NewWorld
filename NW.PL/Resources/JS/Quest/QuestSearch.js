var PageData = {};
PageData.text = "Поиск";

initHashChange();

$(document).ready(Ready);

function Ready() {
    SearchBlocks();
    $("#Search").keyup(SearchLines).click(SearchLines);
   
}

function SearchLines() {
    var Search = $(this).val(),
        List = $("#SearchList");
    var id = $("#id").val();
    var Data = null;
    $.ajax({
        type: "POST",
        url: "/Quest/SearchLines",
        data: { Search: Search, id: id },
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
    var id = $("#id").val();
    var Data = null;

    $.ajax({
        type: "POST",
        url: "/Quest/SearchBlocks",
        data: { Search: Search, id: id },
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
    if (Data.length === 0) {
        SearchResult.append(BlockNull.clone());
    }
    else {
        $.each(Data, function (i, v) {
            var BlockClone = Block.clone();

            BlockClone.attr("href", "/Quest/InformQuest/" + v.Id);
            BlockClone.find(".ImgBlock").css({ "background-image": "url('" + v.MainPhoto + "')" });
            BlockClone.find(".ImgBlock .Title").text(v.Name);
            

            SearchResult.append(BlockClone);
        });
    }
}