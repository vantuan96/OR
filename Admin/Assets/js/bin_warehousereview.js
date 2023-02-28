


var warehousereviewEvent = {

    getByHashString: function(hash) {
        if (hash.length == 0) {
            warehousereviewEvent.getWareHouseReviewInfo(0);
        }
        else {
            var hashcode = hash.replace('#', '');
            var arrhash = hashcode.split('-');

            switch (arrhash[0]) {
                case 'wh':
                    if (arrhash[1] == 0) {
                        warehousereviewEvent.getWareHouseReviewInfo(0);
                    }
                    else {
                        warehousereviewEvent.getZoneReviewInfo(arrhash[1]);
                    }
                    break;
                case 'zone':
                    warehousereviewEvent.getAreaReviewInfo(arrhash[1]);
                    break;
                case 'area':
                    warehousereviewEvent.getRowReviewInfo(arrhash[1]);
                    break;
                case 'row':
                    warehousereviewEvent.getPalletReviewInfo(arrhash[1]);
                    break;
                case 'pallet':
                    warehousereviewEvent.getBinReviewInfo(arrhash[1]);
                    break;
            }
        }
    },

    GetAllWareHouseDetailByWarehouseId: function (WarehouseId) {
        $('#divWareHouseTreeView').html(innerHTMLLoading);
        $.ajax({
            url: "/lay-toan-bo-thong-tin-kho-theo-warehouse-id",
            data: { WarehouseId: WarehouseId },
            type: "GET",
            cache: false
        }).done(function (res) {
            $('#divWareHouseTreeView').html(res + '<div class="bg_page"><i class="fa fa-sitemap"></i></div>');
            initTrees();
            addscroll();
            $(window).resize(function () {
                addscroll();
            });
            $("#scroll_left").mCustomScrollbar({});

            //$('#wh_ul_warehousereview [data-target="#iframePopup"]').click(function (e) {
            //    loadIframePopup($(this));
            //});

        });
    },

    getWareHouseReviewInfo: function (WarehouseId) {

        ShowOverlayLoadWaiting(true);
        $.ajax({
            url: "/get-warehouse-review-info",
            data: {WarehouseId : WarehouseId},
            type: "GET",
            cache: false
        }).done(function (res) {
            $('.wh_ajax_detail').html(res);
            warehousereviewEvent.bindDeleteWhObject();
            warehousereviewEvent.bindMenuUpdateRule();

            $('.wh_ul_Main [data-target="#iframePopup"]').click(function (e) {
                loadIframePopup($(this));
            });
        });
    },
 
    getZoneReviewInfo: function (WarehouseId) {

        ShowOverlayLoadWaiting(true);
        $.ajax({
            url: "/get-zone-review-info",
            data: { WarehouseId: WarehouseId },
            type: "GET",
            cache: false
        }).done(function (res) {
            $('.wh_ajax_detail').html(res);
            warehousereviewEvent.binEditWareHouseInfo();
            warehousereviewEvent.bindDeleteWhObject();
            warehousereviewEvent.bindMenuUpdateRule();

            $('#wh_ul_zonereview [data-target="#iframePopup"]').click(function (e) {
                loadIframePopup($(this));
            });
        });
    },

    getAreaReviewInfo: function (ZoneId) {
        ShowOverlayLoadWaiting(true);
        $.ajax({
            url: "/get-area-review-info",
            data: { ZoneId: ZoneId },
            type: "GET",
            cache: false
        }).done(function (res) {
            $('.wh_ajax_detail').html(res);
            warehousereviewEvent.binEditWareHouseInfo();
            warehousereviewEvent.bindDeleteWhObject();
            warehousereviewEvent.bindMenuUpdateRule();

            $('#wh_ul_areareview [data-target="#iframePopup"]').click(function (e) {
                loadIframePopup($(this));
            });
        });
    },

    getRowReviewInfo: function (AreaId) {
        ShowOverlayLoadWaiting(true);
        $.ajax({
            url: "/get-row-review-info",
            data: { AreaId: AreaId },
            type: "GET",
            cache: false
        }).done(function (res) {
            $('.wh_ajax_detail').html(res);
            warehousereviewEvent.binEditWareHouseInfo();
            warehousereviewEvent.bindDeleteWhObject();
            warehousereviewEvent.bindMenuUpdateRule();
            warehousereviewEvent.bindDeleteAreaAttributeValue();

            $('#wh_ul_rowreview [data-target="#iframePopup"]').click(function (e) {
                loadIframePopup($(this));
            });

            $('.area_tag [data-target="#iframePopup"]').click(function (e) {
                loadIframePopup($(this));
            });

        });

       
    },

    getPalletReviewInfo: function (RowId) {
        ShowOverlayLoadWaiting(true);
        $.ajax({
            url: "/get-pallet-review-info",
            data: { RowId: RowId },
            type: "GET",
            cache: false
        }).done(function (res) {
            $('.wh_ajax_detail').html(res);
            warehousereviewEvent.binEditWareHouseInfo();
            warehousereviewEvent.bindDeleteWhObject();
            warehousereviewEvent.bindMenuUpdateRule();

            $('#wh_ul_palletreview [data-target="#iframePopup"]').click(function (e) {
                loadIframePopup($(this));
            });
        });

    },

    getBinReviewInfo: function (PalletId) {
        ShowOverlayLoadWaiting(true);
        $.ajax({
            url: "/get-bin-review-info",
            data: { PalletId: PalletId },
            type: "GET",
            cache: false
        }).done(function (res) {
            $('.wh_ajax_detail').html(res);
            warehousereviewEvent.bindDeleteWhObject();
            warehousereviewEvent.bindMenuUpdateRule();

            $('#wh_ul_binreview [data-target="#iframePopup"]').click(function (e) {
                loadIframePopup($(this));
            });

            if ($('.aBinReviewInfo').length > 0) {
                $('#divShalvesDetail [data-target="#iframePopup"]').click(function (e) {
                    loadIframePopup($(this));
                });
            }
            

        });
    },
    binEditWareHouseInfo: function () {
        $('.act_edit').click(function (e) {
            var typeid = $(this).data('typeid');
            var infoid = $(this).data('infoid');
            var infoname = $(this).data('infoname');
            var infonameNew = "";

            $("#edit-typeid").val(typeid);
            $("#edit-infoid").val(infoid);
            $("#txtEditCurrent").val(infoname);

            $("#save-info").unbind('click').click(function (e) {

                infonameNew = $("#txtEditNew").val();
                $.ajax({
                    url: "/update-warehouse-info-by-type",
                    type: "POST",
                    data: {
                        typeid: typeid,
                        infoid: infoid,
                        infoname: infonameNew,
                        "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val()
                    },
                    dataType: 'json',
                    async: false,
                    cache: false,
                    success: function (result) {
                        if (result.ID == 1) {
                            $("#edit-typeid").val("");
                            $("#edit-infoid").val("");
                            $("#txtEditCurrent").val("");
                            $("#txtEditNew").val("");
                            $('#popup_edit_info').modal('hide');
                            ShowNotify('success', result.Message);

                            switch (typeid) {
                                case 1:
                                    $('#spWareHouse_' + infoid).text(infonameNew);
                                    warehousereviewEvent.getZoneReviewInfo(infoid);
                                    break;
                                case 2:
                                    $('#spZone_' + infoid).text(infonameNew);
                                    warehousereviewEvent.getAreaReviewInfo(infoid);
                                    break;
                                case 3:
                                    $('#spArea_' + infoid).text(infonameNew);
                                    warehousereviewEvent.getRowReviewInfo(infoid);
                                    break;
                                case 4:
                                    $('#spRow_' + infoid).text(infonameNew);
                                    warehousereviewEvent.getPalletReviewInfo(infoid);
                                    break;
                                case 5:
                                    $('#spPallet_' + infoid).text(infonameNew);
                                    warehousereviewEvent.getBinReviewInfo(infoid);
                                    break;
                                case 6:
                                    warehousereviewEvent.getBinReviewInfo(infoid);
                                    break;
                                default:
                            }

                        }
                        else {
                            bootbox.alert("Lỗi hệ thống, không thể cập nhật");
                        }
                    },
                    error: function () {
                        bootbox.alert("Lỗi hệ thống, không thể cập nhật");
                    }
                });
            });
        });

    },
 
    bindDeleteWhObject: function() {
        $('.btnDeleteObject').click(function (e) {
            e.preventDefault();
            var type = $(this).data('object-type');
            var id = $(this).data('object-id');

            var li = $(this).parentsUntil('li.wh_item').parent();
            var liOnLeftTree = $('li[data-obj-type=' + type + '][data-obj-id=' + id + ']');
            var spanCount = $('.countNumber[data-obj-type=' + type + ']');            

            bootbox.confirm('Bạn có chắc muốn xóa không?', function (result) {
                var param = {
                    objectTypeId: type,
                    objectId: id,
                    __RequestVerificationToken: $('[name=__RequestVerificationToken]').val()
                };

                if (result) {
                    $.post('/do-delete-wh-object', param, function (response) {
                        if (response.ID > 0) {
                            $(li).remove();
                            $(liOnLeftTree).remove();

                            if ($(spanCount).length > 0) {
                                var number = $(spanCount).text();
                                number--;
                                $(spanCount).text(number);
                            }
                            
                            ShowNotify('success', 'Xóa thành công');
                        }
                        else {
                            ShowNotify('error', response.Message);
                        }
                    });
                }
            });
        });
    },

    bindMenuUpdateRule: function () {

        $('.menuUpdateRule a').click(function (e) {
            var ul = $(this).parent().parent();
            var btn = $(ul).prev();
            var iconBin = $(btn).parent().parent();

            var type = $(ul).data('object-type');
            var id = $(ul).data('object-id');
            var ruleId = $(this).data('rule-id');

            if (!$(btn).hasClass('rule_' + ruleId)) {
                var param = {
                    objectTypeId: type,
                    objectId: id,
                    ruleId: ruleId,
                    __RequestVerificationToken: $('[name=__RequestVerificationToken]').val()
                };

                $.post('/do-update-wh-object-rule', param, function (response) {
                    if (response.ID > 0) {
                        $(btn).removeClass('rule_1');
                        $(btn).removeClass('rule_2');
                        $(btn).removeClass('rule_3');
                        $(btn).removeClass('rule_4');

                        $(btn).addClass('rule_' + ruleId);
                        if ($(iconBin).hasClass('icon_bin')) {
                            $(iconBin).removeClass('rule_1');
                            $(iconBin).removeClass('rule_2');
                            $(iconBin).removeClass('rule_3');
                            $(iconBin).removeClass('rule_4');
                            $(iconBin).addClass('rule_' + ruleId);
                        }

                        ShowNotify('success', 'Cập nhật thành công');
                    }
                    else {
                        ShowNotify('error', response.Message);
                    }
                });
            }
        });
    },

    bindDeleteAreaAttributeValue: function () {
        $('.btnDeleteAreaAttrValue').click(function (e) {
            var areaId = $(this).data('area-id');
            var attrValueId = $(this).data('attr-value-id');
            var li = $(this).parent().parent();

            warehousereviewEvent.deleteAttributeValue(3, areaId, attrValueId, li);
        });
        
    },

    addAttributeValue: function (type, oid, att) {
        var param = {
            type: type,
            oid: oid,
            att: att,
            __RequestVerificationToken: $('[name=__RequestVerificationToken]').val()
        };

        $.post('/do-add-bin-attr-value', param, function (response) {
            if (response.ID > 0) {
                ShowNotify('success', 'Thêm thành công');
            }
            else {
                ShowNotify('error', response.Message);
            }
        });
    },

    deleteAttributeValue: function (type, oid, att, li) {
        bootbox.confirm('Bạn có chắc muốn xóa không?', function (result) {
            var param = {
                type: type,
                oid: oid,
                att: att,
                __RequestVerificationToken: $('[name=__RequestVerificationToken]').val()
            };

            if (result) {
                $.post('/do-delete-bin-attr-value', param, function (response) {
                    if (response.ID > 0) {                        
                        ShowNotify('success', 'Xóa thành công');

                        if (li != undefined)
                            $(li).remove();
                    }
                    else {
                        ShowNotify('error', response.Message);
                    }
                });
            }
        });
    },
};


function initTrees() {
    $("#mixed").treeview({
        collapsed: true,
        animated: "medium"
    });
}