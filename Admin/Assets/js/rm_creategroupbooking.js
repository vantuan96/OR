var CreateBookingApp = angular.module('CreateBooking', []);

CreateBookingApp.controller('GroupBookingController', ['$scope', function ($scope) {
    $scope.PendingSave = false;
    $scope.ListRoom = listRoom;
    $scope.ListStartTime = listStartTime;
    $scope.ListEndTime = listEndTime;
    $scope.CurrentBooking = InitCurrentBooking();
    $scope.ListBooking = InitListBooking();
    $scope.TempBookingId = [];

    $scope.PushNewBooking = function (displayDate) {
        var arrDate = displayDate.split('/');
        var fromTime = $scope.CurrentBooking.DisplayFromTime.split(':');
        var toTime = $scope.CurrentBooking.DisplayToTime.split(':');
        var startTime = new Date(arrDate[2], arrDate[1] - 1, arrDate[0], fromTime[0], fromTime[1], 0);
        var endTime = new Date(arrDate[2], arrDate[1] - 1, arrDate[0], toTime[0], toTime[1], 0);

        var bookingId = 0;
        // tìm bookingId của ngày tương ứng
        for (var i = 0; i < $scope.TempBookingId.length; i++) {
            if ($scope.TempBookingId[i].DisplayDate == displayDate) {
                bookingId = $scope.TempBookingId[i].BookingId;
                break;
            }
        }

        var newBookingItem = {
            BookingId: bookingId,
            RoomId: $scope.CurrentBooking.RoomId,
            BookingGroupId: bookingGroupId,
            BookingTitle: $scope.CurrentBooking.BookingTitle,
            BookingDescription: $scope.CurrentBooking.BookingDescription,
            StartTime: startTime,
            EndTime: endTime,
            BranchId: branchId,

            RoomName: FindRoomName($scope.CurrentBooking.RoomId),
            DisplayDate: displayDate,
            DisplayFromTime: bookingCalendar.timeToString(startTime),
            DisplayToTime: bookingCalendar.timeToString(endTime),

            IsEditing: false,
            IsEditted: true,
            IsPassed: false,
            IsError: false,
        };

        $scope.ListBooking.push(newBookingItem);
        $scope.editBooking(startTime);
        $scope.PendingSave = true;
    };

    $scope.SpliceBooking = function (displayDate) {
        var i = 0;
        var changeActiveBooking = false;
        var deleted = false;

        //bootbox.confirm('Bạn có muốn xóa lịch họp này?', function (result) {
        //    if (result) {
                for (i = 0; i < $scope.ListBooking.length; i++) {
                    var booking = $scope.ListBooking[i];
                    if (booking.IsPassed == false && booking.DisplayDate == displayDate) {
                        // giữ lại bookingId để nếu có thêm lại thì xử lý như update
                        if (booking.BookingId > 0) {
                            $scope.TempBookingId.push({
                                DisplayDate: booking.DisplayDate,
                                BookingId: booking.BookingId
                            });
                        }

                        changeActiveBooking = booking.IsEditing;
                        $scope.ListBooking.splice(i, 1);

                        deleted = true;
                        $scope.PendingSave = true;

                        break;
                    }
                }

                if (deleted && changeActiveBooking) {
                    if (i > 0 && $scope.ListBooking[i - 1].IsPassed == false) {
                        $scope.editBooking($scope.ListBooking[i - 1].StartTime);
                    }
                    else {
                        for (i = 0; i < $scope.ListBooking.length; i++) {
                            if ($scope.ListBooking[i].IsPassed == false) {
                                $scope.editBooking($scope.ListBooking[i].StartTime);
                                break;
                            }
                        }
                    }
                }
        //    }
        //});

        return deleted;
    };

    $scope.removeBooking = function (date) {
        var systemDate = bookingCalendar.dateToString(date, 'yyyy-mm-dd');
        var displayDate = bookingCalendar.dateToString(date, 'dd/mm/yyyy');
        bookingCalendar.removeDate(systemDate, displayDate);
    };

    $scope.editBooking = function (date) {
        var displayDate = bookingCalendar.dateToString(date, 'dd/mm/yyyy');

        for (var i = 0; i < $scope.ListBooking.length; i++) {
            var booking = $scope.ListBooking[i];

            if (booking.DisplayDate == displayDate) {
                booking.IsEditing = true;

                $scope.CurrentBooking.BookingId = booking.BookingId;
                $scope.CurrentBooking.BookingTitle = booking.BookingTitle;
                $scope.CurrentBooking.RoomId = booking.RoomId;
                $scope.CurrentBooking.BookingDescription = booking.BookingDescription;

                $scope.CurrentBooking.DisplayFromTime = booking.DisplayFromTime;
                $scope.CurrentBooking.DisplayToTime = booking.DisplayToTime;
            }
            else {
                booking.IsEditing = false;
            }
        }
    };

    $scope.updateBooking = function () {
        for (var i = 0; i < $scope.ListBooking.length; i++) {
            var booking = $scope.ListBooking[i];

            if (booking.IsPassed == false && booking.IsEditing && booking.BookingId == $scope.CurrentBooking.BookingId) {
                booking.BookingTitle = $scope.CurrentBooking.BookingTitle;
                booking.RoomId = $scope.CurrentBooking.RoomId;
                booking.RoomName = FindRoomName(booking.RoomId);
                booking.BookingDescription = $scope.CurrentBooking.BookingDescription;

                booking.DisplayFromTime = $scope.CurrentBooking.DisplayFromTime;
                booking.DisplayToTime = $scope.CurrentBooking.DisplayToTime;

                var arrDate = bookingCalendar.dateToString(booking.StartTime, 'dd/mm/yyyy').split('/');
                var fromTime = $scope.CurrentBooking.DisplayFromTime.split(':');
                var toTime = $scope.CurrentBooking.DisplayToTime.split(':');
                var startTime = new Date(arrDate[2], arrDate[1] - 1, arrDate[0], fromTime[0], fromTime[1], 0);
                var endTime = new Date(arrDate[2], arrDate[1] - 1, arrDate[0], toTime[0], toTime[1], 0);

                booking.StartTime = startTime;
                booking.EndTime = endTime;
                booking.IsEditted = true;
                booking.IsError = false;
                $scope.PendingSave = true;

                break;
            }
        }
    };

    $scope.asyncBookingInfo = function () {
        bootbox.confirm('Bạn có muốn đồng bộ thông tin này cho những lịch họp còn lại?', function (result) {
            if (result) {
                for (var i = 0; i < $scope.ListBooking.length; i++) {
                    var booking = $scope.ListBooking[i];

                    if (booking.IsPassed == false) {
                        booking.BookingTitle = $scope.CurrentBooking.BookingTitle;
                        booking.RoomId = $scope.CurrentBooking.RoomId;
                        booking.RoomName = FindRoomName(booking.RoomId);
                        booking.BookingDescription = $scope.CurrentBooking.BookingDescription;

                        booking.DisplayFromTime = $scope.CurrentBooking.DisplayFromTime;
                        booking.DisplayToTime = $scope.CurrentBooking.DisplayToTime;

                        var arrDate = bookingCalendar.dateToString(booking.StartTime, 'dd/mm/yyyy').split('/');
                        var fromTime = $scope.CurrentBooking.DisplayFromTime.split(':');
                        var toTime = $scope.CurrentBooking.DisplayToTime.split(':');
                        var startTime = new Date(arrDate[2], arrDate[1] - 1, arrDate[0], fromTime[0], fromTime[1], 0);
                        var endTime = new Date(arrDate[2], arrDate[1] - 1, arrDate[0], toTime[0], toTime[1], 0);

                        booking.StartTime = startTime;
                        booking.EndTime = endTime;
                        booking.IsEditted = true;
                    }
                }

                $scope.PendingSave = true;
                $scope.$apply();
            }
        });
    };

    $scope.addClassError = function (dates) {
        for (var i = 0; i < $scope.ListBooking.length; i++) {
            var booking = $scope.ListBooking[i];
            if ($.inArray(booking.DisplayDate, dates) > -1) {
                booking.IsError = true;
            }
        }
    };

    $scope.TimeValidate = function () {
        var failed = false;

        for (var i = 0; i < $scope.ListBooking.length; i++) {
            var booking = $scope.ListBooking[i];
            if (booking.StartTime >= booking.EndTime) {
                booking.IsError = true;
                failed = true;
            }
        }

        return !failed;
    };
}]);

