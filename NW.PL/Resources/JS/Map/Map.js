var flaguser = true;
function showuser() {
    if (flaguser) $("header .Info").addClass("show");
    else $("header .Info").removeClass("show");
    flaguser = !flaguser;
}