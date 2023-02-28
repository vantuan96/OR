$(function () {
    userManager.init();
});

var userManager = {
    init: function () {
        $('[data-event=lock-unlock-user]').click(userManager.onShowPopupLockUser);
        $('[data-event=delete-user]').click(userManager.onShowPopupDeleteUser);
        $('[data-event=reset-pass-user]').click(userManager.onShowPopupResetPasswordUser);
        $('#user_lock .btn-cancel, #user_unlock .btn-cancel, #notice_delete_user .btn-cancel').click(userManager.hidePopup);

        $('#user_lock .btn-submit, #user_unlock .btn-submit').click(userManager.lockUnlockUser);
        $('#notice_delete_user .btn-submit').click(userManager.deleteUser);
        $('#notice_reset_password_user .btn-submit').click(userManager.resetPasswordUser);
    },
    
    onShowPopupLockUser: function () {
        var isLock = $(this).hasClass('act_unlock');
        var userName = $(this).data('name');
        var email = $(this).data('email');
        var box;
        if (isLock) {
            $('#user_lock .btn-submit').attr('data-id', $(this).data('id'));
            $('#user_lock .btn-submit').attr('data-lock', 'True');
            box = $('#user_lock');
        }
        else {
            $('#user_unlock .btn-submit').attr('data-id', $(this).data('id'));
            $('#user_unlock .btn-submit').attr('data-lock', 'False');
            box = $('#user_unlock');
        }

        $('#user_lock .user_fullname, #user_unlock .user_fullname').text(userName);
        $('#user_lock .user_email, #user_unlock .user_email').text(email).attr('href', 'mailto:' + email);

        box.modal('show');
    },
    lockUnlockUser: function () {
        var postData = new Array();
        var isLock = $(this).data('lock');
        var id = $(this).data('id');
        postData.push({ name: 'userId', value: id });
        postData.push({ name: 'lockStatus', value: isLock });
        ShowOverlay(true);
        commonUtils.postAjaxWithToken(_lockUrl, postData, function (ret) {
            if (ret.ID == 1) {
                ReloadWithMasterDB();
            }
            else {
                ShowNotify(ret.ID == 1 ? 'success' : 'error', ret.Message, 5000);
            }
            HideOverlay();
        });
    },
    onShowPopupDeleteUser: function () {
        var userName = $(this).data('name');
        var email = $(this).data('email');

        var box = $('#notice_delete_user');
        $('#notice_delete_user .btn-submit').attr('data-id', $(this).data('id'));
        $('#notice_delete_user .user_fullname').text(userName);
        $('#notice_delete_user .user_email').text(email).attr('href', 'mailto:' + email);

        box.modal('show');
    },
    deleteUser: function () {
        var postData = new Array();
        var id = $(this).data('id');
        postData.push({ name: 'userId', value: id });
        ShowOverlay(true);
        commonUtils.postAjaxWithToken(_deleteUrl, postData, function (ret) {
            if (ret.ID == 1) {
                ReloadWithMasterDB();
            }
            else {
                ShowNotify(ret.ID == 1 ? 'success' : 'error', ret.Message, 5000);
            }
            HideOverlay();
        });
    },
    onShowPopupResetPasswordUser: function () {
        var userName = $(this).data('name');
        var email = $(this).data('email');

        var box = $('#notice_reset_password_user');
        $('#notice_reset_password_user .btn-submit').attr('data-id', $(this).data('id'));
        $('#notice_reset_password_user .user_fullname').text(userName);
        $('#notice_reset_password_user .user_email').text(email).attr('href', 'mailto:' + email);
        
        box.modal('show');
    },
    resetPasswordUser: function () {
        var postData = new Array();
        var userId = $(this).data('id');
        var userAuthenId = $(this).data('authenid');
        postData.push({ name: 'userId', value: userId });
        postData.push({ name: 'userAuthenId', value: userAuthenId });
        ShowOverlay(true);
        commonUtils.postAjaxWithToken(_resetPasswordUrl, postData, function (ret) {
            if (ret.ID == 1) {
                window.location.reload();
            }
            else {
                ShowNotify(ret.ID == 1 ? 'success' : 'error', ret.Message, 3000);
            }
            HideOverlay();
        });
    },
    hidePopup: function () {
        $(this).closest('.modal_booking').find('.close').trigger('click');
    },
};


