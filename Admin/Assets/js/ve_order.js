$(function () {
    $('#frmSubmitListTO').submit(function (e) {
        if ($('input:checkbox:checked', $(this)).length == 0){
            ShowNotify('error', 'Vui lòng chọn đơn hàng cần thao tác !');
            e.preventDefault();
        }
    });

    $('.ddlPageSize').change(function (e) {
        var url = window.location.href;
        var pagesize = $(this).val();
        var rex = new RegExp("ps=[0-9]+");
        if (rex.test(url)) {
            url = url.replace(rex, 'ps=' + pagesize);
        }
        else {
            if (url.indexOf('?') > 0) {
                url += '&ps=' + pagesize;
            }
            else {
                url += '?ps=' + pagesize;
            }
        }

        rex = new RegExp("(p=[0-9]+&)|(&p=[0-9]+)");
        if (rex.test(url)) {
            url = url.replace(rex, '');
        }

        window.location.href = url;
    });


    $('.btnApproveShipmentBooking').click(function (e) {
        e.preventDefault();
        var shId = $(this).data('shid');
        bootbox.confirm({
            title: "CHÚ Ý",
            message: "Bạn có chắc chắn muốn duyệt thời gian hẹn giao của NCC này không?",
            callback: function (result) {
                if (result) {
                    $.post(urlApproveShipmentBooking, { shId: shId, isApprove: true }, function (data) {
                        if (data.ID > 0) {
                            ReloadWithMasterDB();
                        }
                        else {
                            bootbox.alert(data.Message);
                        }
                    });
                }
            }
        });

    });

    $('.btnNotApproveShipmentBooking').click(function (e) {
        e.preventDefault();
        var shId = $(this).data('shid');
        var oldScheduleDay = $(this).data('oldscheduleday');
        var oldLeadTimeId = $(this).data('oldleadtimeid');
        var purchaseOrderDate = $(this).data('purchaseorderdate');
        var desireDeliveryDate = $(this).data('desiredeliverydate');

        bootbox.dialog({
            message: BootboxNotApprove(oldScheduleDay, oldLeadTimeId, purchaseOrderDate, desireDeliveryDate),
            title: "Vui lòng nhập lý do không duyệt",
            buttons: {
                main: {
                    label: "Xác nhận",
                    className: "btn-success",
                    callback: function () {
                        var note = $.trim($('#frmNotApprove #note').val());
                        var scheduleDay = $('#frmNotApprove #scheduleDate').val();
                        var leadtimeId = $('#frmNotApprove #ddlLeadTimeHour').val();

                        if (note.length == 0) 
                        {
                            ShowNotify('error', 'Vui lòng nhập lý do không duyệt');
                            return false;
                        }

                        if (scheduleDay.length > 0 && leadtimeId == 0)
                        {
                            ShowNotify('error', 'Vui lòng chọn khung giờ');
                            return false;
                        }

                        if (scheduleDay.length == 0 && leadtimeId > 0) {
                            ShowNotify('error', 'Vui lòng chọn ngày mong muốn giao hàng');
                            return false;
                        }

                        if (scheduleDay.length > 0 && leadtimeId > 0) {
                            if (scheduleDay == oldScheduleDay && leadtimeId == oldLeadTimeId) {
                                ShowNotify('error', 'Ngày mong muốn giao không được trùng với ngày hẹn giao cũ');
                                return false;
                            }
                        }

                        $.post(urlApproveShipmentBooking, { shId: shId, isApprove: false, note: note, date: scheduleDay, leadTimeId: leadtimeId }, function (data) {
                            if (data.ID > 0) {
                                ReloadWithMasterDB();
                            }
                            else {
                                bootbox.alert(data.Message);
                            }
                        });
                    }
                }
            }
        });
    });
});


