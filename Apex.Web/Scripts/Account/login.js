function loginComplete(res) {
    const result = JSON.parse(res.responseText);
    if (result.Status) {
        const ar = location.search.match(/\bReturnUrl=[^&]+/i);

        if (ar.length) {
            const u = decodeURIComponent(ar[0].replace(/^[^=]+=/, ""));
            if (/resetpassword/gi.test(u))
                location.reload();
            else
                location.replace(u);

        } else
            location.reload();

    } else {
        $("#alertBox span").text(result.Message);
    }
}