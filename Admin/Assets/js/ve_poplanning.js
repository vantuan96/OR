var listUpdate = [];

$(function () {
    poplanning.init();

    if ($('table.datatable').length > 0) {
        var dt = $('table.datatable').DataTable({
            keys: true,
            //paging: false,
            scrollX: true,
            //scrollY: 500,
            scrollCollapse: true,
            autoWidth: false,
            language: dtvn
        });
        dt.on('draw', function () {
            var body = $(dt.table().body());

            body.unhighlight();
            body.highlight(dt.search());
        });

        dt.on('key-focus', function (e, datatable, cell, originalEvent) {
            var td = $('table tbody td.focus');
            if ($(td).hasClass('allowEdit')) {
                e.preventDefault();

                $(td).addClass('editting');
                $('input', $(td)).data('before-val', $('input', $(td)).val());
                $('input', $(td)).select().focus();
            }
        })
        .on('key-blur', function (e, datatable, cell) {
            var td = $('table tbody td.editting');
            if ($(td).length > 0) {
                $(td).removeClass('editting');
                ChangeValue(td);
            }
        });
    }

    $('.btnSave').click(function (e) {
        if (listUpdate.length > 0) {
            LazySubmit(listUpdate, function (tempList, callback) {
                $.post(_updateDetailUrl, { pfId: pfId, siteId: siteId, detail: tempList }, function (response) {
                    callback(response);
                });
            }, ReloadWithMasterDB);
        }
        else {
            ReloadWithMasterDB();
        }
    });

    $('.btnCancel').click(function (e) {
        window.location.reload();
    });
});

