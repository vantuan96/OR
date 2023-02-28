var denyActionIndex = 0;

var _tempRole = '<div class="tab-content">'+
                '<div class="panel panel-primary panel-primary-master panel-primary-cms">' +
                    '<div class="panel-heading">' +
                        '<h4 class="panel-title">' +
                            '<a data-toggle="collapse" data-parent="#lstRole" href="#accordion_{RoleID}" class="collapsed btn btn-master">{RoleName}</a>' +
                            '<input type="checkbox" {Disabled} class="pull-right ontoogle-checkbox input_cms" value="{RoleID}" onchange="addRemoveRole(this, {RoleID})" checked/>' +
                            '<input type="hidden" name="UserLocalInfo.ListUserRoles[{RoleIndex}]" value="{RoleID}" />' +
                        '</h4>' +
                    '</div>' +
                    '<div id="accordion_{RoleID}" class="panel-collapse collapse" style="height: 0px;">' +
                        '<div class="panel-body panel-parent">' +
                            '<div class="panel panel-default panel-child">' +
                                '<div class="panel-group" id="lstController_{RoleID}">' +
                                    '{ControllerHtml}' +
                                '</div>' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
                '</div>';

var _tempController = 
                      '<div class="panel  panel-primary panel-primary-child">' +
                            '<div class="panel-heading">' +
                                '<h4 class="panel-title">' + 
                                    '<a data-toggle="collapse" data-parent="#lstController_{RoleID}" href="#accordion_{RoleID}_{ControllerID}" class="collapsed btn btn-master">{ControllerDisplayName}</a>' +
                                '</h4>' +
                            '</div>' +
                            '<div id="accordion_{RoleID}_{ControllerID}" class="panel-collapse collapse" style="height: 0px;">' +
                                '<ul class="todo_list_wrapper">' +
                                    '{ActionHtml}' +
                                '</ul>' +
                            '</div>' +
                        
                        '</div>';

var _tempRoleAction = '<li>' +
                        '<div class="todo_checkbox">' +
                            '<input type="checkbox" value="{ActionID}" {Disabled} onchange="addRemoveDenyAction(this, {ActionID})" checked>' +
                        '<input type="hidden" name="UserLocalInfo.ListUserDenyActions[{denyActionIndex}]" value="0" />' +
                        '</div>' +
                        '<h5 class="todo_title"><a>{ActionDisplayName}</a></h5>' +
                    '</li>';

var _tempNoRoleTitle =  '<div class="row"><div class="panel-body text-left">' +
                            '<ul class="noty_inline_layout_container i-am-new" style="margin: 0px; padding: 0px;">' +
                                '<li class="ad_theme noty_container_type_warning">' +
                                    '<div class="noty_bar noty_type_success">' +
                                        '<div class="noty_message"><span class="noty_text">{Mesg}</span></div>' +
                                    '</div>' +
                                '</li>' +
                            '</ul>' +
                        '</div></div>';

var _tempLockUnlockBtn = '<a class="{style} Tip_mouse_on" onclick="UpdateUserStatus({uid}, {statusid})" title="{title}">{statusname}</a>';

