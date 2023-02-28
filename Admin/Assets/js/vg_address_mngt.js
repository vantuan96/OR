$(function () {
    $('#CityId').change(function (e) {
        var id = $(this).val();
        var options = '<option></option>';
        if ($('#Id').val() == '0') refreshMap();

        if (id == '') {
            $('#DistrictId').html(options).select2('val', '');
            return;
        }

        ShowOverlay(true);
        $.post(getDistrictUrl, { cityId: id }, function (list) {
            for (var i = 0 ; i < list.length; i++) {
                options += '<option value="' + list[i].Value + '">' + list[i].Text + '</option>';
            }

            $('#DistrictId').html(options).select2('val', '');
            HideOverlay();
        });
    });

    $('#DistrictId, #FullAddress').change(function (e) {
        if ($('#Id').val() == '0') refreshMap();
    });

    $('#frmCreateAddress').submit(function (e) {
        e.preventDefault();
        ShowOverlay(true);
        $.post(createUpdateAddressUrl, $(this).serialize(), function (response) {
            if (response.ID == 1) {
                parent.ReloadWithMasterDB();
            }
            else {
                ShowNotify('error', response.Message);
                HideOverlay();
            }
        });
    });

    $('#ApprovalStatus').change(function (e) {
        if ($(this).val() == approvedStatusId)
        {
            $('#IsOnsite').prop('disabled', false);
        }
        else
        {
            $('#IsOnsite').prop('disabled', true);
        }
    });

    $('#IsOnsite').change(function (e) {
        if ($(this).is(':checked'))
        {
            $('#ApprovalStatus').prop('disabled', true);
        }
        else
        {
            $('#ApprovalStatus').prop('disabled', false);
        }
    });
})

function refreshMap() {
    var cityName = $('option:selected', $('#CityId')).text();
    var districtName = $('option:selected', $('#DistrictId')).text();
    var txtFullAddress = $('#FullAddress').val();    

    var address = 'Việt Nam';
    var zoom = 5;

    if (cityName != '') {
        if (districtName == '') {
            address = cityName + ', ' + address;
            zoom = 9;
        }
        else {
            if (txtFullAddress == undefined || txtFullAddress.length < 10) {
                address = districtName + ', ' + cityName + ', ' + address;
                zoom = 13;
            }
            else {
                address = txtFullAddress + ', ' + address;
                zoom = 15;
            }
        }
    }

    initMap(address, zoom);
}

var geocoder, map;
function initMap(address, zoom) {
    if (zoom == undefined)
        zoom = 5;

    if (address == undefined)
        address = 'Việt Nam';
    else
        address = address + ', Việt Nam';

    geocoder = new google.maps.Geocoder();
    geocoder.geocode({
        'address': address
    }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var position = results[0].geometry.location;
            $('#Latitude').val(position.lat());
            $('#Longitude').val(position.lng());

            initMapByPosition(position.lat(), position.lng(), zoom);
        }
    });
}

function initMapByPosition(latitude, longitude, zoom) {
    if (latitude == undefined) latitude = $('#Latitude').val();
    if (longitude == undefined) longitude = $('#Longitude').val();
    if (zoom == undefined) zoom = 15;

    var myOptions = {
        zoom: zoom,
        center: new google.maps.LatLng(latitude, longitude),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("addressMap"), myOptions);

    var marker = new google.maps.Marker({
        map: map,
        position: new google.maps.LatLng(latitude, longitude),
        draggable: true,
    });

    google.maps.event.addListener(marker, 'dragend', function () {
        var position = marker.getPosition();
        $('#Latitude').val(position.lat);
        $('#Longitude').val(position.lng);
    });
}

function DeleteAddress(id) {
    bootbox.confirm({
        title: layout_lang.Shared_PopupConfirmTitle,
        message: msg_lang.AddressMngt_DeleteConfirmText,
        callback: function (confirm) {
            if (confirm) {
                ShowOverlay(true);
                $.post(deleteAddressUrl, { id: id }, function (result) {
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

function UpdateFullAddress(addressId, lang, name, fullAddress, contentId)
{
    $("#modal_title").html(layout_lang.AddressMngt_AddressDetail_UpdateFullAddressPromptText);
    $("#create_update_address_content #hidden_addressId").val(addressId);
    $("#create_update_address_content #hidden_lang").val(lang);
    $("#create_update_address_content #hidden_contentId").val(contentId);

    $("#create_update_address_content #UpdateName").val(name);
    $("#create_update_address_content #UpdateFullAddress").val(fullAddress);
    $("#create_update_address_content").modal('show');

    //bootbox.prompt({
    //    title: layout_lang.AddressMngt_AddressDetail_UpdateFullAddressPromptText,
    //    value: fullAddress,
    //    inputType: 'text',
    //    callback: function (tag) {
    //        if (tag != null) {
    //            tag = tag.trim();
    //            if (tag.length > 0) {
    //                ShowOverlay(true);
    //                $.ajax({
    //                    url: createUpdateAddressContentUrl,
    //                    type: "POST",
    //                    data: {
    //                        addressId: addressId,
    //                        lang: lang,
    //                        fullAddress: tag,
    //                        contentId: contentId
    //                    },
    //                    dataType: 'json',
    //                    async: false,
    //                    cache: false,
    //                    success: function (result) {
    //                        if (result.ID == 1) {
    //                            ReloadWithMasterDB();
    //                        } else {
    //                            ShowNotify('error', result.Message);
    //                            HideOverlay();
    //                        }
    //                    },
    //                    error: function () {
    //                        ShowNotify('error', result.Message);
    //                        HideOverlay();
    //                    }
    //                });
    //            }
    //            else {
    //                ShowNotify('error', msg_lang.Shared_ModelState_InValid);
    //            }
    //        }
    //    }
    //});
}

function UpdateAddressContent() {
    var addressId = $("#create_update_address_content #hidden_addressId").val();
    var lang = $("#create_update_address_content #hidden_lang").val();
    var contentId = $("#create_update_address_content #hidden_contentId").val();

    var name = $("#create_update_address_content #UpdateName").val();
    var fullAddress = $("#create_update_address_content #UpdateFullAddress").val();

    if (name.length > 0 && $("#create_update_address_content").valid()) {
        ShowOverlay(true);
        $.ajax({
            url: createUpdateAddressContentUrl,
            type: "POST",
            data: {
                addressId: addressId,
                lang: lang,
                fullAddress: fullAddress,
                name: name,
                contentId: contentId
            },
            dataType: 'json',
            async: false,
            cache: false,
            success: function (result) {
                if (result.ID == 1) {
                    ReloadWithMasterDB();
                } else {
                    ShowNotify('error', result.Message);
                    HideOverlay();
                }
            },
            error: function () {
                ShowNotify('error', result.Message);
                HideOverlay();
            }
        });
    }
    else {
        ShowNotify('error', msg_lang.Shared_ModelState_InValid);
    }

    return false;
}