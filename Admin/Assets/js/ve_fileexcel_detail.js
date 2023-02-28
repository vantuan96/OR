var listUpdate = [];

$(function () {
    if ($('#multipleUpdate input[data-type="DateTime"]').length > 0)
        $('#multipleUpdate input[data-type="DateTime"]').datetimepicker();
    if ($('#multipleUpdate input[data-type="Date"]').length > 0) 
        $('#multipleUpdate input[data-type="Date"]').datetimepicker({ timepicker: false, format: 'd/m/Y' });

    var dt = $('table').DataTable({
        keys: true,
        lengthMenu: [[10, 50, 100, -1], [10, 50, 100, "Tất cả"]],
        pageLength: 50,
        //paging: false,
        scrollX: true,
        scrollY: 500,
        scrollCollapse: true,
        autoWidth: false,
        language: dtvn,
        order: [[1, 'asc']],
        columnDefs: [{
            "targets": 'no-sort',
            "orderable": false,
        }]
    });
    
    dt.on('draw', function () {
        var body = $(dt.table().body());

        body.unhighlight();
        body.highlight(dt.search());
    });

    dt.on('key-focus', function (e, datatable, cell, originalEvent) {
        var td = $('table tbody td.focus');
        if ($(td).hasClass('allowEdit')) {
            e.preventDefault();

            $(td).addClass('editting');
            CreateInput($(td));
        }
    })
    .on('key-blur', function (e, datatable, cell) {
        var td = $('table tbody td.editting');
        if ($(td).length > 0) {
            $(td).removeClass('editting');
            ValueChanged($('.displayControl > *', $(td)));
        }
    });

    $('.btnSave').click(function (e) {
        e.preventDefault();
        
        LazySubmit(listUpdate, function (tempList, callback) {
            $.post(urlUpdateFileExcelDetail, { fid: fid, fsid: fsid, listUpdate: tempList }, function (response) {
                callback(response);
            });
        }, ReloadWithMasterDB);
    });

    $('.btnCancel').click(function (e) {
        window.location.reload();
    });

    $('.ckbCheckAll').change(function (e) {
        $('.ckbToUpdate').prop('checked', $(this).is(':checked'));
    });

    $('.ckbMultipleUpdate').change(function (e) {
        var control = $('.inputValue', $(this).parent().parent());

        if ($(this).is(':checked'))
        {
            $(control).prop('disabled', false);
            $(control).focus();
        }
        else
        {
            $(control).prop('disabled', true);
        }
    });

    $('#ddlChangeSite').change(function (e) {
        var newFsId = $(this).val();
        window.location.href = urlListFileExcelDetail + '?fid=' + fid + '&fsid=' + newFsId;
    });
});

function CreateInput(element)
{
    if ($('.displayControl *', $(element)).length == 0)
    {
        $('.controls .' + $(element).data('control')).clone().appendTo($('.displayControl', $(element)));

        var type = $('.displayControl .' + $(element).data('control'), $(element)).data('type');
        if (type == 'IntType' || type == 'DoubleType')
        {
            InputNumber($('.displayControl .' + $(element).data('control'), $(element)));
        }
        else if (type == 'DateTime')
        {
            $($('.displayControl .' + $(element).data('control'), $(element))).datetimepicker();
        }
        else if (type == 'Date') {
            $($('.displayControl .' + $(element).data('control'), $(element))).datetimepicker({ timepicker: false, format: 'd/m/Y' });
        }
    }

    var control = $('.displayControl .' + $(element).data('control'), $(element));
    var oldValue = GetOldValue($(control).data('type'), $('.displayValue', $(element)));

    if ($(control).data('type') == 'BoolType')
    {
        $(control).prop('checked', oldValue);
        $(control).data('old-value', oldValue);
    }
    else
    {
        $(control).val(oldValue);
        $(control).data('old-value', oldValue);
    }

    $(control).focus();

    if ($(control).attr('type') == 'text')
    {
        $(control).select();
    }
}

function GetOldValue(type, displayElement)
{
    if (type == 'BoolType')
    {
        return $('i', $(displayElement)).length > 0;
    }
    else if (type == 'IntType' || type == 'DoubleType')
    {
        return $(displayElement).text().split(',').join('');
    }
    else 
    {
        return $(displayElement).text();
    }
}

