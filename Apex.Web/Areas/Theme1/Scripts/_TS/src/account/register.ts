$(function () {});

function registerComplete(res) {
  const result = JSON.parse(res.responseText);
  alerty(result.Message, result.Status ? "success" : "error");
  if (result.Status) setTimeout(() => location.reload(), 2000);
  else recaptcha();
}
