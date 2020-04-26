var bdy = $("body");
$(function () {
    setArvalMin();

    if (!$.isNEU($.validator)) {
        $.validator.methods.range = function (value, element, param) {
            const globalizedValue = value.replace(",", ".");
            return this.optional(element) || globalizedValue >= param[0] && globalizedValue <= param[1];
        };

        $.validator.methods.number = function (value, element) {
            return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
        };
    }

    $("[arval*='digit']").not("[arval*='nocomma']").removeAttr("data-val-number");

    $("input[type='checkbox'][check]").prop("checked", true);
    $("input[type='checkbox'][uncheck]").prop("checked", false);

    bdy.on("show.bs.modal", ".modal", function () {

        $.fillCurrentDate($(this));
        $.addRequiredRedStar();
        setArvalMin();
        $.buildBTooltip();
        const form = $(this).find("form").first();
        try {
            form.valid().element("[data-val-remote]");
        } catch (e) {
            //console.log( );
        }
        form.find(":input[data-val],select[data-val]").removeClass("input-validation-error");
        form.find("span[data-valmsg-for]").addClass("field-validation-valid").removeClass("field-validation-error").html("");
        $.resetFormValidation(form);
    });
    bdy.on("shown.bs.modal", ".modal", function () {
        $(document).off("focusin.modal");
        $(this).find("[data-val-remote]").each(function () {
            $(this).trigger("blur");
        });

    });



    bdy.on("keydown", "[prevent-key]", function (e) {
        return $.isNEU($(this).attr("prevent-key"), "13").split("|").indexOf(e.keyCode.toString()) === -1;
    });

    function setArvalMin() {
        $("input[arvalmin]").each(function (i, e) {
            if ($.isNEU($(e).val()))
                $(e).val($.isNEU($(e).attr("arvalmin"), 0));
        });
    }
    /*------------------------- BEGIN arval----------------------*/
    //arval=persian|english|digit
    bdy.on("change keyup input paste", "[arval*='digit']", function (e) {
        if (!$.isNEU(e.key)) {
            const input = $(this);
            const arvalStr = input.attr("arval");
            const nodot = /nodot/g.test(arvalStr);
            const nocomma = /nocomma/g.test(arvalStr);
            const nofill = /nofill/g.test(arvalStr);
            const digstr = /digstr/g.test(arvalStr);
            let val = input.val();
            if (digstr) {
                input.val(val.replace(new RegExp("\\D"), ""));
                return;
            }

            const before = !nodot && /^\.\d*$/g.test(val);
            const after = !nodot && /^[\d,]+\.$/g.test(val);
            val = nodot ? parseIntForce(val, nofill ? "" : 0, true) : parseFloatForce(val, nofill ? "" : 0, true);

            if (!nofill) {
                if (input.hasAttr("arvalmin")) {
                    const min = nodot ? parseIntForce(input.attr("arvalmin"), 0) : parseFloatForce(input.attr("arvalmin"), 0);
                    val = val < min ? min : val;
                }
            }

            if (input.hasAttr("arvalmax")) {
                const max = nodot ? parseIntForce(input.attr("arvalmax"), 0) : parseFloatForce(input.attr("arvalmax"), 0);
                val = val > max ? max : val;
            }

            val = nocomma ? val : val.thousandComma();
            input.val(`${before ? "0." : val}${after ? "." : ""}`);

        }
    });

    bdy.on("blur", "[arval*='digit']", function () {
        const input = $(this);
        input.val(input.val().replace(/\.$/g, "").replace(/^(\.)/g, "0$1")).trigger("change");
    });

    bdy.on("change keyup input paste", "[arval*='persian']", function () {
        $(this).val($(this).val().replace(new RegExp("[^اًآكيةپچجحخهعغفقثصضشسیبلاتنمکگوئدذرزطظژؤإأءًٌٍَُِّ\\s\\n\\r\\t\\d\(\)\[\]\{\}.,،;\-؛]", "gim"), ""));
    });

    bdy.on("change keyup input paste", "[arval*='english']", function () {
        $(this).val($(this).val().replace(new RegExp("[^A-Za-z0-9 !@#$%^&*()_+-|,./\]", "gim"), ""));
    });

    /*------------------------- End arval------------------------*/

    //---------------BeginForm with Ajax------
    bdy.on("submit", "form[ajax-oncomplete],form[ajax-onbeforesend]", function () {
        try {
            var $this = $(this);
            $.ajax({
                url: $this.attr("action"),
                type: $this.attr("method"),
                data: new FormData($this.get(0)),
                async: false,
                processData: false,
                contentType: false,
                beforeSend: function (xhr, settings) {
                    const callback = $this.attr("ajax-onbeforesend");
                    if (!$.isNEU(callback) && isFunction(callback))
                        window[callback](xhr, settings);
                },
                complete: function (xhr) {
                    const callback = $this.attr("ajax-oncomplete");
                    if (!$.isNEU(callback) && isFunction(callback))
                        window[callback](xhr);
                }
            });
            return false;
        } catch (e) {
            console.log(e);
            return false;
        }
    });


    bdy.on("click", "[kick]", function () {
        const $this = $(this);
        const arr = $this.attr("kick").split("|");
        if ($(arr[1]).length) {
            eval(`$('${arr[1]}').${$.isNEU(arr[0], "toggle")}(${parseIntForce(arr[2], 500)})`);
            const callback = $this.attr("kick-callback");
            if (!$.isNEU(callback))
                if (isFunction(callback))
                    window[callback](xhr, settings);
                else
                    eval(callback);
        }
    });
});

