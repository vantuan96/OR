$(function () {
    report.init();
});

var report = {
    init: function () {

        if ($('#reportrange').length > 0) {
            $('#reportrange').data('daterangepicker').setStartDate(_initFromdate);
            $('#reportrange').data('daterangepicker').setEndDate(_initTodate);
        }

        if (_jsonData.length > 0) {
            report.ShowReportData(_jsonData, "#tableDiv");
        }
        
       
    },
    ShowReportData: function (jsonData, $tbDiv) {

            var tableHeaders = "";
            var tbBody = "";
            $.each(jsonData, function (key, value) {

                if (key == 0) {
                    $.each(value, function (title, val) {
                        if (title !== 'TotalRows') {
                            tableHeaders += "<th>" + title + "</th>";
                        }
                    });
                }

                var rowData = '';
                $.each(value, function (title, val) {
                    if (title !== 'TotalRows') {
                        rowData += '<td>' + val + '</td>';
                    }
                });
                tbBody += '<tr>' + rowData + '</tr>';
            });
            $($tbDiv).empty();
            $($tbDiv).append('<table id="displayTable" class="table table-striped"><thead><tr style="font-size:9px">'
                + tableHeaders + '</tr></thead><tbody>' + tbBody + '</tbody></table>');
       
    }
};