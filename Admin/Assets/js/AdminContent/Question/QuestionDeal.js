
window.onload = function() {
    pageNumber = 1;
    pageSize = 200;
    question.disableInputFormEnterKey();
    if (totalRows > pageSize)
        $("#pagingQuestion").html(base.paging(pagNumber, pageSize, totalRows, "danh-sach-cau-hoi-deal"));
    $("#txt_search").keypress(function(e) {
        if (e.which == 13) {
            question.changeCat();
        }
    });

    $("#x_txt_question,#x_txt_new_group,#x_group_description,#x_txt_new_deal_field_type,#x_txt_note").keyup(function(e) {
        $(this).val(base.ReplaceSpecChar($(this).val()));
    });
    question.tisaToolTips();
    question.initCheckbox();
    $(".chk_visible").bootstrapSwitch();
};
var question = {
    questionTypeChange: function() {
        var val = $("#x_ddl_question_type").val();
        $("#cont-field").html("");
        $("#x_fieldSuggest").val("");
        if (val === "1" || val === "3" || val === "5") {
            question.createNewField();
        }
    },
    //tao suggest khi muon them moi
    createNewField: function() {
        var childEl = $("#cont-field").children().length;
        var str = "<div>" +
            "<input class='x_question_type' data-dsfsid='0' style='width:53%;margin-top:10px;text-indent:15px' onchange='question.suggestChange();'>" +
            "<a class='btn_addsuggest btn-success btn-sm' style='font-size:13px;cursor:pointer' database-suggest='0' onclick='question.createNewField();'>Thêm </a>";
        if (childEl > 0) {
            str += "<a class='btn-danger btn-sm' style='font-size: 13px;cursor:pointer;' onclick='question.removeField(this);'>Xóa</a>";
        }
        str += "<ul class='parsley-errors-list filled' style='display:none;'><li class='parsley-required'>Nhập gợi ý</li></ul>";
        str += "</div>";
        $("#cont-field").append(str);
        question.disableInputFormEnterKey();
    },
    //xoa suggest 
    removeField: function(el) {
        //neu truong hop ton tai trong database roi thi gui ajax delete di ,nguoc lai thi khong can gui ajax
        if ($(el).attr("database-suggest") == "1") {
            bootbox.confirm("Bạn muốn xóa câu gợi ý này ?", function(result) {
                if (result) {
                    //ma suggest 
                    var dsfsid = $(el).attr("data-dsfsid");
                    var url = "/xoa-goi-y-deal/?dsfsid=" + dsfsid;
                    var success = function(response) {
                        el.parentNode.remove();
                        bootbox.alert(response, function() {
                        });
                    };
                    $.ajax({ url: url, type: "POST", success: success, async: false });
                }
            });

        } else {
            el.parentNode.remove();
        }
        question.suggestChange();

    },
    //change category thi lay danh sach cau hoi theo category
    changeCat: function() {
        iosOverlay({
            text: "Đang tải...",
            icon: "fa fa-spinner fa-spin",
            shadow: true,
            id: 'LoadingOverlay'
        });

        var catID = $("#cat_question").val();
        if ($("#x_txt_case_change_cate").val() == 1) {
            catID = $("#from_cat_question").val();
        }
        window.location.href = "/tao-cau-hoi-deal?from_cat_question=" + catID;

        //base.loading();
        //var catID = $("#cat_question").val();
        //if ($("#x_txt_case_change_cate").val() == 1) {
        //    catID = $("#from_cat_question").val();
        //}

        //var txtSearch = $("#txt_search").val();
        //var url = "/danh-sach-cau-hoi-deal/?isSet=0&DSFID=0&catID=" + catID + "&pageNumber=" + pageNumber + "&txtSearch=" + txtSearch;
        //var success = function (response) {
        //    question.createListQuestion(response, pageNumber, pageSize, txtSearch);
        //}
        //$.ajax({url:url,type:"POST",success:success,async:false});
    },
    changeCatCopy: function() {
        base.loading();
        var catID = $("#cat_question").val();
        if ($("#x_txt_case_change_cate").val() == 1) {
            catID = $("#from_cat_question").val();
        }

        var txtSearch = $("#txt_search").val();
        var url = "/danh-sach-cau-hoi-deal/?isSet=0&DSFID=0&catID=" + catID + "&pageNumber=" + pageNumber + "&txtSearch=" + txtSearch;
        var success = function(response) {
            question.createListQuestion(response, pageNumber, pageSize, txtSearch);
        };
        $.ajax({ url: url, type: "POST", success: success, async: false });
    },
    //inner danh sach cau hoi
    createListQuestion: function(response, pageNumber, pageSize, txtSearch) {
        if (response.length > 10)
            window.Location.href = window.Location.href;
        if (response !== 0) {
            var str = "<thead>" +
                "tr>" +
                "<th style='text-align:center;width:30px;'><input class='check_all' type='checkbox'/></th>" +
                "<th style='text-align:left;'></th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            for (var i = 0; i < response.length; i++) {
                var FieldSuggest = [];
                if (response[i].SuggestValue != null && response[i].SuggestValue != "") {
                    FieldSuggest = response[i].SuggestValue.split("*$#");
                }

                if (i == 0 || (i < response.length - 1 && (i > 0 && response[i].DSGID != response[i - 1].DSGID))) {
                    str += "<tr><td colspan='2' style='border:none;padding: 0;'>" +
                        "<h4 class='panel-heading' style='margin-top: 15px; margin-bottom: 5px; font-style: italic; font-weight: bold; padding: 5px; background-color: #dddddd;'>" +
                        response[i].GroupName +
                        "</h4>" +
                        "</td></tr>";
                }
                str += "<tr style='border-bottom: 1px dashed #aaa;overflow:hidden;'>" +
                    "<td style='border: none;'><input class='check_row' type='checkbox' value='" + response[i].DSFID + "'/></td>" +
                    "<td style='text-align:left;border:none;'>" +
                    "<div class='col-lg-12' >" +
                    "<div class=' col-lg-6 col-md-6'>" +
                    "<h5>" +
                    response[i].No + " . " + response[i].FieldName +
                    "</h5>";
                switch (response[i].DSFTID) {
                case 1:
                    str += "<div class=' col-sm-12 col-xs-8'>";
                    for (var m = 0; m < FieldSuggest.length; m++) {
                        str += "<label style='font-weight:normal;'>";
                        str += "<input type='radio' name=" + response[i].DSFID + " /><span>" + FieldSuggest[m] + "</span></label>";
                    }
                    str += "</div>";
                    break;
                case 2:
                    str += "<div class=' col-sm-12 col-xs-8'>" +
                        "<input type='checkbox'  class='chk_visible' checked data-size='small' data-on-color='success' />" +
                        "</div>";
                    break;
                case 3:
                    str += "<div class=' col-sm-12 col-xs-8'>" +
                        "<select class='form-control'>";
                    for (var m = 0; m < FieldSuggest.length; m++) {
                        str += "<option>" + FieldSuggest[m] + "</option>";
                    }
                    str += "</select>" +
                        "</div>";
                    break;
                case 4:
                    str += "<div class=' col-sm-12 col-xs-8'>" +
                        "<input style='font-weight: normal;margin-top:5px;' type='text' class='form-control'>" +
                        "</div>";

                    break;
                case 5:
                    str += "<div class=' col-sm-12 col-xs-8'>";
                    for (var m = 0; m < FieldSuggest.length; m++) {
                        str += "<label class='checkbox-inline' style='display: block; margin-left: 0; margin-top: 5px'>" +
                            "<input type='checkbox'  value='1' class='checkbox_list' />" +
                            FieldSuggest[m] +
                            "</label>";
                    }
                    str += "</div>";
                    break;
                case 6:
                    str += "<div class='form-group col-sm-9 col-xs-8'>" +
                        "<input type='checkbox' class='chk_visible' checked  data-size='small' data-on-color='success'>" +
                        "<input style='font-weight: normal; margin-top: 10px;' type='text' class='form-control'>" +
                        "</div>";
                    break;
                }

                str += "</div>";

                var checkvoucher = response[i].IsVoucher == 1 ? "checked" : "";
                var checkWeb = response[i].IsWeb == 1 ? "checked" : "";
                var isvoucher = response[i].IsVoucher == 1 ? "0" : "1";
                var isweb = response[i].IsWeb == 1 ? "0" : "1";
                var style = "display:none;";
                if ($("#x_txt_case_change_cate").val() == 0) {
                    style = "";
                }
                str += "<div class='col-lg-3 col-md-3' style='padding-left:0;padding-right:0'>" +
                    "<label style='font-weight: normal; float: left; margin-top: 5px;margin-left: 5px;margin-right: 5px; color: #428bca;'>" +
                    "<input type='checkbox' data-isset='1' data-isvoucher='" + isvoucher + "' data-isweb='" + isweb + "' data-dsfid='" + response[i].DSFID + "'  onclick='question.updateiswebisvoucher(this);'  " + checkWeb + " > Web-GCN " +
                    "</label>" +
                    "<label style='font-weight: normal; float: left; margin-top: 5px; margin-left: 5px;margin-right: 5px; color: #E31837'>" +
                    "<input type='hidden'  value='true'>" +
                    "<input data-isset='0' data-isvoucher='" + isvoucher + "' data-isweb='" + isweb + "' data-dsfid='" + response[i].DSFID + "'  onclick='question.updateiswebisvoucher(this);'  type='checkbox'" + checkvoucher + " > VC" +
                    "</label>" +
                    "<div style='clear: both'></div>" +
                    "</div>" +
                    "<div class='col-lg-3 col-md-3'>" +
                    "<button class='btn btn-xs btn-success  btn_view' data-catid='" + response[i].CateID + "' data-dsfid='" + response[i].DSFID + "' onclick='question.viewQuesitionDetail(this);' style='margin-right:3px;padding:3px 12px;" + style + "'><i class='fa fa-file-text-o'></i></button>" +
                    "<button class='btn btn-xs btn-danger  btn_delete' data-dsfid='" + response[i].DSFID + "' onclick='question.deleteQuestion(this);' style='padding:3px 12px;" + style + "'><i  class='fa fa-trash-o'></i></button>" +
                    "</div>" +
                    "</div>" +
                    "</td>" +
                    "</tr>";
            }
            ;
            str += "</tbody>";
            $("#table_question").html(str);
            $("#countQuestion").text("(có " + response.length + " điều kiện)");
            $(".question_no_data").hide();
            var str = base.paging(pageNumber, pageSize, response[0].TotalRows, "danh-sach-cau-hoi-deal");
            $("#pagingQuestion").html(str);

            question.initCheckbox();
            $(".chk_visible").bootstrapSwitch();
        } else {
            $(".question_no_data").show();
            $("#table_question").html("");
            $("#pagingQuestion").html("");
        }

    },
    //kiem tra du lieu truoc khi gui len server , update hoac them moi mot cau hoi
    validFormQuestion: function() {
        //$("#UL_QuestionResult").hide();
        var x_ddl_cate = $("#x_ddl_cate").val();
        var x_ddl_group = $("#x_ddl_group").val();
        var x_txt_question = $("#x_txt_question").val();
        var x_ddl_question_type = $("#x_ddl_question_type").val();
        var x_txt_note = $("#x_txt_note").val();
        var x_continue = true;

        if (x_ddl_cate === "0") {
            $("#e_x_ddl_cate").show();
            x_continue = false;
        } else {
            $("#e_x_ddl_cate").hide();
        }
        if (x_ddl_group === "0") {
            $("#e_x_ddl_group").show();
            x_continue = false;
        } else {
            $("#e_x_ddl_group").hide();
        }

        if (x_txt_question === "") {
            $("#e_x_txt_question").show();
            x_continue = false;
        } else {
            $("#e_x_txt_question").hide();
        }

        if (x_ddl_question_type === "0") {
            $("#e_x_ddl_question_type").show();
            x_continue = false;
        } else {
            $("#e_x_ddl_question_type").hide();
            var x_fieldSuggest = "";
            //kiem tra suggest loai cau hoi 1,3,5
            $("#cont-field .x_question_type").each(function(index) {
                if ($(this).val().trim() == "") {
                    $(this).parent().find(".filled").show();
                    x_continue = false;
                } else {
                    $(this).parent().find(".filled").hide();
                }
                x_fieldSuggest += $(this).val() + "@$#";
            });

            $("#x_fieldSuggest").val(x_fieldSuggest);
        }

        return x_continue;
    },
    //tao cau hoi moi thanh cong
    createQuestionComplete: function(response) {
        bootbox.alert(response.Message, function() {

            question.changeCat();
            //var catID = $("#x_ddl_cate").val();
            //question.clearForm();
            //$("#cat_question").val(catID);
            //var txtSearch = $("#txt_search").val();
            //var url = "/danh-sach-cau-hoi-deal/?isSet=0&DSFID=0&catID=" + catID + "&pageNumber=" + pageNumber + "&txtSearch=" + txtSearch;
            //var success = function (response) {
            //    question.createListQuestion(response, pageNumber, pageSize, txtSearch);
            //}
            //$.ajax({ url: url, type: "POST", success: success, async: false });
        });
    },
    //tao nhung suggest khi lay tu database xuong
    suggestChange: function() {
        var x_fieldSuggest = "";
        //kiem tra suggest loai cau hoi 1,3,5
        $("#cont-field .x_question_type").each(function(index) {
            x_fieldSuggest += $(this).attr("data-dsfsid") + ":" + $(this).val() + "@$#";
        });
        $("#x_fieldSuggest").val(x_fieldSuggest);
    },
    //xem chi tiet cua mot cau hoi
    viewQuesitionDetail: function(el) {
        base.loading();
        //$("#UL_QuestionResult").hide();
        var DSFID = $(el).attr("data-dsfid"),
            catID = $(el).attr("data-catid");

        $("#table_question tr").removeClass("tr_active");
        $(el).parent().parent().parent().parent().addClass("tr_active");
        $(".id_qs_mer").text("Chi tiết điều kiện");
        var url = "/danh-sach-cau-hoi-deal/?isSet=1&DSFID=" + DSFID + "&catID=" + catID;
        var listFieldSuggest = "";
        var success = function(response) {
            base.scrollTop();
            if (response != "0") {
                if (response.length > 0) {
                    $("#x_ddl_cate").val(response[0].CateID);
                    $("#x_ddl_group").val(response[0].DSGID);
                    $("#x_txt_question").val(response[0].FieldName);
                    $("#x_ddl_question_type").val(response[0].DSFTID);
                    $("#x_txt_note").val(response[0].FieldTip);
                    $("#x_deal_field_type").val(response[0].TypeFieldID);
                    if (response[0].IsWeb == 0) {
                        $("#x_is_web").prop("checked", false);
                    } else {
                        $("#x_is_web").prop("checked", true);
                    }
                    if (response[0].IsVoucher == 0) {
                        $("#x_is_voucher").prop("checked", false);
                    } else {
                        $("#x_is_voucher").prop("checked", true);
                    }
                    if (response[0].IsCertificate == 0) {
                        $("#x_is_certificate").prop("checked", false);
                    } else {
                        $("#x_is_certificate").prop("checked", true);
                    }

                    if (response[0].isRequired == 1) {
                        $(".chk_true").prop("checked", true);
                        $(".chk_false").prop("checked", false);
                    } else {
                        $(".chk_false").prop("checked", true);
                        $(".chk_true").prop("checked", false);
                    }
                }
                $("#cont-field").html("");
                $("#x_isset").val(1);
                $("#x_dsfid").val(DSFID);

                //suggest field 
                for (var i = 0; i < response.length; i++) {
                    if (response[i].SuggestValue != "" & response[i].SuggestValue != null) {
                        listFieldSuggest += response[i].DSFSID + ":" + response[i].SuggestValue + "@$#";
                        question.FieldSuggestUpdate(response[i]);
                    }
                }
                $("#x_fieldSuggest").val(listFieldSuggest);
            }
        };
        $.ajax({ url: url, type: "POST", success: success, async: false });
    },
    //tao text box suggest da co
    FieldSuggestUpdate: function(data) {
        var childEl = $("#cont-field").children().length;
        var str = "<div>" +
            "<input data-dsfsid=" + data.DSFSID + " class='x_question_type' value='" + data.SuggestValue + "' style='width:53%;margin-top:10px;text-indent:15px' onchange='question.suggestChange();'>" +
            "<a class='btn_addsuggest btn-success btn-sm' style='font-size:13px;cursor:pointer' onclick='question.createNewField();'>Thêm </a>";
        if (childEl > 0) {
            str += "<a class='btn-danger btn-sm' style='font-size: 13px;cursor:pointer;' database-suggest='1' data-dsfsid='" + data.DSFSID + "' onclick='question.removeField(this);'>Xóa</a>";
        }
        str += "<ul class='parsley-errors-list filled' style='display:none;'><li class='parsley-required'>Nhập gợi ý</li></ul>";
        str += "</div>";
        $("#cont-field").append(str);
    },
    //khong cho textbox submit khi su dung enterkey
    disableInputFormEnterKey: function() {

        $("form").find("input").keypress(function(e) {
            if (e.which == 13)
                return false;
        });
    },
    //lam moi lai form
    clearForm: function() {

        $("#x_isset,#x_ddl_group,#x_ddl_question_type").val(0);
        $("#x_txt_question,#x_txt_note").val("");
        $(".chk_true").prop("checked", true);
        $("#cont-field").html("");
        $("#table_question tr").removeClass("tr_active");
        $(".id_qs_mer").text("Tạo điều kiện mới");
    },
    deleteQuestion: function(el) {
        bootbox.confirm("Bạn muốn xóa điều kiện này ?", function(result) {
            if (result) {
                var DSFID = $(el).attr("data-dsfid");
                var url = "/xoa-cau-hoi-deal/?dsfid=" + DSFID;
                var success = function(response) {

                    alert(response);
                    question.changeCat();
                    //bootbox.alert(response , function (result1) {
                    //    question.changeCat();
                    //});
                };
                $.ajax({ url: url, type: "POST", success: success, async: false });
            }
        });
    },
    pagingQuestion: function(pageNumber, pageSize, href) {
        var catID = $("#cat_question").val();
        var txtSearch = $("#txt_search").val();
        var url = "/danh-sach-cau-hoi-deal/?isSet=0&DSFID=0&catID=" + catID + "&pageNumber=" + pageNumber + "&txtSearch=" + txtSearch;
        var success = function(response) {
            question.createListQuestion(response, pageNumber, pageSize, txtSearch);
        };
        $.ajax({ url: url, type: "POST", success: success, async: false });

    },
    changeGroup: function() {
        this.x_isset = $("#x_txt_isset").val();
        this.x_dsgid = $("#x_txt_group_id").val();
        this.x_group_name = $("#x_txt_new_group").val();
        this.x_groupdesc = $("#x_group_description").val();
        this.regex = /\w+/gi;
        this.visible = true;
        if (!this.regex.test(this.x_group_name) && this.x_isset < 2) {
            $("#e_x_txt_new_group").show();
            this.visible = false;
        } else {
            $("#e_x_txt_new_group").hide();
        }
        var url = "/change-nhom-cau-hoi-deal";
        var data = {
            x_isset: this.x_isset,
            x_dsgid: this.x_dsgid,
            x_groupname: this.x_group_name,
            x_groupdesc: this.x_groupdesc
        };
        var success = function(response) {
            bootbox.alert(response.Message, function() {
                if (response.ID > 0) {
                    question.getListDealStructureGroup(0);
                    base.hidePopup();
                }
            });

        };
        if (this.visible) {
            $.ajax({ url: url, data: data, success: success, type: "POST", async: false });
        }
    },
    tisaToolTips: function() {
        $.fn.powerTip.defaults.smartPlacement = true;
        $(".pTip_top").powerTip({ placement: 'n' });
        $(".add-dealgroup").data('powertipjq', $(['<p><b>' + $("#tooltips_add_deal_structure_group").val() + '</b></p>'].join('\n')));
        $(".del-dealgroup").data('powertipjq', $(['<p><b>' + $("#tooltips_del_deal_structure_group").val() + '</b></p>'].join('\n')));
        $(".up-dealgroup").data('powertipjq', $(['<p><b>' + $("#tooltips_up_deal_structure_group").val() + '</b></p>'].join('\n')));

        $(".saveGroup").data('powertipjq', $(['<p><b>' + $("#tooltips_save_deal_structure_group").val() + '</b></p>'].join('\n')));
        $(".cancelPopup").data('powertipjq', $(['<p><b>' + $("#tooltips_exit_popup").val() + '</b></p>'].join('\n')));
        $(".up-dealfieldtype").data('powertipjq', $(['<p><b>Cập nhật loại dữ liệu</b></p>'].join('\n')));
        $(".add-dealfieldtype").data('powertipjq', $(['<p><b>Thêm loại dữ liệu</b></p>'].join('\n')));
        $(".del-dealfieldtype").data('powertipjq', $(['<p><b>Xoá loại dữ liệu</b></p>'].join('\n')));


    },
    showAddNewGroup: function(el) {
        $("#x_txt_new_group").val("");
        $("#x_group_description").val("");
        $("#txt_group_id").val(0);
        $("#x_txt_isset").val(0);
        base.showPopup(el);
    },
    showUpdateGroup: function(el) {
        var dsgid = $("#x_ddl_group").val();
        if (dsgid == 0)
            bootbox.alert("Chưa chọn nhóm điều kiện", function() {
            });

        else {
            $("#x_txt_isset").val(1);
            question.getListDealStructureGroup(dsgid);
            base.showPopup(el);
        }
        return false;
    },
    getListDealStructureGroup: function(DSGID) {
        this.url = "lay-danh-sach-nhom-cau-hoi-deal?DSGID=" + DSGID;
        this.success = function(response) {
            //get cho dropdownlist
            if (DSGID == 0) {
                this.op = "<option value='0'> ---Chọn group--- </option>";
                for (var i = 0; i < response.length; i++) {
                    this.op += "<option value ='" + response[i].DSGID + "'>" + response[i].GroupName + "</option>";
                }
                $("#x_ddl_group").html(this.op);
            }
                //get cho update 
            else {
                if (response.length > 0) {
                    $("#x_txt_new_group").val(response[0].GroupName);
                    $("#x_group_description").val(response[0].GroupDesc);
                    $("#x_txt_group_id").val(response[0].DSGID);
                }
            }
        };
        $.ajax({ url: this.url, success: this.success, type: "POST", async: false });
    },
    deleteDealStructureGroup: function() {
        var GroupName = $("#x_ddl_group option:selected").text();
        bootbox.confirm("Bạn muốn xóa nhóm điều kiện tên là : " + GroupName, function(result) {
            if (result) {
                $("#x_txt_isset").val(2);
                this.dsgid = $("#x_ddl_group").val();
                $("#x_txt_group_id").val(this.dsgid);
                if (this.dsgid == 0)
                    bootbox.alert("Chưa chọn nhóm điều kiện.", function() {
                    });
                else
                    question.changeGroup();
            }
        });

    },
    showFormDealFieldType: function(x_isset) {

        $("#e_x_txt_new_dealfieldtype").hide();
        if (x_isset == 0) {
            $("#x_txt_new_deal_field_type").val("");
            $("#x_typefieldid").val(0);
            $("#x_isset_deal_field_type").val(x_isset);
            $(".popup-editable1").show();
        } else if (x_isset == 1) {
            if ($("#x_deal_field_type").val() != 0) {
                question.getListDealFieldType();
            } else {
                bootbox.alert("Bạn chưa chọn loại dữ liệu.", function() {
                });
            }

        } else if (x_isset == 2) {
            $(".popup-editable1").hide();
            $("#x_isset_deal_field_type").val(x_isset);
            $("#x_typefieldid").val($("#x_deal_field_type").val());


            if ($("#x_deal_field_type").val() != 0) {

                bootbox.confirm("Bạn muốn xóa loại dữ liệu này?", function(result) {
                    if (result) {
                        question.changeDealFieldType();
                    }
                });
            } else {
                bootbox.alert("Bạn chưa chọn loại dữ liệu cần xóa.", function() {
                });
            }


        }

    },
    hideFormDealFieldType: function() {
        $(".popup-editable1").hide();
    },
    getListDealFieldType: function() {


        var fieldtypeid = $("#x_deal_field_type").val();
        if (fieldtypeid > 0) {
            var url = "/lay-deal-field-type?fieldtypeid=" + fieldtypeid;

            var success = function(response) {
                $("#x_txt_new_deal_field_type").val(response[0].TypeFieldName);
                $("#x_typefieldid").val(response[0].TypeFieldID);
                $("#x_isset_deal_field_type").val(1);
                $(".popup-editable1").show();
            };

            $.ajax({ url: url, success: success, async: false, type: "POST" });
        } else {

            bootbox.alert("Chưa chọn loại dữ liệu", function() {
            });
        }
    },
    getListDealFieldTypeDropDownList: function() {
        var url = "/lay-deal-field-type?fieldtypeid=0";
        var success = function(response) {
            var op = "";
            if (response.length > 0) {
                op += "<option value=0>--- Chọn loại dữ liệu ---</option>";
                for (var i = 0; i < response.length; i++) {
                    op += "<option value=" + response[i].TypeFieldID + ">" + response[i].TypeFieldName + "</option>";
                }

                $("#x_deal_field_type").html(op);
            } else {
                $("#x_deal_field_type").html(op);
            }
        };

        $.ajax({ url: url, success: success, async: false, type: "POST" });
    },
    changeDealFieldType: function() {
        var x_fieldtypename = $("#x_txt_new_deal_field_type").val();
        var x_typefieldid = $("#x_typefieldid").val();
        var x_issset = $("#x_isset_deal_field_type").val();
        this.regex = /\w+/gi;
        if (x_issset == 1 || x_issset == 0) {
            if (this.regex.test(x_fieldtypename)) {
                $("#e_x_txt_new_dealfieldtype").hide();
            } else {
                $("#e_x_txt_new_dealfieldtype").show();
                return false;
            }
        }
        var url = "/change-deal-field-type";
        var data = { "x_isset": x_issset, "x_fieldtypeid": x_typefieldid, "x_fieldtypename": x_fieldtypename };
        var success = function(response) {
            bootbox.alert(response.Message, function() {
            });
            question.getListDealFieldTypeDropDownList();
            $(".popup-editable1").hide();
        };
        $.ajax({ url: url, success: success, data: data, async: false, type: "POST" });

    },
    initCheckbox: function() {
        $('.check_all,.check_row').prop('checked', true).iCheck({
            checkboxClass: 'icheckbox_minimal-green',
            radioClass: 'iradio_minimal-green'
        });

        $('.check_all').on('ifChecked', function(event) {
            $('.check_row').iCheck('check');
        }).on('ifUnchecked', function(event) {
            $('.check_row').iCheck('uncheck');
        });

        $('.check_row').on('ifChecked', function(event) {
            $(this).closest('tr').addClass('active_row');
        }).on('ifUnchecked', function(event) {
            $(this).closest('tr').removeClass('active_row');
        });
    },
    showCoppyQuestion: function() {
        $("#from_cat_question").val($("#cat_question").val());
        $("#cont_left").attr("class", "col-lg-12");
        $("#cont_right").hide();
        question.changefromcate();
        $("tr td .btn_view,tr td .btn_delete,#x_view_form_deal").hide();
        $("#tabs_content_c").addClass("copy_cate");
    },

    hideCoppyQuestion: function() {
        $("#x_create_new_question").show();
        $("#x_txt_case_change_cate").val(0);
        $("#cat_question").val($("#from_cat_question").val());
        $("#box_coppy_question").hide();
        $("#box_cate_question,#btn_show_coppy").show();
        $("#cont_left").attr("class", "col-lg-8");
        $("#cont_right").show();
        $("tr td .btn_view,tr td .btn_delete,#x_view_form_deal").show();
        $("#tabs_content_c").removeClass("copy_cate");
    },
    changefromcate: function() {
        var x_cateid = $("#from_cat_question").val();
        var url = "/lay-chuyen-muc-coppy-den?x_cateid=" + x_cateid;
        var success = function(response) {
            this.op = "";
            if (response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    this.op += "<option value ='" + response[i].CateID + "'>" + response[i].CateName + "</option>";
                }
            } else {
                this.op += "<option value ='0'>--- Không tìm thấy chuyên mục---</option>";
            }
            $("#x_create_new_question").hide();
            $("#x_txt_case_change_cate").val(1);
            $("#box_coppy_question").show();
            $("#box_cate_question,#btn_show_coppy").hide();
            $("#to_cat_question").html(this.op);
            //question.changeCat();
        };

        $.ajax({ url: url, success: success, async: false, type: "POST" });
    },
    saveCoppy: function() {

        var from_cate = $("#from_cat_question").val();
        var to_cate = $("#to_cat_question").val();
        if (from_cate == to_cate) {
            bootbox.alert("Không  được chọn cùng chuyên mục, vui lòng chọn chuyên mục khác", function() {
            });
            return false;
        }
        var url = "/coppy-cau-hoi-den-chuyen-muc-khac";
        var data = [];
        var success = function(response) {

            bootbox.alert(response.Message, function() {
                question.hideCoppyQuestion();
                question.changeCat();
            });
        };

        $(".check_row").each(function(index) {
            if ($(this).prop("checked") == true) {
                data.push({ id: $(this).val(), cateid: to_cate });
            }
        });
        if (data.length > 0) {
            $.ajax({ url: url, data: JSON.stringify({ "param": data }), success: success, type: "POST", async: false, dataType: 'json', contentType: 'application/json; charset=utf-8' });
        } else {
            bootbox.alert("Bạn chưa chọn điều kiện cần copy !", function() {
            });
        }
    },
    createNewQuestion: function() {
        var cateid = $("#cat_question").val();
        $("#x_ddl_cate").val(cateid);
        question.clearForm();
    },
    showIframe: function(el) {
        $this = $(el);
        var cateid = $("#cat_question").val();
        $("#form_question_deal").prop("src", "/xem-form-deal?cateid=" + cateid);
    },
    updateiswebisvoucher: function(el) {
        var isset = $(el).attr("data-isset");
        var dsfid = $('input[name="data-dsfid"]', $(el).parent().parent());
        var ischecked = $(el).is(":checked");

        var url = "/update-web-voucher";
        var data = {
            isset: isset,
            ischecked: ischecked,
            dsfid: $(dsfid).val()
        };

        var success = function(response) {
            if (response.ID == 1) {
                ShowNotify('success', response.Message, 2000);
            } else {
                ShowNotify('error', response.Message);
            }
        };

        iosOverlay({
            text: "Đang tải...",
            icon: "fa fa-spinner fa-spin",
            shadow: true,
            id: 'LoadingOverlay'
        });

        $.ajax({ url: url, type: "POST", data: data, async: false, success: success });
    }
};

