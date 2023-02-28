$(function () {
    vegcombo.init();
});

var vegcombo = {
    init: function () {
        
        if ($('[data-event=cancel-file]').length > 0) {
            $('[data-event=cancel-file]').click(vegcombo.Cancel);
        }
        
        if ($('[data-event=complete-update]').length > 0) {
            $('[data-event=complete-update]').click(vegcombo.CompleteUpdate);
        }

        if ($('[data-event=complete]').length > 0) {
            $('[data-event=complete]').click(vegcombo.Complete);
        }

        if ($('[data-event=confirm-site]').length > 0) {
            $('[data-event=confirm-site]').click(vegcombo.ConfirmSite);
        }

        if ($('[data-event=update-deadline]').length > 0) {
            $('[data-event=update-deadline]').click(vegcombo.UpdateDeadline);
        }

        if ($('[data-event=rollback]').length > 0) {
            $('[data-event=rollback]').click(vegcombo.Rollback);
        }

        if ($('[data-event=remake]').length > 0) {
            $('[data-event=remake]').click(vegcombo.Remake);
        }
    },
    
    Cancel: function () {
        var id = $(this).data('id');
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn muốn hủy đợt đăng ký này?",
            callback: function (confirm) {
                if (confirm) {
                    $.ajax({
                        url: _cancelFileUrl,
                        type: "POST",
                        data: {
                            ComboRtId: id
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
    Complete: function () {
        var id = $(this).data('id');
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn muốn đóng đợt đăng ký này?",
            callback: function (confirm) {
                if (confirm) {
                    $.ajax({
                        url: _completeRegistrationTimeUrl,
                        type: "POST",
                        data: {
                            ComboRtId: id
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
    CompleteUpdate: function () {
        var ComboRsId = $(this).data('ComboRsId');
        
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn sẽ không thể cập nhật số lượng đăng ký combo sau khi hoàn tất. Bạn có muốn hoàn tất không?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'ComboRsId', value: ComboRsId });

                    commonUtils.postAjaxWithToken(_completeUpdateUrl, postData, function (result) {
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
                    postData.push({ name: 'ComboRsId', value: id });

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
    UpdateDeadline: function ()
    {
        var id = $(this).data('id');
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
                                    ComboRtId: id,
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
                                error: function (response) {
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
    Rollback: function ()
    {
        //rollback trang thai file site
        var id = $(this).data('id');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn muốn Rollback trạng thái cho cửa hàng ?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'ComboRsId', value: id });

                    commonUtils.postAjaxWithToken(_rollbackUrl, postData, function (result) {
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
    Remake: function () {
        //nạp lại dữ liệu
        var id = $(this).data('id');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn muốn nạp lại dữ liệu ?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'ComboRtId', value: id });

                    commonUtils.postAjaxWithToken(_remakeUrl, postData, function (result) {
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
