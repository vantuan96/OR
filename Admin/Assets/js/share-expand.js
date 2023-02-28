
var TBL_CONTENT_TYPE = {
    INPUT: 'input',
    LABEL: 'label',
    BUTTON: 'button',
};
//chung luu
function cusQuickSubmit(button) {
    openLoading();
    $form = $(button).parents('form:first');
    $(button).prop('disabled', true);
    var url = $form.attr('action');

    var data = new FormData($form[0]);
    //var target = $form.attr("data-target");
    $.ajax({
        url: url,
        method: "POST",
        enctype: 'multipart/form-data',
        processData: false,
        contentType: false,
        data: data,
        success: function (data) {
            $(button).prop('disabled', false);
            if (data.Type == 'success') {
                ShowNotify('success', data.Message, 2000);
                window.location.reload();
            } else {
                ShowNotify('error', data.Message);
            }
            closeLoading();
        },
        error: function (data) {
            ShowNotify('error', data.Message);
        }
    });
}
//lấy thông tin bệnh nhân hiện popup
function getPatientInfo(pid) {
    openLoading();
    var formdata = {
        kw: pid,
        siteId: ''
    };
    getDataJson('/OR/GetPatientInfo', formdata, function (data) {

        applyPatienInfo(data);
        $('#div-patient-info').show();
        closeLoading();
    }, false);
}
function applyPatienInfo(data) {
    $('#pMa').val(data.MA_BN);
    $('#pName').val(data.HO_TEN);
    switch (data.GIOI_TINH) {
        case 1:
            $('#pSexTxt').val('Nam');
            break;
        case 2:
            $('#pSexTxt').val('Nữ');
            break;
        case 3:
        default:
            $('#pSexTxt').val('Chưa xác định');
            break;
    }

    $('#pSex').val(data.GIOI_TINH);
    $('#pPhone').val(data.PHONE);
    $('#pAge').val(data.TUOI);
    $('#pBirthday').val(convertDateSectoStr(data.NGAY_SINH));
    $('#pAddress').val(data.DIA_CHI);
    $('#pNational').val(data.QUOC_TICH);
}
//----------------

//linhht đóng loading khi chuyển trang mới
window.onunload = function () {
    try {
        closeLoading();
    } catch (e) {

    }
};

//gọi API trả về data.data, cần đúng form trả về để đón đc dữ liệu
function getDataJson(url, inputForm, callback, isNotiSuccess = true) {
    let dataJson = null;
    openLoading();
    setTimeout(function () {
        $.ajax({
            cache: false,
            url: url,
            data: inputForm,
            async: false,
            global: false,     // this makes sure ajaxStart is not triggered: Hide pace default when call ajax
            success: function (data) {

                if (!data) {
                    closeLoading();
                    return false;
                }
                try {
                    data = JSON.parse(data);
                } catch (e) {
                    return false;
                }

                if (data.Type == 'success' || data.Status == 'OK') {
                    if (isNotiSuccess)
                        ShowNotify('success', data.Message, 2000);
                    if (data.data) {
                        dataJson = data.data;
                    }
                }
                else if (data.Type == 'ERR' || data.Status == 'ERR') {
                    ShowNotify('error', data.Message, 2000);
                }
                closeLoading();
                return callback(dataJson);
            },
            error: function (data) {
                closeLoading();
                ShowNotify('error', data.Message, 2000);
            },
            type: 'POST',
            dataType: 'text'
        });
    }, 100);
}
//gọi API kiểu lưu, update
function notGetDataJson(url, inputForm, pageGoto, isNotiSuccess = true, isReload = false) {
    openLoading();
    setTimeout(function () {
        $.ajax({
            cache: false,
            url: url,
            data: inputForm,
            async: false,
            global: false,     // this makes sure ajaxStart is not triggered: Hide pace default when call ajax
            success: function (data) {

                if (!data) {
                    closeLoading();
                    return false;
                }

                data = JSON.parse(data);
                if (data.Type == 'success' || data.Status == 'OK') {
                    if (pageGoto) {
                        gotoPage(pageGoto);
                        return false;
                    }
                    if (isNotiSuccess)
                        ShowNotify('success', data.Message, 2000);
                }
                else if (data.Type == 'ERR' || data.Status == 'ERR') {
                    ShowNotify('error', data.Message, 2000);
                }
                if (isReload) {
                    window.location.reload();
                }
                closeLoading();
            },
            error: function (data) {
                closeLoading();
                ShowNotify('error', data.Message, 2000);
            },
            type: 'POST',
            dataType: 'text'
        });
    }, 100)

}
//gọi API với form confirm 
function notGetDataJsonWithConfirm(url, inputForm, message, pageGoto, isNotiSuccess = true, isReload = false) {
    $.confirm({
        backgroundDismiss: true,
        escapeKey: true,
        icon: 'fa fa-warning',
        type: 'green',
        typeAnimated: true,
        title: 'XÁC NHẬN',
        content: message,
        buttons: {
            "ĐỒNG Ý": {
                btnClass: 'btn btn-default',
                keys: ['ctrl'],
                action: function () {
                    notGetDataJson(url, inputForm, pageGoto, isNotiSuccess = true, isReload = false);
                }
            },
            "ĐÓNG": {
                btnClass: 'btn-default',
                action: function () { }
            }
        }
    });
    return false;
}
function removeCache(id) {
    var formdata = {
        pid: ErrorObj.pid,
        visitCode: ErrorObj.visitCode,
        __RequestVerificationToken: $('[name=__RequestVerificationToken]').val()
    }
    //Check on server
    $('#btn-error-retry').css('pointer-events', 'none');
    $('#btn-error-go-back').css('pointer-events', 'none');
    openLoading();
    $.ajax({
        type: "POST",
        url: ErrorObj.clearCache_url,
        //headers: { "VMVerificationToken": $('input[name=__RequestVerificationToken]').val() },
        data: formdata,
        success: function (result) {
            if (id == 'btn-error-go-back') {
                setTimeout(function () {
                    window.history.go(-1);
                }, 1000);
                return;
            }

            if (result.IsSuccess == "1") {
                //linhht froce check pid reload
                if (ErrorObj.currentUrl.includes('pid')) {
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);

                } else {
                    setTimeout(function () {
                        window.history.go(-1);
                    }, 1000);
                }
            }
            else {
                window.location.reload();
            }
        }
        ,
        error: function (jqXHR, exception) {

            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Không có kết nối internet';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.n' + jqXHR.responseText;
            }

            alert(msg);

        }
    });
    return false;
}
//Loading
function openLoading() {
    $('#shadowOverlay').show();
}

