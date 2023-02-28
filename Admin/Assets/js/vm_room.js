$(function () {
    roomManager.init();
});

var roomManager = {
    init: function () {
        $('[data-event=hide-show-room]').click(roomManager.hideOrShow);
        $('[data-event=delete-room]').click(roomManager.deleteRoom);
    },
    hideOrShow: function () {
        var id = $(this).data('id');
        var actiontype = $(this).attr('data-action');
        bootbox.confirm({
            title: "CHÚ Ý",
            message: (actiontype == "act-show") ? "Bạn có chắc muốn hiển thị phòng này chứ?" : "Bạn có chắc muốn ẩn phòng này chứ?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'id', value: id });
                    postData.push({ name: 'actiontype', value: actiontype });
                    ShowOverlay(true);
                    commonUtils.postAjaxWithToken(_hideOrShowUrl, postData, function (ret) {
                        if (ret.ID == 1) {
                            ReloadWithMasterDB();
                        }
                        else {
                            ShowNotify(ret.ID == 1 ? 'success' : 'error', ret.Message, 3000);
                        }
                        HideOverlay();
                    });
                }
            }
        });
    },
    deleteRoom: function () {
        var id = $(this).data('id');
        bootbox.confirm({
            title: "CHÚ Ý",
            message: "Bạn có chắc muốn xóa phòng này không?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'id', value: id });
                    ShowOverlay(true);
                    commonUtils.postAjaxWithToken(_deleteUrl, postData, function (ret) {
                        if (ret.ID == 1) {
                            ReloadWithMasterDB();
                            //ShowNotify(ret.ID == 1 ? 'success' : 'error', ret.Message, 3000);
                            //ReloadWithDelay(3000);
                        }
                        else {
                            ShowNotify(ret.ID == 1 ? 'success' : 'error', ret.Message, 3000);
                        }
                        HideOverlay();
                    });
                }
            }
        });
    }
};


