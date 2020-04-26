var funcOptions = {
    objectName: "دسته بندی",
    jsTreeSelector: "#tree_1",
    addRootNodeId: "btnShMdlAddCategoryRoot",
    addModalSelector: "#mdlAddCategory",
    addModalContainerSelector: "#AddCategoryContainer",
    deleteAction: "/Category/Delete",
    getAction: "/Category/Get",
    getChildsAction: "/Category/GetChilds"
};

$(function () {
    //--------------------------------
    setChilds(null, null, { node: null });
    //--------------------------------
    //Object 'core' and 'check_callback": true' needed for create_node function.
    $(funcOptions.jsTreeSelector).jstree({ "core": { "check_callback": true } })
        .on("before_open.jstree", beforeOpenNode);

    bdy.on("click", `[data-addnode],#${funcOptions.addRootNodeId}`, function () {
        const mdl = $(funcOptions.addModalSelector);
        mdl.find(".modal-title").text(funcOptions.objectName + " جدید");
        const parentId = $.isNEU($(this).data("addnode"), null);
        const parenText = $.isNEU($(this).data("text"), "");
        mdl.clearFormInputs();
        mdl.find("#Id").val(0);
        mdl.find("#ParentId").val(parentId);
        mdl.find("#ParentText").val(parenText);
        mdl.modal();
    });

    bdy.on("click", "[data-editnode]", function () {
        const id = $(this).data("editnode");
        $.get(funcOptions.getAction, { id: id }, function (result) {
            if (result.Status) {
                $(funcOptions.addModalContainerSelector).setPartialView(result.Data);
                $(funcOptions.addModalSelector).modal();
            }
        });
    });

    bdy.on("click", "[data-delnode]", function () {
        var node = $(funcOptions.jsTreeSelector).jstree("get_selected")[0];
        if (node === $(this).parents(".jstree-node[role='treeitem']").first().attr("id")) {
            var id = $(this).data("delnode");
            $.confirm(`آیا از حذف ${funcOptions.objectName} "${$(this).data("text")}" اطمینان دارید؟`, function (res) {
                if (res) {
                    delNode(id, node);
                }
            });
        }
    });

    //--------------------------
    function delNode(id, node) {
        var selector = $(`#${node}`).parents(".jstree-node[role='treeitem']")[0];

        $.post(funcOptions.deleteAction, { id: id },
            function (result) {
                if (result.Status) {
                    $(funcOptions.jsTreeSelector).jstree("delete_node", node);
                    $(funcOptions.jsTreeSelector).jstree("select_node", $(selector).attr("id"));
                    checkNullTree();
                }
                $.alert(result.Status ? "success" : "error", result.Message, 5000);
            });
    }

    var completedAjaxNode = [];

    function beforeOpenNode(e, data) {
        const liId = data.node.id;
        if (completedAjaxNode.indexOf(liId) === -1) {
            //var level = parseInt(data.node.id.match(new RegExp("\\d+(?=_\\d+)", "gi")));
            const id = data.node.li_attr["data-id"];

            $(funcOptions.jsTreeSelector).jstree("delete_node", data.node.children);
            setChilds(liId, id, data);
        }
    }

    function setChilds(liId, id, data) {
        $.get(funcOptions.getChildsAction, { id: id }, function (result) {
            if (result.Status) {
                $(result.Data).each(function (idx, elem) {
                    const newNode = {
                        li_attr: { "data-id": elem.Id },
                        state: "close",
                        text: $.handleBarsHelper("#nodeTemplate", elem)
                    };
                    $(funcOptions.jsTreeSelector).jstree("create_node", $.isNEU(data.node, null), newNode);
                    if (elem.ChildsCount > 0)
                        $(funcOptions.jsTreeSelector).jstree("create_node", $(funcOptions.jsTreeSelector + ' [data-id="' + elem.Id + '"].jstree-node').attr("id"), { state: "close" });
                });

                if (!$.isNEU(liId));
                completedAjaxNode.push(liId);
                checkNullTree();

            }
        });
    }
});

function checkNullTree() {
    $("#lblNoTreeResult").css("display", $(funcOptions.jsTreeSelector + " .jstree-node").length ? "none" : "block");
}
function addCategoryConfirm(res) {
    const result = JSON.parse(res.responseText);
    if (result.Status) {
        const parent = $(funcOptions.jsTreeSelector).jstree("get_selected");
        const elem = result.Data;
        if (result.ExData) {
            $(funcOptions.jsTreeSelector).jstree(true).get_node(parent[0]).text = $.handleBarsHelper("#nodeTemplate", elem);
            $(funcOptions.jsTreeSelector).find(`li#${parent}`).children(".jstree-anchor").find("[indic-up]").text(elem.Name);
        } else {
            const newNode = {
                li_attr: { "data-id": elem.Id },
                state: "close",
                text: $.handleBarsHelper("#nodeTemplate", elem)
            };
            $(funcOptions.jsTreeSelector).jstree("create_node", elem.ParentId === null || parent.length === 0 ? null : parent, newNode);
            checkNullTree();
        }
        $(funcOptions.addModalSelector).modal("hide");
    }
    $.alert(result.Status ? "success" : "error", result.Message, 5000);
}