var poplanning = {
    init: function () {
        if ($('[data-event=active-file]').length > 0) {
            $('[data-event=active-file]').click(poplanning.ActiveFile);
        }

        if ($('[data-event=cancel-file]').length > 0) {
            $('[data-event=cancel-file]').click(poplanning.CancelFile);
        }

        if ($('[data-event=confirm-file]').length > 0) {
            $('[data-event=confirm-file]').click(poplanning.ConfirmFile);
        }

        if ($('[data-event=reimport-file]').length > 0) {
            $('[data-event=reimport-file]').click(poplanning.ReImportFile);
        }

        if ($('[data-event=complete-update-quantity]').length > 0) {
            $('[data-event=complete-update-quantity]').click(poplanning.CompleteUpdateQuantity);
        }

        if ($('[data-event=close-and-export]').length > 0) {
            $('[data-event=close-and-export]').click(poplanning.CloseAndExport);
        }

        if ($('[data-event=confirm-site]').length > 0) {
            $('[data-event=confirm-site]').click(poplanning.ConfirmSite);
        }

        if ($('[data-event=confirm-all-region-site]').length > 0) {
            $('[data-event=confirm-all-region-site]').click(poplanning.ConfirmAllRegionSite);
        }


        if ($('[data-event=rollback-poplanningfilesite]').length > 0) {
            $('[data-event=rollback-poplanningfilesite]').click(poplanning.RollBackPoplanningFileSiteStatus);
        }
        

        if ($('[data-event=copy-quantity]').length > 0) {
            $('[data-event=copy-quantity]').click(poplanning.CopyQuantity);
        }
    },
    ActiveFile: function () {
        var pfId = $(this).data('id');
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Xác nhận bắt đầu cho phép chỉnh sửa số lượng đặt hàng?",
            callback: function (confirm) {
                if (confirm) {
                    //var postData = new Array();
                    //postData.push({ name: 'pfId', value: $(this).data('id') });
                    //ShowOverlayLoadWaiting(true);
                    $.ajax({
                        url: _activeFileUrl,
                        type: "POST",
                        data: {
                            pfId: pfId,
                            "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val()
                        },
                        dataType: 'json',
                        async: false,
                        cache: false,
                        success: function (result) {
                            if (result.ID > 0) {
                                ReloadWithMasterDB();
                            } else {
                                ShowNotify('error', result.Message);
                                HideOverlay();
                            }
                        },
                        error: function (xhr) {
                            ShowNotify('error', 'Lỗi ' + xhr.status);
                            HideOverlay();
                        }
                    });
                }
            }
        });
    },
    CancelFile: function () {
        var pfId = $(this).data('id');
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn muốn hủy file này?",
            callback: function (confirm) {
                if (confirm) {
                    //var postData = new Array();
                    //postData.push({ name: 'pfId', value: $(this).data('id') });
                    //ShowOverlayLoadWaiting(true);
                    $.ajax({
                        url: _cancelFileUrl,
                        type: "POST",
                        data: {
                            pfId: pfId,
                            "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val()
                        },
                        dataType: 'json',
                        async: false,
                        cache: false,
                        success: function (result) {
                            if (result.ID > 0) {
                                ReloadWithMasterDB();
                            } else {
                                ShowNotify('error', result.Message);
                                HideOverlay();
                            }
                        },
                        error: function (xhr) {
                            ShowNotify('error', 'Lỗi ' + xhr.status);
                            HideOverlay();
                        }
                    });
                }
            }
        });
    },
    ConfirmFile: function () {
        var pfId = $(this).data('id');
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn có muốn xác nhận file này?",
            callback: function (confirm) {
                if (confirm) {
                    //var postData = new Array();
                    //postData.push({ name: 'pfId', value: $(this).data('id') });
                    //ShowOverlayLoadWaiting(true);
                    $.ajax({
                        url: _confirmFileUrl,
                        type: "POST",
                        data: {
                            pfId: pfId,
                            "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val()
                        },
                        dataType: 'json',
                        async: false,
                        cache: false,
                        success: function (result) {
                            if (result.ID > 0) {
                                ReloadWithMasterDB();
                            } else {
                                ShowNotify('error', result.Message);
                                HideOverlay();
                            }
                        },
                        error: function (xhr) {
                            ShowNotify('error', 'Lỗi ' + xhr.status);
                            HideOverlay();
                        }
                    });
                }
            }
        });
    },
    ReImportFile: function () {
        var pfId = $(this).data('id');
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Chuyển trạng thái file?",
            callback: function (confirm) {
                if (confirm) {
                    //var postData = new Array();
                    //postData.push({ name: 'pfId', value: $(this).data('id') });
                    //ShowOverlayLoadWaiting(true);
                    $.ajax({
                        url: _reImportFileUrl,
                        type: "POST",
                        data: {
                            pfId: pfId,
                            "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val()
                        },
                        dataType: 'json',
                        async: false,
                        cache: false,
                        success: function (result) {
                            if (result.ID > 0) {
                                ReloadWithMasterDB();
                            } else {
                                ShowNotify('error', result.Message);
                                HideOverlay();
                            }
                        },
                        error: function (xhr) {
                            ShowNotify('error', 'Lỗi ' + xhr.status);
                            HideOverlay();
                        }
                    });
                }
            }
        });
    },
    CompleteUpdateQuantity: function () {
        var pfId = $(this).data('pfid');
        var siteId = $(this).data('site-id');
        
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn sẽ không thể cập nhật số lượng đặt hàng sau khi hoàn tất. Bạn có muốn hoàn tất không?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'pfId', value: pfId });
                    postData.push({ name: 'siteId', value: siteId });

                    commonUtils.postAjaxWithToken(_completeQuantityUrl, postData, function (result) {
                        if (result.ID > 0) {
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
    },
    ConfirmSite: function () {
        var pfsiId = $(this).data('id');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Xác nhận số lượng đặt hàng?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'pfsiId', value: pfsiId });

                    commonUtils.postAjaxWithToken(_confirmSiteUrl, postData, function (result) {
                        if (result.ID > 0) {
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
    },
    ConfirmAllRegionSite: function () {
        var pfId = $(this).data('id');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn có muốn xác nhận toàn bộ cửa hàng đang quản lý ?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'pfId', value: pfId });

                    commonUtils.postAjaxWithToken(_confirmAllRegionSiteUrl, postData, function (result) {
                        if (result.ID > 0) {
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
    },
    CloseAndExport: function () {
        var url = $(this).data('export-url');

        isConfirmCloseAndExport = false;
        var htmlMessage = '<p>+ Thao tác này chỉ xuất những yêu cầu ở tab "Đã xác nhận" ra file Excel.</p>';
        htmlMessage += '<p>+ Sau khi lưu file Excel, những yêu cầu này sẽ được chuyển sang tab "Đã đóng".</p>';
        htmlMessage += '<p style="text-align:center;margin-top: 10px;"><a href="'+url+'" onclick="isConfirmCloseAndExport = true;$(this).hide();" class="btn btn-defaut"><i class="fa fa-download"></i> Lưu file Excel</a></p>';
        bootbox.dialog({
            title: 'CHÚ Ý',
            message: htmlMessage,
            buttons: {
                cancel: {
                    label: "Đóng",
                    className: "btn-default",
                    callback: function () {                        
                        if (isConfirmCloseAndExport)
                        {
                            window.location.href = window.location.href;
                        }
                    }
                }
            },
            onEscape: function() {
                if (isConfirmCloseAndExport) {
                    window.location.href = window.location.href;
                }
            }
        });
    },
    RollBackPoplanningFileSiteStatus: function () {
        var pfsiId = $(this).data('id');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Rollback trạng thái file cho cửa hàng?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'pfsiId', value: pfsiId });

                    commonUtils.postAjaxWithToken(_rollbackPoplanningFileSiteUrl, postData, function (result) {
                        if (result.ID > 0) {
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
    },
    CopyQuantity: function () {
        var pfId = $(this).data('pfid');
        var siteId = $(this).data('siteid');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn có muốn sao chép số lượng TTĐH đặt hàng không?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'pfId', value: pfId });
                    postData.push({ name: 'siteId', value: siteId });

                    commonUtils.postAjaxWithToken(_copyQuantityUrl, postData, function (result) {
                        if (result.ID > 0) {
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
};


function ChangeValue(td) {
    var control = $('input', $(td));
    var beforeValue = $.trim($(control).data('before-val'));
    var afterValue = $.trim($(control).val());

    var PdId = $(control).data('pdid');
    var Quantity = $('#quantity_' + PdId).val();
    var NoteConfirm = $('#note_' + PdId).val();

    if (beforeValue != afterValue) {
        var exist = false;
        for (var i = 0 ; i < listUpdate.length; i++) {
            if (listUpdate[i].PdId == PdId) {
                listUpdate[i].Quantity = Quantity;
                listUpdate[i].NoteConfirm = NoteConfirm;
                exist = true;
            }
        }

        if (exist == false) {
            listUpdate.push({
                PdId: PdId,
                Quantity: Quantity,
                NoteConfirm: NoteConfirm
            });
        }

        if ($(control).hasAttr('data-val-decimal')) {
            $('.displayValue', $(td)).text(numberWithCommas(afterValue));
        }
        else {
            $('.displayValue', $(td)).text(afterValue);
        }
        $('.btnSave, .btnCancel').removeClass('disabled');
    }
}
