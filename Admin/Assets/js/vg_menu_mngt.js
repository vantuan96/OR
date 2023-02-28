$(function () {

    if ($("#sTree2").length > 0) {
        var options = {
            opener: {
                active: true,
                as: 'html',  // or "class" or skip if using background-image url
                close: '<i class="fa fa-minus red"></i>', // or 'fa fa-minus' or './imgs/Remove2.png'
                open: '<i class="fa fa-plus"></i>', // or 'fa fa-plus' or './imgs/Add2.png'
                openerClass: 'sortAbleOpen'
            },
            currElClass: 'currElemClass',
            currElCss: { 'background-color': 'green', 'color': '#fff' },
            insertZonePlus: true,
            //listSelector: 'ol',
            baseClass: 'sTree2',
            ignoreClass: 'clickable',
            placeholderClass: 'placeholderClass',
            placeholderCss: { 'background-color': '#e4e4e4' },
            //listSelector: 'ol',
            hintClass: 'hintClass',
            // or like a jQuery css object
            hintCss: { 'background-color': 'green', 'border': '1px dashed white' },
            isAllowed: function (currEl, hint, target) {
                //if(hint.parents('li').first().data('module') === 'x' && currEl.data('module') !== 'x')
                if (currEl.data('module') === 'x' || hint.parents('ul').length > 3 || hint.parents('ul').length == 1)//hint.parents('li').first().data('module') == undefined)
                {
                    hint.css('background-color', '#ff9999');
                    return false;
                }
                else {
                    hint.css('background-color', '#99ff99');
                    return true;
                }
            },
            onChange: function (cEl) {
                $('#btnSaveOrder').show();
            }
        }

        $("#sTree2").sortableLists(options);
    }

    //console.log($('.sTree2').sortableListsToArray());
    //console.log($('.sTree2').sortableListsToHierarchy());
    //console.log($('.sTree2').sortableListsToString());
});

function UpdateStatus(status) {
    bootbox.confirm({
        title: layout_lang.Shared_PopupConfirmTitle,
        message: msg_lang.MenuMngt_MenuDetail_UpdateStatusConfirmText,
        callback: function (confirm) {
            if (confirm) {
                ShowOverlay(true);
                $.post(updateStatusUrl, { id: menuItemId, status: status }, function (result) {
                    if (result.ID == 1) {
                        ReloadWithMasterDB();
                    }
                    else {
                        ShowNotify('error', result.Message);
                        HideOverlay();
                    }
                });
            }
        }
    });
}

function DeleteMenu(id) {
    bootbox.confirm({
        title: layout_lang.Shared_PopupConfirmTitle,
        message: msg_lang.MenuMngt_MenuDetail_DeleteMenuConfirmText,
        callback: function (confirm) {
            if (confirm) {
                ShowOverlay(true);
                $.post(deleteMenuUrl, { id: id }, function (result) {
                    if (result.ID == 1) {
                        ReloadWithMasterDB();
                    }
                    else {
                        ShowNotify('error', result.Message);
                        HideOverlay();
                    }
                });
            }
        }
    });
}

function SaveSortOrder() {
    var menuData = $('#sTree2').sortableListsToArray();
    var menuAr = [];
    var isOnsite = false;
    for (var i = 0; i < menuData.length; i++) {
        var parentId = (menuData[i].parentId === undefined || menuData[i].parentId === "item_Onsite" || menuData[i].parentId === "item_NotOnsite") ? undefined : menuData[i].parentId.replace('item_', '');

        var menuId =  menuData[i].id.replace('item_', '')
        if (menuId === "Onsite") {
            isOnsite = true;
        }
        else if (menuId === "NotOnsite") {
            isOnsite = false;
        }



        if (menuId !== "Onsite" && menuId !== "NotOnsite") {
            menuAr.push({
                Id: menuId,
                ParentId: parentId,
                Order: menuData[i].order,
                IsOnsite: isOnsite
            });
        }
        
    }

    ShowOverlay(true);
    $.post(updateMenuSortUrl, { menudata: menuAr }, function (result) {
        if (result.ID == 1) {
            ReloadWithMasterDB();
        }
        else {
            ShowNotify('error', result.Message);
            HideOverlay();
        }
    });
}

function GenerateMenu()
{
    bootbox.confirm({
        title: layout_lang.Shared_PopupConfirmTitle,
        message: msg_lang.MenuMngt_ListMenu_GenerateMenuConfirmText,
        callback: function (confirm) {
            if (confirm) {
                ShowOverlay(true);
                $.post(generateMenuUrl, {}, function (result) {
                    if (result.ID == 1) {
                        ReloadWithMasterDB();
                    }
                    else {
                        ShowNotify('error', result.Message);
                        HideOverlay();
                    }
                });
            }
        }
    });
}