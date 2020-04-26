function resetPasswordConfirm(res) {
    const result = JSON.parse(res.responseText);
    if (result.Status)
        setTimeout(function () { location.reload(); }, 200);
    $.alert(result.Status ? "success" : "error", result.Message, 2000);
}