$(function () {
    initLayoutTime();
    $.addRequiredRedStar();
    activateLayoutMenuItem();

    bdy.on("change", "select#Province,select[id*='ProvinceId']", function () {
        var selP = $(this);
        const provinceId = $(this).val();
        var html = "";
        if ($.isNumeric(provinceId)) {
            $.get("/Home/GetCities",
                { provinceId },
                function (result) {
                    if (result.Status) {
                        $(result.Data).each(function (idx, el) {
                            html += `<option value='${el.Id}'>${el.Name}</option>`;
                        });
                        selP.oneParent("form,[cityindic]").find("select[id*='CityId']").html(html).find("option").eq(0).attr("selected", true);
                        $.buildSelect2();
                    }
                });
        } else {
            selP.oneParent("form").find("select[id*='CityId']").html(html);
            $.buildSelect2();
        }
    });

    bdy.on("click", "[id^='uniform-']>span", function () {
        if (!$(this).find(":checkbox[menuinput]").length)
            $(this).find(":checkbox").val($(this).hasClass("checked"));
    });

    //For Uniform Plugin that use for checkbox and radio button.
    bdy.on("change", "[id^='uniform-'] :radio,[id^='uniform-'] :checkbox", function () {
        if ($(this).prop("checked"))
            $(this).parent().addClass("checked");
        else
            $(this).parent().removeClass("checked");
    });

    $.fillCurrentDate();
    $.buildBTooltip();
    //----------------------------------

    $(window).bind("beforeunload", function () {
        if ($("form[preventpostback]:visible").length)
            return "This note not show to user";
        // ReSharper disable once NotAllPathsReturnValue
    });

    function initLayoutTime() {
        const lmt = $("[layoutMainTime]");
        const cdt = new Date(`2014-01-01 ${lmt.text()}`);
        cdt.setSeconds(cdt.getSeconds() + 1);
        lmt.text(`${cdt.getHours().leadingZero()}:${cdt.getMinutes().leadingZero()}:${cdt.getSeconds().leadingZero()}`);
        setTimeout(initLayoutTime, 1000);
    }
    function activateLayoutMenuItem() {
        const target = $(`.page-sidebar-menu li>a[href='${$("#layoutMenuActivator").data("url")}${location.search}']`);
        target.parents("li").addClass("active").children("a").find(".arrow").addClass("open");
        const title = target.children(".title").first().text();
        document.title = $.isNEU(title) ? ($.isNEU($("*:not(.modal) .portlet-title").first().find(".caption").text().trim(), "نرم افزار حسابداری")) : title;
    }


    bdy.on("mouseenter", ".el-options", function (e) {
        const el = e.currentTarget;
        var rect = el.getBoundingClientRect();
        const ul = $(this).children("ul").first()[0];
        var top;
        var left;
        var placement = $.isNEU($(this).attr("placement"), "left");
        var plc = placement;

        switch (placement) {
            //LEFT
            case "left":
                top = rect.top - ul.offsetHeight / 2 + rect.height / 2;
                left = rect.left + rect.width;

                if (left < 0) {
                    plc = "right";
                    //--> right
                    left = rect.left - ul.offsetWidth;
                }
                break;
            //RIGHT
            case "right":
                top = rect.top - ul.offsetHeight / 2 + rect.height / 2;
                left = rect.left - ul.offsetWidth;

                if (left + ul.offsetWidth > window.innerWidth) {
                    plc = "left";
                    //--> left
                    left = rect.left + rect.width;
                }
                break;
            //TOP
            case "top":
                top = rect.top - ul.offsetHeight;
                left = rect.left - ul.offsetWidth / 2 + rect.width / 2;
                if (top < 0) {
                    plc = "bottom";
                    //--> bottom
                    top = rect.top + rect.height;
                }
                break;
            //BOTTOM
            case "bottom":
                top = rect.top + rect.height;
                left = rect.left - ul.offsetWidth / 2 + rect.width / 2;
                if (top + ul.offsetHeight > window.innerHeight) {
                    plc = "top";
                    //--> top
                    top = rect.top - ul.offsetHeight;
                }
                break;
        }

        if (left < 0) left = 0;
        if (left + ul.offsetWidth > window.innerWidth)
            left = window.innerWidth - ul.offsetWidth;
        if (top < 0) top = 0;
        if (top + ul.offsetHeight > window.innerHeight)
            top = window.innerHeight - ul.offsetHeight;
        //var modal =$(this).oneParent(".modal-dialog")
        var modalDialog = $(this).oneParent(".modal-dialog");
        if (modalDialog.length) {
            const mRect = modalDialog.first()[0].getBoundingClientRect();
            top = top - mRect.top;
            left = left - mRect.left;
        }
        ul.style.top = `${top}px`;
        ul.style.left = `${left}px`;
        ul.setAttribute("class", `plc-${plc}`);
        $(this).addClass("el-options-hover");
    });
    bdy.on("mouseleave", ".el-options", function () {
        $(this).removeClass("el-options-hover");
    });
});
//============================================================
window.isFunction = function (func) {
    return typeof func === "function" || typeof func === "string" && typeof window[func] === "function";
};

