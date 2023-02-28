$(function () {
    $("#content-6").mCustomScrollbar({
        axis: "x",
        theme: "light-3",
        advanced: { autoExpandHorizontalScroll: true },
        callbacks: {
            onInit: function () {
                $('.page_content.loading').removeClass('loading');
                $("#content-6").mCustomScrollbar("scrollTo", $(scrollToHour));
            }
        }
    });

    if ($('.mCustomScrollbar').width() >= $('.table_div_content').width()) {
        $('.page_content.loading').removeClass('loading');
    }

    $('#changeDateCalendar').datepicker('setDate', selectedDate).on('changeDate', function (ev) {
        var selectedDate = $("#changeDateCalendar").data('datepicker').getFormattedDate('yyyy-mm-dd');

        ShowOverlay(true);
        window.location.href = urlListBookingOnDay + "?date=" + selectedDate;
    });
});