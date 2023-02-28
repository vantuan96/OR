/* common functions */

//* detect touch devices
//function is_touch_device() {
//	return !!('ontouchstart' in window);
//}

/*
* debouncedresize: special jQuery event that happens once after a window resize
*/
(function (a) {
    var d = a.event, b, c;
    b = d.special.debouncedresize = {
        setup: function () { a(this).on("resize", b.handler); },
        teardown: function () { a(this).off("resize", b.handler); },
        handler: function (a, f) {
            var g = this, h = arguments, e = function () {
                a.type = "debouncedresize";
                d.dispatch.apply(g, h);
            };
            c && clearTimeout(c);
            f ? e() : c = setTimeout(e, b.threshold);
        },
        threshold: 150
    };
})(jQuery);
$.fn.hasAttr = function (name) {
    return this.attr(name) !== undefined;
};

var reloadWhenClosePopupViewer = false;
var isWaitingAjaxDone = false;
var adrDateFormat = 'dd/mm/yyyy';

var momentDateFormat = 'DD/MM/YYYY HH:mm';
var systemDateFormat = 'yyyy/MM/dd HH:mm';

$(function () {
    bootbox.setDefaults({
        locale: "vi"
    });

    top_navigation.init();
    tooltips_popovers.init();
    ADRLoadImage();

    $.validator.addMethod('date',
        function (value, element) {
            if (this.optional(element)) {
                return true;
            }
            var check = true;
            try {
                $.fn.datepicker.DPGlobal.parseDate(value, adrDateFormat);
            }
            catch (err) {
                check = false;
            }
            return check;
        }
    );

    $('#popup_viewer').on('hidden.bs.modal', function (e) {
        // do something...
        if (reloadWhenClosePopupViewer == true) {
            ShowOverlay(true);
            window.location.reload();
        }
        else {
            $('#popupIframe').html("");
        }
    });

    $('#popup_viewer').on('show.bs.modal', function (e) {
        ShowPopupViewerOverlay();
    });

    /* hàm dùng để show power tip */
    if ($('.Tip_mouse_on').length) {
        $.fn.powerTip.smartPlacementLists.n = ['n', 's', 'e', 'w'];
        $('.Tip_mouse_on').powerTip({
            placement: 'n',
            smartPlacement: true
        });
    }

    bindEvent.loadingURL();

    if ($('.linkPop').length) {
        $('.linkPop').click(function () {
            iosOverlay({
                text: "Đang tải...",
                icon: "fa fa-spinner fa-spin",
                duration: 1000,
                shadow: true
            });
        });
    }

    if ($("form.custom-validator-ignore").length > 0) {
        $("form.custom-validator-ignore").data("validator").settings.ignore = ".ignore-validator";
    }

    $("form:not(.frmSearch)").submit(function () {
        var checkAttr = $(this).attr('data-parsley-validate');
        var isParsleyForm = (typeof checkAttr !== typeof undefined && checkAttr !== false);
        var isValid = false;

        if (isParsleyForm) {
            isValid = $(this).parsley().isValid();
        }
        else {
            isValid = $(this).valid();
        }

        if (isValid) {
            ShowOverlay(true);

            $(this).find("input[data-html-denied='true'], textarea[data-html-denied='true']").each(function () {
                var val = $(this).val();
                val = val.replace(/</g, "< ");
                val = val.replace(/ ( )*/g, " ");
                val = val.replace(/^ /, "");
                val = val.replace(/ $/, "");
                $(this).val(val);
            });
        }
        //else {
        //    var validator = $(this).validate()
        //    console.log(validator.errorList);
        //}
    });

    $('.btn-iframe-cancel').click(function (e) {
        e.preventDefault();
        $(window.parent.document.body).find(".close").trigger("click");
    });

    $(document).ajaxComplete(function (event, xhr, settings) {
        //HideOverlay();

        if (xhr.status == 302) {
            bootbox.alert("Lỗi mất kết nối hoặc bạn KHÔNG có quyền thực hiện chức năng ngày. Bạn có muốn tải lại trang?", function (result) {
                iosOverlay({
                    text: "Đang tải...",
                    icon: "fa fa-spinner fa-spin",
                    shadow: true
                });
                window.location.reload();
            });
        }
        if (xhr.status == 403) {
            bootbox.alert("Bạn KHÔNG có quyền thực hiện chức năng ngày. Bạn có muốn tải lại trang?", function (result) {
                iosOverlay({
                    text: "Đang tải...",
                    icon: "fa fa-spinner fa-spin",
                    shadow: true
                });
                window.location.reload();
            });
        }

        //if (xhr.status == 500) {
        //    if (confirm("Lỗi mất kết nối. Vui lòng tải lại trang để kết nối lại")) {
        //        window.location.reload();
        //    }
        //}
    });

    if ($('#txtGlobalSearch').length > 0) {
        $('#txtGlobalSearch').keyup(function (e) {
            var keycode = e.keyCode || e.which;
            //var kw = $.trim($(this).val());

            if (keycode == 13) //enter
            {
                //if (kw.length > 0) {
                //    var href = $('ul#globalSearchIns li.active a').attr('href');
                //    ShowOverlay(true);
                //    window.location.href = href;
                //}
                fnSearchGlobal();
            }
            /*
            else if (keycode == 38) //up 
            {
                //var li = $('ul#globalSearchIns li');
                //var index = $(li).index($('ul#globalSearchIns li.active'));

                //if (index == 0) {
                //    index = $(li).length - 1;
                //}
                //else {
                //    index--;
                //}

                //$('ul#globalSearchIns li.active').removeClass('active');
                //$(li[index]).addClass('active');
            }
            else if (keycode == 40) //down
            {
                //var li = $('ul#globalSearchIns li');
                //var index = $(li).index($('ul#globalSearchIns li.active'));

                //if (index == $(li).length - 1) {
                //    index = 0;
                //}
                //else {
                //    index++;
                //}

                //$('ul#globalSearchIns li.active').removeClass('active');
                //$(li[index]).addClass('active');
            }
            else {
                if (kw.length > 0) {
                    $('li .globalsearchkw').text(kw);
                    $('ul#globalSearchIns li a').each(function () {
                        var href = $(this).parent().attr('data-value') + '?kw=' + kw;
                        $(this).attr('href', href);
                    });

                    $('ul#globalSearchIns').show();
                }
                else {
                    $('ul#globalSearchIns').hide();
                }
            }*/
        });
    }


    //if ($('input[data-val-number]').length > 0) {
    //    $('input[data-val-number]').each(function () {
    //        var decimalPlaces = $(this).attr('data-decimal-places');

    //        if (typeof decimalPlaces == typeof undefined) { // số nguyên
    //            decimalPlaces = 0;
    //        }
    //        else {
    //            //$(this).val($(this).val().replace('.', ','));
    //        }

    //        $(this).number(true, decimalPlaces, '.', ',');
    //    });

    //    $("input[data-val-number]").keypress(function (n) {
    //        var text = $(this).val().replace(/\./g, '');

    //        if (text.length > 20) {
    //            n.which != 8 && n.preventDefault();
    //        }
    //    });

    //    $("input[data-val-number]:not([allownegative])").keydown(function (n) {
    //        if ($(this).val().indexOf('-') != -1) {
    //            $(this).val($(this).val().replace('-', ''));
    //        }
    //    });
    //}


    if ($('input[data-val-decimal]').length > 0) {
        InputNumber($('input[data-val-decimal]'));
    }       
    
    if ($('textarea.newline-denied').length > 0) {
        $('textarea.newline-denied').change(function () {
            var txt = $(this).val();
            txt = txt.replace(/\n/g, "");
            txt = txt.replace(/\r/g, "");
            $(this).val(txt);
        });

        $('textarea.newline-denied').keydown(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                return false;
            }
        });
    }

    $('[data-address-ref-element]').change(function (e) {
        adr_address.getAddress($(this));
    });

    window.onbeforeunload = function () {
        console.log("back history");
    };

    $('.btn-clear-iframe').click(function (e) {
        e.preventDefault();
        var href = $(this).attr('href');
        window.top.$('#popup_viewer iframe').attr('src', '');
        window.top.location.href = href;
    });

    $('#btn_collaps').click(function (e) {
        if ($(this).hasClass('maxi')) {
            $(this).removeClass('maxi');
            $(this).addClass('mini');

            $('#divWareHouseTreeView').hide();
            $('#divWareHouseTreeView').next().removeClass('col-md-8').addClass('col-md-12');
        }
        else {
            $(this).removeClass('mini');
            $(this).addClass('maxi');

            $('#divWareHouseTreeView').show();
            $('#divWareHouseTreeView').next().removeClass('col-md-12').addClass('col-md-8');
        }
    });

    $('#btn_collaps_his').click(function (e) {
        if ($(this).hasClass('maxi')) {
            $(this).removeClass('maxi');
            $(this).addClass('mini');

            $('#div_his_ex_in').next().hide();
            $('#div_his_ex_in').removeClass('col-md-8').addClass('col-md-12');
        }
        else {
            $(this).removeClass('mini');
            $(this).addClass('maxi');

            $('#div_his_ex_in').next().show();
            $('#div_his_ex_in').removeClass('col-md-12').addClass('col-md-8');
        }
    });

    adrHelper.Init();
});

