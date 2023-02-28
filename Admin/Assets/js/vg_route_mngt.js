var txtFrom = '', txtTo = '';
var directionsDisplay, directionsService ;

$(function () {
    $('#TextFrom').change(function (e) {
        txtFrom = $(this).val();
        changeAddress();
    });

    $('#TextTo').change(function (e) {
        txtTo = $(this).val();
        changeAddress();
    });
});

function changeAddress() {
    if (txtFrom.length > 0 && txtTo.length > 0)
    {
        displayRoute(txtFrom, txtTo, directionsService, directionsDisplay);
    }
}

function initMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: mapZoom,
        center: mapCenter
    });

    directionsService = new google.maps.DirectionsService;
    directionsDisplay = new google.maps.DirectionsRenderer({
        draggable: true,
        map: map,
        panel: document.getElementById('right-panel')
    });

    directionsDisplay.addListener('directions_changed', function () {
        var result = directionsDisplay.getDirections();
        var frompos = result.routes[0].legs[0].start_location;
        var topos = result.routes[0].legs[0].end_location;

        if (typeof frompos.lat === 'function') {
            $('#FromLatitude').val(frompos.lat());
            $('#FromLongitude').val(frompos.lng());
        }

        if (typeof topos.lat === 'function') {
            $('#ToLatitude').val(topos.lat());
            $('#ToLongitude').val(topos.lng());
        }
    });

    map.addListener('center_changed', function () {
        var centerpos = map.getCenter();
        $('#CenterPosLatitude').val(centerpos.lat());
        $('#CenterPosLongitude').val(centerpos.lng());
    });

    map.addListener('zoom_changed', function () {
        var zoom = map.getZoom();
        $('#ZoomLevel').val(zoom);
    });

    if (mapFrom.length > 0 && mapTo.length > 0) {
        displayRoute(mapFrom, mapTo, directionsService, directionsDisplay);
    }
}

function displayRoute(origin, destination, service, display) {
    service.route({
        origin: origin,
        destination: destination,
        travelMode: 'DRIVING'
    }, function (response, status) {
        if (status === 'OK') {
            display.setDirections(response);
        } else {
            ShowNotify('error', msg_lang.RouteMngt_CanNotFindDirection);
            console.log('Could not display directions due to: ' + status);
        }
    });
}
