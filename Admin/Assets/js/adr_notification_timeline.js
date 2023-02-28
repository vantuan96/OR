$(function () {
    var pageIndex = 2;
    var loading = false;
    $('.lazy-loading').hide();
    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() == $(document).height()) {
            if (pageIndex <= total_pages && loading == false) {
                loading = true;
                $('.lazy-loading').show();
                $.post(_urlLazyLoadNotification, { 'page': pageIndex }, function (data) {
                    $("#notification-timeline").append(data);
                    $('.lazy-loading').hide();
                    pageIndex++;
                    loading = false;

                    // Xóa ngày giống nhau
                    var seen = '';
                    $('.day_line').each(function () {
                        var see = $(this).find('span').text();
                        if (seen.match(see)) {
                            $(this).parent().remove();
                        }
                        else {
                            seen = seen + $(this).find('span').text();
                        }
                    });

                    if ($('.Tip_mouse_on').length) {
                        $.fn.powerTip.smartPlacementLists.n = ['n', 's', 'e', 'w'];
                        $('.Tip_mouse_on').powerTip({
                            placement: 'n',
                            smartPlacement: true
                        });
                    }
                }).fail(function (xhr, ajaxOptions, thrownError) {
                    bootbox.alert(thrownError);
                    $('.lazy-loading').hide();
                    loading = false;
                });
            }
        }
    });
});