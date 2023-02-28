var lazySubmit_Processed = 0;
var lazySubmit_Submitting = false;
var lazySubmit_Error = false;

function LazySubmit(listData, funcAjaxSubmit, funcSuccess) {
    if (listData == null || listData.length == 0) return;

    var keys = CountKeyOfAnObject(listData[0]);
    var maxItems = 300 / keys;

    lazySubmit_Processed = 0;
    lazySubmit_Error = false;
    ShowLazySubmitProgressBar(listData.length);

    var interval1 = setInterval(function () {
        if (lazySubmit_Submitting) return;
        lazySubmit_Submitting = true;

        if (lazySubmit_Processed < listData.length)
        {
            var tempList = [];
            for (var i = lazySubmit_Processed; i < listData.length && i < (lazySubmit_Processed + maxItems) ; i++)
            {
                tempList.push(listData[i]);
            }

            funcAjaxSubmit(tempList, function (response) {
                if (response.ID > 0)
                    lazySubmit_Processed += tempList.length;
                else {
                    lazySubmit_Error = true;
                    lazySubmit_Processed = listData.length;
                    ShowNotify('error', response.Message);
                }

                lazySubmit_Submitting = false;
            });
        }
        else
        {
            clearInterval(interval1);

            if (lazySubmit_Error == false)
                funcSuccess();

            lazySubmit_Submitting = false;
        }
    }, 500);
}


function CountKeyOfAnObject(obj)
{
    var keys = [];

    for (key in obj) {
        keys.push(key);
    }

    return keys.length;
}

function ShowLazySubmitProgressBar(datalength) {
    $('#shadowProgress').show();
    ShowOverlay(true);
    var interval = setInterval(function () {
        var percent = Math.floor(lazySubmit_Processed / datalength * 100) + "%";
        $('#shadowProgress .progress-bar').text(percent);
        $('#shadowProgress .progress-bar').css({ width: percent });

        if (lazySubmit_Processed >= datalength) {
            HideLazySubmitProgressBar();
            clearInterval(interval);
        }
    }, 500);
}

function HideLazySubmitProgressBar() {
    $('#shadowProgress').hide();
    HideOverlay();
    $('#shadowProgress .progress-bar').text("");
    $('#shadowProgress .progress-bar').css({ width: "0" });
}