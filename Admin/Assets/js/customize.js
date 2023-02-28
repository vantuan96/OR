$(document).ready(function () {

    $(".nav_trigger").find(".sub_panel").css({ "height": "0" });

    if (getCookie("adrshowmenu") === "hide" || $(document).width() <= 1136) {
        if ($(document).width() <= 1136) {
            hideMenu(false);
        }
        else {
            hideMenu();
        }
        
    }

    //Click toggle_menu
    $(".slidebar-toggle_menu").click(function () {

        if ($("html").hasClass("show_menu_tree") == true) {
            hideMenu();
        }
        else {
            showMenu();
        }
    });

    //Click menu
    $(".nav_trigger > a").click(function () {
        if ($(this).parents(".nav_trigger").hasClass("open_tree") == true) {
            if ($('.show_menu_tree').length > 0) {
                $(".nav_trigger").removeClass("open_tree");
                $(this).parents(".nav_trigger").find(".sub_panel").css({ "height": "0", "transition": "height 0.3s", "-moz-transition": "height 0.3s", "-webkit-transition": "height 0.3s", "-o-transition": "height 0.3s" });
            }
            else {
                $(".nav_trigger").find(".sub_panel").css({ "height": "100%", "transition": "height 0.3s", "-moz-transition": "height 0.3s", "-webkit-transition": "height 0.3s", "-o-transition": "height 0.3s" });
            }
           
        }
        else {
            //$(".nav_trigger").removeClass("open_tree");
            //$(this).addClass("open_tree");
            //$(".nav_trigger").find(".sub_panel").css({ "height": "0", "transition": "height 0.3s", "-moz-transition": "height 0.3s", "-webkit-transition": "height 0.3s", "-o-transition": "height 0.3s" });
            
            if ($('.show_menu_tree').length > 0) {
                $(".nav_trigger").removeClass("open_tree");
                $(this).parents(".nav_trigger").addClass("open_tree");
                $(".nav_trigger").find(".sub_panel").css({ "height": "0", "transition": "height 0.3s", "-moz-transition": "height 0.3s", "-webkit-transition": "height 0.3s", "-o-transition": "height 0.3s" });
                var a = $(this).parents(".nav_trigger").find(".side_inner").height();
                $(this).parents(".nav_trigger").find(".sub_panel").css({ "height": a, "transition": "height 0.3s", "-moz-transition": "height 0.3s", "-webkit-transition": "height 0.3s", "-o-transition": "height 0.3s" });
            }
            else {
                $(".nav_trigger").find(".sub_panel").css({ "height": "100%", "transition": "height 0.3s", "-moz-transition": "height 0.3s", "-webkit-transition": "height 0.3s", "-o-transition": "height 0.3s" });
            }
        }
        
    });

    //Resize for menu
    $(window).resize(function () {

        if ($(document).width() <= 1136) {
            hideMenu(false);
        }
        else if (getCookie("adrshowmenu") !== "hide") {
            showMenu();
        }
    });

    //Set active menu for menu_tree
   
    $('li.nav_trigger').find("a").each(function () {
        if ($(this).attr("href") != null) {
            var page = $(this).attr("href");
            var address = window.location.pathname;
            if (page.toLowerCase() == address.toLowerCase()) {
                $(this).parent().addClass("active");
                $(this).parents(".nav_trigger").addClass("open_tree");
                var a = $(this).parents(".nav_trigger").find(".side_inner").height();
                $(".nav_trigger").find(".sub_panel").css({ "height": "0" });
                $(this).parents(".nav_trigger").find(".sub_panel").css({ "height": a });
                if ($('.show_menu_tree').length > 0) {
                    $(this).parents(".nav_trigger").find(".sub_panel").css({ "height": a, "transition": "0s", "-moz-transition": "0s", "-webkit-transition": "0s", "-o-transition": "0s" });
                }
                else {
                    $(".nav_trigger").find(".sub_panel").css({ "height": "100%", "transition": "height 0.3s", "-moz-transition": "height 0.3s", "-webkit-transition": "height 0.3s", "-o-transition": "height 0.3s" });
                }
            }
        }
        
    });

    /*
    //4 Nút chữ nhật lớn trang dashboard
    $(".btn_deal_dashboard").click(function () {
        $(".tag_total").removeClass('active');
        $(this).addClass('active');
        $(".div_box_dashboard").hide();
        $(".div_deal_dashboard").fadeIn("normal");
    });

    $(".btn_merchant_dashboard").click(function () {
        $(".tag_total").removeClass('active');
        $(this).addClass('active');
        $(".div_box_dashboard").hide();
        $(".div_merchant_dashboard").fadeIn("normal");
    });

    $(".btn_comment_dashboard").click(function () {
        $(".tag_total").removeClass('active');
        $(this).addClass('active');
        $(".div_box_dashboard").hide();
        $(".div_comment_dashboard").fadeIn("normal");
    });

    $(".btn_accmerchant_dashboard").click(function () {
        $(".tag_total").removeClass('active');
        $(this).addClass('active');
        $(".div_box_dashboard").hide();
        $(".div_accmerchant_dashboard").fadeIn("normal");
    });

    //Nút control danh sách Deal    
    var timer;
    $('.user_menu_deal').on({
        mouseenter: function () {
            var self = this;
            clearTimeout(timer);
            timer = setTimeout(function() {
                $('.user_menu_deal').removeClass('open');
                $(self).addClass('open');
            }, 0);
        },
        mouseleave: function () {
            var self = this;
            setTimeout(function () {
                if (!$(self).children('.dropdown-menu-right_deal').is(":hover") && !$(self).is(":hover")) {
                    $(self).removeClass('open');
                }
            }, 500);
        }
    });*/

    //$('.grid').masonry({
    //    // options
    //    itemSelector: '.grid-item',
    //    columnWidth: 210
    //});

});



