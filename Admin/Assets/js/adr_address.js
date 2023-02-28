adr_address = {
    getAddress: function (idGet, targetAppend, type, defaultVal, isAsync, delayElements) {
        defaultVal = typeof defaultVal !== 'undefined' ? defaultVal : "";
        isAsync = typeof isAsync !== 'undefined' ? isAsync : true;
        delayElements = typeof delayElements !== 'undefined' ? delayElements : '#' + targetAppend;

        var loading = '<span class="ion-loading-a loading-address" style="position:absolute;margin-left: -18px;margin-top: 10px;"></span>';
        $(delayElements).attr('readonly', 'readonly');
        $(loading).insertAfter($(delayElements));

        if (idGet == '' || idGet == '0') {
            $("#" + targetAppend).html("<option value=''>--- Chọn ---</option>");
            $(delayElements).removeAttr('readonly');
            $('.loading-address').remove();
            if (isAsync) { $("#" + targetAppend).trigger("change"); }
        }
        else {
            $.ajax({
                url: "/as-addr-mc",
                type: "post",
                async: isAsync,
                data: { "id": idGet, "type": type },
                success: function (result) {
                    try {
                        var options = '<option value="">--- Chọn ---</option>';
                        if (result.length != 0) {
                            for (var i = 0; i < result.length; i++) {
                                options += "<option " + (result[i].Id == defaultVal ? "selected='selected'" : "") + " value='" + result[i].Id + "'>" + result[i].Name + "</option>";
                            }
                        }
                        else {
                            options = "<option value=''>Không có dữ liệu</option>";
                        }

                        $("#" + targetAppend).html(options);
                        $(delayElements).removeAttr('readonly');
                        $('.loading-address').remove();
                        if (isAsync) { $("#" + targetAppend).trigger("change"); }

                    } catch (e) {
                        window.location.reload();
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    if (isAsync) {
                        alert("Lỗi kết nối với Address Service. Vui lòng thử lại");
                    }

                    $(delayElements).removeAttr('readonly');
                    $('.loading-address').remove();
                }
            });
        }
    },
}
