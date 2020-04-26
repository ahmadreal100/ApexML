$(function () {
    $(".mvc-grid").mvcgrid();

    $("#btnShMdlAddSlider").click(function () {
        const mdl = $("#mdlAddSlider");
        mdl.find(".modal-title").text("اسلایدر جدید");
        mdl.clearFormInputs();
        $("#pictureContainer").html("");
        mdl.find("#Id").val(0);
        mdl.modal();
    });

    bdy.on("click", "[data-editslider]", function () {
        const id = $(this).data("editslider");
        $.get("/Slider/Get", { id },
            function (result) {
                if (result.Status) {
                    $("#AddSliderContainer").setPartialView(result.Data);
                    $.buildSelect2();
                    $("#mdlAddSlider").modal();
                }
                else {
                    $.alert(result.Status ? "success" : "error", result.Message, 5000);
                }
            });
    });

    bdy.on("click", "[data-delslider]", function () {
        var id = $(this).data("delslider");
        $.confirm(`آیا از حذف "${$(this).data("text")}" اطمینان دارید؟`,
            function (res) {
                if (res) {
                    $.post("/Slider/Delete", { id }, function (result) {
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
            if ($this.attr("fromser") === undefined)
                $this.remove();
            else
                $this.find("[id$='__Link']").val(null);
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
});

function addSliderComplete(res) {
    const result = JSON.parse(res.responseText);
    if (result.Status) {
        $.reloadDetList();
        $("#mdlAddSlider").modal("hide");
    }
    $.alert(result.Status ? "success" : "error", result.Message, 5000);
}
