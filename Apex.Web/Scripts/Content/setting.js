$(function () {
    //https://ckeditor.com/latest/samples/toolbarconfigurator/index.html#basic
    CKEDITOR.replace("ThemeSetting_HtmlContent");
    CKEDITOR.replace("ThemeSetting_FullDescription");

    CKEDITOR.replace("ThemeSetting_PrivacyText");
    //CKEDITOR.replace("ThemeSetting_LawsText");
});

function updateComplete(res) {
    const result = JSON.parse(res.responseText);
    $.alert(result.Status ? "success" : "error", result.Message, 5000);
}
