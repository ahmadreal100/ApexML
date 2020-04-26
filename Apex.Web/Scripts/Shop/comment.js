$(function () {
    $("#commentGrid").mvcgrid();

    bdy.on("click", "[data-showcomment]", function () {
        const id = $(this).data("showcomment");
        $.get("/Comment/Get", { id: id, show: true },
            function (result) {
                if (result.Status) {
                    console.log(result.Data);
                    $("#ShowCommentContainer").setPartialView(result.Data);
                    $("#mdlShowComment").modal();
                    modifySeen();
                }
                else
                    $.alert(result.Status ? "success" : "error", result.Message, 5000);
            });
    });

    bdy.on("click", "[data-delcomment]", function () {
        var id = $(this).data("delcomment");
        $.confirm(`آیا از حذف "${$(this).data("text")}" اطمینان دارید؟`,
            function (res) {
                if (res) {
                    $.post("/Comment/Delete", { id }, function (result) {
                        if (result.Status)
                            $.reloadDetList("#commentGrid");
                        $.alert(result.Status ? "success" : "error", result.Message, 5000);
                    });
                }
            });
    });

    function modifySeen() {
        const cm = $("#cominfo");

        if (!cm.data("seen")) {
            $.post("/Comment/Seen", { id: cm.data("id"), seen: true }, function (result) {
                if (result.Status)
                    $("#commentGrid").mvcgrid({ reload: true });
                const badge = $("#commentsCount");
                if (!$.isNEU(badge)) {
                    const cnt = parseIntForce(badge.text(), 0);
                    if (cnt === 1)
                        badge.remove();
                    else
                        badge.text(cnt - 1);
                }
            });
        }
    };
});