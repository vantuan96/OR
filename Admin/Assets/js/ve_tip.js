function Ve_Tip(id, version, title, content) {
    this.TipId = id;
    this.TipVersion = version;
    this.TipTitle = title;
    this.TipContent = content;
    this.TipUnderstoodCheckbox = '<p style="margin-top:10px"><label><input id="understoodTip" type="checkbox" /> <span>Đã hiểu và không thông báo lại lần sau.</span></label></p>';

    this.ShowTip = function (uid) {
        var tipMessage = this.TipContent + this.TipUnderstoodCheckbox;
        this.ShowDialog(uid, this.TipId, this.TipVersion, this.TipTitle, tipMessage);
    };
    this.ShowTipWithParams = function (uid, params) {
        var tipMessage = this.StringFormat(this.TipContent, params) + this.TipUnderstoodCheckbox;
        this.ShowDialog(uid, this.TipId, this.TipVersion, this.TipTitle, tipMessage);
    };
    this.ShowDialog = function (_uid, _tipid, _tipversion, _tiptitle, _tipmessage) {
        var thistip = this;
        if (thistip.CheckCookie(_uid, _tipid, _tipversion)) {
            bootbox.dialog({
                title: _tiptitle,
                message: _tipmessage,
                buttons: {
                    success: {
                        label: "<i class='fa fa-times'></i> Đóng",
                        className: "btn-primary",
                        callback: function () {
                            if ($('#understoodTip').is(':checked')) {
                                thistip.SetCookie(_uid, _tipid, _tipversion);
                            }
                        }
                    }
                }
            });
        }
    };
    this.CheckCookie = function (uid, tipid, version) {
        if (version == '') return false;

        var cookiename = this.BuildCookieName(uid, tipid, version);
        return ($.cookie(cookiename) != 1);
    };
    this.SetCookie = function (uid, tipid, version) {
        var cookiename = this.BuildCookieName(uid, tipid, version);
        var expires = new Date();
        expires.setTime(expires.getTime() + (1000 * 60 * 60 * 24 * 180)); // hết hạn sau 180 ngày
        $.cookie(cookiename, 1, { expires: expires });
    };
    this.BuildCookieName = function (uid, tipid, version) {
        return 'Tip_' + uid + '_' + tipid + '_' + version;
    };
    this.StringFormat = function (str, obj) {
        return str.replace(/\{\s*([^}\s]+)\s*\}/g, function (m, p1, offset, string) {
            return obj[p1]
        })
    };
}

//var TipUpdatePoPlanningQuantity = new Ve_Tip('1', '', 'CHÚ Ý', 'Bạn đang chỉnh sửa từ dòng {0} đến dòng {1} trong tổng số {2} dòng.<br/>Sau khi "Lưu thay đổi", bạn phải bấm qua trang để chỉnh sửa các dòng còn lại.');
var TipDownloadPoPlanningTemplate = new Ve_Tip('2', '', 'CHÚ Ý', 'File cấu trúc đã được cập nhật ngày <b>{0}</b>. Vui lòng download file mới nhất để sử dụng.');
//var TipUpdateMinStock = new Ve_Tip('3', '1', 'CHÚ Ý', 'Bạn đang chỉnh sửa từ dòng {0} đến dòng {1} trong tổng số {2} dòng.<br/>Sau khi "Lưu thay đổi", bạn phải bấm qua trang để chỉnh sửa các dòng còn lại trước khi bấm nút "Hoàn tất cập nhật số lượng".');
var TipIntro = new Ve_Tip('4', '1', 'CHÚ Ý', 'Bạn có thể click vào biểu tượng <button class="btn btn-warning"><i class="fa fa-lightbulb-o"></i></button> ở góc trên bên phải để xem hướng dẫn sử dụng.');
