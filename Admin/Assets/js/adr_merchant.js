$(function () {
    
    bootbox.setDefaults({
        locale: "vi_submit",
        backdrop: false
    });

    if (_alertData != null) {
        showNoty(_alertData.ID, _alertData.Message);
    }

    $("[data-event=changeLevelType]").change(function () {
        var tag = $(this);
        bootbox.confirm(generateAlertFormHtml("Vui lòng ghi rõ lý do thay đổi cấp độ merchant"), function (result) {
            if (result) {
                if ($('#txtComment').val() != "") {
                    $.ajax({
                        url: changeLevelLink,
                        type: "POST",
                        data: { "MDID": tag.attr("data-id"), "LMTID": tag.val(), "Comment": $('#txtComment').val() }
                    }).done(function (res) {
                        if (res.Status == 1) {
                            tag.attr("data-def-val", tag.val());
                        }
                        else {
                            tag.val($(tag).attr("data-def-val"));
                        }

                        showNoty(res.Status, res.ReturnMsg);

                        $("select[data-jid=" + $(tag).attr("data-jid") + "]").val($(tag).val());

                        //noty({
                        //    text: res.ReturnMsg,
                        //    type: res.Status == 1 ? "success" : "error",
                        //    theme: 'ad_theme',
                        //    layout: _notifyPosition,
                        //    closeWith: ['button'],
                        //    maxVisible: 2,
                        //    killer: true,
                        //    timeout : 5000
                        //});
                    });
                }
                else {
                    alert("Vui lòng nhập lý do");
                    return false;
                }
            }
            else {
                tag.val(tag.attr("data-def-val"));
            }
        });
    });

    $("[data-event=approve-merchant-1]").click(function () {
        var tag = $(this);
        bootbox.confirm(generateAlertFormHtml("Vui lòng nhập thêm lý do (nếu có)"), function (result) {
            if (result) {
                $.ajax({
                    url: approveMerchantFirstLink,
                    type: "POST",
                    data: { "MID": tag.attr("data-id"), "StatusID": 1, "Comment": $('#txtComment').val() }
                }).done(function (res) {
                    window.location.reload();
                });
            }
            else {
                tag.val(tag.attr("data-def-val"));
            }
        });
    });

    $("[data-event=approve-merchant-2]").click(function () {
        var tag = $(this);
        bootbox.confirm(generateAlertFormHtml("Vui lòng nhập thêm lý do (nếu có)"), function (result) {
            if (result) {
                $.ajax({
                    url: approveMerchantSecondLink,
                    type: "POST",
                    data: { "MID": tag.attr("data-id"), "StatusID": 2, "Comment": $('#txtComment').val() }
                }).done(function (res) {
                    window.location.reload();
                });
            }
            else {
                tag.val(tag.attr("data-def-val"));
            }
        });
    });


    $("[data-event=resend-merchant]").click(function () {
        var tag = $(this);
        bootbox.confirm(generateAlertFormHtml("Vui lòng nhập thêm lý do (nếu có)"), function (result) {
            if (result) {
                $.ajax({
                    url: reSendMerchantLink,
                    type: "POST",
                    data: { "MID": tag.attr("data-id"),  "Comment": $('#txtComment').val() }
                }).done(function (res) {
                    if (res.Status) {
                        alert('Đã cập nhập thành công');
                        window.location.reload();
                    }
                    else {
                        alert('Cập nhập thất bại');
                    }
                    
                });
            }
            else {
                tag.val(tag.attr("data-def-val"));
            }
        });
    });

  
    $("[data-event=rejectMerchant]").click(function () {
        var tag = $(this);
        bootbox.confirm(generateAlertFormHtml("Vui lòng nhập lý do từ chối"), function (result) {
            if (result) {
                if ($('#txtComment').val() != "") {
                    $.ajax({
                        url: rejectMerchantLink,
                        type: "POST",
                        data: { "MID": tag.attr("data-id"), "StatusID": 3, "Comment": $('#txtComment').val() }
                    }).done(function (res) {
                        if (res.Status) {
                            alert('Đã cập nhập thành công');
                            window.location.reload();
                        }
                        else {
                            alert('Cập nhập thất bại');
                        }
                    });
                }
                else{
                    alert("Vui lòng nhập lý do");
                    return false;
                }
            }
            else {
                tag.val(tag.attr("data-def-val"));
            }
        });
        //if (confirm("Bạn có chắc muốn từ chối merchant này??")) {
        //    var tag = $(this);
        //    $.ajax({
        //        url: rejectMerchantLink,
        //        type: "POST",
        //        data: { "MID": $(this).attr("data-id"), "StatusID": 3 }
        //    }).done(function (res) {
        //        if (res.Status == true) {
        //            $("button[data-id=" + $(tag).attr("data-id") + "]").parent().html("<i class='fa fa-times' style='color: red;font-size:20px;'></i>&nbsp;Từ chối");
        //        }
        //        showNoty(res.Status, res.ReturnMsg);
        //    });
        //}
    });

    $("[data-event=change-exclusive]").change(function () {
        var tag = $(this);
        bootbox.confirm(generateAlertFormHtml("Vui lòng nhập thêm lý do (nếu có)"), function (result) {
            if (result) {
                $.ajax({
                    url: updateMerchantExclusiveLink,
                    type: "POST",
                    data: { "MDID": tag.attr("data-id"), "IsExclusive": tag.val(), "Comment": $('#txtComment').val() }
                }).done(function (res) {
                    showNoty(res.Status, res.ReturnMsg);
                });
            }
            else {
                //console.log(tag.attr("data-def-val"));
                tag.val(tag.attr("data-def-val"));
            }
        });
    });

    
    //$.fn.powerTip.defaults.smartPlacement = true;
    //$('.pTip_top_right').powerTip({ placement: 'ne' });
    //$('.pTip_mouse_on.view-details').data('powertipjq', $(['<p><b>Xem chi tiết merchant</b></p>'].join('\n')));
    //$('.pTip_mouse_on.view-deals').data('powertipjq', $(['<p><b>Xem danh sách deal của merchant</b></p>'].join('\n')));
    //$('.pTip_mouse_on.create-deal').data('powertipjq', $(['<p><b>Tạo deal</b></p>'].join('\n')));
    //$('.pTip_mouse_on.create-contract').data('powertipjq', $(['<p><b>Tạo hợp đồng</b></p>'].join('\n')));
    //$('.pTip_mouse_on.view-contract').data('powertipjq', $(['<p><b>Xem chi tiết hợp đồng</b></p>'].join('\n')));
    //$('.pTip_mouse_on').powerTip({
    //    placement: 'ne'
    //});
});

function showNoty(status, mesg) {
    noty({
        text: mesg,
        type: status > 0 ? "success" : "error",
        theme: 'ad_theme',
        layout: _notifyPosition,
        closeWith: ['button'],
        maxVisible: 2,
        killer: true,
        timeout: 3000
    });
}
function showPopup(id) {
    $('[data-event=view-deal][data-id=' + id + ']').click();
}
function closePopup() {
    $('#popup_viewer .close').click();
}
function generateAlertFormHtml(alertMsg) {
    var formHtml = "<form id='frmComment' data_parsley_validate novalidate>" +
                    "<p>" + alertMsg + "</p>" +
                    "<input type='text' placeholder='Lý do' name='Comment' id='txtComment' data_parsley_required ='true' data_parsley_length = '[1, 2000]' class='form-control' /></form>";
    return formHtml;
}