function hideMenu(isSetcookie)
{
    //$("html").addClass("close_menu_tree");
    //$("html").removeClass("show_menu_tree");
    //$(".nav_trigger").find(".sub_panel").css({ "height": "100%", "transition": "height 0.3s", "-moz-transition": "height 0.3s", "-webkit-transition": "height 0.3s", "-o-transition": "height 0.3s" });
    if (isSetcookie != false) {
        setCookie("adrshowmenu", "hide");
    }
    
    $("html").removeClass("show_menu_tree");
    $("html").addClass("close_menu_tree");
    $(".nav_trigger").find(".sub_panel").css({
        "height": "100%",
        "transition": "height 0.3s",
        "-moz-transition": "height 0.3s",
        "-webkit-transition": "height 0.3s",
        "-o-transition": "height 0.3s"
    });
}

function showMenu()
{
    //$("html").addClass("show_menu_tree");
    //$("html").removeClass("close_menu_tree");
    setCookie("adrshowmenu", "show");
    $(".nav_trigger").find(".sub_panel").css({ "height": "0", "transition": "height 0s", "-moz-transition": "height 0s", "-webkit-transition": "height 0s", "-o-transition": "height 0s" });
    $("html").addClass("show_menu_tree");
    $("html").removeClass("close_menu_tree");
    $('li.open_tree').each(function () {
        var a = $(this).find(".side_inner").height();
        $(this).find(".sub_panel").css({ "height": a, "transition": "height 0s", "-moz-transition": "height 0s", "-webkit-transition": "height 0s", "-o-transition": "height 0s" });
    });
    
}

function showMenu_nonanimate() {
    $("html").addClass("show_menu_tree");
    $("html").removeClass("close_menu_tree");
}

function setCookie(cname, cvalue) {//,exdays) {
    var exdays = 30;
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
    }
    return "";
}
function switchSite(controlid) {
    //Hide all link
    $("a").each(function () {
        var aLink = $(this);
        var id = aLink.attr("id");
        if (id != controlid) {
            aLink.attr("style", "font-weight:bold;opacity: 0.5;");
            aLink.attr("onclick", "return false;")
            aLink.attr("href", "javascript:return false;")
        }
    });
    var itemA = $("#" + controlid + "");
    var id = itemA.attr("data-id");
    $("#" + controlid + " > i").removeClass();
    $("#" + controlid + " > i").addClass("fa fa-check-square");
    var postData = new Array();
    postData.push({ name: 'siteId', value: id });
    ShowOverlay(true);
    commonUtils.postAjaxWithToken(switch_site_url, postData, function (ret) {
        console.log(ret);
        if (ret.IsSuccess == 1) {
            ShowNotify('success', ret.Message, 2000);
            setTimeout(function () {
                //location.reload();
                window.location.replace(default_url);
            }, 2000);
        }
        else {
            ShowNotify(ret.IsSuccess == 1 ? 'success' : 'error', ret.Message, 3000);
        }
        HideOverlay();
    });
};
function checkIsLogin(placeCheck ="AnyWhere") {
    var returnValue = false;
    var postData = new Array();
    postData.push({ name: 'placeCheck', value: placeCheck });
    commonUtils.postAjaxWithTokenWaitResponse(checklogin_url, postData, function (ret) {
        //console.log(ret);
        if (ret.IsSuccess == 1) {
            //Login
            returnValue= true;
        }
        else {
            //Logout. Redirect logout
            window.location.replace(ret.Message + "?refurl=" + window.location.href);
        }
    });
    return returnValue;
};
function editEmailOrPhone(control, type) {
    var currentValue = $(control).val();
    var currentSelectEleId;
    var currentUserSelected;
    if (type == "email") {
        currentSelectEleId = $(control).parents('td').eq(0).prev().find("select").attr("id");
        currentUserSelected = $(control).parents('td').eq(0).prev().find("select").val();
    } else if (type == "phone") {
        currentSelectEleId = $(control).parents('td').eq(0).prev().prev().find("select").attr("id");
        currentUserSelected = $(control).parents('td').eq(0).prev().prev().find("select").val();
    }
    //Duyet xem co User nao dam nhiem 2 vi tri ko
    $('#bodyEkip select').each(function () {
        //console.log($(this).attr("id"));
        if (currentSelectEleId != $(this).attr("id") && currentUserSelected == $(this).val()) {
            //console.log($(this).val());
            if (type == "email") {
                $(this).parents('td').eq(0).next().find("input").val(currentValue);
            } else if (type == "phone") {
                $(this).parents('td').eq(0).next().next().find("input").val(currentValue);
            }

        }
    });
};