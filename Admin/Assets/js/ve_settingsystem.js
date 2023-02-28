$(function () {
    settingSystemManager.init();
});

var settingSystemManager = {
    init: function () {
        $('[data-event=comfirm-show-default-password]').click(settingSystemManager.onShowPopupConfirmShowPassword);
        $('#recheck_password_user .btn-cancel').click(settingSystemManager.hidePopup);
        $('#recheck_password_user .btn-submit').click(settingSystemManager.confirmShowDefaultPassword);
    },
    onShowPopupConfirmShowPassword: function () {
        var box = $('#recheck_password_user');
        var isShow = $("[data-event=comfirm-show-default-password]").attr("data-show");
        if (isShow) {
            $('[data-id=UserDefaultPassword]').popover("hide");
            box.modal('show');
        }
        else {
            
            $('[data-id=UserDefaultPassword]').on("shown", function (e, editable) {
                editable.input.$input.val($('[data-id=UserDefaultPassword]').html());
            });
        }
        
    },
    confirmShowDefaultPassword: function () {
        var postData = new Array();
        var passConfirm = $("#txtPassword").val();
        postData.push({ name: 'password', value: passConfirm });
        ShowOverlay(true);
        commonUtils.postAjaxWithToken(_confirmShowDefaultPasswordUrl, postData, function (ret) {
            if (ret.IsSuccess == 1) {
                //Set show password
                //console.log($('[data-event=comfirm-show-default-password]'));
                $("[data-event=comfirm-show-default-password]").html(ret.Message);
                //$("[data-event=comfirm-show-default-password]").addClass("editable editable-click settingvalue");
                $("[data-event=comfirm-show-default-password]").attr("data-show", false);
                $("[data-event=comfirm-show-default-password]").attr("data-event", "");
                $('[data-id=UserDefaultPassword]').on("shown", function (e, editable) {
                    editable.input.$input.val(ret.Message);
                });
                $('#recheck_password_user').modal('hide');
                HideOverlay();
                $('[data-id=UserDefaultPassword]').click();
            }
            else {
                ShowNotify(ret.IsSuccess == 1 ? 'success' : 'error', ret.Message, 3000);
                HideOverlay();
            }
        });
    },
    hidePopup: function () {
        $(this).closest('.modal_booking').find('.close').trigger('click');
    },
};


