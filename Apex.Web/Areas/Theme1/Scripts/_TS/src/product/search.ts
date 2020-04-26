$(function () {
  var pageNumber = 1;
  const selSize = $("#selPgSize");
  const keyword = $("#searchKeyword").text();
  const catId = parseIntForce($("#catItems>a:last-child").data("id"), 0);

  doc.on("change", "#selPgSize", () => {
    pageNumber = 1;
    getItems();
  });
  doc.on("click", "[data-page]", function () {
    const pn = parseIntForce($(this).data("page"), 0);
    if (pn) getItems(pn);
  });

  async function getItems(pn?: number) {
    pageNumber = pn || pageNumber;
    const pageSize = parseIntForce(selSize.val().toString(), 12);
    const data = {
      filter: { Keyword: keyword, CategoryId: catId },
      top: pageSize,
      skip: (pageNumber - 1) * pageSize,
    };

    $("#pgBox").slideUp(200);

    var result: IAjaxResult<Array<string>> = await $.post(
      "/Product/GetProducts",
      data
    );

    if (result.Status) {
      if (result.ExData) {
        var htm = result.Data.map(
          (x) => `<div class="col-sm-12 col-md-6 col-lg-4 col-xl-3">${x}</div>`
        ).join("");
        $("#productItems").html(htm);
        pagination(result.ExData, pageNumber, pageSize);
      } else {
        $("#productItems").html(
          `<h6 class="text-black-50 text-center w-100">${Str.noItemsFound}</h6>`
        );
      }
    }
  }

  getItems();

  function pagination(total: number, pn: number, ps: number) {
    $("#searchResultCount").text(`(${total.thousand()})`);

    if (!total) return;

    const pageCount = Math.ceil(total / ps);
    $("#pgInfo").text(`${pn}/${pageCount}`);

    var first = pn;
    var last = first + 2;
    if (last > pageCount) {
      last = pageCount;
      first = last - 2 > 1 ? last - 2 : 1;
    }

    const htm = ` <a data-page="1" class="${pn === 1 ? "disabled" : ""}"
        ><i class="fa fa-angle-double-left"></i
      ></a>
      <a data-page="${pn - 1}" class="${pn === 1 ? "disabled" : ""}"
        ><i class="fa fa-angle-left"></i
      ></a>

      ${getRange(first, last)
        .map(
          (x) =>
            `<a data-page="${x}" class="${pn == x ? "active" : ""}">${x}</a>`
        )
        .join("")}
      ${last < pageCount ? "<small>...</small>" : ""}

      <a data-page="${pn + 1}" class="${pn === pageCount ? "disabled" : ""}"
        ><i class="fa fa-angle-right"></i
      ></a>
      <a data-page="${pageCount}" class="${pn === pageCount ? "disabled" : ""}"
        ><i class="fa fa-angle-double-right"></i
      ></a>`;

    $("#pgBtns").html(htm);
    $("#pgBox").slideDown(200);
  }
});
