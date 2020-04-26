$(function () {
    $("body").on("change", "[menuinput]:checkbox,[submenuinput]:checkbox", function () {
        const tthis = $(this);
        if (!tthis.hasAttr("submenuinput"))
            tthis.oneParent("[indic-li]").find("[menuinput]:checkbox,[submenuinput]:checkbox").prop("checked", tthis.prop("checked"));
        checkParent(tthis);
    });

    $("#checkAll").change(function () {
        $("[indic-li] [menuinput]:checkbox,[indic-li] [submenuinput]:checkbox").prop("checked", $(this).prop("checked"));
    });

    function checkParent(tthis) {

        const pIndic = tthis.oneParent("[indic-li]");
        if (tthis.prop("checked")) {
            pIndic.parents("[indic-li]").children(".ar-checkbox").children(":checkbox").prop("checked", true);
        }
        else
            if (!tthis.hasAttr("submenuinput") && !pIndic.siblings().children(".ar-checkbox").children(":checkbox:checked").length)
                pIndic.oneParent("[indic-li]").children(".ar-checkbox").children(":checkbox").prop("checked", false).trigger("change");
    }
});

function permissionConfirm(res) {
    const result = JSON.parse(res.responseText);
    $.alert(result.Status ? "success" : "error", result.Message, 5000);
}