$.tooltip = function (selector, options) {
    options = $.isNEU(options,
        {
            animation: "grow",
            delay: 100,
            theme: "tooltipster-punk",
            maxWidth: 500
        });

    if ($.type(selector) === "object")
        // ReSharper disable once UnknownCssClass
        $(selector).removeClass("tooltipstered").tooltipster(options);
    else
        $(selector).tooltipster(options);
};

$.fn.reArrengeNumber = function (startFrom) {
    var sf = 0;
    if ($.isNumeric(startFrom))
        sf = startFrom;
    else
        if ($.isPagingQuery(startFrom)) {
            const num = parseInt(startFrom.match(/page=\d+(?=&rows=\d+\b)/gim)[0].replace(/\D/gim, ""));
            const size = parseInt(startFrom.match(/&rows=\d+\b/gim)[0].replace(/\D/gim, ""));
            sf = (num - 1) * size;
        }

    $(this).each(function (tIdx, tElem) {
        $(tElem).find("tr>td[no],tr>td.ar-rear-no").each(function (idx, el) {
            $(el).html(idx + 1 + sf);
        });
    });
};

$.alert = function (type, message, duration) {
    try {
        duration = (duration || 3000) / 1000;
        // ReSharper disable once UseOfImplicitGlobalInFunctionScope
        alertify.set("notifier", "position", "bottom-left");
        // ReSharper disable once UseOfImplicitGlobalInFunctionScope
        alertify.notify(message, type, duration);
        const div = $("body .ajs-message").last();
        div.html(`<small></small><span>${div.html()}</span>`);
        const ld = div.oneChild("small");
        ld.width(div.innerWidth());
        setInterval(() => {
            ld.width(ld.width() - 1);
        }, duration * 1000 / ld.width() - 0.35);
    } catch (e) {
        //
    }
};

$.alertAfterReload = function (message) {
    $.setCookie("alert", message);
    location.reload();
};

$.confirm = function (message, callback) {

    try {
        // ReSharper disable once UseOfImplicitGlobalInFunctionScope
        bootbox.confirm({
            title: "توجه !",
            message: message,
            buttons: {
                confirm: {
                    label: "بله",
                    className: "btn-danger col-xs-offset-1 col-xs-5"
                },
                cancel: {
                    label: "خیر",
                    className: "btn-default col-xs-offset-1 col-xs-5"
                }
            },
            callback: callback
        });
    } catch (e) {
        console.log(e);
    }
};