function ValueChanged(control)
{
    var value = GetControlValue(control);
    var oldvalue = $(control).data('old-value');
    var tdElement = $(control).parent().parent();

    //if (value == '' && $(element).data('is-required') == 'True')
    //{
    //    ShowNotify('error', 'Ô này không được để trống');
    //    event.preventDefault();
    //    return false;
    //}

    if (value != oldvalue)
    {
        var row = $(control.parent()).data('row-number');
        var col = $(control).data('col-index');

        ShowValue($(control).data('type'), value, tdElement);
        PushToListUpdate(row, col, value);

        $('.btnSave').removeClass('disabled');
        $('.btnCancel').removeClass('disabled');
    }

    $(tdElement).removeClass('editting');
}

function ShowValue(type, value, td)
{
    if (type == 'BoolType') {
        if (value == true)
            $('.displayValue', td).html('<i class="fa fa-check"></i>');
        else
            $('.displayValue', td).text('');
    }
    else {
        if (value.length == 0) {
            $('.displayValue', td).text('');
        }
        else {
            if (type == 'IntType' || type == 'DoubleType')
                $('.displayValue', td).text(numberWithCommas(value));
            else
                $('.displayValue', td).text(value);
        }
    }
}

function PushToListUpdate(row, col, value)
{
    var exist = false;
    for (var i = 0; i < listUpdate.length; i++) {
        if (listUpdate[i].RowNumber == row && listUpdate[i].ColumnIndex == col) {
            listUpdate[i].Value = value;
            exist = true;
        }
    }

    if (exist == false) {
        listUpdate.push({
            RowNumber: row,
            ColumnIndex: col,
            Value: value
        });
    }
}

function GetControlValue(element)
{
    if ($(element).attr('type') == 'checkbox') 
        return $(element).is(':checked');
    
    var value = $.trim($(element).val());

    if ($(element).data('type') == 'IntType' || $(element).data('type') == 'DoubleType')
    {
        value = value > 2147483647 ? 0 : value;
    }
    else if ($(element).data('type') == 'DateTime')
    {
        var d = moment(value, 'DD/MM/YYYY HH:mm');
        var year = d.toDate().getFullYear();
        if (year < 100)
        {
            year = year + 2000;
            value = FormatDateTime(d.toDate(), 'dd/MM/' + year + ' HH:mm');
        }
        else
        {
            value = FormatDateTime(d.toDate(), 'dd/MM/yyyy HH:mm');
        }
    }
    else if ($(element).data('type') == 'Date') {
        var d = moment(value, 'DD/MM/YYYY');
        var year = d.toDate().getFullYear();
        if (year < 100) {
            year = year + 2000;
            value = FormatDateTime(d.toDate(), 'dd/MM/' + year);
        }
        else {
            value = FormatDateTime(d.toDate(), 'dd/MM/yyyy');
        }
    }

    return value;
}

function ConfirmUpdateFileExcel(fsId)
{
    bootbox.confirm({
        title: "CHÚ Ý",
        message: "Bạn phải chắc chắn đã cập nhật toàn bộ file. Bạn có muốn hoàn tất không?",
        callback: function (confirm) {
            if (confirm) {
                var postData = new Array();
                postData.push({ name: 'fsId', value: fsId });
                commonUtils.postAjaxWithToken(urlConfirmUpdateFileExcel, postData, function (result) {
                    if (result.ID > 0) {
                        ReloadWithMasterDB();
                    }
                    else {
                        ShowNotify('error', result.Message);
                        HideOverlay();
                    }
                });
            }
        }
    });
}

function ApplyMultipleUpdate()
{
    $('.ckbMultipleUpdate:checked').each(function () {
        var control = $('.inputValue', $(this).parent().parent());
        var value = GetControlValue(control);
        var col = $(control).data('col-index');

        $('.ckbToUpdate:checked').each(function () {
            var row = $(this).data('row-number');
            var tr = $(this).parent().parent();
            var tdElement = $('.allowEdit[data-col-index="' + col + '"]', $(tr));
            var oldValue = GetOldValue($(control).data('type'), $('.displayValue', $(tdElement)));

            if (value != oldValue) {
                ShowValue($(control).data('type'), value, tdElement);
                PushToListUpdate(row, col, value);
            }
        });
    });

    if (listUpdate.length > 0) {
        $('.btnSave').removeClass('disabled');
        $('.btnCancel').removeClass('disabled');
    }

    $('.ckbMultipleUpdate').prop('checked', false).trigger('change');
}

function ShowMultipleUpdate()
{
    if ($('.ckbToUpdate:checked').length == 0)
    {
        ShowNotify('error', 'Vui lòng check chọn các dòng cần cập nhật');
    }
    else
    {
        $('#checkedCount').text($('.ckbToUpdate:checked').length);
        var box = $('#multipleUpdate');
        box.modal('show');
    }
}