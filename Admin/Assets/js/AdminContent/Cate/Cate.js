

var cate = {
    jsonResponse: [],
    visible: true,
    url: [
        "/lay-danh-sach-chuyen-muc",
        "lay-danh-sach-chuyen-muc-parent-cap1",
        "xem-chi-tiet-chuyen-muc",
        "change-category",
        "xoa-picture-chuyen-muc",
        "xoa-icon-chuyen-muc"],
    innerHTML: "",
    success: function() {
    },
    getListCategory: function(x_CateID) {
        var url = cate.url[0] + "?x_CateID=" + x_CateID;
        var success = function(response) {
            cate.jsonResponse = response;

        };
        $.ajax({ url: url, success: success, type: "POST", async: false });
    },
    //xem theo cay thu muc
    viewTreeCate: function(x_CateID) {
        cate.reFreshForm();
        $(".box_category li").removeClass("active");
        this.jsonResponse = [];
        this.visible = true;
        this.innerHTML = "";
        if (x_CateID != 0) {
            if ($("#cate_" + x_CateID).attr("class").indexOf("fa-minus-square-o") > -1) {
                $("#cate_" + x_CateID).removeClass("fa-minus-square-o").addClass("fa-plus-square-o");
                $("#child_cate_" + x_CateID).html("");
                this.visible = false;
            }
        }
        if (this.visible) {
            $("#cate_" + x_CateID).removeClass("fa-plus-square-o").addClass("fa-minus-square-o");
            this.getListCategory(x_CateID);

        }

        for (var i = 0; i < this.jsonResponse.length; i++) {
            this.styleID = "#ac4840";
            if (this.jsonResponse[i].CatePath.split(".").length == 2) {
                this.styleID = "#799f3b";
            } else if (this.jsonResponse[i].CatePath.split(".").length == 3) {
                this.styleID = "#8e3561";
            } else if (this.jsonResponse[i].CatePath.split(".").length == 4) {
                this.styleID = "#31833f";
            }

            this.style = "";
            if (this.jsonResponse[i].Visible == 0) {
                this.style = "style='opacity:0.3;'";
            }
            if (this.jsonResponse[i].Visible == 0) {
                this.style = "style='opacity:0.3;'";
            }
            this.innerHTML +=
                "<li><span class='cont_event' style='position:relative;width:100%;height:100%;'>" +
                    "<span class='cat-icon fa fa-plus-square-o' id='cate_" + this.jsonResponse[i].CateID + "' onclick='cate.viewTreeCate(" + this.jsonResponse[i].CateID + ");'></span>" +
                    "<span class='box_event'>" +
                    "<span " + this.style + ">" + this.jsonResponse[i].CateName + "<strong class='badge' style='margin-left:15px;background-color:" + this.styleID + ";'>ID :" + this.jsonResponse[i].CateID + "</strong></span>" +
                    "</span>" +
                    "<span class='outline_event' style='float:right;'>" +                            
                    "<span class='pTip_top cat-icon fa fa-trash-o  del-cate  color_g pull-right item_event' onclick=cate.deleteCategory(" + this.jsonResponse[i].CateID + "," + this.jsonResponse[i].CateParentID + "); style='margin-right: 4px;'></span>" +
                    "<span class='pTip_top cat-icon fa fa-plus   add-cate  color_d pull-right item_event' onclick='cate.addChildCategory(" + this.jsonResponse[i].CateID + "," + this.jsonResponse[i].CateContractID + ");'></span>" +
                    "<span class='pTip_top cat-icon fa fa-file-text-o  up-cate  color_a pull-right item_event' onclick='cate.viewCategoryDetail(" + this.jsonResponse[i].CateID + ");' ></span>" +
                    "</span>" +
                    "</span>" +
                    "<ul class='child_cate' id='child_cate_" + this.jsonResponse[i].CateID + "'></ul>" +
                    "</li>";
        }
        if (x_CateID == 0) {
            $(".box_category").html(this.innerHTML);
        } else {
            $("#child_cate_" + x_CateID).html(this.innerHTML).slideDown(500);
        }
        cate.hoverWhenNewLoad();
        cate.tisaToolTips();

    },
    //Tao dropdownlist category 
    getListCategoryAsParentName: function(cateID) {
        this.success = function(response) {
            // base.loading();
            this.innerHTML = "";
            this.jsonResponse = response;
            for (var i = 0; i < this.jsonResponse.length; i++) {
                this.innerHTML += "<option data-contractid='" + this.jsonResponse[i].CateContractID + "' cat-path='" + this.jsonResponse[i].CatePath + "' value=" + this.jsonResponse[i].CateID + ">" + this.jsonResponse[i].CateName + "</option>";
            }
            $("#ddl_cate_0").html(this.innerHTML);
            $("#ddl_cate_0").val(cateID);
            //cate.getSelectedText();
        };
        $.ajax({ url: cate.url[1], success: this.success, type: "POST", async: true });
    },
    getSelectedText: function() {
        if ($("#ddl_cate_0").val() == 0)
            return false;

        this.innerHTML = "";
        var catPath = $("#ddl_cate_0 option:selected").attr("cat-path");
        var split = catPath.split(".");
        for (var i = 0; i < split.length; i++) {
            this.innerHTML += "<span>" +
                $("#ddl_cate_0 option[value=" + split[i] + "]").text().split("/")[1] +
                "</span>";

            this.innerHTML += "<span class='fa fa-angle-right' style='margin:0 5px;color:red;'></span>";

        }
        $(".txt-path").html(this.innerHTML + $("#x_txt_cate_name").val());
    },
    viewCategoryDetail: function(x_CateID) {

        $("#cont_cate_detail").removeClass("chamgebackground");
        $("#e_x_txt_cate_name,#e_x_txt_cateid_contact").hide();
        $(".box_category li").removeClass("active");
        $(".id_qs_mer").text("Chi tiết ngành hàng");
        $("#cate_" + x_CateID).parent().parent().addClass("active");
        if ($("#cate_" + x_CateID).attr("class").indexOf("fa-minus-square-o") > -1) {
            $("#cate_" + x_CateID).removeClass("fa-minus-square-o").addClass("fa-plus-square-o");
        }


        $("#child_cate_" + x_CateID).html("");
        this.success = function(response) {
            //base.loading();
            this.innerHTML = "";
            this.jsonResponse = response;
            $("#cont_cate_detail").addClass("chamgebackground");
            $("#x_txt_cateid_contact").val(this.jsonResponse.CateContractID).attr("disabled", true);
            $("#x_txt_cate_name").val(this.jsonResponse.CateName);
            $("#ddl_cate_0").val(this.jsonResponse.CateParentID);
            $("#chk_visible").prop("checked", this.jsonResponse.Visible);
            if (this.jsonResponse.Visible) {
                $("#chk_visible").parent().parent().removeClass("switch-off").addClass("switch-on");
            } else {
                $("#chk_visible").parent().parent().removeClass("switch-on").addClass("switch-off");
            }

            $("#x_cateid").val(this.jsonResponse.CateID);
            $("#x_txt_rewrite_cate").val(this.jsonResponse.CateRewrite);
            $("#x_isset").val(1);

            $(".listPicture").html("");
            $("#x_pictureurl").val("");

            if (this.jsonResponse.PictureURL != "" && this.jsonResponse.PictureURL != null) {
                var xmlDoc = $.parseXML(this.jsonResponse.PictureURL);
                var xml = $(xmlDoc);
                var picture = xml.find("PictureURL");

                for (var i = 0; i < picture.length; i++) {
                    var imgSave = $(picture[i]).text();
                    var fulllkThum = x_domainGlobal + 'resize/131_131/' + imgSave;
                    var fulllk = x_domainGlobal + imgSave;
                    var colorboxRel = "";
                    var imgStr = '<li>'
                        + '<a href="' + fulllk + '" class="imgDemoCate" data-colorbox-rel="ImageDemo"><img src="' + fulllkThum + '"  onclick="ImgColorbox(\'ImageDemo\')"  class="thumbnail img-responsive" alt="" style="float:left"></a>'
                        + '<a href="javascript:void(0)" title="Xóa hình này" data-name="' + (imgSave) + '" onclick="cate.removePicture(this);" class="btnRemoveImage" style="display:inline-block;float:left;margin-top:-5px;position:absolute;top:-11px;left:0px"><span class="fa fa-times"></span></a></li>';
                    $(".listPicture").append(imgStr);

                    var arLinkImg = $("#x_pictureurl").val();
                    if (arLinkImg == "") {
                        $("#x_pictureurl").val(imgSave);
                    } else {
                        $("#x_pictureurl").val(arLinkImg + ',' + imgSave);

                    }
                }
            }
            if (this.jsonResponse.IconURL != "" && this.jsonResponse.IconURL != null) {
                var imgSave = this.jsonResponse.IconURL;
                var fulllkThum = x_domainGlobal + 'resize/131_131/' + imgSave;
                var fulllk = x_domainGlobal + imgSave;
                var colorboxRel = "";
                var imgStr = '<li>'
                    + '<a href="' + fulllk + '" class="imgDemoCate" data-colorbox-rel="ImageDemo"><img src="' + fulllkThum + '" onclick="ImgColorbox(\'ImageDemo\')"  class="thumbnail img-responsive" alt="" style="float:left"></a>'
                    + '<a href="javascript:void(0)" title="Xóa hình này" data-name="' + (imgSave) + '" onclick="cate.removeICon(this,' + this.jsonResponse.CateID + ');" class="btnRemoveImage" style="display:inline-block;float:left;margin-top:-5px;position:absolute;top:-11px;left:0px"><span class="fa fa-times"></span></a></li>';
                $(".listIcon").html(imgStr);
                $("#x_txt_icon_url").val(this.jsonResponse.IconURL);
            } else {
                $(".listIcon").html("");
                $("#x_txt_icon_url").val("");
            }

            base.scrollTop();
        };
        $.ajax({ url: cate.url[2] + "?x_CateID=" + x_CateID, success: this.success, type: "POST", async: true });
    },
    //insert update delete
    changeCategory: function() {
        if ($("#ddl_cate_0").val() == -1) {
            bootbox.alert("Bạn chưa chọn ngành hàng ,Vui lòng chọn ngành hàng cần tạo", function() {
            });
            return false;
        }
        this.x_isset = $("#x_isset").val();
        this.x_cateparentid = $("#ddl_cate_0").val();
        this.x_cateid = $("#x_cateid").val();
        this.x_catename = $("#x_txt_cate_name").val();
        this.x_catepath = $("#ddl_cate_0 option:selected").attr("cat-path");
        this.x_visible = $("#chk_visible").prop("checked") == true ? 1 : 0;
        this.x_caterewrite = $("#x_txt_rewrite_cate").val();
        this.x_txt_cateid_contact = $("#x_txt_cateid_contact").val();
        this.x_pictureurl = $("#x_pictureurl").val();
        this.x_iconurl = $("#x_txt_icon_url").val();
        this.visible = true;
        this.regex = /\w+/gi;

        if (!this.regex.test(this.x_catename)) {
            $("#e_x_txt_cate_name").show();
            this.visible = false;
        } else {
            $("#e_x_txt_cate_name").hide();
        }

        if (this.x_txt_cateid_contact == "") {
            $("#e_x_txt_cateid_contact").show();
            this.visible = false;
        } else {
            $("#e_x_txt_cateid_contact").hide();
        }
        this.success = function(response) {
            //base.loading();
            bootbox.alert(response.Message, function() {
                cate.getListCategoryAsParentName(response.ID);
                var x_cateid = $("#ddl_cate_0").val();
                var x_cateparentid = $("#ddl_cate_0").val();

                if (x_cateid == x_cateparentid) {
                    x_cateid = 0;
                }
                cate.viewTreeCateWhenChange(x_cateid);
                $(".box_category li").removeClass("active");
            });

        };
        var data =
        {
            x_isset: this.x_isset,
            x_cateparentid: this.x_cateparentid,
            x_cateid: this.x_cateid,
            x_catename: this.x_catename,
            x_catepath: this.x_catepath,
            x_visible: this.x_visible,
            x_caterewrite: this.x_caterewrite,
            x_catecontractid: this.x_txt_cateid_contact,
            x_sort: 1,
            x_pictureurl: this.x_pictureurl,
            x_iconurl: this.x_iconurl
        };

        if (this.visible) {
            $.ajax({ url: cate.url[3], data: data, success: this.success, type: "Put", async: false });
        }
    },
    reFreshForm: function() {
        $(".txt-path").html("");
        $("#ddl_cate_0,#x_isset,#x_cateid").val(0);
        $("#x_txt_cate_name").val("");
        $("#e_x_txt_cate_name").hide();
        $("#chk_visible").prop("checked", true);
        $("#x_txt_rewrite_cate").val("");
        $(".box_category li").removeClass("active");
        $(".id_qs_mer").text("Tạo ngành hàng mới");
        $("#x_txt_cateid_contact").val("").prop("disabled", false);
        $("#e_x_txt_cateid_contact").hide();
        $(".listPicture,.listIcon").html("");
        $("#x_pictureurl,#x_txt_icon_url").val("");
    },
    deleteCategory: function(x_cateid, x_cateparentid) {

        bootbox.confirm("Bạn muốn xóa ngành hàng này?", function(result) {
            if (result) {
                var data =
                {
                    x_isset: 2,
                    x_cateparentid: 0,
                    x_cateid: x_cateid,
                    x_catename: "",
                    x_catepath: "",
                    x_visible: 0
                };

                this.success = function(response) {
                    bootbox.alert(response.Message, function() {
                    });
                    if (x_cateid == x_cateparentid) {
                        x_cateparentid = 0;
                    }
                    cate.viewTreeCateWhenChange(x_cateparentid);
                };

                $.ajax({ url: cate.url[3], data: data, success: this.success, type: "Put", async: false });
            }
        });

    },
    hoverWhenNewLoad: function() {
        $(".box_category li").hover(function() {
            $(".box_category li").removeClass("hover");
            //$(".outline_event").hide();
            $(this).addClass("hover");
            //$("li.hover .outline_event").show();
        }, function() {
            $(".box_category li").removeClass("hover");
            //(".outline_event").hide();
        });
    },
    addChildCategory: function(cateID, cateContractID) {
        $("#cont_cate_detail").removeClass("chamgebackground");
        base.scrollToElement($(".out_cont_cate_detail").offset().top - 50);
        $(".box_category li").removeClass("active");
        $("#cate_" + cateID).parent().parent().addClass("active");

        if ($("#cate_" + cateID).attr("class").indexOf("fa-minus-square-o") > -1) {
            $("#cate_" + cateID).removeClass("fa-minus-square-o").addClass("fa-plus-square-o");
        }
        $("#child_cate_" + cateID).html("");
        $("#ddl_cate_0").val(cateID);
        $("#x_isset,#x_cateid").val(0);
        $("#x_txt_cate_name").val("");
        $("#x_txt_cateid_contact").val(cateContractID).prop("disabled", true);
        $("#x_txt_rewrite_cate").val("");
        //cate.getSelectedText();
        $("#cont_cate_detail").addClass("chamgebackground");
    },
    tisaToolTips: function() {
        $.fn.powerTip.defaults.smartPlacement = true;
        $(".pTip_top").powerTip({ placement: 'n' });

        $(".add-cate").data('powertipjq', $(['<p><b>' + $("#tooltips_addcate").val() + '</b></p>'].join('\n')));
        $(".del-cate").data('powertipjq', $(['<p><b>' + $("#tooltips_delcate").val() + '</b></p>'].join('\n')));
        $(".up-cate").data('powertipjq', $(['<p><b>' + $("#tooltips_upcate").val() + '</b></p>'].join('\n')));
    },
    viewCateRewrite: function() {
        var value = $("#x_txt_cate_name").val();
        $("#x_txt_rewrite_cate").val(base.convert_Rewrite(base.replaceVN_To_English(base.ReplaceSpecChar(value))));
        $("#x_txt_rewrite_cate").val($("#x_txt_rewrite_cate").val().replace(/-+/g, "-"));
    },
    formCreateCategory: function() {
        $("#cont_cate_detail").removeClass("chamgebackground");
        base.scrollToElement($(".out_cont_cate_detail").offset().top - 50);
        cate.reFreshForm();
        $("#cont_cate_detail").addClass("chamgebackground");
    },
    changeParentCategory: function() {
        var catecontractid = $("#ddl_cate_0 option:selected").attr("data-contractid");
        if (catecontractid != 0) {
            $("#x_txt_cateid_contact").val(catecontractid).prop("disabled", true);
        } else {
            $("#x_txt_cateid_contact").val("").prop("disabled", false);
        }
    },
    removePicture: function(el) {
        bootbox.confirm("Bạn muốn xóa ảnh này ?", function(result) {
            if (result) {
                var pictureURL = $(el).attr("data-name");
                var success = function(response) {
                    if (response.ID == 1) {
                        $(el).parent().remove();
                        $("#x_pictureurl").val("");
                        var pictureURL = "";
                        $(".btnRemoveImage").each(function(index) {
                            pictureURL += $(this).attr("data-name") + ",";
                        });
                        $("#x_pictureurl").val(pictureURL.substring(0, pictureURL.length - 1));
                    } else {
                        bootbox.alert(response.Message, function() {
                        });
                    }
                };
                $.ajax({ url: cate.url[4] + "?pictureURL=" + pictureURL + "", success: success, async: false, type: "POST" });
            }
        });

    },
    removeICon: function(el, x_cateid) {

        if (x_cateid == 0) {
            $(el).parent().remove();
            $("#x_txt_icon_url").val("");
        } else {
            bootbox.confirm("Bạn muốn xóa icon này?", function(result) {
                if (result) {
                    var success = function(response) {
                        if (response.ID == 1) {
                            $(el).parent().remove();
                            $("#x_txt_icon_url").val("");
                        }
                    };

                    $.ajax({ url: cate.url[5] + "?x_cateid=" + x_cateid, success: success, async: false, type: "POST" });
                }
            });

        }
    },
    viewTreeCateWhenChange: function(x_CateID) {
        cate.reFreshForm();
        $(".box_category li").removeClass("active");
        this.jsonResponse = [];
        this.visible = true;
        this.innerHTML = "";
        this.getListCategory(x_CateID);


        for (var i = 0; i < this.jsonResponse.length; i++) {
            this.styleID = "#ac4840";
            if (this.jsonResponse[i].CatePath.split(".").length == 2) {
                this.styleID = "#799f3b";
            } else if (this.jsonResponse[i].CatePath.split(".").length == 3) {
                this.styleID = "#8e3561";
            } else if (this.jsonResponse[i].CatePath.split(".").length == 4) {
                this.styleID = "#31833f";
            }

            this.style = "";
            if (this.jsonResponse[i].Visible == 0) {
                this.style = "style='opacity:0.3;'";
            }
            if (this.jsonResponse[i].Visible == 0) {
                this.style = "style='opacity:0.3;'";
            }
            this.innerHTML +=
                "<li><span class='cont_event' style='position:relative;width:100%;height:100%;'>" +
                    "<span class='cat-icon fa fa-plus-square-o' id='cate_" + this.jsonResponse[i].CateID + "' onclick='cate.viewTreeCate(" + this.jsonResponse[i].CateID + ");'></span>" +
                    "<span class='box_event'>" +
                    "<span " + this.style + ">" + this.jsonResponse[i].CateName + "<strong class='badge' style='margin-left:15px;background-color:" + this.styleID + ";'>ID :" + this.jsonResponse[i].CateID + "</strong></span>" +
                    "</span>" +
                    "<span class='outline_event' style='float:right;'>" +
                    "<span class='pTip_top cat-icon fa fa-trash-o  del-cate  color_g pull-right item_event' onclick=cate.deleteCategory(" + this.jsonResponse[i].CateID + "," + this.jsonResponse[i].CateParentID + "); style='margin-right: 4px;'></span>" +
                    "<span class='pTip_top cat-icon fa fa-plus   add-cate  color_d pull-right item_event' onclick='cate.addChildCategory(" + this.jsonResponse[i].CateID + "," + this.jsonResponse[i].CateContractID + ");'></span>" +
                    "<span class='pTip_top cat-icon fa fa-file-text-o  up-cate  color_a pull-right item_event' onclick='cate.viewCategoryDetail(" + this.jsonResponse[i].CateID + ");' ></span>" +
                    "</span>" +
                    "</span>" +
                    "<ul class='child_cate' id='child_cate_" + this.jsonResponse[i].CateID + "'></ul>" +
                    "</li>";
        }
        if (x_CateID == 0) {
            $(".box_category").html(this.innerHTML);
        } else {
            $("#child_cate_" + x_CateID).html(this.innerHTML).slideDown(500);
        }
        cate.hoverWhenNewLoad();
        cate.tisaToolTips();

    },
};

$(document).ready(function () {
    $("#x_txt_cate_name").keyup(function (e) {
        
        if (e.which == 8 || e.which==46)
            return false;
        if (e.which < 36 || e.which > 41) {
            cate.viewCateRewrite();
            $(this).val(base.ReplaceSpecCharSecond($(this).val()));
        }
    });

    $("#chk_visible").bootstrapSwitch();
    cate.viewTreeCate(0);
    cate.getListCategoryAsParentName(0);

    $("#x_txt_cateid_contact").keyup(function (e) {
        $(this).val(base.ReplaceSpecCharSecond($(this).val()));
    });
    

    $("#x_txt_cateid_contact").keypress(function (e) {
        if (e.which < 48 || e.which > 57) {
            $("#e_x_txt_cateid_contact").show();
            return false;
        }
        else {
            $("#e_x_txt_cateid_contact").hide();
        }
    });
});