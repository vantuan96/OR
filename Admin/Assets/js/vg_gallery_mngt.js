$(function () {
    gallery_grid($('.freewall_item'));

    $('#status').change(function (e) {
        if ($(this).val() == approvedStatusId) {
            $('#onsite').prop('disabled', false);
        }
        else {
            $('#onsite').prop('disabled', true);
        }
    });

    $('#onsite').change(function (e) {
        if ($(this).is(':checked')) {
            $('#status').prop('disabled', true);
        }
        else {
            $('#status').prop('disabled', false);
        }
    });

    $('#frmCreateGallery').submit(function (e) {
        e.preventDefault();
        ShowOverlay(true);
        $.post(createUpdateGalleryContentUrl, $(this).serialize(), function (response) {
            console.log(response);
            if (response.ID == 1) {
                parent.ReloadWithMasterDB();
            }
            else {
                ShowNotify('error', response.Message);
                HideOverlay();
            }
        });
    });

    $('#frmUpdateGalleryStatus').submit(function (e) {
        e.preventDefault();
        ShowOverlay(true);
        $.post(updateGalleryStatusUrl, $(this).serialize(), function (response) {
            console.log(response);
            if (response.ID == 1) {
                parent.ReloadWithMasterDB();
            }
            else {
                ShowNotify('error', response.Message);
                HideOverlay();
            }
        });
    });

    if ($("div#dropzone").length > 0) {
        var dropzone = $("div#dropzone").dropzone({
            url: urlUploadedFile,
            maxFiles: 20,
            acceptedFiles: allowUploadImageExt,
            processing: function () { ShowOverlay(true); },
            success: function (file) {
                var response = JSON.parse(file.xhr.response);
                var path = photoDomain + response.Message;

                $.post(createGalleryImagesUrl, { id: galleryId, imageUrl: response.Message }, function (response) {
                    if (response.ID > 0) {
                        var template = $('#galleryImageTemplate').html();
                        var imgItem = template.replace(/{imageSrc}/g, path)
                            .replace(/{imageId}/g, response.ID)
                            .replace(/{galleryId}/g, galleryId);

                        $('#previewImageBox').append(imgItem);
                        gallery_grid($('#previewImageBox .freewall_item').last());

                        ShowNotify('success', response.Message);
                    }
                    else {
                        ShowNotify('error', response.Message);
                    }
                });
            },
            error: function (file) {
                Dropzone.forElement("div#dropzone").removeAllFiles(true);
                HideOverlay();
            },
            queuecomplete: function () {
                Dropzone.forElement("div#dropzone").removeAllFiles(true);
                HideOverlay();
            }
        });
    }

    $(document).on('click', '[data-toggle="lightbox"]', function (event) {
        event.preventDefault();
        $(this).ekkoLightbox();
    });

    $('#previewImageBox').sortable({ helper: 'clone' });

    $(".toggle-btn").on('click', function () {
        $(this).parent().next().slideToggle();
        $(this).toggleClass("fa-chevron-up");
        $(this).parent().toggleClass("no-border");
    });

    $("#btnSaveSort").on('click', function (e) {
        UpdateItemOrder();
        return false;
    });
});

function gallery_grid(selector) {
    $(selector).on('mouseenter', function () {
        if (!$(this).hasClass('.item_selected')) {
            $(this).children('.item_overlay').addClass('show_overlay');
        }
    }).on('mouseleave', function () {
        if (!$(this).hasClass('.item_selected')) {
            $(this).children('.item_overlay').removeClass('show_overlay');
        }
    });
}

function DeleteGallery(id) {
    bootbox.confirm({
        title: layout_lang.Shared_PopupConfirmTitle,
        message: msg_lang.GalleryMngt_GalleryDetail_DeleteGalleryConfirmText,
        callback: function (confirm) {
            if (confirm) {
                ShowOverlay(true);
                $.post(deleteGalleryUrl, { id: id }, function (result) {
                    if (result.ID == 1) {
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

function DeleteGalleryImage(imageId) {
    bootbox.confirm({
        title: layout_lang.Shared_PopupConfirmTitle,
        message: msg_lang.GalleryMngt_GalleryDetail_DeleteGalleryImageConfirmText,
        callback: function (confirm) {
            if (confirm) {
                ShowOverlay(true);
                $.post(deleteGalleryImageUrl, { galleryId: galleryId, imageId: imageId }, function (result) {
                    if (result.ID == 1) {
                        $('.image_' + imageId).remove();
                        ShowNotify('success', result.Message);
                    }
                    else {
                        ShowNotify('error', result.Message);
                    }

                    HideOverlay();
                });
            }
        }
    });
}

function bindVideoHtml(response) {   
    //response = {
    //    ImageUrl: "http://10.220.44.44:8123/Images/Upload/6eabe722-0dcc-4e46-b2fa-d9ba00ebde0d.jpg",
    //    VideoUrl: "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
    //    ImageId: 1,
    //    GalleryId: 1
    //};
    var template = $('#galleryVideoTemplate').html();
    var imgItem = template.replace(/{imageSrc}/g, response.ImageUrl)
        .replace(/{videoSrc}/g, response.VideoUrl)
        .replace(/{imageId}/g, response.ImageId)
        .replace(/{galleryId}/g, response.GalleryId);

    $('#previewImageBox').append(imgItem);
    gallery_grid($('#previewImageBox .freewall_item').last());

    $("#iframePopup").modal('hide');
}

function UpdateItemOrder() {
    var items = $("#previewImageBox .freewall_item");
    if (items.length > 0) {
        bootbox.confirm({
            title: layout_lang.Shared_PopupConfirmTitle,
            message: layout_lang.GalleryMngt_Image_UpdateSortConfirmText,
            callback: function (confirm) {
                if (confirm) {
                    var items = $("#previewImageBox .freewall_item");
                    var ids = "";
                    items.each(function (i) {
                        ids += $(this).data("media") + ",";
                    });

                    $("#imageIds").val(ids);

                    $("#frmSaveSortOrder").submit();
                }
            }
        });
    }
    else {
        bootbox.alert({
            title: layout_lang.Shared_PopupConfirmTitle,
            message: layout_lang.GalleryMngt_Image_NoImageInList            
        });
    }
}

function SaveSortOrderResult(data) {
    if (data.ID == '1') {
        ShowNotify('success', data.Message);
    }
    else {
        ShowNotify('error', data.Message);
    }
    HideOverlay(0);
}