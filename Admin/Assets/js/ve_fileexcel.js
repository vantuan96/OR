$(function () {
    fileexcel.init();
});

var fileexcel = {
    init: function () {
        if ($('.ckbCheckAll').length > 0)
        {
            $('.ckbCheckAll').change(function (e) {
                if ($(this).is(':checked'))
                {
                    $('.ckbExportedFid').prop('checked', true);
                }
                else
                {
                    $('.ckbExportedFid').prop('checked', false);
                }
            });
        }
        
        if ($('[data-event=active-file]').length > 0) {
            $('[data-event=active-file]').click(fileexcel.ActiveFileExcel);
        }

        if ($('[data-event=cancel-file]').length > 0) {
            $('[data-event=cancel-file]').click(fileexcel.CancelFileExcel);
        }

        if ($('[data-event=confirm-file]').length > 0) {
            $('[data-event=confirm-file]').click(fileexcel.CloseFileExcel);
        }

        if ($('[data-event=reimport-file]').length > 0) {
            $('[data-event=reimport-file]').click(fileexcel.ReImportFileExcel);
        }

        if ($('[data-event=remake-file]').length > 0) {
            $('[data-event=remake-file]').click(fileexcel.RemakeFileExcel);
        }

        if ($('[data-event=complete-update-quantity]').length > 0) {
            $('[data-event=complete-update-quantity]').click(fileexcel.CompleteUpdateQuantity);
        }

        if ($('[data-event=close-and-export]').length > 0) {
            $('[data-event=close-and-export]').click(fileexcel.CloseAndExport);
        }

        if ($('[data-event=confirm-site]').length > 0) {
            $('[data-event=confirm-site]').click(fileexcel.ConfirmSite);
        }

        if ($('[data-event=confirm-all-region-site]').length > 0) {
            $('[data-event=confirm-all-region-site]').click(fileexcel.ConfirmAllRegionSite);
        }

        if ($('[data-event=update-deadline]').length > 0) {
            $('[data-event=update-deadline]').click(fileexcel.UpdateDeadline);
        }

        if ($('[data-event=update-tag]').length > 0) {
            $('[data-event=update-tag]').click(fileexcel.UpdateFileExcelTag);
        }

        if ($('[data-event=rollback-fileexcelsitestatus]').length > 0) {
            $('[data-event=rollback-fileexcelsitestatus]').click(fileexcel.RollbackFileExcelSiteStatus);
        }
    },
    ActiveFileExcel: function () {
        var fId = $(this).data('id');
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Xác nhận chuyển file đến cửa hàng?",
            callback: function (confirm) {
                if (confirm) {
                    $.ajax({
                        url: _activeFileUrl,
                        type: "POST",
                        data: {
                            fId: fId
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
    CancelFileExcel: function () {
        var fId = $(this).data('id');
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
                            fId: fId
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
    CloseFileExcel: function () {
        var fId = $(this).data('id');
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn có muốn đóng file này?",
            callback: function (confirm) {
                if (confirm) {
                    //var postData = new Array();
                    //postData.push({ name: 'pfId', value: $(this).data('id') });
                    //ShowOverlayLoadWaiting(true);
                    $.ajax({
                        url: _closeFileExcel,
                        type: "POST",
                        data: {
                            fId: fId
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
    ReImportFileExcel: function () {
        var fId = $(this).data('id');
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Chuyển trạng thái file sang chờ hệ thống xử lý?",
            callback: function (confirm) {
                if (confirm) {
                    //var postData = new Array();
                    //postData.push({ name: 'pfId', value: $(this).data('id') });
                    //ShowOverlayLoadWaiting(true);
                    $.ajax({
                        url: _reImportFileUrl,
                        type: "POST",
                        data: {
                            fId: fId
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
    RemakeFileExcel: function () {
        var fId = $(this).data('id');
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Nạp lại dữ liệu file lên Redis?",
            callback: function (confirm) {
                if (confirm) {
                    $.ajax({
                        url: _remakeFileUrl,
                        type: "POST",
                        data: {
                            fId: fId
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
        var id = $(this).data('id');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn có muốn xác nhận cho cửa hàng này?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'fsid', value: id });

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
    UpdateDeadline: function ()
    {
        var fId = $(this).data('id');
        var deadline = $(this).data('deadline');

        bootbox.dialog({
            message: BootboxContent(deadline),
            title: "Cập nhật ngày hết hạn",
            buttons: {
                main: {
                    label: "Xác nhận",
                    className: "btn-success",
                    callback: function () {
                        var date = $('#txtDeadline').val();
                        if (date.length == 0) {
                            ShowNotify('error', 'Vui lòng nhập ngày hết hạn');
                        }
                        else {
                            $.ajax({
                                url: _updateDeadlineUrl,
                                type: "POST",
                                data: {
                                    fId: fId,
                                    deadline: date,
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
                                error: function () {
                                    ShowNotify('error', response.Message);
                                    HideOverlay();
                                }
                            });
                        }
                    }
                }
            }
        });
    },
    UpdateFileExcelTag: function () {
        var fId = $(this).data('id');

        bootbox.prompt({
            title: "Chỉnh sửa nhãn file",
            inputType: 'text',
            callback: function (tag) {
                if (tag != null) {
                    tag = tag.trim();
                    if (tag.length > 0) {
                        $.ajax({
                            url: _updateFileExcelTagUrl,
                            type: "POST",
                            data: {
                                fId: fId,
                                tag: tag
                            },
                            dataType: 'json',
                            async: false,
                            cache: false,
                            success: function (result) {
                                if (result.ID == 1) {
                                    ReloadWithMasterDB();
                                } else {
                                    ShowNotify('error', result.Message);
                                    HideOverlay();
                                }
                            },
                            error: function () {
                                ShowNotify('error', response.Message);
                                HideOverlay();
                            }
                        });
                    }
                    else {
                        ShowNotify('error', 'Vui lòng nhập nhãn file');
                    }
                }
            }
        });
    },
    RollbackFileExcelSiteStatus: function ()
    {
        //rollback trang thai file site
        var fsid = $(this).data('id');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn muốn Rollback trạng thái cho cửa hàng ?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'fsid', value: fsid });

                    commonUtils.postAjaxWithToken(_rollbackFileExcelSiteStatusUrl, postData, function (result) {
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
};

function BootboxContent(deadline) {
    var frm_str = '<form id="frmUpdateDeadline" class="form-inline">'
                   + '<div class="form-group" style="margin: 30px 0 20px 60px;">'
                      + '<label for="date" style="display:inline-block">NGÀY HẾT HẠN &emsp;</label>'
                      + '<div class="input-group col-md-8">'
                      + '<span class="input-group-addon"><i class="fa fa-calendar"></i></span>'
                      + '<input id="txtDeadline" class="date span2 form-control input-sm" size="16" type="text" style="float:none" value="' + deadline + '">'
                      + '</div>'
                      + '</div>'
                   + '</form>';

    var object = $('<div/>').html(frm_str).contents();
    object.find('.date').datetimepicker({ minDate: new Date() });
    return object;
}

function ExportMultipleFile() {
    var listCkb = $('.ckbExportedFid:checked');
    if (listCkb.length > 0)
    {
        var fids = $(listCkb[0]).val();
        var type = $(listCkb[0]).data('filetype');

        for (var i = 1; i < listCkb.length; i++)
        {
            if (type == $(listCkb[i]).data('filetype'))
            {
                fids += ',' + $(listCkb[i]).val();
            }
            else
            {
                fids = '';
                ShowNotify('error', 'Chỉ được xuất các file trong cùng loại file');
                break;
            }
        }

        if (fids.length > 0)
        {
            window.open(_exportFileExcelUrl + '?fid=' + fids);
        }
    }
    else
    {
        ShowNotify('error', 'Vui lòng check chọn các file cần xuất');
    }
}

function RemoveSearchTag() {
    $('#tag').val('0');
    $('#tagname').val('');
    $('#frm_search').submit();
}