const doc = $(document);
const win = $(window);
const appVal = $("#appVal");
const csLang = `${appVal.data("lang")}`;
const csSearch = new URL(location.href).searchParams;
function fixUrl(url) {
    url = url || "";
    if (/^http/g.test(url))
        return url;
    return url
        .replace(/^(\/\w{2}(?=$|\/|\?))?/g, `/${csLang}/`)
        .replace(/\/+/g, "/");
}
doc.ajaxSend((_, __, options) => {
    options.url = fixUrl(options.url);
    GL.showForce();
});
doc.ajaxStop(() => {
    GL.hideForce();
});
$(function () {
    doc.on("input change", `textarea,
    :text,
    :password,
  input[type="date"],
  input[type="datetime-local"],
  input[type="email"],
  input[type="month"],
  input[type="number"],
  input[type="search"],
  input[type="text"],
  input[type="time"],
  input[type="url"],
  input[type="week"]`, function () {
        this.value = this.value
            .replace(/۰|٠/g, "0")
            .replace(/۱|١/g, "1")
            .replace(/۲|٢/g, "2")
            .replace(/۳|٣/g, "3")
            .replace(/۴|٤/g, "4")
            .replace(/۵|٥/g, "5")
            .replace(/۶|٦/g, "6")
            .replace(/۷|٧/g, "7")
            .replace(/۸|٨/g, "8")
            .replace(/۹|٩/g, "9");
    });
});
String.prototype.capitalize = function () {
    const str = this || "";
    return `${str[0] || ""}${str.substring(1)}`;
};
String.prototype.ft = function (...items) {
    var s = this.toString();
    if (s.includes("{")) {
        for (let i = 0; i < items.length; i++)
            s = s.replace(`{${i}}`, items[0].toString());
    }
    else
        s = `${s} ${items.join(" ")}`;
    return s.trim().capitalize();
};
Number.prototype.thousand = function (decimal = 2) {
    const parts = this.toString().split(".");
    if (parts[1])
        parts[1] = parts[1]
            .substring(0, decimal > parts[1].length ? parts[1].length : decimal)
            .replace(/0+$/, "");
    if (decimal === 0 || !parts[1])
        parts.splice(1, 1);
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parseFloatForce(this.toString(), 0) < 0
        ? `(${parts.join(".").replace("-", "")})`
        : parts.join(".");
};
const Toast = Swal.mixin({
    toast: true,
    position: "bottom",
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    onOpen: (toast) => {
        toast.addEventListener("mouseenter", Swal.stopTimer);
        toast.addEventListener("mouseleave", Swal.resumeTimer);
    },
});
function parseFloatForce(obj, out, toInt = false) {
    var _a;
    var str = (((_a = obj) === null || _a === void 0 ? void 0 : _a.toString()) || "").replace(/[^0-9+-\.]/gi, "");
    if (toInt)
        str = str.replace(/\./gi, "");
    return (toInt ? parseInt(str) : parseFloat(str)) || out;
}
function parseIntForce(obj, out) {
    return parseFloatForce(obj, out, true);
}
function alerty(msg, type, duration = 5) {
    Toast.fire({
        toast: true,
        showConfirmButton: false,
        icon: type,
        title: " ",
        text: msg,
        timer: duration * 1000,
        timerProgressBar: true,
    });
}
function recaptcha() {
    const img = $(".captcha-box img");
    const v = parseIntForce((img.attr("src").match(/\d+$/g) || [""])[0], 0) + 1;
    img.attr("src", img.attr("src").replace(/\d+$/g, v.toString()));
}
setTimeout(() => {
    if ($("#ArCaptcha").length)
        $("#ArCaptcha").rules("add", {
            messages: { required: Str.rq.ft(Str.securityPicture) },
        });
}, 100);
function getRange(from, to) {
    const ns = [];
    for (let i = from; i <= to; i++)
        ns.push(i);
    return ns;
}
class Loading {
    constructor(selector, active = false, du = 200) {
        this.el = $(selector);
        this.active = active;
        this.du = du;
    }
    show(du = this.du) {
        this.active = true;
        this.el.fadeIn(du);
    }
    showForce(du = this.du) {
        this.active = true;
        this.force = true;
        this.el.fadeIn(du);
    }
    hide(du = this.du) {
        if (!this.force) {
            this.active = false;
            this.el.fadeOut(du);
        }
    }
    hideForce(du = this.du) {
        this.active = false;
        this.force = false;
        this.el.fadeOut(du);
    }
}
const GL = new Loading("#gLoading", true);
function imgAlt(el, url) {
    if (el.src.toLowerCase() !== url.toLowerCase()) {
        el.src = url;
        el.removeAttribute("onerror");
    }
}
//# sourceMappingURL=helpers.js.map