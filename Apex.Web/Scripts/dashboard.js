$(function () {
    bdy.on("change", "[name='datecheque']", function () {
        const tthis = $(this);
        if (tthis.prop("checked")) {
            const t = $(tthis.data("target"));
            t.siblings().hide(200);
            t.show(200);
        }
    });
});