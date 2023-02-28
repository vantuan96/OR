$(function () {
    if ($('#Description').length > 0) {
        var editor = CKEDITOR.replace('Description', {
            extraPlugins: 'image2,vgslider',
            uploadUrl: urlUploadedFileFromCKEditor,
            filebrowserUploadUrl: urlUploadedFileFromCKEditor,
            filebrowserImageUploadUrl: urlUploadedFileFromCKEditor
        });
    }

    if ($('#Body').length > 0) {
        var editor = CKEDITOR.replace('Body', {
            extraPlugins: 'image2,vgslider',
            uploadUrl: urlUploadedFileFromCKEditor,
            filebrowserUploadUrl: urlUploadedFileFromCKEditor,
            filebrowserImageUploadUrl: urlUploadedFileFromCKEditor
        });
    }

    if ($("div#dropzone").length > 0) {
        var dropzone = $("div#dropzone").dropzone({
            url: urlUploadedFile,
            dictDefaultMessage:layout_lang.Shared_Upload_Image,
            thumbnailWidth: 98,
            thumbnailHeight: 98,
            maxFiles: 1,
            acceptedFiles: allowUploadImageExt,
            thumbnailMethod: 'contain',
            success: function (file) {
                var response = JSON.parse(file.xhr.response);
                var path = photoDomain + response.Message;
                $("#product_Image").attr("src", path);
                $("#ImageUrl").val(response.Message).trigger("keyup");

                Dropzone.forElement("#dropzone").removeAllFiles(true);
            },
            error: function (file) {
                Dropzone.forElement("#dropzone").removeAllFiles(true);
            },
        });
    }

    if ($('.ckbDelete').length > 0)
    {
        $('.ckbDelete').change(function () {
            var checked = $(this).is(':checked');
            $('.hiddenDelete', $(this).parent().parent()).val(checked == false);

            if (checked)
            {
                $('.inputvalue', $(this).parent().parent()).prop('disabled', false);
            }
            else
            {
                $('.inputvalue', $(this).parent().parent()).prop('disabled', true);
            }
        });
    }

    if ($(".btn-copy-attr").length > 0) {
        $(".btn-copy-attr").on("click", function (e) {
            var clsGroup = $(this).data("group");
            var copiedValue = $("." + clsGroup).first().val();
            $("." + clsGroup + ":enabled").val(copiedValue);
        })
    }
});

function DeleteProduct(id) {
    bootbox.confirm({
        title: layout_lang.Shared_PopupConfirmTitle,
        message: msg_lang.ProductMngt_ProductDetail_DeleteProductConfirmText,
        callback: function (confirm) {
            if (confirm) {
                ShowOverlay(true);
                $.post(deleteProductUrl, { id: id }, function (result) {
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