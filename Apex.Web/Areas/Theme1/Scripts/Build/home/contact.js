$(function () { });
function addShopCommentComplete(res) {
    const result = JSON.parse(res.responseText);
    if (result.Status)
        $(`[data-ajax-complete="addShopCommentComplete"]`).get(0).reset();
    alerty(result.Message, result.Status ? "success" : "error");
    recaptcha();
}
//# sourceMappingURL=contact.js.map