$.isPagingQuery = function (str) {
    if ($.isNEU(str) || typeof str !== "string") return false;
    return !$.isNEU(str.match(new RegExp("page=\\d{1,}&rows=\\d{1,}", "gim")));
};

$.isNEU = function (obj, out) {
    if (obj === null || $.type(obj) === $.type(undefined) || obj === "")
        return arguments.length === 1 ? true : out;
    return arguments.length === 1 ? false : obj;
};


$.calcNestedElementsSize = function (outerWidth, outerHeight, innerWidth, innerHeight) {
    const rg = outerWidth / outerHeight;
    const rs = innerWidth / innerHeight;
    if (rg > rs) {
        innerHeight = outerHeight;
        innerWidth = innerHeight * rs;
    } else {
        innerWidth = outerWidth;
        innerHeight = innerWidth / rs;
    }
    return { width: innerWidth, height: innerHeight };
};

$.calcRelativeSelectionArea = function (originalSize, containerSize, selectionArea) {
    const wr = originalSize.width / containerSize.width;
    const hr = originalSize.height / containerSize.height;

    return {
        x: selectionArea.x * wr,
        y: selectionArea.y * hr,
        width: selectionArea.width * wr,
        height: selectionArea.height * hr
    };
};


$.fn.serializeJson = function () {
    const $this = $(this);
    var jsonObj = $this.serializeToJSON();
    $this.find("[type='file']").each(function () {
        var ipt = this;
        $.convertToFileBase($(ipt)[0].files[0], function (filebase) { jsonObj[ipt.name + "FileBase"] = filebase });
    });
    return jsonObj;
};
$.convertToBase64 = function (file, callback) {
    if ($.isNEU(file)) {
        callback(null);
    } else {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            callback(reader.result);
        };
        reader.onerror = function () {
            callback(null);
        };
    }
};
$.convertToFileBase = function (file, callback) {
    if ($.isNEU(file)) {
        callback(null);
    } else {
        $.convertToBase64(file,
            function (base64) {
                if ($.isNEU(base64))
                    callback(null);
                else
                    callback({
                        FileName: file.name,
                        ContentType: file.type,
                        ContentLength: file.size,
                        Content: base64.replace(/data:(.*?),/gim, "")
                    });
            });
    }
};
window.parseIntForce = function (obj, out, force) {
    if ($.isNumeric(obj))
        return parseInt(obj);
    else {
        const trueNum = $.isNEU(obj, force || false ? out : "0").toString().replace(/(\..+)|(\D)/gim, "");
        return $.isNumeric(trueNum) ? parseInt(trueNum) : out;
    }
};
window.parseFloatForce = function (obj, out, force) {
    if ($.isNumeric(obj))
        return parseFloat(obj);
    else {
        const trueNum = $.isNEU(obj, force || false ? out : "0").toString().replace(/[^0-9.]|\.(?=.*\.)/gim, "");
        return $.isNumeric(trueNum) ? parseFloat(trueNum) : out;
    }
};

window.isInt = function (obj) {
    return obj.toString().match(/\D/gi) === null;
};
$.handleBarsHelper = function (templateSelector, dataSource) {

    const source = $(templateSelector).html();
    const template = Handlebars.compile(source);
    const context = $.isArray(dataSource) ? { Items: dataSource } : dataSource;
    return template(context);
};

//$.fn.setFormModel = function (model) {
//    $(this).find(":text,:checkbox:hidden,select");
//}

$.fn.clearFormInputs = function (excludedIdsArray, clHidden) {
    clHidden = $.isNEU(clHidden, true);
    $(this).find(`:text,:password,${clHidden ? "input[type=hidden]," : ""}textarea,select`)
        .not("[name='__RequestVerificationToken'],[type='radio'],[type='checkbox'],[noclear]").toArray().forEach((elem) => {
            if ($.isNEU(excludedIdsArray) || !excludedIdsArray.some(x => $(elem)[0].matches(x))) {
                if ($(elem).prop("tagName").toLowerCase() === "select")
                    $(elem).selectFirst();
                else
                    $(elem).val("");
            }
        });
};

