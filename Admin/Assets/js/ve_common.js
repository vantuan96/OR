var iframePopupUrl = "/";
var flagReloadPage = false;
var innerHTMLLoading = '<span class="ion-loading-a"></span>';

$(function () {
    if (typeof (_alertData) == "object" && _alertData != null) {
        ShowNotify(_alertData.ID == 0 ? "error" : "success", _alertData.Message, 5000);
    }

    $('[data-target="#iframePopup"]').click(function (e) {
        loadIframePopup($(this));
    });

    $('.modal-header .close').click(function (e) {
        parent.$('#iframePopup').modal('hide');
    });

    $('.modal').on('hidden.bs.modal', function (e) {
        if (flagReloadPage == true) {
            window.location.reload();
        }
    });
});

var commonUtils = {
    postAjaxWithToken: function (url, data, successFunc) {
        var token = $('[name=__RequestVerificationToken]').val();
        var tokenObj = { name: '__RequestVerificationToken', value: token };
        data = data == null ? new Array() : data;

        data.push({ name: '__RequestVerificationToken', value: token });
        $.ajax({
            url: url,
            type: "POST",
            data: data,
            cache: false,
            success: successFunc
            
        });
    },
    postAjaxWithTokenWaitResponse: function (url, data, successFunc) {
        var token = $('[name=__RequestVerificationToken]').val();
        var tokenObj = { name: '__RequestVerificationToken', value: token };
        data = data == null ? new Array() : data;

        data.push({ name: '__RequestVerificationToken', value: token });
        $.ajax({
            url: url,
            async: false,
            type: "POST",
            data: data,
            cache: false,
            success: successFunc

        });
    }
};

jQuery.fn.exists = function () { return this.length > 0; }

function loadIframePopup(element) {
    iframePopupUrl = $(element).attr('data-modal-src');
    iframeHeight = $(element).attr('data-iframe-height');
    var iframe = '<iframe  onload="javascript:resizeIframe(this);" src="' + iframePopupUrl + '" style="height:' + iframeHeight + 'px;"></iframe>';

    if ($(element).attr("data-iframe-size") !== undefined) {
        $('#iframePopup .modal-dialog').addClass('modal-lg');
    }
    else {
        $('#iframePopup .modal-dialog').removeClass('modal-lg');
    }

    if ($(element).attr("data-iframe-size-md") !== undefined) {
        $('#iframePopup .modal-dialog').addClass('modal-md');
    }
    else {
        $('#iframePopup .modal-dialog').removeClass('modal-md');
    }

    $('#iframePopup .modal-content').html("");
    $('#iframePopup .modal-content').html(iframe);
}

function showIframePopup(url) {
    iframeHeight = 380;
    var iframe = '<iframe onload="javascript:resizeIframe(this);" src="' + url + '" style="height:' + iframeHeight + 'px;"></iframe>';
       

    $('#iframePopup .modal-content').html("");
    $('#iframePopup .modal-content').html(iframe);
}

function resizeIframe(obj) {
    setTimeout(function () {
        try {
            if (obj != null && obj.contentWindow.document.body != null) {
                var h = obj.contentWindow.document.body.clientHeight;
                if (h > 0) {
                    if (h > 600) h = 600;
                    if (h < 100) h = 100;

                    if (h + 'px' != obj.style.height) {
                        obj.style.height = h + 'px';
                    }
                }
                //console.log(obj.contentWindow.document.body.scrollHeight);
            }
        } catch (e) {
            obj.style.height = '380px';
        }
    }, 200);
}

function resizeIframePopup() {
    var popup = document.getElementById('iframePopup');
    var iframe = popup.getElementsByTagName('iframe')[0];
    resizeIframe(iframe);
}

$(window).on('load', function () {
    //$(".ui-ios-overlay-shadow").hide();
    if (document.getElementById("shadowOverlay") != null) {
        document.getElementById("shadowOverlay").style.display = 'none';
    }
});

//$(window).load(function () {
//    $(".ui-ios-overlay-shadow").hide();
//    //ADRLoadImage();
//});

function addscroll() {
    var a = $(window).height();
    $('.scroll_left').css({ 'height': '0' });
    $('#infovis').css({ 'height': '0' });
    $('.scroll_left').css({ 'height': (a - 200) + 'px', 'overflow-y': 'auto' });
    $('#infovis').css({ 'height': (a - 200) + 'px' });
}

function PreventChangePage(msg) {
    ShowNotify('error', msg);
    HideOverlay();
    return false;
}

function AddParamUrl(url, param, value)
{
    if (url.indexOf('?') !== -1)
    {
        url = url + '&' + param + '=' + value;
    }
    else
    {
        url = url + '?' + param + '=' + value;
    }

    return url;
}

function ReloadWithMasterDB()
{
    var url = window.location.href;

    //if (url.indexOf('dbSlave=false') == -1) {
    //    url = AddParamUrl(url, 'dbSlave', false);
    //}

    window.location.href = url;
}

function CreateUpdateFormSubmitSuccess(response) {
    if (response.ID == 1) {
        parent.ReloadWithMasterDB();
    }
    else if (response.status === 401) {
        window.location.href = response.redirect;
    }
    else {
        ShowNotify('error', response.Message);
        HideOverlay();
    }
}
/*Check is anesth service*/
var anesthArr = ["e30", "gây mê", "gây tê", "anesthesia"];
function IsAnesthService(serviceCode, serviceName) {
    var returnValue = false;
    for (var i = 0; i < anesthArr.length; i++) {
        if (serviceCode.toLowerCase().match("^" + anesthArr[i])) {
            return true;
        }
        else if (serviceName.toLowerCase().indexOf(anesthArr[i]) !== -1) {
            return true;
        }
    }
    return returnValue;
}
/**
 * Check service is surgical or procedure
 * @param {any} serviceCode
 */
var surgicalProcedureArr = ["e15", "phẫu thuật", "surgical", "procedure", "thủ thuật"];
function IsSurgicalProcedureService(serviceCode, serviceName) {
    var returnValue = false;
    for (var i = 0; i < surgicalProcedureArr.length; i++) {
        if (serviceCode.text().toLowerCase().match("^" + surgicalProcedureArr[i])) {
            return true;
        }
        else if (serviceName.text().toLowerCase().indexOf(surgicalProcedureArr[i]) !== -1) {
            return true;
        }
    }
    
    return returnValue;
}
String.prototype.checkValidHour = function () {
    var regexp = /^([01]?[0-9]|2[0-3]):[0-5][0-9]$/;
    var correct = regexp.test(this);
    return correct;
};
String.prototype.dateToINT = function () {
    var strReturn = this.substring(0, this.indexOf(' '));
    strReturn = strReturn.replaceAll("/", "");
    return strReturn;
}