function ComfirmManyOrders() {
    var param = [];
    $('.chkSumit:checkbox:checked').each(function () {
        param.push({ ToId: $(this).data('toid'), ShId : $(this).data('shid')});
    });

    
        
    if (param.length > 0) {
        //param = JSON.stringify({ 'param': param });
        bootbox.confirm({
            title: "XÁC NHẬN ĐƠN HÀNG",
            message: "Bạn muốn xác nhận " + param.length + " đơn hàng ?",
            callback: function (confirm) {
                if (confirm) {

                    $.ajax({
                        url: urlConfirmManyOrders,
                        type: "POST",
                        data: { param: param },
                        dataType: 'json',
                        async: false,
                        cache: false,
                        success: function (result) {
                            if (result.ID > 0) {
                                ReloadWithMasterDB();
                            }
                            else {
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
        });
    }
    else {
        ShowNotify('error', 'Vui lòng chọn đơn hàng cần thao tác !');
    }
}

function BootboxNotApprove(scheduleDate, leadTimeId, startDate, endDate) {
    var frm_str = '<form id="frmNotApprove" class="form-inline">'
                    + '<div class="row">'
                        + '<div class="col-md-8 col-md-offset-1">'
                            + '<label for="note">'
                                + 'Nhập lý do không duyệt <span class="text-red"">(*)</span>'
                            + '</label>'
                            + '<input type="text" id="note" name="note" class="form-control" style="width:100%" />'
                        + '</div>'
                    + '</div>'
                    + '<div class="row">'
                        + '<div class="col-md-5 col-md-offset-1">'
                            + '<div class="form-group">'
                                + '<label for="date" style="display:inline-block">'
                                    + 'Nhập ngày mong muốn  '
                                + '</label>'
                                + '<div class="input-group col-md-12">'
                                    + '<span class="input-group-addon">'
                                        + '<i class="fa fa-calendar">'
                                        + '</i>'
                                    + '</span>'
                                   + '<input id="scheduleDate" data-date-format="dd/mm/yyyy" value="" class="date span2 form-control input-sm" size="16" type="text" style="float:none">'
                                + '</div>'
                            + '</div>'
                        + '</div>'
                        + '<div class="col-md-3">'
                            + '<label for="date" style="display:inline-block">'
                                + 'Khung giờ'
                            + '</label>'
                            + '<div class="input-group col-md-12">'
                                + $('#divSelectLeadTime').html()
                            + '</div>'
                        + '</div>'
                    + '</div>'
                + '</form>';
    var object = $('<div/>').html(frm_str).contents();
    object.find('#scheduleDate').datepicker(
        {
            startDate: new Date(startDate),
            endDate: new Date(endDate),
            autoclose: true
        }
    );
    return object
}


/*

function fnDeliveredOrder(toId, purchaseOrderDate) {
    //Show the datepicker in the bootbox
    bootbox.dialog({
        message: BootboxContent(purchaseOrderDate),
        title: "Xác nhận đã giao hàng thành công",
        buttons: {
            main: {
                label: "Xác nhận",
                className: "btn-success",
                callback: function()
                {
                    var date = $('#deliveredDate').val();
                    if (date.length == 0)
                    {
                        ShowNotify('error', 'Vui lòng nhập ngày giao hàng');
                    }
                    else
                    {
                        $.ajax({
                            url: urlDeliveredOrder,
                            type: "POST",
                            data: {
                                toId: toId,
                                date: date,
                                "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val()
                            },
                            dataType: 'json',
                            async: false,
                            cache: false,
                            success: function (result) {
                                if (result.status.toLowerCase() == "ok") {
                                    ReloadWithMasterDB();
                                } else {
                                    ShowNotify('error', result.message);
                                }
                            },
                            error: function () {
                                ShowNotify('error', response.Message);
                            }
                        });
                    }
                }
            }
        }
    });
}


function BootboxContent(purchaseOrderDate) {
    var frm_str = '<form id="frmDelivered" class="form-inline">'
                   + '<div class="form-group" style="margin: 30px 0 20px 60px;">'
                      + '<label for="date" style="display:inline-block">NHẬP NGÀY GIAO HÀNG &emsp;</label>'
                      + '<div class="input-group col-md-8">'
                      + '<span class="input-group-addon"><i class="fa fa-calendar"></i></span>'
                      + '<input id="deliveredDate" class="date span2 form-control input-sm" size="16" type="text" style="float:none">'
                      + '</div>'
                      + '</div>'
                   + '</form>';

    var object = $('<div/>').html(frm_str).contents();
    object.find('.date').datetimepicker({ minDate: new Date(purchaseOrderDate) });
    return object
}

*/