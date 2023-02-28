var urlDeal = [];
urlDeal.push("gia-han-chuong-trinh");
urlDeal.push("cap-nhat-gia-han-chuong-trinh");
urlDeal.push("dang-va-keo-chuong-trinh");
urlDeal.push("cap-nhat-dang-va-keo-chuong-trinh");
urlDeal.push("lich-chay-chuong-trinh");
urlDeal.push("cap-nhat-lich-chay-chuong-trinh");

$(function () {
    // Gia hạn chương trình
    $('.btn-gia-han').click(function (e) {
        ShowOverlay();
        var self = $(this);
        var DID = self.parent().data('did');

        $.ajax({
            type: "POST",
            url: urlDeal[0],
            data: { "DID": DID },
            success: function (response) {
                $("#dealExtend").html(response);

                // xử lý cho form
                var date = new Date();
                date.setDate(date.getDate() + 1);
                $('.ts_datepicker').datepicker({
                    todayHighlight: true,
                    startDate: (date),
                    endDate: $("#LimitEndDate").val()
                });

                // neu nganh hang cha la du lich, max ngay gia han = VoucherExpireDate - DivisionMaxOnDateForTravelPromotion
                $('.ts_datepicker').datepicker('setDate', $('#deal-enddate').val());

                $.noty.closeAll();
                $(self.data('href')).modal();

                // cập nhật form
                $('#btn-update').click(function (e) {
                    var newDate = $("#number-date").val();
                    var did = $("#extendDID").val();
                    $('.upd-reason').val($('.upd-reason').val().replace(/\s+/gi, " ").trim());
                    var reason = $('.upd-reason').val();
                    var startDate = $("#startDate").text();

                    $("#frm-model").parsley().validate();
                    if ($("#frm-model").parsley().isValid()) {
                        ShowOverlay();
                        $.ajax({
                            url: urlDeal[1],
                            type: "POST",
                            data: { "did": did, "newDate": newDate, "reason": reason, "startDate": startDate, "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val() },
                            success: function (result) {
                                if (result.ID == 1) {
                                    $('#btn-update').attr('disabled', 'disabled');
                                    // không cần hiện thông báo lên vì thông báo được gắn vào tempdata trong action
                                    location.reload();
                                }
                                else {
                                    $('#btn-update').removeAttr('disabled');
                                    ShowNotify('error', result.Message, 2000);
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                bootbox.alert(msg_lang.CMS_GetRuntimeErrorMsg);
                            }
                        });
                    }
                });
            }
        });
    });

    // 1 - kéo chương trình, 2 - đăng chương trình
    $('.btn-update-type').click(function (e) {
        ShowOverlay();
        var self = $(this);
        var DID = self.parent().data('did');
        var updateType = self.parent().data('type-update'); // biến này dùng để phân biệt trường hợp up chương trình và kéo chương trình, vì 2 cái y chang nhau nên dùng biên phân biệt

        $.ajax({
            type: "POST",
            url: urlDeal[2],
            data: { "DID": DID },
            success: function (response) {
                $('#dealPullPush').html(response);

                // Xử lý cho form
                if (updateType == "pushdeal") {
                    $('.modal-title').text('Đăng Chương trình');
                    $('#updateType').val(2);
                }
                else if (updateType == "pulldeal") {
                    $('.modal-title').text('Kéo Chương trình');
                    $('#updateType').val(1);
                }

                $.noty.closeAll();
                $(self.data('href')).modal();

                // Lưu form
                $('.submit-deal').click(function (e) {
                    var did = $("#pullpushdDID").val();
                    $('.upd-pull-push-reason').val($('.upd-pull-push-reason').val().replace(/\s+/gi, " ").trim());
                    var reason = $('.upd-pull-push-reason').val();
                    var updateType = $('#updateType').val();

                    $("#validate-deal").parsley().validate();
                    if ($("#validate-deal").parsley().isValid()) {
                        ShowOverlay();
                        $.ajax({
                            url: urlDeal[3],
                            type: "POST",
                            data: { "reason": reason, "did": did, "updateType": updateType, "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val() },
                            success: function (result) {
                                if (result.ID == 1) {
                                    $('.submit-deal').attr('disabled', 'disabled');
                                    // không cần hiện thông báo lên vì thông báo được gắn vào tempdata trong action
                                    location.reload();
                                }
                                else {
                                    ShowNotify('error', result.Message, 2000);
                                    $('.submit-deal').removeAttr('disabled');
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                bootbox.alert(msg_lang.CMS_GetRuntimeErrorMsg);
                            }
                        });
                    }
                });
            }
        });
    });

    // Chạy chương trình
    $('.btn-update-cal').click(function (e) {
        ShowOverlay();
        var self = $(this);
        var DID = self.parent().data('did');

        $.ajax({
            type: "POST",
            url: urlDeal[4],
            data: { "DID": DID },
            success: function (response) {
                $('#dealOnOff').html(response);

                // Sử lý form
                var strTime = $('#allowTime').val();
                var arrTime = strTime.split(",");
                var typeID = $("#typeID").val();
                var dealOnsite = $("#dealOnsite").val();
                var dealVCEx = $("#dealVCEx").val();
                var now = moment(new Date());

                // typeID = 1 la CTKM, ngay OFF = ngay ON + DealDefaultOnsiteTimeout(14)
                if (typeID == 1) {
                    $('.from-date').datetimepicker({
                        allowTimes: arrTime,
                        //mask: true,
                        format: 'd/m/Y h:i A',
                        formatTime: 'h:i A',
                        onShow: function (ct) {
                            var minTime = "00:00 AM";
                            var ctVal = moment(ct);

                            if (ctVal.startOf('day').toString() == now.startOf('day').toString()) {
                                var tmp = moment(new Date()).add(1, 'h');
                                tmp = tmp.minute(0);
                                minTime = tmp.format('hh:mm a').toUpperCase();
                            } else if (ctVal.startOf('day')._d < now.startOf('day')._d) {
                                minTime = "11:01 PM";
                            }
                            this.setOptions({
                                //maxDate: addDate($('.to-date').text().substring(0, 10), -1),
                                minTime: minTime,
                                minDate: $('#startDateWithValue').val(),
                            })
                        },
                        onChangeDateTime: function (dp, $input) {
                            var ct = moment($('.from-date').val().substring(0, 10), "DD/MM/YYYY");
                            var minTime = "00:00 AM";
                            var ctVal = moment(ct);

                            if (ctVal.startOf('day').toString() == now.startOf('day').toString()) {
                                var tmp = moment(new Date()).add(1, 'h');
                                tmp = tmp.minute(0);
                                minTime = tmp.format('hh:mm a').toUpperCase();
                            } else if (ctVal.startOf('day')._d < now.startOf('day')._d) {
                                minTime = "11:01 PM";
                            }

                            this.setOptions({
                                minTime: minTime,
                                minDate: $('#startDateWithValue').val()
                            });
                            
                            //Neu ngay gio nho hon mindate
                            if (dp < convertDateToSystemDate($('#startDateWithValue').val())) {
                                $("#modal_update_deal .submit-update-deal").prop("disabled", true);
                                ShowNotify('error', frm_lang.CMS_Deal_InvalidStartDate, 2000);
                            } else {
                                $("#modal_update_deal .submit-update-deal").prop("disabled", false);
                            }
                        },
                        timepicker: true
                    }).change(function () {
                        var toDate = addDate($('.from-date').val().substring(0, 10), parseInt(dealOnsite));

                        $('.to-date').text(toDate);
                    });
                }
                else {
                    $('.from-date').datetimepicker({
                        allowTimes: arrTime,
                        //mask: true,
                        format: 'd/m/Y h:i A',
                        formatTime: 'h:i A',
                        onShow: function (ct) {
                            var minTime = "00:00 AM";
                            var ctVal = moment(ct);

                            if (ctVal.startOf('day').toString() == now.startOf('day').toString()) {
                                var tmp = moment(new Date()).add(1, 'h');
                                tmp = tmp.minute(0);
                                minTime = tmp.format('hh:mm a').toUpperCase();
                            } else if (ctVal.startOf('day')._d < now.startOf('day')._d) {
                                minTime = "11:01 PM";
                            }
                            this.setOptions({
                                //maxDate: addDate($('.to-date').text().substring(0, 10), -1),
                                minTime: minTime,
                                minDate: $('#startDateWithValue').val(),
                            })
                        },
                        onChangeDateTime: function (dp, $input) {
                            var ct = moment($('.from-date').val().substring(0, 10), "DD/MM/YYYY");
                            var minTime = "00:00 AM";
                            var ctVal = moment(ct);

                            if (ctVal.startOf('day').toString() == now.startOf('day').toString()) {
                                var tmp = moment(new Date()).add(1, 'h');
                                tmp = tmp.minute(0);
                                minTime = tmp.format('hh:mm a').toUpperCase();
                            } else if (ctVal.startOf('day')._d < now.startOf('day')._d) {
                                minTime = "11:01 PM";
                            }

                            this.setOptions({
                                minTime: minTime,
                                minDate: $('#startDateWithValue').val()
                            });

                            //Neu ngay gio nho hon mindate
                            if (dp < convertDateToSystemDate($('#startDateWithValue').val())) {
                                $("#modal_update_deal .submit-update-deal").prop("disabled", true);
                                ShowNotify('error', frm_lang.CMS_Deal_InvalidStartDate, 2000);
                            } else {
                                $("#modal_update_deal .submit-update-deal").prop("disabled", false);
                            }
                        },
                        timepicker: true
                    }).change(function (a) {
                        if (dealVCEx > 0) {
                            var toDate = addDate($('.from-date').val().substring(0, 10), 0, parseInt(dealVCEx));
                        }
                        $('.to-date').text(toDate);
                    });

                    //$('.to-date').datetimepicker({
                    //    mask: true,
                    //    closeOnDateSelect: true,
                    //    format: 'd/m/Y',
                    //    onShow: function (ct) {
                    //        // so sanh ngay hien tai va ngay bat dau, neu ngay bat dau lon hon ngay hien tai  + 1 ngay thi lay ngay bat dau, con neu nho hon ngay hien tai thi lay ngay hien tai +1
                    //        var fromDate = convertDateToSystemDate($('.from-date').val());
                    //        var systemDate = new Date();
                    //        var selectDate;

                    //        if (fromDate > systemDate) {
                    //            selectDate = addDate($('.from-date').val().substring(0, 10), 1);
                    //        }
                    //        else {
                    //            var date = new Date();
                    //            date.setDate(date.getDate() + 1);
                    //            selectDate = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
                    //        }

                    //        this.setOptions({
                    //            minDate: selectDate,
                    //            minTime: "00:00"
                    //        })
                    //    },
                    //    timepicker: false
                    //});
                }

                if ($('.Tip_mouse_on[title]').length) {
                    $.fn.powerTip.smartPlacementLists.n = ['n', 's', 'e', 'w'];
                    $('.Tip_mouse_on[title]').powerTip({
                        placement: 'n',
                        smartPlacement: true
                    });
                }

                // đảm bảo chuỗi truyền vào phải là format dd/MM/yyyy "01/01/2014"
                function addDate(dateInput, numberDate, numOfMonth) {
                    if (numOfMonth == undefined) numOfMonth = 0;

                    var fullOldDate = convertDateToSystemDate(dateInput);

                    var fullNewDate = new Date(fullOldDate);
                    if (numOfMonth == 0) {
                        fullNewDate.setDate(fullOldDate.getDate() + numberDate);
                    } else {
                        //Get last day of month
                        var y = fullNewDate.getFullYear();
                        var m = fullNewDate.getMonth();
                        var d = fullNewDate.getDate();

                        fullNewDate = new Date(y, m + numOfMonth + 1, 0);
                        //Kiem tra co nhay thang k? so ngay thang hien tai > so ngay thang ke tiep, khi cong thang se nhay 2 thang
                        if (fullNewDate.getDate() < d) {
                            d = fullNewDate.getDate();
                        }
                        fullNewDate = new Date(y, m + numOfMonth, d);
                    }
                    var fullDate = (fullNewDate.getDate() < 10) ? "0" + fullNewDate.getDate() : fullNewDate.getDate();
                    var fullMonth = (fullNewDate.getMonth() + 1 < 10) ? "0" + (fullNewDate.getMonth() + 1) : (fullNewDate.getMonth() + 1);
                    var fullYear = fullNewDate.getFullYear();

                    var formartNewDate = fullDate + '/' + fullMonth + '/' + fullYear;

                    return formartNewDate;
                }

                // Chuyển định dạng ngày có format là dd/MM/yyyy sang ngày hệ thống
                function convertDateToSystemDate(dateInput) {
                    var date = dateInput.substring(0, 2);
                    var month = Number(dateInput.substring(3, 5)) - 1; // tru 1 vi khi truyen vao no truyen san thang + 1
                    var year = dateInput.substring(6, 10);
                    var fullDate = new Date(year, month, date);

                    return fullDate;
                }

                $.noty.closeAll();
                $(self.data('href')).modal();

                // Lưu form
                $('.submit-update-deal').click(function (e) {
                    var did = $("#onoffDID").val();
                    $('.upd-deal-reason').val($('.upd-deal-reason').val().replace(/\s+/gi, " ").trim());
                    var reason = $('.upd-deal-reason').val();
                    var startDate = $('.from-date').val();
                    var endDate = $('.to-date').text();

                    //if ($("#typeID").val() == 1) {
                    //    var endDate = $('.to-date').text();
                    //}
                    //else {
                    //    var endDate = $('.to-date').val();
                    //}

                    $("#update-deal").parsley().validate();
                    if ($("#update-deal").parsley().isValid()) {
                        ShowOverlay();
                        $.ajax({
                            url: urlDeal[5],
                            type: "POST",
                            data: { "reason": reason, "did": did, "startDate": startDate, "endDate": endDate, "typeID": typeID, "dealOnsite": dealOnsite, "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val() },
                            success: function (result) {
                                if (result.ID == 1) {
                                    $('.submit-update-deal').attr('disabled', 'disabled');
                                    // không cần hiện thông báo lên vì thông báo được gắn vào tempdata trong action
                                    location.reload();
                                }
                                else {
                                    $('.submit-update-deal').removeAttr('disabled');
                                    ShowNotify('error', result.Message, 2000);
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                if (xhr.status != 401 && xhr.status != 403 && xhr.status != 500) {
                                    bootbox.alert(msg_lang.CMS_GetRuntimeErrorMsg);
                                }
                            }
                        });
                    }
                });
            }
        });
    });

    $(document).delegate("#number-date", "keypress paste", function (e) {
        return false;
        e.preventDefault();
    });
});