function InitCurrentBooking() {
    var startTime = new Date(parseInt(currentBooking.StartTime.substr(6)));
    var endTime = new Date(parseInt(currentBooking.EndTime.substr(6)));

    var newBookingItem = {
        BookingId: currentBooking.BookingId,
        RoomId: currentBooking.RoomId,
        BookingGroupId: currentBooking.BookingGroupId,
        BookingTitle:  currentBooking.BookingTitle == null ? null : currentBooking.BookingTitle.replace(/&lt;/g, '<').replace(/&gt;/g, '>'),
        BookingDescription: currentBooking.BookingDescription == null ? null : currentBooking.BookingDescription.replace(/&lt;/g, '<').replace(/&gt;/g, '>'),
        StartTime: startTime,
        EndTime: endTime,
        BranchId: currentBooking.BranchId,

        DisplayFromTime: bookingCalendar.timeToString(startTime),
        DisplayToTime: bookingCalendar.timeToString(endTime),
    };

    return newBookingItem;
}

function InitListBooking() {
    var newListBooking = [];
    var currentTime = new Date();

    for (var i = 0; i < listBooking.length; i++) {
        var booking = listBooking[i];
        var startTime = new Date(parseInt(booking.StartTime.substr(6)));
        var endTime = new Date(parseInt(booking.EndTime.substr(6)));

        var systemDate = bookingCalendar.dateToString(startTime, 'yyyy-mm-dd');
        var displayDate = bookingCalendar.dateToString(startTime, 'dd/mm/yyyy');

        var newBookingItem = {
            BookingId: booking.BookingId,
            RoomId: booking.RoomId,
            BookingGroupId: booking.BookingGroupId,
            BookingTitle: booking.BookingTitle.replace(/&lt;/g, '<').replace(/&gt;/g, '>'),
            BookingDescription: booking.BookingDescription == null ?
                null : booking.BookingDescription.replace(/&lt;/g, '<').replace(/&gt;/g, '>'),
            StartTime: startTime,
            EndTime: endTime,
            BranchId: booking.BranchId,

            RoomName: FindRoomName(booking.RoomId),
            DisplayDate: displayDate,
            DisplayFromTime: bookingCalendar.timeToString(startTime),
            DisplayToTime: bookingCalendar.timeToString(endTime),

            IsEditing: booking.BookingId == currentBooking.BookingId,
            IsEditted: false,
            IsPassed: startTime <= currentTime,
            IsError: false,
        };

        selectedDates.push(systemDate);
        newListBooking.push(newBookingItem);
    }

    return newListBooking;
}

