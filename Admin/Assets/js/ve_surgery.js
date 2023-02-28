$(function () {
    surgeryManager.init();
});

var surgeryManager = {
    init: function () {
        $('[data-event=export-receipt-surgery]').click(surgeryManager.onShowPopupPrintReceiptSurgery);
        $('[data-event=delete-surgery]').click(surgeryManager.onShowPopupDeleteSurgery);
        $('#user_lock .btn-cancel, #user_unlock .btn-cancel, #notice_delete_surgery .btn-cancel').click(surgeryManager.hidePopup);

        $('#notice_delete_surgery .btn-submit').click(surgeryManager.deleteSurgery);
    },

    onShowPopupPrintReceiptSurgery: function () {
        var box = $('#print_receipt_surgery');
        var pgid = $(this).attr('data-id');
        //$('#print_receipt_surgery .btn-submit').attr('data-id', $(this).data('id'));
        //$('#print_receipt_surgery .user_code').text($(this).data('code'));
        //$('#print_receipt_surgery .service_name').text($(this).data('service'));
        //Get surgery information
        $.ajax({
            url: '/OR/GetReceiptSurgeryInfo/',
            type: 'GET',
            dataType: 'json',
            data: {
                pgid: pgid,
                __RequestVerificationToken: $('[name=__RequestVerificationToken]').val()
            },
            async: false,
            success: function (response) {
                var entity = JSON.parse(JSON.stringify(response));
                if (entity != null) {
                    $('#data-pg-id').val(entity.Id);
                    JsBarcode(".barcode", entity.PId, { height: 60 });
                    $('#mainReport #print_FullName').html(entity.HoTen);
                    $('#mainReport #print_Sex').html(entity.Sex);
                    $('#mainReport #print_DOB').html(entity.Dob);
                    $('#mainReport #print_Diagnosis').html(entity.Diagnosis);
                    $('#mainReport #print_Indication').html(entity.HpServiceName);
                    $('#mainReport #print_Surgeon').html(entity.NamePTVMain);
                    $('#mainReport #print_Operation_Date').html(entity.ShowdtOperation);
                    $('#mainReport #print_Operation_Time').html(entity.ShowdtStart);

                    $('#mainReport #print_Admission_Date').html(entity.ShowdtAdmission);
                    $('#mainReport #print_Admission_Time').html(entity.ShowTimeAdmission);

                    $('#mainReport #print_AdmissionWard').html(entity.AdmissionWard);
                    $('#mainReport #print_RegNote').html(entity.RegDescription);

                    $('#mainReport #print_NoFoodFrom').html(entity.NoFoodFrom);
                    $('#mainReport #print_NoDrinkFrom').html(entity.NoDrinkFrom);
                    $('#mainReport #print_AdvanceAmount').html(entity.AdvanceAmount);
                }
            },
            error: function (response) {
                alert("Đã có lỗi xảy ra");
            }
        });
        $('.editValueReceiptSurgery').editable({
            validate: function (value) {
                var result = UpdateReceiptSurgery($('#data-pg-id').val(), $(this).data("key"), $.trim(value.replace(",", "")));
                $("#" + $(this).data("key")).html(value);
                if (result != "") return result;
            }
        });
        box.modal('show');
    },

    onShowPopupDeleteSurgery: function () {
        var box = $('#notice_delete_surgery');
        $('#notice_delete_surgery .btn-submit').attr('data-id', $(this).data('id'));
        $('#notice_delete_surgery .user_code').text($(this).data('code'));
        $('#notice_delete_surgery .service_name').text($(this).data('service'));

        box.modal('show');
    },
    deleteSurgery: function () {
        var postData = new Array();
        var id = $(this).data('id');
        postData.push({ name: 'id', value: id });
        postData.push({ name: 'type', value: 2 });
        ShowOverlay(true);
        //console.log(postData);
        commonUtils.postAjaxWithToken(_deleteUrl, postData, function (ret) {
            if (ret.ID >0) {
                ShowNotify('success', ret.Message, 2000);
                ReloadWithMasterDB();
            }
            else {
                ShowNotify('error', ret.Message, 5000);
            }
            HideOverlay();
        });
    },
    hidePopup: function () {
        $(this).closest('.modal_booking').find('.close').trigger('click');
    },
};