if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}
$(function () {
    if (typeof (_alertData) == "object" && _alertData != null) {
        ShowNotify(_alertData.ID == 0 ? "error" : "success", _alertData.Message, 5000);
    }

    //$.fn.datepicker.dates['vi'] = {
    //    days: ["Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "Chủ nhật"],
    //    daysShort: ["CN", "T2", "T3", "T4", "T5", "T6", "T7", "CN"],
    //    daysMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7", "CN"],
    //    months: ["Tháng 1 năm ", "Tháng 2 năm ", "Tháng 3 năm ", "Tháng 4 năm ", "Tháng 5 năm ", "Tháng 6 năm ", "Tháng 7 năm ", "Tháng 8 năm ", "Tháng 9 năm ", "Tháng 10 năm ", "Tháng 11 năm ", "Tháng 12 năm "],
    //    monthsShort: ["Th.1", "Th.2", "Th.3", "Th.4", "Th.5", "Th.6", "Th.7", "Th.8", "Th.9", "Th.10", "Th.11", "Th.12"],
    //    today: "Hôm nay",
    //    clear: "Xóa"
    //};

    $(".dateminnow").datepicker().on("changeDate", function (e) {
        //$('#hdBirthday').val($("#dBirthday").val().replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$2/$1/$3"));
        //$('#hdBirthday').val($("#dBirthday").val().replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$3-$2-$1"));
        $('#hdBirthday').val(e.format("yyyy-mm-dd"));
    });

    $("#btnStartEdit").click(startUpdateUser);
    $('#btnCancelEdit').click(function () {
        window.location.reload();
    });

    $("form").submit(function () {
        ShowOverlay(true);
    });
    //console.clear();

    $('#UserLocalInfo_CityID').select2({
        allowClear: true,
        placeholder: "Chọn thành phố..."
    });
    $('#ddlUserDept').select2({
        allowClear: true,
        placeholder: "Chọn phòng ban..."
    });
    $('#ddlUserTitle').select2({
        allowClear: true,
        placeholder: "Chọn chức danh..."
    });
    $('#ddlUserManager').select2({
        allowClear: true,
        placeholder: "Chọn người quản lý..."
    });

    $('#UserInfo_Gender').select2({
        allowClear: true,
        placeholder: "Chọn giới tính..."
    });

    $("#UserInfo_Email").keydown(function () {
        var ret = true;
        if ($("#Password").is(":disabled")) {
            $('#UserInfo_MobileNumber').val("");
            $('#UserInfo_Username').val("");
            $('#UserInfo_FullName').val("");
            $('#UserInfo_Gender').val(0);

            $('.dateminnow').find('.input-group-addon').removeClass('disabled');

            $('#UserInfo_FullName, #UserInfo_MobileNumber,#UserInfo_Username,#UserInfo_Gender,.dateminnow input, #Password, #confirmpassword').removeAttr("disabled");
            $('#Password, #confirmpassword').val("");
            $("#change-email-msg").removeClass("filled").find("li").remove();
        }       
    }).focusout(function () {
        $(this).val($(this).val().replace(/\s/gi,'')).trigger("keyup");
    });

    $("#frmRequest button[type=submit]").on("click", function () {
        if ($("#frmRequest").parsley().isValid()) {
            $('.dateminnow').find('.input-group-addon').removeClass('disabled');
            $('#UserInfo_FullName, #UserInfo_Email, #UserInfo_MobileNumber,#UserInfo_Username,#UserInfo_Gender,.dateminnow input, #Password, #confirmpassword').removeAttr("disabled");
        }        
    });

    //$("#dBirthday").on("click", function () {
    //    $(this).next().next().trigger("click");
    //});
    $("#UserInfo_Email").focus();

    //Select 2 focus tabindex
    $(".select2-container .select2-focusser").focus(function () {
        $(this).parent().attr("style", "border-color: #1c81d8;box-shadow: 0 0 3px #54a0e2;");
    }).focusout(function () {
        $(this).parent().attr("style", "");
    });
});

function initFirstSelectData() {
    $('#ddlUserDept').trigger("change");
}

function startUpdateUser() {
    $('.dateminnow').find('.input-group-addon').removeClass("disabled");
    $("#btnStartEdit").hide();
    $('#btnSaveEdit').show();
    $('#btnCancelEdit').show();
    $('#frmRequest .form-control').not("[readonly]").removeAttr("disabled");
    $('#lstRole [type=checkbox]').prop("disabled", false);
    $("#dBirthday").prop("disabled", false);
    $("#UserInfo_FullName").focus();
}

function cancelUpdateUser() {
    $('#frmRequest select, #frmRequest input').not('[type="hidden"]').prop("disabled", true);    
    window.location.reload(true);
}

function onChangeUserDepartment(tag) {
    var selectDept = $(tag).val();
    $('#lstRole').html(_tempNoRoleTitle.supplant({ Mesg: "Chọn chức danh để phân quyền" }));
    var html = "";
    if (selectDept != "0") {
        var listTitle = Enumerable.From(_deptData).Where(function (x) { return x.DepartmentID == selectDept }).Select(function (x) { return x.ListTitles }).First();
        
        html = '<select class="form-control" data-parsley-min="1" tabindex="1" id="ddlUserTitle" data-parsley-min-message="Vui lòng chọn chức danh" onchange="onChangeUserTitle(this)" name="UserLocalInfo.TitleID">' +
                    '<option value="0" selected>--- Chọn chức danh --- </option>';

        if (listTitle != null) {
            listTitle.forEach(function (item) {
                html += '<option value="' + item.TitleID + '">' + item.TitleName + '</option>';
            });
        }
        html += "</select>";

        $('#dTitleCont').html(html);

        var listManager = Enumerable.From(_deptData).Where(function (x) { return x.DepartmentID == selectDept }).Select(function (x) { return x.ListManagers }).First();
        if (listManager != null) {
            html = "<select  tabindex='1'  class='form-control' id='ddlUserManager' name='UserLocalInfo.ManagerID'>" +
                        "<option value='0' selected>--- Chọn người quản lý ---</option>";

            listManager.forEach(function (item) {
                if (item.UserID != 0 && item.UserID != parseInt($('#UserInfo_Id').val())) {
                    html += "<option value='" + item.UserID + "'>" + item.UserFullName + "</option>";
                }
            });
            $('#dManagerCont').html(html);
        }
        else {
            $('#dManagerCont').html("<select class='form-control' tabindex='1'  id='ddlUserManager'><option value='0'>--- Chọn người quản lý ---</option></select>");
        }
        $('#ddlUserTitle').select2({
            allowClear: true,
            placeholder: "--- Chọn chức danh ---"
        });
        $('#ddlUserManager').select2({
            allowClear: true,
            placeholder: "--- Chọn người quản lý ---"
        });

        //Select 2 focus tabindex
        $(".select2-container .select2-focusser").focus(function () {
            $(this).parent().attr("style", "border-color: #1c81d8;box-shadow: 0 0 3px #54a0e2;");
        }).focusout(function () {
            $(this).parent().attr("style", "");
        });
    }
    else {
        $('#dTitleCont').html("<select disabled tabindex='1' class='form-control'></select>");

        $('#dManagerCont').html("<select disabled tabindex='1'  class='form-control'></select>");
    }
}