$.resetCustomInputs = function (selector, excludedIdsArray) {
    $(selector).not("[name='__RequestVerificationToken'],[type='radio'],[type='checkbox']").each(function (idx, elem) {
        if ($.isNEU(excludedIdsArray) || excludedIdsArray.indexOf($(elem).attr("id")) === -1) {
            if ($(elem).prop("tagName").toLowerCase() === "select")
                $(elem).selectFirst();
            else
                $(elem).val("");
        }
    });
};
/**
 * str should be like "class1 class2". class1 will replace with class2.
 * @param {any} str string name of calsses
 */
$.fn.toggleClass2 = function (str) {
    const obj = $(this);
    const cls = str.split(" ");
    if (cls.length === 2) {
        if (obj.hasClass(cls[0]))
            obj.attr("class", obj.attr("class").replace(new RegExp(cls[0], "gim"), $.isNEU(cls[1], "")));
        else
            if (obj.hasClass(cls[1]))
                obj.attr("class", obj.attr("class").replace(new RegExp(cls[1], "gim"), $.isNEU(cls[0], "")));
            else
                obj.addClass($.isNEU(cls[1], ""));
    }
};

$.fn.oneParent = function (selector) {
    return $(this).parents(selector).first();
};

$.fn.oneChild = function (selector) {
    return $(this).children(selector).first();
};

$.fn.selectFirst = function (orIndex, orValue, orText) {
    orIndex = $.isNEU(orIndex, 0);
    if (arguments.length === 2)
        $(this).find("option").attr("selected", false).filter(`[value='${orValue}']`).attr("selected", true);
    else
        if (arguments.length === 3)
            $(this).find("option").attr("selected", false).filter(function () { return $(this).text().toLowerCase() === orText; }).attr("selected", true);
        else
            $(this).find("option").attr("selected", false).eq(orIndex).attr("selected", true);
};

$.fn.valThenRemove = function () {
    if ($(this).length) {
        const val = $(this).val();
        $(this).remove();
        return val;
    }
    return null;
};

$.fn.selectedText = function () {
    return $(this).find("option:selected").text();
};

$.fn.hasAttr = function (attr) {
    return $(this).attr(attr) !== undefined;
};

$.isValidHtmlTagName = function (string) {
    return document.createElement(string).toString() !== "[object HTMLUnknownElement]";
};

$.resetFormValidation = function (selector) {
    selector = typeof selector === "string" ? $(selector) : selector;
    selector.removeData("validator");
    selector.removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse(selector);
};

$.addRequiredRedStar = function (selector) {
    const reqIndic = "<span req-indic>*</span>";
    $($.isNEU(selector) ? "[data-val='true'][data-val-required]" : selector).not(":checkbox,[data-val-required*='field']").each(function () {
        const lbl = $(this).oneParent("form").find(`[for='${this.id}']`);
        if (!lbl.find("[req-indic]").length)
            lbl.append(reqIndic);
    });
};

//========================Cookie=====================
$.setCookie = function (cname, cvalue, exSeconds) {
    if (cvalue === null || cvalue === undefined)
        return;
    const d = new Date();
    exSeconds = exSeconds === undefined ? 1800 : exSeconds;
    d.setTime(d.getTime() + (exSeconds * 1000));
    const expires = `expires=${d.toUTCString()}`;
    document.cookie = cname + "=" + encodeURIComponent(cvalue) + ";" + expires + ";path=/";
};

$.getCookie = function (cname) {
    const name = cname + "=";
    const decodedCookie = decodeURIComponent(document.cookie);
    const m = decodedCookie.match(new RegExp(`${name}[^;]+(?=(;|))`, "gim"));
    return m === null || !m.length ? null : decodeURIComponent(m[0].replace(name, ""));
};

