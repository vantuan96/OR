

$(function() {
    $('#x_txt_fromdate,#x_txt_todate').datepicker({
        todayHighlight: true
    });
});

var errorlog =
{
    getDetailErrorLog: function (elt) {
        el = elt;
        $('#table_question').find('tr').removeAttr('style');
        $('#table_question').find('.btn-clear').removeClass('disabled');
        $(el).closest('tr').css('background-color', 'cornsilk');
        $(el).addClass('disabled');
        errorlog.viewDetailErrorLog();
    },
    viewDetailErrorLog: function ()
    {
        this.element = $(el);

        $('#logId').val(this.element.data("id"));
        $('#appId').val(this.element.data("appid"));
        $('#createDate').val(this.element.data("createdate"));

        this.element.tagcontent = $("#cont_errorlog_only");
        this.element.tagcontent.empty();

        this.element.tagcontent.append($("<label style='clear:both;'>Nội dung</label><textarea disabled='disabled' rows='28' style='width: 100%;padding-left: 5px;text-indent: 0px !important;resize:none;'>" + this.element.data("content") + "</textarea>"));
       
    }
}