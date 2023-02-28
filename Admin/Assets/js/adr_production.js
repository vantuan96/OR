
$(function() {
    jQuery.exists = function(selector) { return ($(selector).length > 0); };
    // gallery grid
    adr_freewall.gallery_grid();
    // lightbox
    adr_image_lightbox.init();
});

deal_photos_page = {
    init: function() {
        deal_photos_page.LoadCheckedDealPhotos();
        deal_photos_page.SetDefaultPhotoInGroup();
        deal_photos_page.LoadPhotoIDsDefault();
        var wrap_height_current = 250;
        $('.deal-photos-page .freewall_item .item_select').on('click', function(e) {
            e.preventDefault();
            var $elem = $(this).closest('.freewall_item');
            if ($elem.hasClass('item_selected') && $elem.hasClass('deal-photo-default')) {
                $elem.removeClass('deal-photo-default');
                $(this).closest('.group-photo-type').find('.input-default-id').val('');
            }
            $elem.toggleClass('item_selected');
            $(this).children('.fa').toggleClass('fa-square-o fa-check-square-o');

            if ($('.panel-slide-error').length > 0) {
                $('.slide-wrapper-note').html('<p class="sort-note"><i>Di chuyển ảnh để sắp xếp</i></p>');
            }

            deal_photos_page.LoadPhotoIDsChecked();
            deal_photos_page.LoadCheckedDealPhotos();
            deal_photos_page.LoadPhotoIDsDefault();
            $('.ad-image-wrapper').css('height', wrap_height_current);

        });

        $('#resize-box-deal-photo').click(function() {
            $(window).trigger("resize");
            var showSize = $(this).attr('data-show');
            if (showSize != 'small') {
                $(this).find('span').removeClass('glyphicon-chevron-left').addClass('glyphicon-chevron-right');
                $(this).attr('data-show', 'small');
                $('.deal-photos-list').removeClass('col-md-8').addClass('col-md-5');
                $('.deal-photos-preview').removeClass('col-md-4').addClass('col-md-7');
                wrap_height_current = 400;
                $('.ad-image-wrapper').css('height', wrap_height_current);
            } else {
                $(this).find('span').removeClass('glyphicon-chevron-right').addClass('glyphicon-chevron-left');
                $(this).attr('data-show', 'big');
                $('.deal-photos-list').removeClass('col-md-5').addClass('col-md-8');
                $('.deal-photos-preview').removeClass('col-md-7').addClass('col-md-4');
                wrap_height_current = 250;
                $('.ad-image-wrapper').css('height', wrap_height_current);
            }
        });

        $('.set-default-photo').click(function(e) {
            e.preventDefault();
            var ids = $(this).attr('data-default');
            var typeID = ids.split('_')[0];
            var photoID = ids.split('_')[1];

            var $elemGroup = $('#photos_deal_wall_grid_' + typeID);
            $elemGroup.find('.freewall_item').removeClass('deal-photo-default');

            var $elem = $(this).closest('.freewall_item');
            if (!$elem.hasClass('item_selected')) {
                $elem.find('.item_select').trigger("click");
            }

            $(this).closest('.freewall_item').addClass('deal-photo-default');
            $elemGroup.find('.input-default-id').val(ids);
            deal_photos_page.LoadPhotoIDsDefault();
        });
    },
    SetDefaultPhotoInGroup: function() {
        $('.group-photo-type').each(function() {
            if (!$(this).hasClass('photo-slide-group')) {
                var defaultID = $(this).find('.input-default-id').val();
                if (defaultID != '') {
                    var photoID = defaultID.split('_')[1];
                    $('.deal-photo-item-' + photoID).addClass('deal-photo-default');
                }
            }
        });
    },
    LoadPhotoIDsDefault: function() {
        var ids = '';
        $('.group-photo-type').each(function() {
            if (!$(this).hasClass('photo-slide-group')) {
                var defaultID = $(this).find('.input-default-id').val();
                if (defaultID != '') {
                    var photoID = defaultID.split('_')[1];
                    ids += photoID + ',';
                }
            }
        });

        $('#deal-photo-default-id').val(ids.substr(0, ids.length - 1));
    },
    LoadPhotoIDsSort: function() {
        var dealPhotoIDs = '';
        $('#ad-deal-photos-sort li').each(function() {
            dealPhotoIDs += $(this).attr('data-id') + ',';
        });
        $('#deal-photos-ids').val(dealPhotoIDs.substr(0, dealPhotoIDs.length - 1));
    },
    LoadPhotoIDsChecked: function() {
        var dealPhotoIDs = '';
        $('.item_selected').each(function() {
            dealPhotoIDs += $(this).attr('data-id') + ',';
        });
        $('#deal-photos-ids').val(dealPhotoIDs.slice(0, -1));
    },
    LoadCheckedDealPhotos: function() {
        $('#photos-gallery-preview').show();
        var listIDs = $('#deal-photos-ids').val() != undefined ? $('#deal-photos-ids').val() : '';
        var arrDealPhotoIDs = listIDs.split(",");
        if (listIDs.length > 0) {
            var dealPhotos = '';
            for (var i = 0; i < arrDealPhotoIDs.length; i++) {
                // Set checked deal photos
                var $elemPhoto = $('.deal-photo-item-' + arrDealPhotoIDs[i]);
                $elemPhoto.addClass('item_selected');
                $elemPhoto.find('span .fa').removeClass('fa-square-o');
                $elemPhoto.find('span .fa').addClass('fa-check-square-o');

                var imgSrcItem = $elemPhoto.attr('data-image');
                var dealphotoId = $elemPhoto.attr('data-id');
                var itemphotoslide = '<li data-id="' + (dealphotoId !== undefined ? dealphotoId : "") + '">' +
                    '<a href="' + imgSrcItem + '">' +
                    '<img class="thumbs-slide" src="' + imgSrcItem + '" />' +
                    '</a>' +
                    '</li>';
                dealPhotos += itemphotoslide;
            }

            var container_gallery = $('<div id="photos-gallery-preview" class="ad-gallery"></div>');
            var ad_image_wrapper = $('<div class="ad-image-wrapper"></div>').appendTo(container_gallery);
            var ad_nav = $('<div class="ad-nav"></div>').appendTo(container_gallery);
            var ad_thumb = $('<div class="ad-thumbs"></div>').appendTo(ad_nav);
            var ad_photo_sort = $('<ul id="ad-deal-photos-sort" class="ad-thumb-list"></ul>').appendTo(ad_thumb);
            ad_photo_sort.append(dealPhotos);

            $('.ad-gallery-container').html(container_gallery);

            $("#ad-deal-photos-sort").sortable({
                update: function(event, ui) { deal_photos_page.LoadPhotoIDsSort(); }
            });

            $("#ad-deal-photos-sort").disableSelection();

            SlideGallery();
        } else {
            $('#photos-gallery-preview').hide();
        }
    }
};