$.fn.arrengeCollectionIndex = function (fullIdText, startFrom, indic) {
    startFrom = $.isNEU(startFrom, 0);
    //id="SubsidiaryViewModels_0__Id"
    //name="SubsidiaryViewModels[0].Id"
    if ($.isNEU(indic)) {
        $(this).find(`[id^="${fullIdText}"],[name^="${fullIdText}"]`).each(function (idx, obj) {
            generateNameId(obj, startFrom + idx);
        });
    } else {
        $(this).find(`[${indic}]`).each(function (bigIdx, inc) {
            $(inc).find(`[id^="${fullIdText}"],[name^="${fullIdText}"]`).each(function (idx, obj) {
                generateNameId(obj, startFrom + bigIdx);
            });
        });
    }

    function generateNameId(obj, i) {
        const elem = $(obj);
        if (elem.hasAttr("id"))
            elem.attr("id", elem.attr("id").replace(new RegExp("\\d+(?=__)", "gi"), i));

        if (elem.hasAttr("name"))
            elem.attr("name", elem.attr("name").replace(new RegExp("\\d+(?=]\\.(\\w+)$)", "gi"), i));

        var lbl = elem.siblings("[for]");
        if (!lbl.length)
            lbl = elem.parent().siblings("[for]");
        if (lbl.length)
            lbl.attr("for", lbl.attr("for").replace(new RegExp("\\d+(?=__)", "gi"), i));

        const spn = elem.siblings("[data-valmsg-for]");
        if (spn.length)
            spn.attr("data-valmsg-for", spn.attr("data-valmsg-for").replace(new RegExp("\\d+(?=]\\.(\\w+)$)", "gi"), i));
    }
};

$.fn.setPartialView = function (partialResult) {
    $(this).html(partialResult);
    $.resetFormValidation("form");
};
$.metronicInit = function () {
    // ReSharper disable once UseOfImplicitGlobalInFunctionScope
    Metronic.init();
};

$.reloadDetList = function (selector, options) {
    options = $.isNEU(options, { reload: true });
    if ($.isNEU(options.reload))
        options.reload = true;
    $($.isNEU(selector, ".mvc-grid")).mvcgrid(options);
};

$.buildSelect2 = function (selector, options) {
    selector = typeof selector === "object" ? selector : $($.isNEU(selector, "select.form-control"));
    options = $.isNEU(options, {});
    if (options.formatNoMatches === undefined)
        options.formatNoMatches = function () { return "موردی یافت نشد."; };

    selector.select2("destroy");
    selector.select2(options);
};

$.buildTagInput = function (selector, options) {
    selector = $.isNEU(selector, "[buildtaginput]");
    options = $.isNEU(options, { defaultText: "تگ جدید", width: "100%" });
    $(selector).each(function () {
        var $this = $(this);
        $this.next(".tagsinput").remove();
        $this.tagsInput(options);
    });
};

$.fn.buildSimpleSwitch = function () {
    $(this).simpleSwitch();
};

$.fillCurrentDate = function (selector) {
    selector = $.isNEU(selector, "body");
    if (typeof selector === "string")
        selector = $(selector);

    selector.find("[id^='bd-root-']>:text").each(function () {
        const tthis = $(this);
        if ($.isNEU(tthis.val()) && !tthis.hasAttr("nofill-curdate")) {
            const hasSetF = tthis.hasAttr("setFromTime");
            const hasSetT = tthis.hasAttr("setToTime");
            if (hasSetF)
                tthis.val(`${$.getCurrentDate()}${$.isNEU(tthis.attr("setFromTime"), " 00:00")}`);
            else
                if (hasSetT)
                    tthis.val(`${$.getCurrentDate()}${$.isNEU(tthis.attr("setToTime"), " 23:59")}`);
                else
                    tthis.val($.getCurrentDate());
        }
    });
};

$.getCurrentDate = function (isPersian) {
    return $.isNEU(isPersian, true) ? $("#currentPersianDate").val() : $("#currentGregorianDate").val();
};

$.uploadFileBase = function (options) {
    var settings = $.extend({
        accept: [".png", ".jpg", ".jpeg", ".gif"],
        callback: () => { }
    }, options);

    const fileInput = $(`<input type="file" accept="${settings.accept.join(", ")}"/>`);
    fileInput.trigger("click");
    fileInput.on("change", function () {
        const file = this.files[0];
        const formData = new FormData(); // Currently empty
        formData.append("file", file);
        if (settings.accept.indexOf(file.name.match(/\.[^\.]+$/).toString().toLowerCase()) === -1) {
            $.alert("error", `تصویر انتخابی باید دارای یکی از فرمت های زیر باشد : ${settings.accept.join(", ")}`);
            return;
        }

        $.ajax(
            {
                url: "/File/Upload",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (result) {
                    if (result.Status && isFunction(settings.callback))
                        settings.callback(result.Data);
                }
            });

    });
};

$.buildBTooltip = function (selector, options) {
    selector = $.isNEU(selector, "[data-toggle='tooltip']");
    options = $.isNEU(options, { placement: "top" });
    if (typeof selector === "string")
        selector = $(selector);
    selector.tooltip(options);
};

function genericOnComplete(res) {
    const result = JSON.parse(res.responseText);
    $.alert(result.Status ? "success" : "error", result.Message, 5000);
}

jQuery(document).ready(function () {
    Metronic.init();
    Layout.init();
    Demo.init();
    if (isFunction("loadOnReady"))
        window["loadOnReady"]();
});