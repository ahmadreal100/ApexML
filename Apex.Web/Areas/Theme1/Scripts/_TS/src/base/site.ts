$(function () {
  const isMain = !$(".top-box").hasClass("no-main");
  var menus: Array<ICategory> = [];

  const mainCatsEl = $("#mainCats");
  $("#btnMainCat").click(function () {
    if (!mainCatsEl.hasClass("show")) setCats(buildCats(getItem(null)));
    mainCatsEl.toggleClass("show");
    $(this).find("i").toggleClass("up");
  });

  doc.on("click", ".cats a[data-id]", function () {
    const id = parseInt($(this).data("id")) || null;
    if (id) setCats(buildCats(getItem(id)));
  });

  doc.on("keyup", ".search-box input", (e) => {
    if (e.keyCode === 13) search();
  });
  doc.on("click", ".search-box i", () => search());
  //#region Funcs

  if (csSearch.get("keyword"))
    $(".search-box input").val(csSearch.get("keyword"));
  function search() {
    const val = $(".search-box input").val().toString().trim();

    const loc = new URL(location.href);
    loc.pathname = fixUrl("search");
    if (val) loc.searchParams.set("keyword", val);
    else loc.searchParams.delete("keyword");

    location.href = loc.href;
  }

  function setCats(html: string) {
    mainCatsEl.html(html);
    setTimeout(() => $("#cats").addClass("show"), 100);
  }

  function getItem(id: number) {
    if (!id)
      return { Id: null, Name: Str.all.ft(Str.categories), Childs: menus };

    const get = (ms: Array<ICategory>) => {
      var fs: ICategory = null;
      for (let index = 0; index < ms.length; index++) {
        const m = ms[index];
        if (m.Id === id) {
          fs = m;
          break;
        } else if ((m.Childs || []).length) {
          fs = get(m.Childs);
          if (fs != null) break;
        }
      }

      return fs;
    };

    return get(menus);
  }

  function buildCats(item: ICategory) {
    const hasAll = item?.Id ?? 0 > 0;
    const parent = getItem(item.ParentId);
    return `
      <div id="cats" class="cats">
        ${
          hasAll
            ? `
                <div>
                  <a class="back" data-id="${item.ParentId}">
                    <i class="fa fa-angle-left"></i>
                    <span>${parent.Name}</span>
                  </a>
                </div>
              `
            : ""
        }
        ${item.Childs.map((x) => {
          const hasChild = (x.Childs || []).length;
          return `
            <div>
              <a ${
                hasChild
                  ? `class="hasChild" data-id="${x.Id}"`
                  : `href="${x.Link}" class="${x.Id ? "" : "non"}"`
              }>
                <span>${x.Name}</span>
                ${hasChild ? `<i class="fa fa-angle-right"></i>` : ""}
              </a>
            </div>`;
        }).join("")}
        ${
          hasAll
            ? `
              <div>
                <a class="all" href="${item.Link}">
                  <span>${Str.postsOfCat.ft(item.Name)}</span>
                </a>
              </div>`
            : ""
        }
      </div>
    `;
  }

  //#endregion

  if (isMain) {
    (async () => {
      var cats = [];
      try {
        cats = await $.get("/Category/Get");
      } catch (e) {
        //
      }

      menus = cats;
      if (menus.length) {
        const loc = new URL(location.href);
        loc.pathname = fixUrl("search");
        loc.searchParams.delete("categoryId");
        menus = [
          { Name: Str.all.ft(Str.categories), Link: loc.href, Id: 0 },
          ...menus,
        ];
      }
    })();
  }

  doc.ready(() => {
    GL.hide();
  });
});