deal_photos_pr = {
    init: function() {
        //adr_freewall.gallery_grid();
        deal_photos_pr.LoadDealPhotosPRChecked();
        // Photos list PR
        $('.deal-photos-pr-list .freewall_item .item_select').on('click', function(e) {
            e.preventDefault();
            $(this).closest('.freewall_item').toggleClass('item_selected');
            $(this).children('.fa').toggleClass('fa-square-o fa-check-square-o');
            deal_photos_pr.LoadPhotoPRIDsChecked();
        });
    },
    LoadPhotoPRIDsChecked: function() {
        var dealPhotoPRIDs = '';
        $('.item_selected').each(function() {
            dealPhotoPRIDs += $(this).attr('data-id') + ',';
        });
        $('#deal-photos-pr-ids').val(dealPhotoPRIDs.slice(0, -1));

        // Save changes database
    },
    LoadDealPhotosPRChecked: function() {
        var arrDealPhotoPRIDs = $('#deal-photos-pr-ids').val().split(",");
        if ($('#deal-photos-pr-ids').val().length > 0) {
            var dealPhotos = '';
            for (var i = 0; i < arrDealPhotoPRIDs.length; i++) {
                // Set checked deal photos
                var $elemPhoto = $('.deal-photo-pr-item-' + arrDealPhotoPRIDs[i]);
                $elemPhoto.addClass('item_selected');
                $elemPhoto.find('span .fa').removeClass('fa-square-o');
                $elemPhoto.find('span .fa').addClass('fa-check-square-o');
            }
        }
    }
};
// freewal grid
adr_freewall = {
    gallery_grid: function(id) {
        var idnew = id != undefined ? id : '#photos_deal_wall_grid';
        // check if all images have finished loading and then init freewall
        imagesLoaded('.freewall_item > img', function() {
            gallery_wall = new freewall(idnew);
            gallery_wall.reset({
                selector: '.freewall_item',
                animate: true,
                cellW: 160,
                cellH: 'auto',
                onResize: function() {
                    gallery_wall.fitWidth();
                }
            });
            gallery_wall.fitWidth();
            $(window).trigger("resize");

            $('.freewall_item').on('mouseenter', function() {
                if (!$(this).hasClass('.item_selected')) {
                    $(this).children('.item_overlay').addClass('show_overlay');
                }
            }).on('mouseleave', function() {
                if (!$(this).hasClass('.item_selected')) {
                    $(this).children('.item_overlay').removeClass('show_overlay');
                }
            })
        });
    }
};

adr_image_lightbox = {
    init: function() {
        if ($('.freewall_item .item_preview').length) {
            $('.freewall_item .item_preview').magnificPopup({
                type: 'image',
                gallery: {
                    enabled: true,
                    tCounter: "",
                    arrowMarkup: '<i title="%title%" class="glyphicon glyphicon-chevron-%dir% mfp-nav"></i>'
                },
                removalDelay: 500, //delay removal by X to allow out-animation
                callbacks: {
                    beforeOpen: function() {
                        // just a hack that adds mfp-anim class to markup 
                        this.st.image.markup = this.st.image.markup.replace('mfp-figure', 'mfp-figure mfp-with-anim');
                        this.st.mainClass = 'mfp-zoom-in';
                    },
                    afterClose: function() {

                    },
                    change: function (openerElement) {
                        var title = openerElement.el.context.title;
                        var href = openerElement.el.context.href;
                        var img = new Image();
                        img.onload = function () {
                            $(".mfp-title").html(layout_lang.CMS_ImageNameText + ": " + title + "<br/> " + layout_lang.CMS_ImageSizeText + ": " + this.width + "x" + this.height);
                        }
                        img.src = href;
                    }
                },
                closeOnContentClick: true,
                midClick: true // allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source.
            });
        }
    }
};


function SlideGallery() {
    $('.ad-gallery').adGallery({
        thumb_opacity: 1
    });
}