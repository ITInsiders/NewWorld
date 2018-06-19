var Data = {};

$(document).ready(function () {
    
    Data.PictureLoad = $("#loadPhoto");
    Data.PictureLoad.change(PictureLoad);
})

function PictureLoad() {
    $('.ModelPhotoSave').show();
    var picture = $(".WindowBlock").find(".Picture"),
        input = $("#loadPhoto").get(0);

    var READER = new FileReader();

    READER.addEventListener("load", function (e) {
        picture.css({
            "background-image": "url('" + e.target.result + "')",
            "background-size": "cover",
            "background-position": "top"
        });
    }, false);

    READER.readAsDataURL(input.files[0]);
}

function HiddenModal() {
    $("#loadPhoto").val('');
    $('.ModelPhotoSave').hide();
}

$("#AddPlace").click(function () {
    document.location.href = "/Update/Place";
})
$("#AddQuest").click(function () {
    document.location.href = "/Update/Quest";
})
$("#MyQuest").click(function () {
    document.location.href = "/Quest/Search/5";
})
$("#MyQuestPlay").click(function () {
    document.location.href = "/Quest/Search/6";
})