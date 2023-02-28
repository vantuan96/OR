var gmap = null;
var omarker = null;
window.unonload = function () { GUnload(); };
var mmap = null;
var result_LatLon = "";
var urlNoImage = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcRgS8M6v_6a2IeYQDQ1Qm9tDeDdvyo60xovFv7ewBhCdeS-14UvgfbNDg";
function GoogleMap(divID, w, h) {
    gmap = new google.maps.Map(document.getElementById(divID),
                               {
                                   size: new google.maps.Size(w, h, "px", "px"),
                                   center: new google.maps.LatLng(10.7774197, 106.70196529999998),
                                   zoom: 16,
                                  /* disableDefaultUI: true,*/
                                   mapTypeId: google.maps.MapTypeId.ROADMAP
                               });
    this.initMap = function () {
        var point = new google.maps.LatLng(10.7774197, 106.70196529999998);
        try {
            this.createMarker({ 'point': point, 'name': 'VinEcom Building', 'address': '70 Lê Thánh Tôn, Bến Nghé, Ho Chi Minh, Vietnam', 'phone': '(+84 4) 7308 13', 'pic': 'images/Vincom_Center_B_TPHCM_1416x2128.jpg' });
        } catch (ex) { alert(ex); };
        mmap = this;
    };
    this.createDragMarker = function (info) {
        var myHtml = "<b>" + info.name + "</b><br/>" + info.address;
        var info_window = new google.maps.InfoWindow({
            content: 'loading'
        });
        var xy = info.point.toString().replace('(', '').replace(')', '').replace(' ', '').split(',');
        if (omarker != null) {
            omarker.setMap(null);
        }
        var m = new google.maps.Marker({
            map: gmap,
            clickable: true,
            draggable: info.draggend,
            title: info.name,
            position: info.point,
            html: myHtml
        });
        google.maps.event.addListener(m, 'click', function () {
            info_window.setContent(this.html);
            info_window.open(gmap, this);
            gmap.setCenter(this.position);
            gmap.setZoom(16);
        });
        google.maps.event.addListener(m, 'dragstart', function () {
            info_window.close();
        });
        google.maps.event.addListener(m, 'dragend', function (event) {
            console.log(event);
            // Edit
            document.getElementById("address-lat").value = this.position.lat();
            document.getElementById("address-long").value = this.position.lng();
        });
        omarker = m;
    };
    this.addListPoint = function (dsCty) {
        if (gmap == null || dsCty.length < 0) return;
        var arrX = new Array();
        var arrY = new Array();
        var midPoint = this.calMidPoint(dsCty);
        gmap.setCenter(new google.maps.LatLng(midPoint[0], midPoint[1]));
        for (var i = 0; i < dsCty.length; i++) {
            var xy = dsCty[i].xy.split('_');
            var point = new google.maps.LatLng(xy[0], xy[1]);
            this.createMarker({ 'point': point, 'name': dsCty[i].name, 'address': dsCty[i].address, 'phone': dsCty[i].phone, 'pic': dsCty[i].pic });
        }
        gmap.setZoom(14);
    };
    this.calMidPoint = function (dsCty) {
        if (dsCty.length > 0) {
            var arrX = new Array();
            var arrY = new Array();
            for (var i = 0; i < dsCty.length > 0; i++) {
                var xy = dsCty[i].xy.split('_');
                arrX.push(parseFloat(xy[0]));
                arrY.push(parseFloat(xy[1]));
            }
            arrX.sort();
            arrY.sort();
            xy[0] = (arrX[0] + arrX[arrX.length - 1]) / 2;
            xy[1] = (arrY[0] + arrY[arrY.length - 1]) / 2;
            return xy;
        }
    };
    this.createMarker = function (info) {
        var encodeAdd = encodeURIComponent(info.address + ', Việt Nam');
        var popupTitle = encodeURIComponent("(" + info.name + ")");
        var linkGoogle = "https://maps.google.com/maps?q=" + encodeAdd + popupTitle +
                         "&ll=" + info.point.toString().replace('(', '').replace(')', '') + "&t=m&hl=vi&z=18";
        var pic = (info.pic == undefined || info.pic == "") ? urlNoImage : info.pic;

        var myHtml = '<div style="width:300px;"><div class="photo"><a href="' + linkGoogle + '" target="_blank"><img alt="" width="50" height="50" src="' + pic + '"></a></div>' +
            '<div class="popup_info"><a title="" class="name_branch" href="' + linkGoogle + '" target="_blank">' + info.name + '</a><p>' + info.address + '</p>';
        var strPhone = (info.phone != undefined && info.phone != "") ? ('Tel: ' + info.phone) : '';
        if (strPhone != "") myHtml += "<p>" + strPhone + "</p>";
        myHtml += '<a class="name_branch" target="_blank" href="' + linkGoogle + '" style="font-weight:bold;cursor:pointer;color:#1155CC;" jstcache="0">Chỉ đường</a>';
        myHtml += '</div></div>';

        var info_window = new google.maps.InfoWindow({
            content: 'loading'
        });

        var xy = info.point.toString().replace('(', '').replace(')', '').replace(' ', '').split(',');
        var pos = new google.maps.LatLng(xy[0], xy[1]);
        try {
            var m = new google.maps.Marker({
                map: gmap,
                title: info.name,
                position: pos
                ,html:myHtml
            });
        } catch (ex) { }


        google.maps.event.addListener(m, 'click', function () {
            info_window.setContent(this.html);
            info_window.open(gmap, this);
            var point = new google.maps.LatLng(this.position.lat(), this.position.lng());
            gmap.setCenter(point);
            gmap.setZoom(16);
        });
        omarker = m;
    };
    this.createDragMarkerHtml = function (info) {
        var encodeAdd = encodeURIComponent(info.address + ', Việt Nam');
        var popupTitle = encodeURIComponent("(" + info.name + ")");
        var linkGoogle = "https://maps.google.com/maps?q=" + encodeAdd + popupTitle +
                         "&ll=" + info.point.toString().replace('(', '').replace(')', '') + "&t=m&hl=vi&z=18";
        var pic = (info.pic == undefined || info.pic == "") ? urlNoImage : info.pic;

        var myHtml = '<div class="popup-google-map"><div class="photo"><a href="' + linkGoogle + '" target="_blank"><img alt="" width="50" height="50" src="' + pic + '"></a></div>' +
            '<div class="popup_info"><a title="" class="name_branch" href="' + linkGoogle + '" target="_blank">' + info.name + '</a><p>' + info.address + '</p>';
        var strPhone = (info.phone != undefined && info.phone != "") ? ('Tel: ' + info.phone) : '';
        if (strPhone != "") myHtml += "<p>" + strPhone + "</p>";
        myHtml += '<a class="link-back" target="_blank" href="' + linkGoogle + '" style="font-weight:bold;cursor:pointer;color:#1155CC;" jstcache="0">Chỉ đường</a>';
        myHtml += '</div></div>';

        //var myHtml = "<b>" + info.name + "</b><br/>" + info.address;
        var info_window = new google.maps.InfoWindow({
            content: 'loading'
        });
        var xy = info.point.toString().replace('(', '').replace(')', '').replace(' ', '').split(',');
        if (omarker != null) {
            omarker.setMap(null);
        }
        var m = new google.maps.Marker({
            map: gmap,
            clickable: true,
            draggable: info.draggend,
            title: info.name,
            position: info.point,
            html: myHtml
        });
        google.maps.event.addListener(m, 'click', function () {
            info_window.setContent(this.html);
            info_window.open(gmap, this);
            gmap.setCenter(this.position);
            gmap.setZoom(16);
        });
        google.maps.event.addListener(m, 'dragstart', function () {
            info_window.close();
        });
        google.maps.event.addListener(m, 'dragend', function (event) {
            console.log(event);
            document.getElementById("address-lat").value = this.position.lat();
            document.getElementById("address-long").value = this.position.lng();
        });
        omarker = m;
    };


    this.setLatLon = function (value) {
        var geocoder = new google.maps.Geocoder();
        if (geocoder && value != "") {
            geocoder.geocode({ 'address': value }, function (results, status) {
                if (status != google.maps.GeocoderStatus.OK) {
                      //alert("Không tim được địa chỉ này");
                }
                else {
                  
                    if (omarker != null) {
                        omarker.setMap(null);
                    }
                    var point = new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng());
                    gmap.setCenter(point);
                    gmap.setZoom(16);
                    try {
                        var marker = new google.maps.Marker({
                            map: gmap,
                            position: results[0].geometry.location
                        });

                        omarker = marker;
                        mmap.createDragMarker({ 'point': results[0].geometry.location, 'name': 'Định vị công ty', 'address': value, 'phone': '' });
                    } catch (ex) { }
                }
            });
        }
    };
    this.getLatLon = function (value) {
      
        var geocoder = new google.maps.Geocoder();
        if (geocoder && value != "") {
            geocoder.geocode({ 'address': value }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                  
                    var point = new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng());
                    gmap.setCenter(point);
                    var marker = new google.maps.Marker({
                        map: gmap,
                        position: results[0].geometry.location
                    });
                    omarker = marker;
                    result_LatLon = results[0].geometry.location.toString().replace('(', '').replace(')', '');
                } else {
                    alert("Geocode was not successful for the following reason: " + status);
                }
            });
        }
        this.resultLatLon(result_LatLon);
    };
    this.resultLatLon = function (point) {
        return point;
    };
    this.getPositionIcon = function () {
      
         return omarker != null ? (omarker.position != null ? { "x": omarker.position.lat(), "y": omarker.position.lng() } : null) : null;
    };
    this.initMapByLatLon = function (info) {
        var xy = info.xy.split('_');
        var point = new google.maps.LatLng(parseFloat(xy[0]),parseFloat(xy[1]));

        if (omarker != null) { omarker.setMap(null); };
        gmap.setCenter(point);
        gmap.setZoom(16);
        this.createMarker({ 'point': point, 'name': info.name, 'address': info.address, 'phone': info.phone, 'pic': info.pic });
        mmap = this;
    };
    this.initMapByLatLonForDragIcon = function (info, isDrag) {
        var IsDrag = isDrag != undefined ? isDrag : false;
        var xy = info.xy.split('_');
        var point = new google.maps.LatLng(parseFloat(xy[0]), parseFloat(xy[1]));


        if (omarker != null) { omarker.setMap(null); };
        gmap.setCenter(point);
        gmap.setZoom(16);
        this.createDragMarker({ 'point': point, 'name': info.name, 'address': info.address, 'phone': info.phone, 'pic': info.pic, 'draggend': IsDrag });
    
        mmap = this;
    };
    this.initMapByAddressForDragIcon = function (address, info, isDrag) {
        if (address == '') {
            address = '70 Lê Thánh Tôn, Bến Nghé, Ho Chi Minh, Vietnam';
            //this.setLatLon(address);
        }

        var IsDrag = isDrag != undefined ? isDrag : false;
        var geocoder = new google.maps.Geocoder();
        if (geocoder && address != "") {
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status != google.maps.GeocoderStatus.OK) {
                    //alert("Không tim được địa chỉ này");
                }
                else {

                    if (omarker != null) { omarker.setMap(null); }
                    var point = new google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng());

                    // Edit
                    document.getElementById("address-lat").value = results[0].geometry.location.lat();
                    document.getElementById("address-long").value = results[0].geometry.location.lng();

                    gmap.setCenter(point);
                    gmap.setZoom(16);
                    try {
                        var marker = new google.maps.Marker({
                            map: gmap,
                            position: results[0].geometry.location
                        });

                        omarker = marker;
                        mmap.createDragMarkerHtml({ 'point': point, 'name': info.name, 'address': info.address, 'phone': info.phone, 'pic': info.pic, 'draggend': IsDrag });
                    } catch (ex) { }
                }
            });
        }
        
        mmap = this;
    };

    this.setUrlNoImage = function (urlImage) {
        urlNoImage = urlImage;
    };
   
};


