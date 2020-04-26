(function ($) {
    if ($.validator === undefined)
        return;
    var createTrueOtherName = function (element, otherproperty) {
        var nameArr = $(element).attr("name").split(".");
        nameArr[nameArr.length - 1] = otherproperty;
        return nameArr.join(".");
    };

    $.validator.methods["date"] = function (value) {
        return isneu(value) || /^\d{4}[-_\/\\]\d{1,2}[\-_\/\\]\d{1,2}( [0-2]?\d:[0-5]?\d)?$/g.test(value);
    };

    $.validator.unobtrusive.adapters.add("date",
        function (options) {
            options.messages["date"] = "فرمت تاریخ را به درستی وارد نمایید.";
        });

    function isneu(obj) {
        return obj === null || obj === undefined || obj === "";
    }

    if (!isneu($.validator)) {
        //--------------------------------NotEqual
        //Use 3rd parameter For get data from server validation attribute.[A.R]
        $.validator.addMethod("notequal",
            function (value, element, otherproperty) {
                if (isneu(value) || $("[name='" + createTrueOtherName(element, otherproperty) + "']").val() !== value)
                    return true;
                return false;
            });

        $.validator.unobtrusive.adapters.add("notequal",
            ["otherproperty"],
            function (options) {
                options.rules["notequal"] = options.params.otherproperty;
                options.messages["notequal"] = options.message;
            });

        //--------------------------------GreaterThanDate
        $.validator.addMethod("greaterthandate",
            function (value, element, otherproperty) {
                var ovalue = $("[name='" + createTrueOtherName(element, otherproperty) + "']").val();
                if (isneu(value) || isneu(ovalue) || value > ovalue)
                    return true;
                return false;
            });

        $.validator.unobtrusive.adapters.add("greaterthandate",
            ["otherproperty"],
            function (options) {
                options.rules["greaterthandate"] = options.params.otherproperty;
                options.messages["greaterthandate"] = options.message;
            });
        //--------------------------------MinValue
        $.validator.addMethod("minvalue", function (value, element, mv) {
            if (parseFloatForce(value, 0) >= mv)
                return true;
            return false;
        });

        $.validator.unobtrusive.adapters.add("minvalue", ["mv"], function (options) {
            options.rules["minvalue"] = options.params.mv;
            options.messages["minvalue"] = options.message;
        });

        //--------------------------------MaxValue
        $.validator.addMethod("maxvalue", function (value, element, mv) {
            if (parseFloatForce(value, 0) <= mv)
                return true;
            return false;
        });

        $.validator.unobtrusive.adapters.add("maxvalue", ["mv"], function (options) {
            options.rules["maxvalue"] = options.params.mv;
            options.messages["maxvalue"] = options.message;
        });
    }
})(jQuery);

