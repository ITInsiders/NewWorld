var PageData = {};
PageData.text = "Главная";

initHashChange();

$(window).resize(function () {
    changeMap();
});

$(document).ready(function () {
    changeMap();

    if ($("#IdUser").val() === $("#Creater").val()) {
        $(".Message.Standart").removeClass('Left');
        $(".Message.Standart").addClass('Right');
    }
    console.log(ymaps);
  
})


function SendMessage() {
    var message = $(".MessageInputBlock .MessageInput").val(),
        messageBlock = Data.Message.clone();

    SendMessageServer(message);

    messageBlock.text(message);
    messageBlock.addClass("Right");
    Messages.append(messageBlock);

    $(".MessageInputBlock .MessageInput").val("");
}

function changeMap() {
    if ($(window).width() <= 768) {
        $("#miniMap").append($("#Map"));
    }
    else {
        $("section").before($("#Map"));
    }
}