$.removeCookie = function (name) {
    document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:01 GMT;";
};
//========================/Cookie====================

//========================Storage=====================
$.setStorage = function (key, value) {
    localStorage.setItem(key, value);
};

$.getStorage = function (key) {
    return localStorage.getItem(key);
};

$.removeStorage = function (key) {
    localStorage.removeItem(key);
};
//========================/Storage====================

/********************************************/
// ReSharper disable once NativeTypePrototypeExtending
Array.prototype.last = function () {
    return this[this.length - 1];
};

// ReSharper disable once NativeTypePrototypeExtending
Array.prototype.hasDuplicate = function () {
    var breakException = {};
    try {
        var temp = [];
        this.forEach(function (i) {
            const s = JSON.stringify(i);
            if (temp.indexOf(s) === -1)
                temp.push(s);
            else
                // ReSharper disable once UseOfImplicitGlobalInFunctionScope
                throw breakException;
        });
        return false;
    } catch (e) { return true; }
};

// ReSharper disable once NativeTypePrototypeExtending
Array.prototype.remove = function (value) {
    const list = this.filter(x => x === value);
    var tthis = this;
    list.forEach(function (v) {
        tthis.splice(tthis.indexOf(v), 1);
    });
};
// ReSharper disable once NativeTypePrototypeExtending
Array.prototype.sum = function () {
    var sum = 0;
    this.forEach(function (i) { sum += parseFloatForce(i.toString()); });
    return sum;
};

// ReSharper disable once NativeTypePrototypeExtending
String.prototype.replaceAll = function (search, replacement) {
    //  [RegExp] 53% faster than [split join]
    return this.replace(new RegExp(search, "g"), replacement);

    //return this.split(search).join(replacement);
};

// ReSharper disable once NativeTypePrototypeExtending
String.prototype.toPersianDigits = function () {

    var inputstring = this.toString();
    const persian = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
    const english = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
    for (let i = 0; i < 10; i++) {
        inputstring = inputstring.toString().replaceAll(english[i], persian[i]);
    }

    return inputstring;

};

// ReSharper disable once NativeTypePrototypeExtending
String.prototype.toEnglishDigits = function () {

    var inputstring = this.toString();
    const persian = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
    const english = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
    for (let i = 0; i < 10; i++) {
        inputstring = inputstring.toString().replaceAll(persian[i], english[i]);
    }

    return inputstring;

};

// ReSharper disable once NativeTypePrototypeExtending
String.prototype.capitalize = function () {
    return this.charAt(0).toUpperCase() + this.slice(1);
};

Object.defineProperty(Object.prototype, "thousandComma", {
    value: function thousandComma(withcomma, decimal) {
        withcomma = $.isNEU(withcomma, true);
        decimal = $.isNEU(decimal, 2);
        const parts = this.toString().split(".");
        if (!$.isNEU(parts[1]))
            parts[1] = parts[1].substring(0, decimal > parts[1].length ? parts[1].length : decimal).replace(/0+$/, "");
        if (decimal === 0 | $.isNEU(parts[1]))
            parts.splice(1, 1);
        if (withcomma)
            parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        return parseFloatForce(this.toString(), 0) < 0 ? `(${parts.join(".").replace("-", "")})` : parts.join(".");
    }
});

Object.defineProperty(Object.prototype, "leadingZero", {
    value: function leadingZero(digitCount) {
        digitCount = parseIntForce(digitCount, 2);
        digitCount = digitCount < 2 ? 2 : digitCount;
        const str = this.toString();
        return str.replace(new RegExp("(^\\d)", "g"), "0".repeat(str.length >= digitCount ? 0 : digitCount - str.length) + "$1");
    }
});


