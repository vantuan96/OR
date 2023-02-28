$(function () {
    $('.div_rel').click(function (e) {
        $('.div_rel').next().removeClass('popup-active');
        $('.div_rel').parent().removeClass('menu-active');

        $(this).next().toggleClass('popup-active').animate({ top: 50 }, { duration: 200, easing: 'easeOutBounce' });
        $(this).parent().toggleClass('menu-active');
    });
    $(document).click(function (e) {
        if ($('.notification-popup-main').length > 0) {
            if (!$.contains($('.notification-popup-main').get(0), e.target) && !$.contains($('.div_notification').get(0), e.target)) {
                $('.notification-popup-main').removeClass('popup-active');
                $('.div_rel').parent().removeClass('menu-active');
            }
        }
    });
    notiEvent.initNotification();
    notiEvent.bindMarkAsReadNotification();
});

var notiEvent = {
    initNotification: function() {
        $('#head-notification-lastest').click(function() {
            $('.adr-notification-lastest .notification-ajax-load').show();
            $.ajax({
                url: "/get-notification-lastest",
                type: "GET",
                cache: false
            }).done(function (res) {
                $('.adr-notification-lastest-content').html(res);
                $('.adr-notification-lastest .notification-ajax-load').hide();
                //bindEvent.popupViewer();
                $('#header-notify-content [data-target="#iframePopup"]').click(function (e) {
                    loadIframePopup($(this));
                });

                notiEvent.bindMarkAsReadNotification();

                if($('.list-popup-notifications').length) {
                    var countNotify = parseInt($('.list-popup-notifications').attr('data-count-notify'));
                    $('.notification-new-counter').html(notiEvent.convertNumberMaxPlus(countNotify));
                }
            });
        });
    },
    bindMarkAsReadNotification: function() {
        if ($('.mark-as-read-all').length > -1) {
            $('.mark-as-read-all').click(function (e) {
                e.preventDefault();
                var currentDate = $(this).data('currentdate');
                if ($('.notification-item.notify-new-item').length > 0 || $('.notification-timeline-item .notify-new-item').length > 0
                    || $('.notification-timeline-item.new_notic').length > 0 || $('.notification-timeline-item .new_notic').length > 0) {
                    $.ajax({
                        url: "/mark-as-read-all",
                        data: { strLastNotifiDate: currentDate },
                        type: "POST"
                    }).done(function (res) {
                        if (res.ID > 0) {
                            $('.notification-new-counter').html('');
                            $('.notification-item').removeClass('notify-new-item');
                            $('.notification-timeline-item').removeClass('new_notic');
                            $('.notification-timeline-item .notify-new-item').removeClass('notify-new-item');
                            $('.list-popup-notifications').attr('data-count-notify', 0);
                        } else {
                            ShowNotify(res.ID > 0 ? "success" : "error", res.Message, 4000);
                        }
                    });
                }
            });
        }
    },
    convertNumberMaxPlus: function (number) {
        var numberConvert = parseInt(number);
        return numberConvert > 0 ? (numberConvert > 99 ? "99+" : numberConvert) : '';
    }
};


