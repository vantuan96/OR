
function createURL() {
    var nameEnglish = base.replaceVN_To_English(_modelName);
    $("#URLKey").val(base.convert_Rewrite(nameEnglish));
    $("#URLKey").focusout();
}

function AddKeyWordItem(obj) {
    if (obj == null || obj.value.indexOf("~") <= 0)
        return;
    var array = obj.value.split('~');
    var linkId = array[0];
    var title =  array[1];
    var keyword = obj.text;

    if (IsExistItemInSelectionBox(linkId, $("#InternalLinkIds").val()))
        return;

    var item = "<span id=\"keywordItem_" + linkId + "\" data-link-id=" + linkId;
    item += " class=\"tag label label-info ui-state-default Tip_mouse_on cur_pointer\" title=\"" + title + "\">" + keyword + "<span data-role=\"remove\" onclick=\"RemoveItem('keywordItem_" + linkId + "');\"></span></span>";
    $("#keywordSortAble").append(item);
    $("input.typeahead").val("");
    UpdateSortOrderInternalLinkIds();
}

function RemoveItem(id) {
    $("#" + id).remove();
    UpdateSortOrderInternalLinkIds();
}

function UpdateSortOrderInternalLinkIds() {
    var ids = "";
    $("#keywordSortAble [data-link-id]").each(function () {
        var currentId = $(this).attr("data-link-id");
        ids += "-" + currentId;
    });
    $("#InternalLinkIds").val(ids);
}

function IsExistItemInSelectionBox(id, stringIds) {
    stringIds = stringIds + "-";
    id = "-" + id + "-";
    return stringIds.indexOf(id) >= 0;
}

$(function () {
    
    $("input.typeahead").typeahead({
            onSelect: function (item) {
                AddKeyWordItem(item);
            },
            ajax: {
            url: _internalLinkUrl,
            timeout: 500,
            displayField: "Text",
            valueField: "Id",
            triggerLength: 1,
            method: "get",
            loadingClass: "loading-circle",
            preDispatch: function (query) {
                _isShowLoadingAjax = false;
                return { search: query };
            },
            preProcess: function (result) {
                if (result.status === "fail") {
                    return false;
                }
                return result.data;
            }
        }
    }).keypress(function (e) {
        if (e.which == 13) {
            return false;
        }
    });

    $("#keywordSortAble").sortable({
        items: ".tag",
        helper: 'clone'
    });
            
    $("#keywordSortAble").sortable({
        update: function (event, ui) {
            UpdateSortOrderInternalLinkIds();
        }
    });
});