window.jalaliConvertorString = function (value, toPerJ, hasTime) {


    const jalCal = (d) => {
        var i, a, n, r, t, o, v;
        const e = [
            -61, 9, 38, 199, 426, 686, 756, 818, 1111, 1181, 1210, 1635, 2060, 2097, 2192, 2262, 2324, 2394, 2456, 3178
        ];
        const l = e.length;
        const u = d + 621;
        var m = -14,
            g = e[0];
        if (g > d || d >= e[l - 1]) throw new Error(`Invalid Jalaali year ${d}`);
        for (v = 1; l > v && (i = e[v], a = i - g, !(i > d)); v += 1)
            m = m + 8 * div(a, 33) + div(mod(a, 33), 4), g = i;
        return o = d - g, m =
            m + 8 * div(o, 33) + div(mod(o, 33) + 3, 4), 4 === mod(a, 33) && a - o === 4 && (m += 1), r =
            div(u, 4) - div(3 * (div(u, 100) + 1), 4) - 150, t =
            20 + m - r, 6 > a - o && (o = o - a + 33 * div(a + 4, 33)), n =
            mod(mod(o + 1, 33) - 1, 4), -1 === n && (n = 4), { leap: n, gy: u, march: t };
    };
    const g2d = (d, i, a) => {
        var n = div(1461 * (d + div(i - 8, 6) + 100100), 4) + div(153 * mod(i + 9, 12) + 2, 5) + a - 34840408;
        return n = n - div(3 * div(d + 100100 + div(i - 8, 6), 100), 4) + 752;
    };
    const j2d = (d, i, a) => {
        const n = jalCal(d);
        return g2d(n.gy, 3, n.march) + 31 * (i - 1) - div(i, 7) * (i - 7) + a - 1;
    };
    const d2g = (d) => {
        var i, a, n, r, t;
        return i = 4 * d + 139361631, i = i + 4 * div(3 * div(4 * d + 183187720, 146097), 4) - 3908, a =
            5 * div(mod(i, 1461), 4) + 308, n = div(mod(a, 153), 5) + 1, r = mod(div(a, 153), 12) + 1, t =
            div(i, 1461) - 100100 + div(8 - r, 6), { gy: t, gm: r, gd: n };
    };
    const d2j = (d) => {
        var i, a, n;
        const r = d2g(d).gy;
        var t = r - 621;
        const o = jalCal(t);
        const v = g2d(r, 3, o.march);
        if (n = d - v, n >= 0) {
            if (185 >= n) return a = 1 + div(n, 31), i = mod(n, 31) + 1, { jy: t, jm: a, jd: i };
            n -= 186;
        } else t -= 1, n += 179, 1 === o.leap && (n += 1);
        return a = 7 + div(n, 30), i = mod(n, 30) + 1, { jy: t, jm: a, jd: i };
    };
    const div = (d, i) => { return ~~(d / i) };
    const mod = (d, i) => { return d - ~~(d / i) * i };
    const toGregorianDate = (d, i, a) => { return d2g(j2d(d, i, a)) };

    hasTime = $.isNEU(hasTime, true);
    const timeMatch = value.match(/( ?\d{1,2}:\d{1,2})/g);

    var year = null, month = null, day = null, rightF = false;
    if (/^\/?Date\(\d+\)\/?$/g.test(value)) {
        var netDt = new Date(parseInt(value.replace(/\D/g, "")));
        year = netDt.getFullYear();
        month = netDt.getMonth() + 1;
        day = netDt.getDate();
        rightF = true;
    }
    else
        if (/^\d{4}[\W_]\d{1,2}[\W_]\d{1,2}(T[\d:]+(\.\d+)?)?$/g.test(value)) {

            var pnDt = value.split(/[\W_]/g);
            year = pnDt[0];
            month = pnDt[1];
            day = pnDt[2];
            rightF = true;
        }
        else
            if (/^\d{4}[\W_]\d{1,2}[\W_]\d{1,2}( ?\d{1,2}:\d{1,2})?$/g.test(value)) {
                if (!$.isNEU(timeMatch))
                    value = value.replace(/( ?\d{1,2}:\d{1,2})/g, "");

                var pDt = value.split(/[\W_]/g);
                year = pDt[0];
                month = pDt[1];
                day = pDt[2];
                rightF = true;
            }

    if (rightF) {
        var gdt;
        if (toPerJ) {
            const toJalaaliDate = (d, i, a) => { return d2j(g2d(d, i, a)) };
            gdt = toJalaaliDate(parseInt(year), parseInt(month), parseInt(day));
            return gdt.jy + "/" + gdt.jm + "/" + gdt.jd + (!hasTime || $.isNEU(timeMatch) ? "" : ` ${(timeMatch[0].trim())}`);

        } else {
            gdt = toGregorianDate(parseInt(year), parseInt(month), parseInt(day));
            return gdt.gy + "/" + gdt.gm + "/" + gdt.gd + (!hasTime || $.isNEU(timeMatch) ? "" : ` ${(timeMatch[0].trim())}`);
        }
        // ReSharper disable once UnusedLocals
    }
    return null;
};

