
window.onload = function() {
    pageSize = 200;
    pageNumber = 1;
    question.disableInputFormEnterKey();
    //if (totalRows > pageSize)
    //$("#pagingQuestion").html(base.paging(pagNumber, pageSize, totalRows, "danh-sach-cau-hoi"));
    $("#txt_search").keypress(function(e) {
        if (e.which == 13) {
            question.changeCat();
        }
    });
    $("#x_txt_question,#x_txt_new_group,#x_group_description,#x_txt_new_merchant_detail_type,#x_txt_note").change(function(e) {
        $(this).val(base.ReplaceSpecChar($(this).val()));
    });
    question.tisaToolTips();
    question.initCheckbox();
    $(".chk_visible").bootstrapSwitch();
};

var question = {
    questionTypeChange: function() {
        var val = $("#MDFTID").val();
        $("#cont-field").html("");
        $("#strFieldSuggest").val("");
        if (val === "1" || val === "3" || val === "5") {
            question.createNewField();
        }
    },
    //tao suggest khi muon them moi
    createNewField: function() {
        var childEl = $("#cont-field").children().length;
        var str = "<div>" +
            "<input class='x_question_type' data-mdfsid='0' style='width:53%;margin-top:10px;text-indent:15px' onchange='question.suggestChange();'>" +
            "<a class='btn_addsuggest btn-success btn-sm' style='font-size:13px;cursor:pointer' database-suggest='0' onclick='question.createNewField();'>Thêm </a>";
        if (childEl > 0) {
            str += "<a class='btn-danger btn-sm' style='color:white;font-size: 13px;cursor:pointer;' onclick='question.removeField(this);'>Xóa</a>";
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
                    var mdfsid = $(el).attr("data-mdfsid");
                    var url = "/xoa-goi-y/?mdfsid=" + mdfsid;
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
        window.location.href = "/tao-cau-hoi-merchant?cat_question=" + catID;

        //var catID = $("#cat_question").val();
        //if ($("#x_txt_case_change_cate").val() == 1) {
        //    catID = $("#from_cat_question").val();
        //}
        //var txtSearch = $("#txt_search").val();
        //var url = "/danh-sach-cau-hoi/?isSet=0&MDFID=0&catID=" + catID + "&pageNumber=" + pageNumber + "&txtSearch=" + txtSearch;
        //var success = function (response) {
        //    question.createListQuestion(response, pageNumber, pageSize, txtSearch);
        //}
        //$.ajax({ url: url, type: "POST", success: success, async: false });
    },
    //inner danh sach cau hoi
    createListQuestion: function(response, pageNumber, pageSize, txtSearch) {

        if (response !== 0) {
            var str = "<thead>" +
                "tr>" +
                "<th style='text-align: center; width: 30px;'>" +
                "<input class='check_all' type='checkbox' id='check_all' />" +
                "</th>" +
                "<th style='text-align: left;'></th>" +
                "</tr>" +
                "</thead>" +
                "<tbody>";
            for (var i = 0; i < response.length; i++) {

                var FieldSuggest = [];
                if (response[i].SuggestValue != null && response[i].SuggestValue != "") {
                    FieldSuggest = response[i].SuggestValue.split("*$#");
                }

                if (i == 0 || (i < response.length - 1 && (i > 0 && response[i].MDGID != response[i - 1].MDGID))) {
                    str += "<tr><td colspan='2' style='border:none;padding: 0;'>" +
                        "<h4 class='panel-heading' style='margin-top: 0px; margin-bottom: 5px; font-style: italic; font-weight: bold; padding: 5px; background-color: #dddddd;'>" +
                        response[i].GroupName +
                        "</h4>" +
                        "</td></tr>";
                }

                str += "<tr style='border-bottom: 1px dashed #aaa;overflow:hidden;'>" +
                    "<td style='border: none;'>" +
                    "<input class='check_row' type='checkbox' value=" + response[i].MDFID + " /></td>" +
                    "</td>";

                str += "<td style='text-align: left; border: none;'>" +
                    "<div class='col-lg-12'>" +
                    "<div class=' col-lg-6 col-md-6'>" +
                    "<h5>" +
                    response[i].No + "." + response[i].FieldName +
                    "</h5>";
                switch (response[i].MDFTID) {
                case 1:
                    str += "<div class=' col-sm-12 col-xs-8'>";
                    for (var m = 0; m < FieldSuggest.length; m++) {
                        str += "<label style='font-weight: normal;overflow:hidden;width:100%;margin-top:5px;margin-bottom:0;'>" +
                            "<input type='radio' style='float: left;margin-top: 4px;margin-right: 5px;'/><span>" + FieldSuggest[m] + "</span></label>";
                    }
                    str += "</div>";
                    break;
                case 2:
                    str += "<div class=' col-sm-12 col-xs-8'>" +
                        "<input type='checkbox' class='chk_visible' checked data-size='small' data-on-color='success' />" +
                        "</div>";
                    break;
                case 3:
                    str += "<div class= col-sm-12 col-xs-8' style='padding-top:5px;'>" +
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
                            "<input type='checkbox' value='1' class='checkbox_list' />" +
                            FieldSuggest[m] +
                            "</label>";
                    }
                    str += "</div>";
                    break;
                case 6:
                    str += "<div class=' col-sm-12 col-xs-8'>" +
                        "<input type='checkbox' class='chk_visible' checked  data-size='small' data-on-color='success'>" +
                        "<input style='font-weight: normal; margin-top: 10px;' type='text' class='form-control'>" +
                        "</div>";
                    break;
                }
                str += "</div>";
                var style = "display:none;";
                if ($("#x_txt_case_change_cate").val() == 0) {
                    style = "";
                }

                str += "<div class='form-group col-lg-3'>" +
                    "<button class='btn btn-success  btn_view btn-xs' data-catid=" + response[i].CateID + " data-mdfid=" + response[i].MDFID + " onclick='question.viewQuesitionDetail(this);' style='margin-right:3px;padding:3px 13px;margin-top:10px;margin-left:15px;" + style + "'><i class='fa fa-file-text-o'></i></button>" +
                    "<button class='btn btn-danger btn_delete btn-xs' data-mdfid=" + response[i].MDFID + " onclick='question.deleteQuestion(this);' style='padding:3px 12px;margin-top:10px;" + style + "'><i class='fa fa-trash-o'></i></button>" +
                    "</div>";
                str += "</div>";
                str += "</td>" +
                    "</tr>";
            }
            ;
            str += "</tbody>";
            $("#table_question").html(str);
            $("#countQuestion").text("(có " + response.length + " điều kiện)");

            //var str = base.paging(pageNumber, pageSize, response[0].TotalRows, "danh-sach-cau-hoi");
            //$("#pagingQuestion").html(str);
            $(".question_no_data").hide();
            question.initCheckbox();
            $(".chk_visible").bootstrapSwitch();
        } else {
            $("#table_question").html("");
            $("#pagingQuestion").html("");
            $(".question_no_data").show();
        }
    },
    //kiem tra du lieu truoc khi gui len server , update hoac them moi mot cau hoi
    validFormQuestion: function() {
        //$("#UL_QuestionResult").hide();
        var x_ddl_question_type = $("#x_ddl_question_type").val();
        var x_continue = true;

        if (x_ddl_question_type != "0") {
            var strFieldSuggest = "";
            //kiem tra suggest loai cau hoi 1,3,5
            $("#cont-field .x_question_type").each(function(index) {
                if ($(this).val().trim() == "") {
                    $(this).parent().find(".filled").show();
                    x_continue = false;
                } else {
                    $(this).parent().find(".filled").hide();
                }
                strFieldSuggest += $(this).val() + "@$#";
            });

            $("#strFieldSuggest").val(strFieldSuggest);
        }
        return x_continue;
    },
    //tao cau hoi moi thanh cong
    createQuestionComplete: function(response) {
        //$("#UL_QuestionResult").show();
        //category khi tao moi 
        bootbox.alert(response.responseText, function() {
            question.changeCat();

            //var catID = $("#x_ddl_cate").val();
            //question.clearForm();
            //$("#cat_question").val(catID);
            //var txtSearch = $("#txt_search").val();
            //var url = "/danh-sach-cau-hoi/?isSet=0&MDFID=0&catID=" + catID + "&pageNumber=" + pageNumber + "&txtSearch=" + txtSearch;
            //var success = function (response) {
            //    question.createListQuestion(response, pageNumber, pageSize, txtSearch);
            //}
            //$.ajax({ url: url, type: "POST", success: success, async: false });
        });
    },
    //tao nhung suggest khi lay tu database xuong
    suggestChange: function() {
        var strFieldSuggest = "";
        //kiem tra suggest loai cau hoi 1,3,5
        $("#cont-field .x_question_type").each(function(index) {
            strFieldSuggest += $(this).attr("data-mdfsid") + ":" + $(this).val() + "@$#";
        });
        $("#strFieldSuggest").val(strFieldSuggest);
    },
    //xem chi tiet cua mot cau hoi
    viewQuesitionDetail: function(el) {
        base.loading();
        $("#e_x_ddl_group,#e_x_txt_question,#e_x_ddl_question_type").hide();
        //$("#UL_QuestionResult").hide();
        var MDFID = $(el).attr("data-mdfid"),
            catID = $(el).attr("data-catid");
        $("#table_question tr").removeClass("tr_active");
        $(el).parent().parent().parent().parent().addClass("tr_active");
        $(".id_qs_mer").text("Chi tiết điều kiện");
        var url = "/chi-tiet-cau-hoi?MDFID=" + MDFID;
        var listFieldSuggest = "";
        var success = function(response) {
            //console.log(response);
            base.scrollTop();
            if (response != "0") {
                $("#CateID").val(response.CateID);
                $("#MDGID").val(response.MDGID);
                $("#FieldName").val(response.FieldName);
                $("#MDFTID").val(response.MDFTID);
                $("#FieldTip").val(response.FieldTip);
                $("#MDTID").val(response.MDTID);
                if (response.isRequired == 1) {
                    $(".chk_true").prop("checked", true);
                    $(".chk_false").prop("checked", false);
                } else {
                    $(".chk_false").prop("checked", true);
                    $(".chk_true").prop("checked", false);
                }

                $("#cont-field").html("");
                $("#MDFID").val(MDFID);

                if (response.FieldSuggest != null) {
                    for (var i = 0; i < response.FieldSuggest.length; i++) {
                        suggest = response.FieldSuggest[i];

                        if (suggest.SuggestValue != "" & suggest.SuggestValue != null) {
                            listFieldSuggest += suggest.MDFSID + ":" + suggest.SuggestValue + "@$#";
                            question.FieldSuggestUpdate(suggest);
                        }
                    }

                    $("#strFieldSuggest").val(listFieldSuggest);
                }
            }
        };
        $.ajax({ url: url, type: "POST", success: success, async: false });
    },
    //tao text box suggest da co
    FieldSuggestUpdate: function(data) {
        var childEl = $("#cont-field").children().length;
        var str = "<div>" +
            "<input data-mdfsid=" + data.MDFSID + " class='x_question_type' value='" + data.SuggestValue + "' style='width:53%;margin-top:10px;text-indent:15px' onchange='question.suggestChange();'>" +
            "<a class='btn_addsuggest btn-success btn-sm' style='font-size:13px;cursor:pointer' onclick='question.createNewField();'>Thêm </a>";
        if (childEl > 0) {
            str += "<a class='btn-danger btn-sm' style='font-size: 13px;cursor:pointer;' database-suggest='1' data-mdfsid='" + data.MDFSID + "' onclick='question.removeField(this);'>Xóa</a>";
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
        $('#MDFID').val(0);
        $('#strFieldSuggest').val('');
        $("#MDGID option, #MDFTID option").prop("selected", false);
        $("#FieldName, #FieldTip").val("");

        $(".chk_true").prop("checked", true);
        $("#cont-field").html("");
        $("#table_question tr").removeClass("tr_active");
        $(".id_qs_mer").text("Tạo thông tin mới");
    },
    deleteQuestion: function(el) {

        bootbox.confirm("Bạn muốn xóa điều kiện này ?", function(result) {
            if (result) {
                var MDFID = $(el).attr("data-mdfid");
                var url = "/xoa-cau-hoi/?mdfid=" + MDFID;
                var success = function(response) {

                    alert(response);

                    //bootbox.alert(response, function () { });
                    question.changeCat();
                };
                $.ajax({ url: url, type: "POST", success: success, async: false });
            }
        });


    },
    pagingQuestion: function(pageNumber, pageSize, href) {
        base.loading();
        var catID = $("#cat_question").val();
        var txtSearch = $("#txt_search").val();
        var url = "/danh-sach-cau-hoi/?isSet=0&MDFID=0&catID=" + catID + "&pageNumber=" + pageNumber + "&txtSearch=" + txtSearch;
        var success = function(response) {
            question.createListQuestion(response, pageNumber, pageSize, txtSearch);
            $(".ui-ios-overlay-shadow").hide();
        };
        $.ajax({ url: url, type: "POST", success: success, async: false });

    },
    loading: function() {
        iosOverlay({
            text: "Đang tải...",
            icon: "fa fa-spinner fa-spin",
            shadow: true
        });
    },
    changeGroup: function() {
        this.x_isset = $("#x_txt_isset").val();
        this.x_mdgid = $("#x_txt_group_id").val();
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
        var url = "/change-nhom-cau-hoi-merchant";
        var data = {
            x_isset: this.x_isset,
            x_mdgid: this.x_mdgid,
            x_groupname: this.x_group_name,
            x_groupdesc: this.x_groupdesc
        };
        var success = function(response) {
            if (response.ID > 0) {
                question.getListMerchantDetailGroup(0);
                base.hidePopup();
            }
            bootbox.alert(response.Message, function() {
            });
        };
        if (this.visible) {
            base.loading();
            $.ajax({ url: url, data: data, success: success, type: "POST", async: false });
        }
    },
    tisaToolTips: function() {
        $.fn.powerTip.defaults.smartPlacement = true;
        $(".pTip_top").powerTip({ placement: 'n' });
        $(".add-merchantgroup").data('powertipjq', $(['<p><b>' + $("#tooltips_add_merchant_group").val() + '</b></p>'].join('\n')));
        $(".del-merchantgroup").data('powertipjq', $(['<p><b>' + $("#tooltips_del_merchant_group").val() + '</b></p>'].join('\n')));
        $(".up-merchantgroup").data('powertipjq', $(['<p><b>' + $("#tooltips_up_merchant_group").val() + '</b></p>'].join('\n')));

        $(".saveGroup").data('powertipjq', $(['<p><b>' + $("#tooltips_save_merchant_group").val() + '</b></p>'].join('\n')));
        $(".cancelPopup").data('powertipjq', $(['<p><b>' + $("#tooltips_exit_popup").val() + '</b></p>'].join('\n')));

        $(".up-merchantdetailtype").data('powertipjq', $(['<p><b>Cập nhật loại dữ liệu</b></p>'].join('\n')));
        $(".add-merchantdetailtype").data('powertipjq', $(['<p><b>Thêm loại dữ liệu</b></p>'].join('\n')));
        $(".del-merchantdetailtype").data('powertipjq', $(['<p><b>Xoá loại dữ liệu</b></p>'].join('\n')));
    },
    showAddNewGroup: function(el) {
        $("#x_txt_new_group").val("");
        $("#x_group_description").val("");
        $("#txt_group_id").val(0);
        $("#x_txt_isset").val(0);
        base.showPopup(el);
    },
    showUpdateGroup: function(el) {
        var mdgid = $("#x_ddl_group").val();
        if (mdgid == 0)
            bootbox.alert("Chưa chọn nhóm điều kiện", function() {
            });
        else {
            $("#x_txt_isset").val(1);
            question.getListMerchantDetailGroup(mdgid);
            base.showPopup(el);
        }
        return false;
    },
    getListMerchantDetailGroup: function(MDGID) {
        this.url = "lay-danh-sach-nhom-cau-hoi-merchant?MDGID=" + MDGID;
        this.success = function(response) {
            //get cho dropdownlist
            if (MDGID == 0) {
                this.op = "<option value='0'> ---Chọn group--- </option>";
                for (var i = 0; i < response.length; i++) {
                    this.op += "<option value ='" + response[i].MDGID + "'>" + response[i].GroupName + "</option>";
                }
                $("#x_ddl_group").html(this.op);
            }
                //get cho update 
            else {
                if (response.length > 0) {
                    $("#x_txt_new_group").val(response[0].GroupName);
                    $("#x_group_description").val(response[0].GroupDescription);
                    $("#x_txt_group_id").val(response[0].MDGID);
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
                    bootbox.alert("Chưa chọn nhóm điều kiện", function() {
                    });
                else
                    question.changeGroup();
            }
        });


    },
    showFormDealFieldType: function(x_isset) {

        $("#e_x_txt_new_dealfieldtype").hide();
        if (x_isset == 0) {
            $("#x_txt_new_merchant_detail_type").val("");
            $("#x_save_mdtid").val(0);
            $("#x_isset_merchant_detail_type").val(x_isset);
            $(".popup-editable1").show();
        } else if (x_isset == 1) {
            if ($("#x_mdtid").val() != 0) {
                question.getListDealFieldType();
            } else {
                bootbox.alert("Bạn chưa chọn loại dữ liệu", function() {
                });
            }

        } else if (x_isset == 2) {
            $(".popup-editable1").hide();
            $("#x_isset_merchant_detail_type").val(x_isset);
            $("#x_save_mdtid").val($("#x_mdtid").val());

            bootbox.confirm("Bạn muốn xóa loại dữ liệu này?", function(result) {
                if (result) {
                    if ($("#x_mdtid").val() != 0) {

                        question.changeDealFieldType();
                    } else {
                        bootbox.alert("Bạn chưa chọn loại dữ liệu cần xóa.", function() {
                        });
                    }
                }
            });


        }

    },
    hideFormDealFieldType: function() {
        $(".popup-editable1").hide();
    },
    getListDealFieldType: function() {
        var x_mdtid = $("#x_mdtid").val();
        if (x_mdtid > 0) {
            var url = "/lay-loai-du-lieu-merchant?x_mdtid=" + x_mdtid;

            var success = function(response) {
                $("#x_txt_new_merchant_detail_type").val(response[0].Name);
                $("#x_save_mdtid").val(response[0].MDTID);
                $("#x_isset_merchant_detail_type").val(1);
                $(".popup-editable1").show();
            };

            $.ajax({ url: url, success: success, async: false, type: "POST" });
        } else {

            bootbox.alert("Chưa chọn loại dữ liệu", function() {
            });
        }
    },
    changeDealFieldType: function() {
        var x_name = $("#x_txt_new_merchant_detail_type").val();
        var x_mdtid = $("#x_save_mdtid").val();
        var x_issset = $("#x_isset_merchant_detail_type").val();
        this.regex = /\w+/gi;
        if (x_issset == 1 || x_issset == 0) {
            if (this.regex.test(x_name)) {
                $("#e_x_txt_new_merchantdetailtype").hide();
            } else {
                $("#e_x_txt_new_merchantdetailtype").show();
                return false;
            }
        }
        var url = "/change-merchant-detail-type";
        var data = { "x_isset": x_issset, "x_mdtid": x_mdtid, "x_name": x_name };
        var success = function(response) {
            bootbox.alert(response.Message, function() {
            });
            question.getListMerchantDetailTypeDropDownList();
            $(".popup-editable1").hide();
        };
        $.ajax({ url: url, success: success, data: data, async: false, type: "POST" });

    },
    getListMerchantDetailTypeDropDownList: function() {
        var url = "/lay-loai-du-lieu-merchant?x_mdtid=0";
        var success = function(response) {
            var op = "";
            if (response.length > 0) {
                op += "<option value=0>--- Chọn loại dữ liệu ---</option>";
                for (var i = 0; i < response.length; i++) {
                    op += "<option value=" + response[i].MDTID + ">" + response[i].Name + "</option>";
                }

                $("#x_mdtid").html(op);
            } else {
                $("#x_mdtid").html(op);
            }
        };

        $.ajax({ url: url, success: success, async: false, type: "POST" });
    },
    showCoppyQuestion: function() {
        $("#cont_left").attr("class", "col-lg-12");
        $("#cont_right").hide();
        $("#from_cat_question").val($("#cat_question").val());
        question.changefromcate();
        $("tr td .btn_view,tr td .btn_delete,#x_view_form_merchant").hide();
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
        $("tr td .btn_view,tr td .btn_delete,#x_view_form_merchant").show();
        $("#tabs_content_c").removeClass("copy_cate");
    },
    changefromcate: function() {

        var x_cateid = $("#from_cat_question").val();
        var url = "/lay-chuyen-muc-coppy-den?x_cateid=" + x_cateid;
        var success = function(response) {
            this.op = "";
            if (response.length > 0) {
                if (response[0].CateID == undefined) {
                    bootbox.alert("Bạn chưa được phân quyền copy !", function() {
                    });
                    return false;
                }

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
    createNewQuestion: function() {
        var cateid = $("#cat_question").val();
        $("#CateID").val(cateid);
        question.clearForm();
    },
    saveCoppy: function() {

        var from_cate = $("#from_cat_question").val();
        var to_cate = $("#to_cat_question").val();
        if (from_cate == to_cate) {
            bootbox.alert("Không  được chọn cùng chuyên mục, vui lòng chọn chuyên mục khác", function() {
            });
            return false;
        }
        var url = "/merchant-coppy-cau-hoi-den-chuyen-muc-khac";
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
    showIframe: function(el) {
        $this = $(el);
        var cateid = $("#cat_question").val();
        $("#form_question_merchant").prop("src", "/xem-form-merchant?cateid=" + cateid);
    }
};