function onChangeUserTitle(tag) {
    var selectTitle = $(tag).val();
    var selectDept = $('#ddlUserDept').val();
    var html = "";
    if (selectDept != 0 && selectTitle != 0) {
        var titleOjb = Enumerable.From(_deptData).Where(function (x) { return x.DepartmentID == selectDept }).Select(function (x) { return x.ListTitles }).FirstOrDefault();
        var listRole = Enumerable.From(titleOjb).Where(function (x) { return x.TitleID == selectTitle }).Select(function (x) { return x.ListAdminRoles }).FirstOrDefault();
        if (listRole != undefined) {
            var roleIndex = 0;
            $.each(listRole, function (i) {
                if (this.ListAdminActions != null) {
                    var groupedRecordsCollection = new Array();
                    var listController = Enumerable.From(this.ListAdminActions).GroupBy("{ ControllerID : $.ControllerID }", null,
                                                        function (key, g) {
                                                            var result = {
                                                                ControllerID: key
                                                            };

                                                            var groupResults = [];
                                                            g.ForEach(function (item) {
                                                                groupResults.push(item);
                                                            });
                                                            groupedRecordsCollection.push(groupResults);
                                                        },
                                                        "$.ControllerID").ToArray();
                    var dataObj = $.extend(this, {
                        RoleIndex: roleIndex,
                        ControllerHtml: generateControllerHtml(groupedRecordsCollection, this.RoleID),
                        Disabled: _userRoleEnable == false ? "disabled" : ""
                    });

                    html += _tempRole.supplant(dataObj);
                    roleIndex++;
                }
            });
            html = html == "" ? _tempNoRoleTitle.supplant({ Mesg: "Chức danh chưa có quyền nào" }) : html;
        }
        else {
            html = html == "" ? _tempNoRoleTitle.supplant({ Mesg: "Chức danh chưa có quyền nào" }) : html;
        }
    }
    else {
        html = _tempNoRoleTitle.supplant({ Mesg: "Chọn chức danh để phân quyền" });
    }   

    $('#lstRole').html(html);
    denyActionIndex = 0;

    if (_canDenyAction == 0) {
        $("#lstRole .todo_checkbox input[type=checkbox]").hide();
    }
}

function generateControllerHtml(groupedListController, roleID) {
    var retHtml = "";
    if (groupedListController.length > 0) {
        $.each(groupedListController, function () {
            if (this.length > 0) {
                var controllerObj = {
                    RoleID: roleID,
                    ControllerID: this[0].ControllerID,
                    ControllerDisplayName: this[0].ControllerDisplayName,
                    ActionHtml: generateActionHtml(this)
                };
                retHtml += _tempController.supplant(controllerObj);
            }
        });
    }
    return retHtml;
}

function generateActionHtml(listAction) {
    var retHtml = "";
    $.each(listAction, function () {
        var dataObj = $.extend(this, {
            denyActionIndex: denyActionIndex,
            Disabled: (_userRoleEnable == false) ? "disabled" : ""
        });
        retHtml += _tempRoleAction.supplant(dataObj);

        denyActionIndex++;
    });

    return retHtml;
}

function addRemoveRole(tag, id) {
    var containerId = $(tag).siblings("a").attr("href");
    if ($(tag).prop("checked") == true) {
        $(containerId).find("input[type=checkbox]").prop("checked", true);
        $(tag).siblings("input[type=hidden]").val(id);
    }
    else {
        $(containerId).find("input[type=checkbox]").prop("checked", false);
        $(tag).siblings("input[type=hidden]").val(0);
    }
    $(containerId).find("input[type=hidden]").val(0);
}

