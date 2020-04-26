$(function () {
    const layoutMenus = $.map($(".page-sidebar-menu a[href]").not("[href='javascript:void(0)']"), function (el) {
        return { title: $(el).children("span").text(), link: $(el).attr("href") };
    });

    var dashSearchTimeOut;
    var oldFindText = null;
    bdy.on("change keyup input paste", "#iptDashBoardSearch", function () {
        clearTimeout(dashSearchTimeOut);
        var val = $(this).val();
        dashSearchTimeOut = setTimeout(function () {
            if (oldFindText === val)
                return;
            createFindUl(layoutMenus.filter(x => new RegExp(val, "gi").test(x.title)), val);
            oldFindText = val;
        }, 500);
    });
    bdy.on("focus", "#iptDashBoardSearch", function () {
        var val = $(this).val();
        if (!$.isNEU(val))
            createFindUl(layoutMenus.filter(x => new RegExp(val, "gi").test(x.title)), val);
    });
    bdy.on("click", function (e) {
        if (!($(e.target).parents(".dashboard-search-box").length || $(e.target).hasClass("dashboard-search-box")))
            $(".dashboard-search-box ul").slideUp(200);
    });
    bdy.on("keydown", "#iptDashBoardSearch", function (e) {
        if (e.key === "ArrowUp") {
            const act = $(".dashboard-search-box ul li.active");
            if (act.index() !== 0) {
                act.removeClass("active").prev().first().addClass("active");
                $(".dashboard-search-box ul").scrollTop(act.prev().index() * act.prev().height());
            }
            e.preventDefault();
        } else
            if (e.key === "ArrowDown") {
                const act = $(".dashboard-search-box ul li.active");
                if (act.index() !== act.siblings().length) {
                    act.removeClass("active").next().first().addClass("active");
                    $(".dashboard-search-box ul").scrollTop(act.next().index() * act.next().height());
                }
                e.preventDefault();
            } else
                if (e.key === "Enter") {
                    const act = $(".dashboard-search-box ul li.active");
                    if (act.length)
                        location.href = act.find("a").attr("href");
                    e.preventDefault();
                }
    });

    function createFindUl(items, val) {
        const ul = $(".dashboard-search-box ul");
        ul.slideUp(200, function () {
            if (!$.isNEU(items) && items.length && val.length) {
                //ul.html(items.map(x => `<li><a href="${x.link}">${x.title.replace(new RegExp(val, "gi"), "<strong class='dash-find-sep'>$&</strong>")}</a></li>`));
                ul.html(items.map(x => `<li><a href="${x.link}">${x.title}</a></li>`));
                ul.children("li").first().addClass("active");
            }
            else
                ul.html(`<li><a href="javascript:void(0)" class="text-center">موردی یافت نشد</a></li>`);
            ul.slideDown(200);
        });
    }
});