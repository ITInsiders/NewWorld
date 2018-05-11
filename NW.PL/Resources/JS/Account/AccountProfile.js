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



/*document.querySelector("input").addEventListener("change", function () {
    if (this.files[0]) {
        var fr = new FileReader();

        fr.addEventListener("load", function () {
            document.querySelector("label").style.backgroundImage = "url(" + fr.result + ")";
        }, false);

        fr.readAsDataURL(this.files[0]);
    }
});*/