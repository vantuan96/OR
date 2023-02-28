var AssignPhotoBooking = angular.module('AssignPhotoBooking', []);

AssignPhotoBooking.controller('ListPhotographerController', ['$scope', function ($scope) {
    $scope.CanUpdate = canUpdate;
    $scope.AssignFor = assignFor;
    $scope.StartDate = startDate;
    $scope.EndDate = endDate;

    if (requestDate == null) {
        $scope.RequestDate = null;
        $scope.StrRequestDate = null;
    }
    else {
        $scope.RequestDate = requestDate;
        $scope.StrRequestDate = ToDateFormated(requestDate, systemFormatDate);
    }

    var now = new Date();
    $scope.ToDay = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    $scope.StrToDay = ToDateFormated($scope.ToDay, systemFormatDate);

    $scope.ListDate = BuildListDate(startDate, endDate);
    $scope.ListPhotographer = RepairListPhotographer(listPhotographer, startDate, endDate);

    $scope.OnClickRadioAssign = function (userIndex, dateIndex) {
        $scope.AssignFor = $scope.ListPhotographer[userIndex].UID;
        $scope.RequestDate = $scope.ListPhotographer[userIndex].SkuStatistic[dateIndex].RequestDate;
        $scope.StrRequestDate = $scope.ListPhotographer[userIndex].SkuStatistic[dateIndex].StrRequestDate;
    }

    $scope.WeekChanged = function (listPhotographer, startDate, endDate) {
        $scope.StartDate = startDate;
        $scope.EndDate = endDate;

        $scope.ListDate = BuildListDate(startDate, endDate);
        $scope.ListPhotographer = RepairListPhotographer(listPhotographer, startDate, endDate);

        $scope.$apply();
    }
}]);

function RepairListPhotographer(list, startDate, endDate) {
    for (var i = 0; i < list.length; i++) {
        var user = list[i];
        user.SumSkuStatistic = 0;

        for (var j = 0; j < user.SkuStatistic.length; j++) {
            var date = user.SkuStatistic[j];

            user.SumSkuStatistic += date.NumberOfSku;
            date.RequestDate = parseJsonDate(date.RequestDate);
            date.StrRequestDate = ToDateFormated(date.RequestDate, systemFormatDate);
        }
    }

    return list;
}

function BuildListDate(startDate, endDate) {
    var list = [];

    var i = new Date(startDate);

    while (i <= endDate) {
        var date = new Date(i);

        var objDate = {
            DayOfWeek: listDayOfWeek[date.getDay()],
            StrDate: ToDateFormated(date, viewFormatDate)
        }

        if (date.getDay() == 0) {
            objDate.CssStyles = { color: '#e32b2c' };
        }
        else if (date.getDay() == 6) {
            objDate.CssStyles = { color: '#2684e4' };
        }

        list.push(objDate);

        i.setDate(i.getDate() + 1);
    }

    return list;
}

function ToDateFormated(date, format) {
    function pad(s) { return (s < 10) ? '0' + s : s; }

    return format.replace('dd', pad(date.getDate()))
        .replace('MM', pad(date.getMonth() + 1))
        .replace('yyyy', pad(date.getFullYear()))
}

$(function () {
    $('#SelectWeek').change(function (e) {
        var week = $(this).val();
        
        var startDate = new Date(startDateOfThisWeek);
        var endDate = new Date(startDateOfThisWeek);

        startDate.setDate(startDate.getDate() + week * 7);
        endDate.setDate(endDate.getDate() + week * 7 + 6);

        var reqdata = {
            dealId: $('#DID').val(),
            startDate: ToDateFormated(startDate, systemFormatDate),
            endDate: ToDateFormated(endDate, systemFormatDate)
        };

        $.post(urlGetListPhotographer, reqdata, function (list) {
            angular.element($('#ListPhotographer')).scope().WeekChanged(list, startDate, endDate);
        });

        var strStartDate = ToDateFormated(startDate, viewFormatDate);
        var strEndDate = ToDateFormated(endDate, viewFormatDate);

        $('#viewDateRange').text(strStartDate + ' - ' + strEndDate);
    });

    $('#FormAssign').submit(function (e) {
        e.preventDefault();

        var checkedRadio = $('input[name=RadioAssign]:checked');
        var data = {
            DID: $('#DID').val(),
            RequestType: $('input[name=RequestType]:checked').val(),
            AssignFor: $(checkedRadio).data('uid'),
            RequestDate: $(checkedRadio).data('date'),
            PhotoNote: $('#PhotoNote').val(),
            Comment: $('#Comment').val()
        };
        
        $.post(urlAssignPhotoBooking, data, function (res) {
            if (res.Status) {
                parent.reloadWhenClosePopupViewer = true;
                parent.ShowOverlay(true);
                parent.ClosePopupViewer();
            }
            else {
                ShowNotify('error', res.Message);
            }
        });
    });
});

CKEDITOR.on("instanceCreated", function (e) {
    var editorName = e.editor.name;
    if (editorName == "PhotoNoteEditor") {
        e.editor.on('change', function () {
            if ($('#PhotoNote').html() != $('#PhotoNoteEditor').html()) {
                var plaintext = $('#PhotoNoteEditor').html().replace(/(<p>|<\/p>|<br>|<b>|<\/b>|<strong>|<\/strong>|<i>|<\/i>|<em>|<\/em>|<u>|<\/u>|<s>|<\/s>|<strike>|<\/strike>)/g, '');
                plaintext = plaintext.replace(/&nbsp;| +/g, '');

                if (plaintext.length == 0) {
                    $('#PhotoNote').html('');
                }
                else {
                    $('#PhotoNote').html($('#PhotoNoteEditor').html());
                }

                $('#PhotoNote').trigger('change');
            }
        });
    }
});
