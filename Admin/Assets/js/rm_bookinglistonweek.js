$(function () {
    tisa_easy_pie_chart.init();
    
    $('#changeWeekCalendar').datepicker('setDate', selectedDate).on('changeDate', function (ev) {
        var selectedDay = $('.datepicker .datepicker-days table tr td.active.day');
        $(selectedDay).parent().addClass('week-active');

        var selectedDate = $("#changeWeekCalendar").data('datepicker').getFormattedDate('yyyy-mm-dd');

        ShowOverlay(true);
        window.location.href = urlListBookingOnWeek + "?startDate=" + selectedDate;
    }).on('show', function (ev) {
        var selectedDay = $('.datepicker .datepicker-days table tr td.active.day');
        $(selectedDay).parent().addClass('week-active');
    });
});

tisa_easy_pie_chart = {
    init: function () {
        if ($('.easy_chart_pages').length) {
            $('.easy_chart_pages').easyPieChart({
                animate: 2000,
                size: 38,
                lineWidth: 2,
                scaleColor: false,
                barColor: '#E31837',
                trackColor: '#f3f3f3',
                lineCap: 'square'
            });
        }
    }
}