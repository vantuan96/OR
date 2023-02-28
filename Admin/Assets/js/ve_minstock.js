var listUpdate = [];
var minstockData = [];
var dicSiteArticle = [];

$(function () {
    minstock.init();

    if ($('table.datatable').length > 0) {
        var dt = $('table.datatable').DataTable({
            keys: true,
            //paging: false,
            //scrollX: true,
            //scrollY: 500,
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
    }

    $('.btnCancel').click(function (e) {
        window.location.reload();
    });

    $('.btnSave').click(function (e) {
        SubmitQuantity();
    });

    $('.btnEditMultiple').click(function (e) {
        $('#updateMultipleQuantity').modal('show');
    });

    $('#btnValidate').click(function (e) {
        ShowOverlay(true);
        var input = $('#txtData').val();
        input = $.trim(input).replace(/(\r\n|\r|\n)\"\t/g, "\"\t").replace(/\"/g, "");

        if (input.length == 0) {
            ShowNotify('error', 'Vui lòng nhập min stock');
            HideOverlay();
            return;
        }

        var rows = input.split(/\n/);
        var data = [];
        var validateData = [];
        var isValid = true;
        var keyTable = [];

        for (var i = 0; i < rows.length; i++) {
            var cells = $.trim(rows[i]).split("\t");

            if (cells.length == 0) {
                continue;
            }
            else if (cells.length != 3) {
                ShowNotify('error', 'Dòng ' + (i + 1) + ' không đúng cấu trúc');
                HideOverlay();
                return;
            }
            else {
                var quantity = cells[2].split(',').join('');
                data.push([(i + 1), 0, cells[0], cells[1], quantity, '']);

                if (isNaN(quantity)) {
                    data[i][5] = AddError(data[i][5], 'Min stock phải là số');
                    isValid = false;
                }
                else if (quantity > 999999) {
                    data[i][5] = AddError(data[i][5], 'Min stock phải nhỏ hơn 1,000,000');
                    isValid = false;
                }

                // kiểm tra trùng khóa chính
                var key = cells[0] + ":" + cells[1];
                if (jQuery.inArray(key, keyTable) < 0)
                    keyTable.push(key);
                else {
                    data[i][5] = AddError(data[i][5], 'Trùng mặt hàng');
                    isValid = false;
                }

                validateData.push({
                    SiteCode: cells[0],
                    ProductItemId: cells[1],
                    Quantity: cells[2]
                });
            }
        }

        ValidateData(validateData, data, isValid, function (isValidAll) {
            var table = '<table id="tableMinStock" class="table cell-border" cellspacing="0" width="100%"></table>';
            $('#divTable').html(table);

            $('#tableMinStock').DataTable({
                data: minstockData,
                language: dtvn,
                columns: [
                    { title: "STT" },
                    { title: "Id min stock detail", visible: false },
                    { title: "Mã cửa hàng" },
                    { title: "Mã mặt hàng" },
                    { title: "Min stock" },
                    { title: "Lỗi" }
                ]
            });

            if (isValidAll) {
                $('#divTable .dataTables_length').after('<button onclick="SubmitMinStockData()" class="btn btn-success form-control"><i class="fa fa-save"></i> Lưu lại</button>').hide();
            }
            else {
                ShowNotify('error', 'Dữ liệu nhập không chính xác');
            }

            HideOverlay();
        });
    });
});

var minstock = {
    init: function () {

        //if ($('[data-event=cancel-file]').length > 0) {
        //    $('[data-event=cancel-file]').click(minstock.CancelFile);
        //}

        if ($('[data-event=complete-update-quantity]').length > 0) {
            $('[data-event=complete-update-quantity]').click(minstock.CompleteUpdateQuantity);
        }

        if ($('[data-event=confirm-site]').length > 0) {
            $('[data-event=confirm-site]').click(minstock.ConfirmSite);
        }

        if ($('[data-event=gen-minstock]').length > 0) {
            $('[data-event=gen-minstock]').click(minstock.RunJobMinStock);
        }

        if ($('[data-event=close-minstock]').length > 0) {
            $('[data-event=close-minstock]').click(minstock.RunJobMinStock);
        }

        if ($('[data-event=rollback-minstock]').length > 0) {
            $('[data-event=rollback-minstock]').click(minstock.RollBackMinStockSiteStatus);
        }
       
        if ($('[data-event=copy-minstock').length > 0) {
            $('[data-event=copy-minstock]').click(minstock.CopyMinstock);
        }
       
    },
    RunJobMinStock: function () {
       
        typeId = $(this).data('typeid');
        var note = typeId == 1 ? " Tạo " : "  Đóng "

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn muốn run job đóng" + note + " Minstock?",
            callback: function (confirm) {
                if (confirm) {
                    ShowOverlay();
                    $.ajax({
                        url: _runjobminstockURL,
                        type: "POST",
                        data: {
                            typeId: typeId,
                            "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val()
                        },
                        dataType: 'json',
                        async: false,
                        cache: false,
                        success: function (result) {
                            if (result.ID > 0) {
                                ReloadWithMasterDB();
                            } else {
                                ShowNotify('error', result.Message);
                                HideOverlay();
                            }
                        },
                        error: function (xhr) {
                            ShowNotify('error', 'Lỗi ' + xhr.status);
                            HideOverlay();
                        }
                    });
                }
            }
        });
    },
    CompleteUpdateQuantity: function () {
        var pmsiId = $(this).data('pmsiid');
        
        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Chú ý: Bạn phải chắc chắn đã cập nhật số lượng cho toàn bộ trang yêu cầu MinStock.",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'pmsiId', value: pmsiId });

                    commonUtils.postAjaxWithToken(_completeMinStockQuantityUrl, postData, function (result) {
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
    },
    ConfirmSite: function () {
        var pmsiId = $(this).data('id');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Xác nhận số lượng MinStock của cửa hàng?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'pmsiId', value: pmsiId });

                    commonUtils.postAjaxWithToken(_confirmSiteUrl, postData, function (result) {
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
    },
    CopyMinstock: function () {
        var pmId = $(this).data('pmid');
        var pmsiId = $(this).data('id');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Bạn có muốn sao chép số lượng min stock cũ?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'pmId', value: pmId });
                    postData.push({ name: 'pmsiId', value: pmsiId });

                    commonUtils.postAjaxWithToken(_copyMinstockUrl, postData, function (result) {
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
    },
    RollBackMinStockSiteStatus: function () {
        var pmsiId = $(this).data('id');

        bootbox.confirm({
            title: "THÔNG BÁO",
            message: "Rollback trạng thái chờ cập nhật MinStock cho cửa hàng?",
            callback: function (confirm) {
                if (confirm) {
                    var postData = new Array();
                    postData.push({ name: 'pmsiId', value: pmsiId });

                    commonUtils.postAjaxWithToken(_rollbackMinstockUrl, postData, function (result) {
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
};


function ChangeValue(td) {
    var control = $('input', $(td));
    var beforeValue = $.trim($(control).data('before-val'));
    var afterValue = $.trim($(control).val());

    var PmsdId = $(control).data('pmsdid');
    var ProductId = $(control).data('productid');
    var SiteId = $(control).data('siteid');

    if (beforeValue != afterValue) {
        var exist = false;
        for (var i = 0 ; i < listUpdate.length; i++)
        {
            if (listUpdate[i].PmsdId == PmsdId && listUpdate[i].ProductId == ProductId && listUpdate[i].SiteId == SiteId)
            {
                listUpdate[i].Quantity = afterValue;
                exist = true;
            }
        }

        if (exist == false) {
            listUpdate.push({
                PmsdId: PmsdId,
                ProductId: ProductId,
                SiteId: SiteId,
                Quantity: afterValue
            });
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

function SubmitQuantity() {
    if (listUpdate.length > 0) {
        LazySubmit(listUpdate, function (tempList, callback) {
            $.post(_updateDetailUrl, { pmId: pmId, pmlId: pmlId, pmsiId: pmsiId, detail: tempList }, function (response) {
                callback(response);
            });
        }, ReloadWithMasterDB);
    }
    else {
        ReloadWithMasterDB();
    }
}

function ValidateData(validateData, data, isValid, funcAfterValid) {
    if (validateData.length > 0) {
        LazySubmit(validateData, function (tempList, callback) {
            $.post(_validateMinstockUrl, { pmId: pmId, data: tempList }, function (response) {
                try {
                    var arrDic = $.parseJSON(response);
                    for (var i = 0; i < arrDic.length; i++) {
                        dicSiteArticle[arrDic[i].key] = arrDic[i].value;
                    }

                    callback({ ID: 1, Message: '' });
                }
                catch (e) {
                    callback({ ID: -1, Message: 'Xảy ra lỗi. Vui lòng thử lại' });
                }
            });
        }, function () {
            for (var i = 0; i < data.length; i++) {
                var key = data[i][2] + "-" + data[i][3];
                if (dicSiteArticle.hasOwnProperty(key)) {
                    data[i][1] = dicSiteArticle[key];
                }
                else {
                    data[i][5] = AddError(data[i][5], 'Mã cửa hàng hoặc mã mặt hàng không đúng');
                    isValid = false;
                }
            }

            minstockData = data;
            funcAfterValid(isValid);
        });
    }
    else {
        ShowNotify('error', 'Xảy ra lỗi. Không tìm thấy dữ liệu');
    }
}

function SubmitMinStockData() {
    if (minstockData.length > 0) {
        var list = [];
        for (var i = 0; i < minstockData.length; i++) {
            list.push({
                PmsdId: minstockData[i][1],
                SiteCode: minstockData[i][2],
                ProductItemId: minstockData[i][3],
                Quantity: minstockData[i][4]
            });
        }

        LazySubmit(list, function (tempList, callback) {
            $.post(_updateMultipleMinstockUrl, { pmId: pmId, data: tempList }, function (response) {
                callback(response);
            });
        }, ReloadWithMasterDB);
    }
    else {
        ReloadWithMasterDB();
    }
}

function AddError(str1, str2) {
    if (str1 == undefined || str1 == null || str1.length == 0)
        return str2;

    return str1 + '. ' + str2;
}