$.fn.mvcgrid.lang = {
    text: {
        'contains': "شامل x باشد",
        'equals': "برابر x باشد",
        'not-equals': "برابر x نباشد",
        'starts-with': "با x آغاز شود",
        'ends-with': "با x پایان یابد"
    },
    number: {
        'equals': "مساوی x باشد",
        'not-equals': "مساوی x نباشد",
        'less-than': "کمتر از x باشد",
        'greater-than': "بزرگتر از x باشد",
        'less-than-or-equal': "کوچکتر مساوی x باشد",
        'greater-than-or-equal': "بزرگتر مساوی x باشد"
    },
    date: {
        'equals': "برابر x باشد",
        'not-equals': "برابر x نباشد",
        'earlier-than': "از x کمتر باشد",
        'later-than': "از x بزرگتر باشد",
        'earlier-than-or-equal': "کمتر یا برابر x باشد",
        'later-than-or-equal': "بزرگتر یا برابر x باشد"
    },
    enum: {
        'equals': "برابر x باشد",
        'not-equals': "برابر x نباشد"
    },
    boolean: {
        'equals': "برابر x باشد",
        'not-equals': "برابر x نباشد"
    },
    filter: {
        'apply': "✔",
        'remove': "✘"
    },
    operator: {
        'select': "",
        'and': "و",
        'or': "یا"
    }
};

$(function () {
    //reloadStarted | reloadEnded | reloadFailed
    $("body").on("reloadEnded", ".mvc-grid", function (e, grid) {
        const tthis = $(this);
        tthis.reArrengeNumber(tthis.find("[data-page]").length === 1 ? "" : grid.query);
        $.buildSelect2("select.mvc-grid-pager-rows", { minimumResultsForSearch: -1 });
        tthis.find(".mvc-grid-page-sizes").prepend('<span class="mx-2">نمایش</span>');
        tthis.find(".mvc-grid-page-sizes").append('<span class="mx-2">مورد</span>');
        const totalRows = tthis.find(".mvc-grid-pager").data("totalrows");
        if (totalRows > 0)
            tthis.find(".mvc-grid-pager").append(`<div class="mvc-grid-totalrow"><span>تعداد کل نتایج : </span><span class="mx-2">${totalRows}</span><div>`);
    });
});
