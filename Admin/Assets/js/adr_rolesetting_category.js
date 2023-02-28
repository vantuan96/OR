$(function () {
    bootbox.setDefaults({
        locale: "vi"
    });

    $("button").keypress(function () {
        return false;
    });
    $('[data-event=search-user]').click(function () {
        $.ajax({
            url: _searchLink,
            type: "POST",
            data: { "CateID": $('#hdCateIDSearch').val(), "Keyword": $("#txtSearchUser").val(), "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val() }
        }).done(function (res) {
            if (res != "") {
                $('#dUserList').html(res.html);

                $('[data-event=add-user]').click(function () {
                    var formTag = $(this).parent("form");
                    bootbox.confirm("Bạn có chắc muốn phân quyền người dùng này vào ngành hàng ?", function (result) {
                        if (result) {
                            formTag.submit();
                        }
                    });
                });
            }
        });
    });

    $('[data-event=remove-user]').click(function () {
        var formTag = $(this).parent("form");
        bootbox.confirm("Bạn có chắc muốn xóa phân quyền ngành hàng của người dùng này ?", function (result) {
            if (result) {
                formTag.submit();
            }
        });
    });

    $('[data-event=add-user]').click(function () {
        var formTag = $(this).parent("form");
        bootbox.confirm("Bạn có chắc muốn phân quyền người dùng này vào ngành hàng ?", function (result) {
            if (result) {
                formTag.submit();
            }
        });
    });

    if (_alertData != null) {
        noty({
            text: _alertData.Message,
            type: _alertData.ID == 1 ? "success" : "error",
            theme: 'ad_theme',
            layout: _notifyPosition,
            closeWith: ['button'],
            maxVisible: 1,
            killer: true,
            timeout: 5000
        });
    }

    $('#ddlCate').change(function () {
        location.href = _detailsLink.replace("{token}", $(this).val());
    });
});