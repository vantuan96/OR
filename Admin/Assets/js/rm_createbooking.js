var days = [
    'Chủ nhật', //Sunday starts at 0
    'Thứ hai',
    'Thứ ba',
    'Thứ tư',
    'Thứ năm',
    'Thứ sáu',
    'Thứ bảy'
];

$('#datepickerBooking').datepicker('setStartDate', new Date()).on('hide', function (ev) {
    $('#BookingDate').valid();
    CheckRoomAvailable();
    if (ev.date != undefined) {
        $('#bookingTime').text(days[ev.date.getDay()] + ', ' + $('#BookingDate').val());
    }
});

$('#RoomId').change(function (e) {
    CheckRoomAvailable();
});

$('#FromTime').change(function (e) {
    CheckRoomAvailable();
});

$('#ToTime').change(function (e) {
    CheckRoomAvailable();
});

function CheckRoomAvailable() {
    var bookingId = $('#BookingId').val();
    var roomId = $('#RoomId').val();
    var bookingDate = $('#BookingDate').val();
    var fromTime = $('#FromTime').val();
    var toTime = $('#ToTime').val();
    
    if (roomId.length > 0 && bookingDate.length > 0 && fromTime.length > 0 && toTime.length > 0) {
        bookingDate = bookingDate.replace(/(\d+)\/(\d+)\/(\d+)/, '$3-$2-$1');

        var startTime = bookingDate + ' ' + fromTime;
        var endTime = bookingDate + ' ' + toTime;

        $('#StartTime').val(startTime);
        $('#EndTime').val(endTime);

        $.post(urlCheckRoomAvailable, {
            bookingId: bookingId,
            roomId: roomId,
            startTime: startTime,
            endTime: endTime
        }, function (resObj) {
            if (resObj.ID > 0) {
                $('#checkRoomAvailableMessage').text('');
                $('#checkRoomAvailableMessage').parent().parent().addClass('hidden');
                parent.resizeIframePopup();
                hourIsValid = true;
            }
            else {
                $('#checkRoomAvailableMessage').text(resObj.Message);
                $('#checkRoomAvailableMessage').parent().parent().removeClass('hidden');
                parent.resizeIframePopup();
                hourIsValid = false;
            }
        });
    }
}
$('#FormCreateUpdateBooking [type="submit"]').click(function (e) {
    if (!hourIsValid) {
        e.preventDefault();
        return false;
    }
});

$('#FormCreateUpdateBooking').submit(function (e) {
    e.preventDefault();
    if (!$('#FormCreateUpdateBooking').valid() || !hourIsValid) {
        HideOverlay();
        return false;
    }

    $('#BookingTitle').val($('#BookingTitle').val().replace(/</g, '&lt;').replace(/>/g, '&gt;'));
    $('#BookingDescription').val($('#BookingDescription').val().replace(/</g, '&lt;').replace(/>/g, '&gt;'));
    var data = $(this).serialize();

    $.post(urlCreateBooking, data, function (resObj) {
        if (resObj.Status) {
            parent.flagReloadPage = true;
            $('.modal-header .close').trigger('click');
        }
        else {
            ShowNotify('error', resObj.Message);
        }
    });
});