(async () => {
    $(".mvc-grid").mvcgrid();
    CKEDITOR.replace("FullDescription");

    $("#btnShMdlAddProduct").click(function () {
        const mdl = $("#mdlAddProduct");
        mdl.find(".modal-title").text("مطلب جدید");

        mdl.clearFormInputs();

        $("#categoriesContainer").html("");
        $("#pictureContainer").html("");
        $("[href='#tabGeneral']").trigger("click");
        $.buildTagInput();
        mdl.find("#Id").val(0);
        $.buildSelect2();
        mdl.modal();
    });

    bdy.on("click", "[data-editproduct]", async function () {
        const id = $(this).data("editproduct");
        const result = await $.get("/Product/Get", { id });
        if (result.Status) {
            $("#AddProductContainer").setPartialView(result.Data);
            $.buildSelect2();
            $.buildTagInput();
            $.metronicInit();
            CKEDITOR.replace("FullDescription");
            $("#mdlAddProduct").modal();
        }
        else {
            $.alert(result.Status ? "success" : "error", result.Message, 5000);
        }
    });

    bdy.on("click", "[data-delproduct]", function () {
        var id = $(this).data("delproduct");
        $.confirm(`آیا از حذف "${$(this).data("text")}" اطمینان دارید؟`,
            function (res) {
                if (res) {
                    $.post("/Product/Delete", { id }, result => {
                        if (result.Status)
                            $.reloadDetList();
                        $.alert(result.Status ? "success" : "error", result.Message, 5000);
                    });
                }
            });
    });

    //-----------------------------
    bdy.on("click", "#addPicture", function () {
        const htm = $("#pictureTemplate").html();
        const pincon = $("#pictureContainer");
        pincon.append(htm).arrengeCollectionIndex("Pictures", 0, "indic");
        pincon.reArrengeNumber();
    });

    bdy.on("click", "#pictureContainer [del]", function () {
        $(this).oneParent("[indic]").fadeOut(300, function () {
            const $this = $(this);
            $this.remove();
            const pincon = $("#pictureContainer");
            pincon.arrengeCollectionIndex("Pictures", 0, "indic");
            pincon.reArrengeNumber();
        });
    });

    bdy.on("click", "[upload-img]", function () {
        const img = $(this);
        $.uploadFileBase({
            callback: (data) => {
                img.oneParent("[indic]").find(":hidden[id$='__Link']").val(data.FullName);
                img.attr("src", data.FullName);
            }
        });
    });
})();

function addProductComplete(res) {
    const result = JSON.parse(res.responseText);
    if (result.Status) {
        $.reloadDetList();
        $("#mdlAddProduct").modal("hide");
    }
    $.alert(result.Status ? "success" : "error", result.Message, 5000);
}