//$.ajaxSetup({
//    beforeSend: function (xhr) {
//        xhr.setRequestHeader("RequestVerificationToken", __tokenHeaderValue);
//    }
//});

/* thanhduong: tách ra để gọi trong những sự kiện ajax không phải là form */




var adrHelper = {
    Init: function () {
        $('.adr_datepicker').datepicker({
            todayHighlight: true,
        }).on('changeDate', function (e) {
            var val = $('input', $(this)).val();
            $(this).next().val(val.replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$3/$2/$1"));
        });

        if ($('.adr-datetime-helper').length > 0) {
            $('.adr-datetime-helper').datepicker({
                format: "dd/mm/yyyy",
                autoclose: true,
                todayHighlight: true
            }).on('changeDate', function (e) {
                if ($('#dBirthday').length > 0) {
                    $("#dBirthday").valid();
                }
            });
        }

        if ($('.adr-birthday-helper').length > 0) {
            $('.adr-birthday-helper').datepicker({
                format: "dd/mm/yyyy",
                autoclose: true,
                todayHighlight: true,
                endDate: '0d'
            }).on('changeDate', function (e) {
                if ($('#dBirthday').length > 0) {
                    $("#dBirthday").valid();
                }
            });
        }

        if ($('.reportrange').length) {
            if ($(window).width() < 974) {
                var dropdownPos = 'right';
            }
            else {
                var dropdownPos = 'left';
            }

            $('.reportrange').each(function () {
                try {
                    var control = $(this);
                    var rangedateopen = dropdownPos;
                    if ($(control).hasAttr('data-rangedateopen'))
                    {
                        rangedateopen = $(control).data('rangedateopen');
                    }

                    var initFromDate = $('.fromDateHidden', $(control)).val();
                    var initToDate = $('.toDateHidden', $(control)).val();
                    if (initFromDate == undefined || initToDate == undefined || initFromDate.length == 0 || initToDate.length == 0) {
                        initFromDate = moment().subtract('days', 29);
                        initToDate = moment();
                    }
                    else
                    {
                        initFromDate = new Date(initFromDate);
                        initToDate = new Date(initToDate);
                    }

                    $(control).daterangepicker({
                        opens: rangedateopen,
                        ranges: {
                            'Hôm nay': [moment(), moment()],
                            'Hôm qua': [moment().subtract('days', 1), moment().subtract('days', 1)],
                            '7 ngày gần nhất': [moment().subtract('days', 6), moment()],
                            '7 ngày kế tiếp': [moment(), moment().add('days', 6)],
                            //'30 ngày gần nhất': [moment().subtract('days', 29), moment()],
                            'Tuần này': [moment().subtract('days', (new Date()).getDay()), moment().add('days', 6 - (new Date()).getDay())],
                            //'Tháng này': [moment().startOf('month'), moment().endOf('month')]
                        },
                        startDate: initFromDate,
                        endDate: initToDate,
                        //maxDate: today,
                        buttonClasses: ['btn', 'btn-sm']
                    },
                    function (start, end) {
                        $('span', $(control)).html(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY'));
                        $('.fromDateHidden', $(control)).val(start.format('YYYY-MM-DD'));
                        $('.toDateHidden', $(control)).val(end.format('YYYY-MM-DD'));
                        $('.fromDateHidden', $(control)).trigger('change');
                    });
                } catch (e) { }
            });
        }

        if ($('.reportrange_future').length) {
            if ($(window).width() < 974) {
                var dropdownPos = 'right';
            }
            else {
                var dropdownPos = 'left';
            }

            $('.reportrange_future').each(function () {
                try {
                    var control = $(this);
                    var rangedateopen = dropdownPos;
                    if ($(control).hasAttr('data-rangedateopen'))
                    {
                        rangedateopen = $(control).data('rangedateopen');
                    }

                    $(control).daterangepicker({
                        opens: rangedateopen,
                        ranges: {
                            'Hôm nay': [moment(), moment()],
                            
                            '7 ngày kế tiếp': [moment(), moment().add('days', 6)],
                            '30 ngày kế tiếp': [ moment(), moment().add('days', 29)],
                            //'Tuần này': [moment().subtract('days', (new Date()).getDay()), moment().add('days', 6 - (new Date()).getDay())],
                            //'Tháng này': [moment().startOf('month'), moment().endOf('month')]
                        },
                        startDate: moment().subtract('days', 29),
                        endDate: moment(),
                        //maxDate: today,
                        buttonClasses: ['btn', 'btn-sm']
                    },
                    function (start, end) {
                        $('span', $(control)).html(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY'));
                        $('.fromDateHidden', $(control)).val(start.format('YYYY/MM/DD'));
                        $('.toDateHidden', $(control)).val(end.format('YYYY/MM/DD'));
                        $('.fromDateHidden', $(control)).trigger('change');
                    });
                } catch (e) { }
            });
        }

        $(document).on("select2-close", function (arg) {
            var element = $(arg.target);

            if (!$(element).hasClass('no-validate'))
                $(element).valid();
        });

        if ($('.select2').length > 0) {
            var placeHolder = $('.select2').data('placeholder');
            if (placeHolder == '') placeHolder = 'Nhập từ khóa...';
            $('.select2').select2({
                placeholder: placeHolder,
                allowClear: true
            });
        }

        if ($('.select2-nosearch').length > 0) {
            var placeHolder = $('.select2-nosearch').data('placeholder');
            if (placeHolder == '') placeHolder = 'Nhập từ khóa...';
            $('.select2-nosearch').select2({
                placeholder: placeHolder,
                allowClear: true,
                minimumResultsForSearch: -1
            });
        }

        if ($('.number').length) {
            $(".number").keypress(function (n) {
                n.which != 8 && isNaN(String.fromCharCode(n.which)) && n.preventDefault();
            });
        }
        if ($('.floatnumber').length) {
            $(".floatnumber").keypress(function (n) {
                n.which != 8 && (n.which != 44 || $(this).val().indexOf(",") != -1) && (n.which < 48 || n.which > 57) && n.preventDefault();
            });
        }

        // Khởi tạo validation cho form sử dụng select2
        //var validobj = $(".formValidation").validate({
        //    onkeyup: false,
        //    errorClass: "myErrorClass"
        //});

        //$('.formValidation').on("change", ".select2-offscreen", function () {
        //    if (!$.isEmptyObject(validobj.submitted)) {
        //        validobj.form();
        //    }
        //});

        //$('.formValidation').on("select2-opening", function (arg) {
        //    var elem = $(arg.target);
        //    if ($("#s2id_" + elem.attr("id") + " ul").hasClass("myErrorClass")) {
        //        $(".select2-drop ul").addClass("myErrorClass");
        //    } else {
        //        $(".select2-drop ul").removeClass("myErrorClass");
        //    }
        //});
    }
};

bindEvent = {
    //popupViewer: function () {
    //    $("[data-iframe-url]").click(function () {
    //        var href = $(this).attr("data-iframe-url");

    //        if (href.indexOf("?") >= 0) {
    //            href += "&opener=" + curUrl;
    //        }
    //        else {
    //            href += "?opener=" + curUrl;
    //        }

    //        var iframeHeight = 400;

    //        if ($(this).attr("data-iframe-height") !== undefined) {
    //            iframeHeight = $(this).attr("data-iframe-height");
    //        }

    //        var iframe = '<iframe src="' + href + '" width="100%" frameborder="0" height="' + iframeHeight + '" scrolling="yes"></iframe>';

    //        $('#popupIframe').html(iframe);

    //        if ($(this).attr("title") !== undefined) {
    //            $('#popup_viewer .title-popup').text($(this).attr("title"));
    //        }
    //        else if ($(this).attr("data-iframe-title") !== undefined) {
    //            $('#popup_viewer .title-popup').text($(this).attr("data-iframe-title"));
    //        }

    //        if ($(this).attr("data-iframe-size") !== undefined) {
    //            $('#popup_viewer .modal-dialog').addClass('modal-lg');
    //        }
    //        else {
    //            $('#popup_viewer .modal-dialog').removeClass('modal-lg');
    //        }

    //        if ($(this).attr("data-iframe-size-md") !== undefined) {
    //            $('#popup_viewer .modal-dialog').addClass('modal-md');
    //        }
    //        else {
    //            $('#popup_viewer .modal-dialog').removeClass('modal-md');
    //        }
    //    });
    //},
    loadingURL: function () {
        if ($('.linkURL').length > 0) {
            $('.linkURL').click(function (e) {
                if (e.ctrlKey || e.which == 2) {
                    return;
                }

                ShowOverlay(true);
            });
        }

        if ($('.pagination li').length > 0) {
            $('.pagination li').click(function (e) {
                if ($(this).hasClass('disabled') || $(this).hasClass('active'))
                {
                    return;
                }

                if (e.ctrlKey || e.which == 2)
                {
                    return;
                }

                ShowOverlay(true);
            });
        }
    }
};


function fnSearchGlobal()
{
    var kw =  $('#txtGlobalSearch').val().replace(/[^\w\s]/gi, '');
   
    if (kw.length > 0) {
        var href = $('#txtGlobalSearch').attr('data-link');
        ShowOverlay(true);
        window.location.href = href.replace('999', kw);
    }
    else {
        return false;
    }
}

function exportExcel() {
    var url = window.location.href;
    if (url.indexOf('?') > 0) {
        window.location.href = window.location.href + '&export=true'
    }
    else {
        window.location.href = window.location.href + '?export=true'
    }
}

function ADRLoadImage() {
    if ($('.img_wrapper > img[data-img-src]').length > 0) {
        $('.img_wrapper > img[data-img-src]').each(function () {
            var parent = $(this).parent();
            if ($(parent).hasClass('state_error'))
                $(parent).removeClass('state_error');

            $(this).attr('src', $(this).data('img-src'));
            $(this).removeAttr('data-img-src');
            $(parent).addClass('state_loading');

            $(this).load(function () {
                $(parent).removeClass('state_loading');
            }).error(function () {
                $(parent).removeClass('state_loading');
                $(parent).addClass('state_error');
            });
        });
    }
}

function ClosePopupViewer() {
    $('#popup_viewer .close').trigger('click');
}

function ShowOverlay(shadow) {
    if (typeof shadow == 'undefined') shadow = false;

    //iosOverlay({
    //    text: "Đang tải...",
    //    icon: "fa fa-spinner fa-spin",
    //    shadow: shadow,
    //    id: 'LoadingOverlay'
    //});
    //adrOverlay.hide();

    $('#shadowOverlay').show();
}

function ShowSuccess() {
    if ($(".parsley-errors-list.filled").length == 0) {
        $(".ui-ios-overlay.ios-overlay-show").remove();
        iosOverlay({
            text: "Thành công",
            duration: 800,
            icon: "fa fa-check"
        });
    }
}

function ShowOverlayLoadWaiting(shadow) {
    if (typeof shadow == 'undefined') shadow = false;

    if ($(".parsley-errors-list.filled").length == 0) {
        var adrOverlay = iosOverlay({
            text: "Đang tải...",
            icon: "fa fa-spinner fa-spin",
            shadow: shadow,
            id: 'LoadingOverlay'
        });
    }
}

function SuccessAndReload(message) {
    if ($(".ui-ios-overlay.ios-overlay-show").length > 0) {
        $(".ui-ios-overlay.ios-overlay-show").remove();
    }

    if (typeof message == 'undefined' || message.length == 0) {
        iosOverlay({
            text: "Thành công",
            duration: 10000,
            icon: "fa fa-check"
        });

        setTimeout(function () {
            window.location.reload();
        }, 1000);
    }
    else {
        bootbox.alert(response.message, function () {
            ShowOverlay(true);
            window.location.reload();
        });
    }
}

function HideOverlay(time) {
    if (typeof time == 'undefined') time = 1000;
    //console.log(time);
    //return true;
    if (time == 0) {
        //$("#shadowOverlay").hide();
        if (document.getElementById("shadowOverlay") != null) {
            document.getElementById("shadowOverlay").style.display = 'none';
        }
    }
    else {
        window.setTimeout(function () {
            //$("#shadowOverlay").hide();
            if (document.getElementById("shadowOverlay") != null) {
                document.getElementById("shadowOverlay").style.display = 'none';
            }
        }, time);
    }
}
/**/

function ShowPopupViewerOverlay() {
    $('#popup_viewer .se-pre-con').show();
}

function HidePopupViewerOverlay() {
    $('#popup_viewer .se-pre-con').hide();
}

// top dropdown navigation (mobile nav)
top_navigation = {
    init: function () {
        //$('.top_links').tinyNav({
        //	active: 'selected',
        //	select_class: 'form-control input-sm',
        //	header: '-- Nav --'
        //});
    }
};

// default tooltips, popovers init
tooltips_popovers = {
    init: function () {
        $('[data-toggle=tooltip]').tooltip({
            container: "body"
        });
        $('[data-toggle=popover]').popover({
            container: "body"
        });
    }
};

thanhdt = {
    intNumberInput: function (element) {
        var id = $(element).attr('id');
        id = 'preview_' + id;

        var fakeInput = '<input type="text" id="' + id + '" data-parsley-required="true" />';
        $(element).hide();
        $(fakeInput).insertBefore(element);
        fakeInput = $('#' + id);

        $(fakeInput).attr('class', $(element).attr('class'));
        $(fakeInput).val(thanhdt.formatIntNumber($(element).val()));
        $(fakeInput).attr('data-parsley-maxlength', '10');
        $(fakeInput).attr('data-parsley-maxlength-message', 'Giới hạn tối đa 99.999.999');

        if ($(element).attr('data-parsley-required') == 'true') {
            $(fakeInput).attr('data-parsley-required', 'true');
            $(element).removeAttr('data-parsley-required');
        }

        //if ($(element).attr('data-parsley-min-message') != undefined) {
        //    var val = $(element).attr('data-parsley-min-message');
        //    $(fakeInput).attr('data-parsley-min-message', val);
        //    $(element).removeAttr('data-parsley-min-message');
        //}

        //if ($(element).attr('data-parsley-min') != undefined) {
        //    var val = $(element).attr('data-parsley-min');
        //    $(fakeInput).attr('data-parsley-min', val);
        //    $(element).removeAttr('data-parsley-min');
        //}

        //if ($(element).attr('data-parsley-max-message') != undefined) {
        //    var val = $(element).attr('data-parsley-max-message');
        //    $(fakeInput).attr('data-parsley-max-message', val);
        //    $(element).removeAttr('data-parsley-max-message');
        //}

        //if ($(element).attr('data-parsley-max') != undefined) {
        //    var val = $(element).attr('data-parsley-max');
        //    $(fakeInput).attr('data-parsley-max', val);
        //    $(element).removeAttr('data-parsley-max');
        //}

        $(fakeInput).keypress(function (e) {
            if (e.which != 8 && e.keyCode != 9 && e.keyCode != 37 && e.keyCode != 39 && e.keyCode != 46 && isNaN(String.fromCharCode(e.which)))
                e.preventDefault();
        });

        //$(fakeInput).keyup(function (e) {
        $(fakeInput).on('input propertychange paste', function () {
            //if (e.keyCode != 37 && e.keyCode != 39 && !e.shiftKey) {
            //if (e.which == 8 || e.which == 46 || !isNaN(String.fromCharCode(e.which)))
            //{
            var number = $(this).val().replace(/\./g, '');
            $(element).val(parseInt(number));
            $(element).trigger('change');
            number = thanhdt.formatIntNumber(number);
            $(this).val(number);
            //}
        });
    },
    formatIntNumber: function (number) {
        var result = '';

        while (number.length > 3) {
            var x = number.substring(number.length - 3, number.length);
            result = '.' + x + result;
            number = number.substring(0, number.length - 3);
        }

        result = number + result;
        return result;
    }
};

function InputNumber(selector) {
    $(selector).each(function () {
        $(this).data('key-oldnumber', $(this).val());
        var decimalPlaces = $(this).data('val-decimal');
        var maxValue = $(this).data('max-value');

        if (typeof maxValue == typeof undefined)
            maxValue = 2000000000;


        $(this).bind('paste', function (e) {
            var self = this;
            var newValue = $(self).val();
            setTimeout(function () {
                if (!isValidNumber(newValue, decimalPlaces, maxValue))
                    $(self).val('');
            }, 0);
        });


        //$(this).keydown(function (e) {
        //    var character = String.fromCharCode(e.keyCode)
        //    var newValue = this.value + character;
        //    console.log(character);
        //    //if (!isValidNumber(newValue, decimalPlaces, maxValue)) {
        //    //   $(this).val(this.value);
        //    //}
        //    //else {
        //       $(this).data('key-oldnumber', this.value);
        //    //}


        //});

        $(this).keyup(function (e) {
            var oldNumber = $(this).data('key-oldnumber');
            var newValue = this.value;
            if ($(this).hasAttr('allownegative') && newValue == '-')
                return true;

            if (!isValidNumber(newValue, decimalPlaces, maxValue)) {
                if (isValidNumber(oldNumber, decimalPlaces, maxValue))
                    $(this).val(oldNumber);
                else
                    $(this).val('');
                e.preventDefault();
                return false;
            }
            else {
                $(this).data('key-oldnumber', newValue);
            }
        });

        if ($(this).hasAttr('allownegative') == false) {
            $(this).keydown(function (n) {
                if ($(this).val().indexOf('-') != -1) {
                    $(this).val($(this).val().replace('-', ''));
                }
            });
        }
    });
}

function isValidNumber(newValue, decimalPlaces, maxValue) {
    if (!isValidDecimal(newValue, decimalPlaces) || (decimalPlaces == 0 && newValue.indexOf('.') != -1) || newValue > maxValue)
        return false;
    else
        return true;
}

function isValidDecimal(numberVal, decimalPlaces) {
    if (typeof decimalPlaces == typeof undefined)
        decimalPlaces = 0;

    if (numberVal.indexOf('.') != -1) {
        if (decimalPlaces == 0)
            return false;
        else {
            var arr = numberVal.split('.');
            if (arr[1].length > decimalPlaces)
                return false;
        }
    }

    return (isNaN(numberVal) == false);
}

function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}

function convertDateFromJson(inputDate, format) {
    if (inputDate == null)
        return '';

    if (format == undefined)
        format = "dd/MM/yyyy";

    var d = parseJsonDate(inputDate);

    return FormatDateTime(d, format);
}

function FormatDateTime(d, format)
{
    function pad(s) { return (s < 10) ? '0' + s : s; }

    format = format.replace('dd', pad(d.getDate()));
    format = format.replace('MM', pad(d.getMonth() + 1));
    format = format.replace('yyyy', pad(d.getFullYear()));

    format = format.replace('HH', pad(d.getHours()));
    format = format.replace('mm', pad(d.getMinutes()));
    format = format.replace('ss', pad(d.getSeconds()));
    return format;
}

function ShowNotify(type, text, timeout, position) {
    if (!timeout) timeout = 3000;
    if (position == undefined) position = 'topRight';
    if (_isIframe) {
        window.noty({
            text: text,
            type: type,
            theme: 'ad_theme',
            layout: 'bottomRight',
            closeWith: ['button'],// ['click', 'button', 'hover', 'backdrop']
            maxVisible: 10,
            killer: true,
            timeout: timeout
        });
    } else {
        window.noty({
            text: '<h5>THÔNG BÁO</h5>' + text,
            type: type,
            theme: 'ad_theme',
            layout: position,
            closeWith: ['button'],// ['click', 'button', 'hover', 'backdrop']
            maxVisible: 10,
            killer: true,
            timeout: timeout
        });
    }
}

function ShowNotifyInTag(id, type, text, timeout) {
    if (typeof timeout == 'undefined') timeout = 5000;

    $(id).noty({
        text: text,
        type: type,
        theme: 'ad_theme',
        closeWith: ['button'],
        maxVisible: 10,
        killer: true,
        timeout: timeout
    });
}

if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

if (!String.prototype.format) {
    String.prototype.format = function (args) {
        var str = this;
        return str.replace(String.prototype.format.regex, function (item) {
            var intVal = parseInt(item.substring(1, item.length - 1));
            var replace;
            if (intVal >= 0) {
                replace = args[intVal];
            } else if (intVal === -1) {
                replace = "{";
            } else if (intVal === -2) {
                replace = "}";
            } else {
                replace = "";
            }
            return replace;
        });
    };
    String.prototype.format.regex = new RegExp("{-?[0-9]+}", "g");
}

if (typeof String.prototype.endsWith !== 'function') {
    String.prototype.endsWith = function (suffix) {
        return this.indexOf(suffix, this.length - suffix.length) !== -1;
    };
}

function initDataTable(tableID, filterBoxPlacholder, displayRowNum, displayRowNumChangeable) {
    //khai báo option object
    var langOpt = {
        "sProcessing": "Đang xử lý...",
        "sLoadingRecords": "Đang tải dữ liệu...",
        "sLengthMenu": "_MENU_ bản ghi trên trang",
        "sInfo": "Hiển thị từ _START_ đến _END_ trong tổng số _TOTAL_ bản ghi",
        "sInfoEmpty": "Hiển thị từ 0 đến 0 trong tổng số 0 bản ghi",
        "sInfoFiltered": "(được lọc từ _MAX_ bản ghi)",
        "sInfoPostFix": "",
        "sSearch": "Tìm:",
        "sUrl": "",
        "oPaginate": {
            "sFirst": "<<",
            "sPrevious": "<",
            "sNext": ">",
            "sLast": ">>"
        },
        "zeroRecords": 'Không tìm thấy dữ liệu',

    };
    //khai báo các tag thành phần của bảng
    var tableTag = $(tableID);

    //init sort và search column của bảng
    var columnDefsArr = [];
    var isShowFilter = false;

    if (tableTag.find("thead th").length > 0) {
        $.each(tableTag.find("thead th"), function (index) {
            var isSortable = $(this).attr("sortable") != undefined;
            var isSearchable = $(this).attr("searchable") != undefined;
            columnDefsArr.push({ targets: index, orderable: isSortable, searchable: isSearchable });

            isShowFilter = !isShowFilter ? (isSearchable ? true : false) : true;
        });
    }

    //init datatable
    var table = tableTag.dataTable({
        "displayLength": displayRowNum,
        //"aLengthMenu": [[10, 20, 30], [10 + " bản ghi / trang", 20 + " bản ghi / trang", 30 + " bản ghi / trang"]],
        "drawCallback": function (oSettings, json) {
            var tableWrapper = $(tableID + "_wrapper");
            var tablePaginateTag = $(tableID + "_paginate");
            var tableFilterTag = $(tableID + "_filter");
            HideOverlay(0);
            var totalPage = Math.ceil(this.fnSettings().fnRecordsDisplay() / displayRowNum);
            if (totalPage <= 1) {
                tablePaginateTag.closest("div.row").hide();
            }
            else {
                tablePaginateTag.closest("div.row").show();
            }
            tableFilterTag.find('input').attr('placeholder', filterBoxPlacholder);

            if (!isShowFilter && !displayRowNumChangeable) {
                tableWrapper.find(".well.well-sm").hide();
            }
        },
        "language": langOpt,
        "columnDefs": columnDefsArr,
        "lengthChange": displayRowNumChangeable,
        "info": true,
        "filter": isShowFilter
    });

    return table;
}

adr_client_validate = {
    Required: function (elementId, errmessage) {
        var element = '#' + elementId;

        if (typeof errmessage == 'undefined')
            errmessage = $(this).data('val-required');

        $(element).rules('add', {
            required: true,
            message: {
                required: errmessage
            }
        });
    },
    UnRequired: function (elementId) {
        $(element).rules('remove', 'required');
    },
    RequiredADR: function (elementId, message) {
        var element = '#' + elementId;

        $(element).bind('change', function () {
            var value = $(this).val();
            var msgElement = '[data-valmsg-for="' + elementId + '"]';

            if (value.length == 0) {
                $(element).addClass('input-validation-error');
                $(element).removeClass('valid');

                if (typeof message == 'undefined')
                    message = $(this).data('val-required');

                $(msgElement).text(message);
                $(msgElement).addClass('field-validation-error');
                $(msgElement).removeClass('field-validation-valid');
            } else {
                $(element).removeClass('input-validation-error');
                $(element).addClass('valid');

                $(msgElement).text('');
                $(msgElement).removeClass('field-validation-error');
                $(msgElement).addClass('field-validation-valid');
            }
        });
    },
    UnRequiredADR: function (elementId) {
        var element = '#' + elementId;
        var msgElement = '[data-valmsg-for="' + elementId + '"]';

        $(element).removeClass('input-validation-error');
        $(element).addClass('valid');

        $(msgElement).text('');
        $(msgElement).removeClass('field-validation-error');
        $(msgElement).addClass('field-validation-valid');

        $(element).unbind('change');
    },
    ValidADR: function (form) {
        if ($('.field-validation-error', $(form)).length > 0) {
            $('html, body').animate({
                scrollTop: $('.field-validation-error', $(form)).offset().top - 100
            }, 20);

            return false;
        } else {
            return true;
        }
    }
};

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

var dtvn = {
    "sProcessing": "Đang xử lý...",
    "sLoadingRecords": 	"Đang tải dữ liệu...",
    "sLengthMenu":   "_MENU_ bản ghi trên trang",
    "sZeroRecords":  "Không tìm thấy dòng nào phù hợp",
    "sInfo":         "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
    "sInfoEmpty":    "Đang xem 0 đến 0 trong tổng số 0 mục",
    "sInfoFiltered": "(được lọc từ _MAX_ mục)",
    "sInfoPostFix":  "",
    "sSearch":       "Tìm:",
    "sUrl":          "",
    "oPaginate": {
        "sFirst":    "<<",
        "sPrevious": "<",
        "sNext":     ">",
        "sLast":     ">>"
    }
};

