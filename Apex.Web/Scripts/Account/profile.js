$(function () {
    $.buildSelect2();
    bdy.on("click", "[upload-img]", function () {
        const img = $(this);
        $.uploadFileBase({
            callback: (data) => {
                $(img.attr("upload-img")).val(data.FullName);
                img.attr("src", data.FullName);
            }
        });
    });
});
function profileConfirm(res) {
    const result = JSON.parse(res.responseText);
    $.alert(result.Status ? "success" : "error", result.Message, 5000);
}