$(function () {
    //const ownerIdAdd = $("#addOperatorForm #OwnerId").val();
    //const ownerIdEdit = $("#editOperatorForm #OwnerId").val();
    $(".mvc-grid").mvcgrid();
    $("#btnShMdlAddOperator").click(function () {
        const mdl = $("#mdlAddOperator");

        mdl.clearFormInputs();
        $.buildSelect2();

        mdl.find("#addOperatorForm #Id").val(0);
        $("#addOperatorForm #UserName").trigger("change");
        $(".hide").removeClass("hide");
        $("#Type").trigger("change");

        mdl.modal();
    });

    bdy.on("click", "[data-editoperator]", function () {
        const id = $(this).data("editoperator");
        $.get("/Operator/Get", { id },
            function (result) {
                if (result.Status) {
                    $("#AddOperatorContainer").setPartialView(result.Data);
                    $.buildSelect2();
                    $("#mdlAddOperator").modal();
                }
                else
                    $.alert(result.Status ? "success" : "error", result.Message, 5000);
            });
    });

    bdy.on("click", "[data-deloperator]", function () {
        const id = $(this).data("deloperator");
        $.confirm(`آیا از حذف "${$(this).data("text")}" اطمینان دارید؟`,
            function (res) {
                if (res) {
                    $.post("/Operator/Delete", { id }, function (result) {
                        if (result.Status)
                            $.reloadDetList();
                        $.alert(result.Status ? "success" : "error", result.Message, 5000);
                    });
                }
            });
    });

    //=================
    bdy.on("click", "[miniform-opener]", function () {
        $(".miniForm").removeClass("open");
        $(this).next(".miniForm").addClass("open").find(":text").val(null).focus();
    });

    bdy.on("keydown", ".miniForm :text", function (e) {
        if (e.key === "Enter") {
            $(this).siblings("[per-ok]").trigger("click");
        } else if (e.key === "Escape") {
            $(this).siblings("[per-cancel]").trigger("click");
        }
        return true;
    });

    bdy.on("click", ".miniForm [per-ok]", function () {
        var tthis = $(this);
        const id = tthis.data("id");
        const pass = tthis.siblings("input").val();
        if (pass.length < 6)
            $.alert("error", "حداقل تعداد کاراکترهای رمز عبور 6 کاراکتر میباشد.", 5000);
        else
            $.post("/Operator/ChangePassword", $.addAntiForgeryToken({ id: id, password: pass }), function (result) {
                if (result.Status)
                    tthis.oneParent(".miniForm").removeClass("open");

                $.alert(result.Status ? "success" : "error", result.Message, 5000);
            });
    });

    bdy.on("click", ".miniForm [per-cancel]", function () {
        $(this).oneParent(".miniForm").removeClass("open");
    });

    bdy.on("click", "[miniform-opener]", function () {
        $(this).next("miniForm").addClass("open");
    });
    //=================
});
function addOperatorConfirm(res) {
    const result = JSON.parse(res.responseText);
    if (result.Status) {
        $.reloadDetList();
        $(".modal").modal("hide");
    }
    $.alert(result.Status ? "success" : "error", result.Message, 5000);
}