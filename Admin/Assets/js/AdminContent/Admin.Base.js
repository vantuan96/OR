
var base = {
    disableETKey: function ()
    {
       
    },
    paging: function (pageNumber,pageSize,totalRows,href) {

            var sb ="";
            var totalpagenumber = totalRows % pageSize == 0 ? (totalRows / pageSize) : parseInt(totalRows / pageSize) + 1;
            if (totalpagenumber > 1)
            {

                sb += "<div>" +
                       "<div class='col-md-5 clearfix'>" +
                       "<div class='dataTables_info'>Hiển thị từ " + (parseInt(parseInt(pageNumber - 1) * pageSize) + 1) + " đến " + ((parseInt(pageNumber * pageSize) > totalRows) ? totalRows : parseInt(pageNumber * pageSize) )+ " trong " + totalRows + " bản ghi</div>" +
                       "</div>" +
                       "<div class='col-md-7 clearfix'>" +
                       "<div class='dataTables_paginate paging_simple_numbers'>" +
                       "<ul class='pagination pagination-sm'>";
                if (pageNumber != 1) {
                    sb += "<li class='paginate_button previous'>" +
                            "<a onclick=question.pagingQuestion(" + (pageNumber - 1) + "," + pageSize + ",\'" + href + "\'); >Trang trước</a>" +
                         "</li>";
                }
                else {
                    sb += "<li class='paginate_button previous disabled'>" +
                            "<a >Trang trước</a>" +
                         "</li>";
                }
                var pagestart = 0, pageend = 0;
                pagestart = pageNumber - 3;
                pagestart = pagestart < 1 ? 1 : pagestart;
                pageend = pagestart + 3 >= totalpagenumber ? totalpagenumber : pagestart + 3;
                for (; pagestart <= pageend; pagestart++)
                {
                    if (pagestart != pageNumber)
                    {
                        sb += "<li class='paginate_button'><a onclick=question.pagingQuestion(" + pagestart + "," + pageSize + ",\'" + href + "\'); >" + pagestart + "</a></li>";
                    }
                    else
                    {
                        sb += "<li class='paginate_button active'><a >" + pagestart + "</a></li>";
                    }
                }
                if (pageNumber != totalpagenumber)
                {
                    sb += "<li class='paginate_button next '>" +
                                                "<a onclick=question.pagingQuestion(" + parseInt(pageNumber+1) + "," + pageSize + ",\'" + href + "\'); >Trang sau</a>" +
                           "</li>";
                }
                else {
                    sb += "<li class='paginate_button next  disabled'>" +
                            "<a >Trang sau</a>" +
                         "</li>";
                }
                sb +=   "</ul>"  +
                        "</div>" +
                        "</div>" +
                        "</div>";
                    
            }

            return sb;
    },
    loading: function () {
        iosOverlay({
            text: "Đang tải...",
            icon: "fa fa-spinner fa-spin",
            shadow: true
        });
    },
    scrollTop: function ()
    {
        $("html,body").animate({ scrollTop: 0 }, 500);
    },
    scrollToElement: function (top) {
        $("html,body").animate({ scrollTop: top }, 500);
    },
    showPopup: function (el)
    {
        //$(window).bind("click", function () { $(".popup-editable").hide();});
        $(el).parent().find(".popup-editable").show().bind("click", function () { return false; });
        $(el).click(function () { return false; });
        //$(document).click(function () { base.hidePopup(); });
    },
    hidePopup: function ()
    {
        $(".popup-editable").hide();
    },
    localLoading: function (el)
    {
        $(el).html("");
        $(el).html("<span class='ion-loading-a'></span>");
    },
    replaceVN_To_English: function (s) {
        var TextToFind = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
        var TextToReplace = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
        for (var index = 0; index < s.length; index++) {
            var index2 = TextToFind.indexOf(s[index]);
            if (index2 != -1) {
                s = s.replace(s[index], TextToReplace[index2]);
            }
        }
        return s;
    },
    convert_Rewrite: function (s) {
        s = s.replace(/\s/g, "-");
        return s;
    },
    ReplaceSpecChar: function (values) {
        var TextToFind = "~@#$%^&*;\\|)(,:<>{}+-_.`!=?";
        var value = "";
        for (var index = 0; index < values.length; index++) {
            if (TextToFind.indexOf(values[index]) == -1) {
                value += values[index];
            }
        }
        return value;
    },
    //============================================
    //một số kí tự đặc biệt ko replace
    //============================================
    ReplaceSpecCharSecond: function (values) {
        var TextToFind = "~@#$%^*;/\\|)(:<>{}+`!=?";
        var value = "";
        for (var index = 0; index < values.length; index++) {
            if (TextToFind.indexOf(values[index]) == -1) {
                value += values[index];
            }
        }
        return value;
    }
    
};