//====================================================================

const csLanguage = $.isNEU(location.pathname.match(/^\/[a-zA-Z]{2}\//i), ["/fa/"])[0];

const csIsDebug = /localhost/ig.test(location.hostname);
const csIsRelease = !csIsDebug;

//====================================================================

var keepGlobalLoading = false;
$.toggleGlobalLoading = function (show) {
    if (show)
        $("#loading").fadeIn(100);
    else {
        keepGlobalLoading = false;
        $("#loading").fadeOut(200);
    }
};


$(document).ajaxSend(function (event, xhr, options) {
    if (!new RegExp(`^${csLanguage}`).test(options.url))
        options.url = options.url.replace(/^\//, csLanguage);
    if (!/\/.+exist/ig.test(options.url))
        $("#loading").fadeIn(100);
});

//$(document).ajaxStart(function (event, xhr, options) {
//    //if (!($.isNEU(event.currentTarget.activeElement.dataset.valRemoteUrl, "").toLowerCase().includes("isexistusername")))
//    //    $('#loading').fadeIn(100);
//});

$(document).ajaxComplete(function (event, xhr) {
    if (xhr.getResponseHeader("X-Responded-JSON") !== null &&
        JSON.parse(xhr.getResponseHeader("X-Responded-JSON")).status === 401) {
        keepGlobalLoading = true;
        try {
            $.alert("error", "مدت زمان نشست شما سپری شده است. لطفا مجددا وارد شوید.", 3000);
            setTimeout(function () { location.reload(); }, 3000);
        } catch (e) {
            alert("مدت زمان نشست شما سپری شده است. لطفا مجددا وارد شوید.");
            location.reload();
        }
    } else
        if (!$.isNEU(xhr.getResponseHeader("keepLoading")))
            keepGlobalLoading = true;
});

$(document).ajaxStop(function () {
    if (!keepGlobalLoading)
        $.toggleGlobalLoading(false);
});


$.addAntiForgeryToken = function (data) {
    data.__RequestVerificationToken = $("#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]").val();
    return data;
};



$.fn.buildKamaDatepicker = function (options) {
    options = $.isNEU(options,
        {
            twodigit: true,
            closeAfterSelect: true,
            nextButtonIcon: "fa fa-chevron-right",
            previousButtonIcon: "fa fa-chevron-left",
            buttonsColor: "پیشفرض ",
            forceFarsiDigits: false,
            markToday: true,
            markHolidays: false,
            highlightSelectedDay: true,
            sync: true,
            gotoToday: true
        });
    $(this).each(function () {
        const tthis = $(this);
        if (!tthis.next().hasClass("bd-main")) {
            kamaDatepicker(tthis, options);
        }
    });
};

//(async () => {
//    bdy.on("click", "[data-printorder]", async function () {
//        try {
//            const id = $(this).data("printorder");
//            const result = await $.get("/Order/Print", { id });
//            if (result.Status) {
//                printData(result.Data);
//            }
//        } catch (e) {
//            //
//        }
//    });
//    function printData(html) {
//        const newWin = window.open("");
//        newWin.document.write(html);
//        newWin.print();
//        newWin.close();
//    }
//})();