function FindRoomName(roomId) {
    for (var i = 0; i < listRoom.length; i++) {
        if (listRoom[i].RoomId == roomId) {
            return listRoom[i].RoomName;
        }
    }

    return '';
}

var bookingCalendar = {
    pushDate: function (systemDate, displayDate) {
        selectedDates.push(systemDate);

        angular.element($('#CreateGroupBooking')).scope().PushNewBooking(displayDate);
        $('.day.calendar-day-' + systemDate).addClass('active');
    },
    removeDate: function (systemDate, displayDate) {
        var isSuccess = angular.element($('#CreateGroupBooking')).scope().SpliceBooking(displayDate);

        if (isSuccess) {
            for (var i = 0; i < selectedDates.length; i++) {
                if (selectedDates[i] == systemDate) {
                    selectedDates.splice(i, 1);
                    break;
                }
            }

            $('.day.calendar-day-' + systemDate).removeClass('active');
        }
    },
    refreshCalendar: function () {
        for (var i = 0; i < selectedDates.length; i++) {
            var dayEle = $('.day.calendar-day-' + selectedDates[i]);
            $(dayEle).addClass('active');
        }
    },
    dateToString: function (date, format) {
        var year = date.getFullYear();
        var month = date.getMonth() + 1;
        var day = date.getDate();

        if (month < 10) month = '0' + month;
        if (day < 10) day = '0' + day;

        return format.replace('yyyy', year).replace('mm', month).replace('dd', day);
    },
    timeToString: function (date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();

        if (hours < 10) hours = '0' + hours;
        if (minutes < 10) minutes = '0' + minutes;

        return hours + ':' + minutes;
    }
};