function addRemoveDenyAction(tag, id) {
    var countChecked = $(tag).closest("div.panel-group").find("[type=checkbox]:checked").length;
    var titleID = $(tag).closest('[id^=accordion]').attr("id");
    titleID = titleID.substring(0, titleID.lastIndexOf('_'));
    var titleTag = $('a[href=#' + titleID + ']');
    if ($(tag).prop("checked") == true) { //remove
        $(tag).siblings("[type=hidden]").val(0);

        if (countChecked == 1) {
            var roleVal = titleTag.siblings("[type=checkbox]").val();
            titleTag.siblings("[type=checkbox]").prop("checked", true);
            titleTag.siblings("[type=hidden]").val(roleVal);

            $.each($(tag).closest("div.panel-group").find("[type=checkbox]:not(:checked)"), function () {
                $(this).siblings("[type=hidden]").val($(this).val());
            });
        }
    }

    else { //add
        $(tag).siblings("[type=hidden]").val(id);

        if (countChecked == 0) {
            titleTag.siblings("[type=checkbox]").prop("checked", false);
            titleTag.siblings("[type=hidden]").val(0);

            $(tag).closest('div.panel-group').find("[type=hidden]").val(0);
        }
    }
}

function checkEmail(isSubmit) {   
    var email = $.trim($('#UserInfo_Email').val());
    if (checkValidEmail(email)) {
        ShowOverlay(true);
        $.ajax({
            url: _checkEmailUrl,
            type: "POST",
            data: { "Email": email, "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val() }
        }).done(function (res) {
            if (res.Status == 2)
            {
                bootbox.alert(res.ReturnMsg, function () { $("#UserInfo_Email").focus(); });
                $("#UserInfo_Email").attr("style", "border-color:#c0392b !important");
            }
            else if (res.Status == 1) {
                bootbox.confirm("Tài khoản đã tồn tại trên hệ thống, bạn có muốn tự động điền thông tin đã có vào form bên dưới không?", function (r) {
                    if (r) {
                        $('#UserInfo_MobileNumber').val(res.User.MobileNumber).change();
                        $('#UserInfo_Username').val(res.User.Username).change();
                        $('#UserInfo_FullName').val(res.User.FullName).change();
                        $('#UserInfo_Gender').val(res.User.Gender).change();
                        $('.dateminnow').datepicker('setDate', new Date(convertDateFromJson(res.User.Birthday))).change();
                        $('#hdBirthday').val(convertDateFromJson(res.User.Birthday)).change();

                        $('.dateminnow').find('.input-group-addon').addClass('disabled').change();

                        //$('#UserInfo_FullName, #UserInfo_MobileNumber,#UserInfo_Username,#UserInfo_Gender,.dateminnow input, #Password, #confirmpassword').prop("disabled", true);
                        $('#UserInfo_Username,#Password, #confirmpassword').prop("disabled", true);

                        $('#Password, #confirmpassword').val("aBc1234567890#!").change();
                        $("#change-email-msg").addClass("filled").html("").append("<li>Bạn phải nhập lại các trường bên dưới nếu thay đổi email.</li>");
                        $("#frmRequest").parsley().validate();
                        $('#UserInfo_Email').focus();
                    }
                });                
            }
            else {
                $('#UserInfo_MobileNumber').val("");
                $('#UserInfo_Username').val("");
                $('#UserInfo_FullName').val("");
                $('#UserInfo_Gender').val(0);
                $('#dBirthday').val('');
                $('#hdBirthday').val();

                $('#UserInfo_FullName, #UserInfo_MobileNumber,#UserInfo_Username,#UserInfo_Gender,.dateminnow input, #Password, #confirmpassword').prop("disabled", false);
                $('.dateminnow').find('.input-group-addon').removeClass('disabled');
                $('#Password, #confirmpassword').val("");
                $("#UserInfo_Email").attr("style", "border-color:#00B872  !important");
                if (isSubmit == undefined) {
                    bootbox.alert("Bạn có thể sử dụng địa chỉ email này.", function () { $("#UserInfo_Email").focus(); });
                }
            }
            $('#UserInfo_Email').focus();
            
        });
        $('#UserInfo_Email').focus();
    }
}

function checkValidEmail(email) {
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return filter.test(email);
}

function convertDateFromJson(inputDate) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = parseJsonDate(inputDate);
    return [pad(d.getMonth() + 1), pad(d.getDate()), d.getFullYear()].join('/');
}

function UpdateUserStatus(uid, status) {
    bootbox.confirm("Bạn muốn " + (status == 0 ? "khóa" : "mở khóa") + " người dùng này ?",
        function (result) {
            if (result) {
                ShowOverlay(true);
                $.post(_urlUpdateUserStatus, { uid: uid, statusid: status }, function (ret) {
                    if (ret.ID == 1) {
                        window.location.reload();
                    }
                    else {
                        hideOverlay(0);
                        ShowNotify("error", ret.Message, 5000);
                    }
                });
            }
        }
    );
}