function closeLoading() {
    $('#shadowOverlay').hide();
}


//option selector
function addOption(idSelect, optionText, optionValue) {
    $('#' + idSelect).append(new Option(optionText, optionValue));
}

function removeAllOption(idSelect) {
    $('#' + idSelect).empty();
}

//ul li genarate
//idTarget: id thằng ul
//arr: khai báo phần dữ liệu cần bind, yêu cầu đúng form, ko sẽ lỗi
//-arr[0]: tên map với field trả ra của dataJson
//-arr[1]: type: label, input, button,...
//-arr[2]: width của <td>
//-arr[3]: tên của button
//-arr[4]: function của button 
//dataJson: dữ liệu cần gen, tên đặt theo arr để mapping
//action: mặc định ko có, nếu có thì ăn theo dữ liệu với onClick
function genTable(idTarget, arr, dataJson) {
    $('#' + idTarget).find('tbody').empty();
    var html = "";
    $.each(dataJson, function (idx, item) {
        html += "<tr class='cus-row'>";
        for (var i = 0; i < arr.length; i++) {
            //lay obj từ arr
            var dataArr = arr[i];
            //xử lý với các trường hợp
            //LABEL
            switch (dataArr[1]) {
                case TBL_CONTENT_TYPE.LABEL:
                    var inputValue = item[dataArr[0]];
                    var strInput = "<label style='width:" + dataArr[2] + "'>" + inputValue + "</label>"
                    html += "<td>" + strInput + "</td>";
                    break;
                case TBL_CONTENT_TYPE.INPUT:
                    var inputValue = item[dataArr[0]];
                    var strInput = "<input type='text' value='" + inputValue + "' id='" + dataArr[0] + "'  name='" + dataArr[0] + "' style='width:" + dataArr[2] + "'>";
                    html += "<td>" + strInput + "</td>";
                    break;
                case TBL_CONTENT_TYPE.BUTTON:
                    var inputValue = item[dataArr[0]];
                    var strInput = "<button class='btn btn-success' type='button' style='width:" + dataArr[2] + "'>" + dataArr[3] + "</button>";
                    html += "<td>" + strInput + "</td>";
                    break;
            }
        }
        html += "</tr>";
    });
    $('#' + idTarget).append(html);
}


//linhht
//data of row click
function getDataRow(row, arr) {
    let obj = {};
    $('td', row).each(function (idx, item) {
        obj[arr[idx][0]] = $(this).text();
    });
    return obj;
}

//convert date
function convertDateSectoStr(sDateSec, format = 'yyyy-MM-dd') {
    var date = new Date(parseInt(sDateSec.substr(6, 13)));
    var day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
    var month = (date.getMonth() + 1) < 10 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1);
    switch (format) {
        case 'dd/MM/yyyy':
            return day + '/' + month + '/' + date.getFullYear();
        case 'dd-MM-yyyy':
            return day + '-' + month + '-' + date.getFullYear();
        case 'yyyy-MM-dd':
        default:
            return date.getFullYear() + '-' + month + '-' + day;
    }
}

//get para from url
function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
        }
    }
    return false;
};