$(function () {
    $('#BookingCalendar').clndr({
        template: $('#BookingCalendar-template').html(),
        doneRendering: function () { bookingCalendar.refreshCalendar(); },
        clickEvents: {
            click: function (target) {
                var selectedDate = target.date._d;
                var systemDate = bookingCalendar.dateToString(selectedDate, 'yyyy-mm-dd');
                var displayDate = bookingCalendar.dateToString(selectedDate, 'dd/mm/yyyy');

                var dayEle = $('.day.calendar-day-' + systemDate);

                if (!$(dayEle).hasClass('past')) {
                    var currentBooking = angular.element($('#CreateGroupBooking')).scope().CurrentBooking;
                    if (currentBooking.BookingTitle == null
                        || currentBooking.RoomId == 0
                        || currentBooking.DisplayFromTime == '00:00'
                        || currentBooking.DisplayToTime == '00:30') {
                        bootbox.alert('Vui lòng nhập tiêu đề cuộc họp, phòng họp và giờ họp trước');
                        return false;
                    }

                    if ($(dayEle).hasClass('active')) {
                        bookingCalendar.removeDate(systemDate, displayDate);
                        angular.element($('#CreateGroupBooking')).scope().$apply();
                    }
                    else if (angular.element($('#CreateGroupBooking')).scope().ListBooking.length < 10) {
                        bookingCalendar.pushDate(systemDate, displayDate);
                        angular.element($('#CreateGroupBooking')).scope().$apply();
                    }
                    else {
                        bootbox.alert('Chỉ được đặt tối đa 10 cuộc họp');
                    }
                }
            }
        },
        adjacentDaysChangeMonth: true,
        weekOffset: -1,
        daysOfTheWeek: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
        numberOfRows: 6
    });

    $('#FormCreateUpdateGroupBooking').submit(function (e) {
        e.preventDefault();
        if (!$('#FormCreateUpdateGroupBooking').valid() || !hourIsValid) {
            HideOverlay();
            return false;
        }

        var listBooking = angular.element($('#CreateGroupBooking')).scope().ListBooking;

        if (listBooking.length <= 1) {
            bootbox.alert('Vui lòng chọn ngày họp từ 2 ngày trở lên');
            HideOverlay();
            return false;
        }

        var timeIsValid = angular.element($('#CreateGroupBooking')).scope().TimeValidate();

        if (!timeIsValid) {
            ShowNotify('error', 'Giờ kết thúc phải lớn hơn giờ bắt đầu');
            HideOverlay();
            return false;
        }

        var data = JSON.stringify({ model: listBooking });

        $.ajax({
            type: 'post',
            url: urlCreateGroupBooking,
            data: data,
            contentType: "application/json; charset=utf-8",
            traditional: true,
            success: function (resObj, textStatus, xhr) {
                if (typeof (resObj.Status) == 'undefined') {
                    window.location.href = window.location.href;
                }
                else if (resObj.Status) {
                    if (resObj.Href != null && resObj.Href.length > 0) {
                        window.location.href = resObj.Href;
                    }
                    else {
                        window.location.href = urlBookingListOnDay;
                    }
                }
                else if (typeof (resObj.Messages) == 'undefined') {
                    ShowNotify('error', resObj.Message);
                }
                else {
                    if (resObj.DateError != null && resObj.DateError.length > 0) {
                        angular.element($('#CreateGroupBooking')).scope().addClassError(resObj.DateError);
                        angular.element($('#CreateGroupBooking')).scope().$apply();
                    }
                    var errMgs = resObj.Messages.join('<br/>');
                    ShowNotify('error', errMgs, 60000);
                }
            }
        });
    });
});