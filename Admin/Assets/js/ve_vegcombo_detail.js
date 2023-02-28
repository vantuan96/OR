var listQuantityUpdate = [];
var listNoteUpdate = [];

$(function () {

    var dt = $('table').DataTable({
        keys: true,
        paging: false,
        scrollX: true,
        scrollY: 500,
        scrollCollapse: true,
        autoWidth: false,
        language: dtvn
        
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
            $('input', $(td)).data('before-val', $('input', $(td)).val());
            $('input', $(td)).select().focus();
        }
    })
    .on('key-blur', function (e, datatable, cell) {
        var td = $('table tbody td.editting');
        if ($(td).length > 0) {
            $(td).removeClass('editting');
            ChangeValue(td);
        }
    });

    $('.btnCancel').click(function (e) {
        window.location.reload();
    });

    $('.btnSave').click(function (e) {
        e.preventDefault();

        if (listNoteUpdate.length > 0) {
            $.post(_updateDetailUrl, { ComboRtId: ComboRtId, SiteId: SiteId, ComboRsId: ComboRsId, listNote: listNoteUpdate }, function (response) {
                if (response.ID > 0) {
                    SubmitQuantity();
                }
                else {
                    ShowNotify('error', response.Message);
                }
            });
        }
        else {
            SubmitQuantity();
        }
        
    });
       
    $('#btnCompleteUpdateFile').click(function (e) {
        bootbox.confirm({
            title: "CHÚ Ý",
            message: "Bạn phải chắc chắn đã cập nhật toàn bộ. Bạn có muốn hoàn tất không?",
            callback: function (confirm) {
                if (confirm) {
                    ShowOverlay(true);
                    $.post(_completeUpdateUrl, { ComboRsId: ComboRsId }, function (result) {
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
    });
});

function SubmitQuantity() {
    if (listQuantityUpdate.length > 0) {
        LazySubmit(listQuantityUpdate, function (tempList, callback) {
            $.post(_updateDetailUrl, { ComboRtId: ComboRtId, SiteId: SiteId, ComboRsId: ComboRsId, listQuantity: tempList }, function (response) {
                callback(response);
            });
        }, ReloadWithMasterDB);
    }
    else {
        ReloadWithMasterDB();
    }
}

function ChangeValue(td) {
    var control = $('input', $(td));
    var beforeValue = $.trim($(control).data('before-val'));
    var afterValue = $.trim($(control).val());

    if (beforeValue != afterValue) {
        if ($(control).data('type') == 'note') {
            var comboId = $(control).data('combo');
            PushNoteUpdate(comboId, afterValue);
        }
        else {
            var comboId = $(control).data('combo');
            var date = $(control).data('date');
            PushQuantityUpdate(comboId, date, afterValue);
        }

        if ($(control).hasAttr('data-val-decimal')) {
            $('.displayValue', $(td)).text(numberWithCommas(afterValue));
        }
        else {
            $('.displayValue', $(td)).text(afterValue);
        }
        $('.btnSave, .btnCancel').removeClass('disabled');
    }
}

function PushNoteUpdate(comboId, value) {
    var found = false;
    for (var i = 0; i < listNoteUpdate.length; i++) {
        if (listNoteUpdate[i].ComboId == comboId) {
            listNoteUpdate[i].Note = value;
            found = true;
        }
    }

    if (found == false) listNoteUpdate.push({ ComboId: comboId, Note: value });
}

function PushQuantityUpdate(comboId, date, value) {
    var found = false;
    for (var i = 0; i < listQuantityUpdate.length; i++) {
        if (listQuantityUpdate[i].ComboId == comboId && listQuantityUpdate[i].DeliveryDate == date) {
            listQuantityUpdate[i].Quantity = value;
            found = true;
        }
    }

    if (found == false) listQuantityUpdate.push({ ComboId: comboId, DeliveryDate: date, Quantity: value });
}