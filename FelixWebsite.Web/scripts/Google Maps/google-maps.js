var map;
var marker;
var geocoder; 
var center;
var markerSpiderfier

function initGoogleMap() {

    geocoder = new google.maps.Geocoder();
    // centreren over vlaanderen
    center = new google.maps.LatLng(51.161162, 4.641145);

    var mapOptions = {
        zoom: 9,
        center: center,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        scrollwheel: false,
        draggable: false
    };

    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
    var infowindow = new google.maps.InfoWindow();

    markerSpiderfier = new OverlappingMarkerSpiderfier(map, { 
        markersWontMove: true,
        markersWontHide: true,   
        basicFormatEvents: true,
        keepSpiderfied: true
    });

    function placeMarker(store, location) {
        const marker = new google.maps.Marker({
            position: location
        });

        // Use spider_click as eventListener to show multiple markers on Click
        google.maps.event.addListener(marker, 'spider_click', function (e) {
            infowindow.setContent(
                "<div class='container-fluid'>" +
                "<div class='row'>" +
                "<h4>" + store.name + "</h4>" +
                "</div>" +
                "<div class='row'>" +
                "<p> <i class='felix-red fas fa-map-marker-alt' style='width: 25px'></i> " + store.loc + "</p>" +
                "</div>" +
                "<div class='row'>" +
                "<p> <i class='felix-red fas fa-phone'  style='width: 25px'></i> " + store.tel + "</p>" +
                "</div>" +
                "</div>"
            );
            infowindow.open(map, marker);
        });
        markerSpiderfier.addMarker(marker);
    }

    function convertAddressToLoc(store) {
        geocoder.geocode({ 'address': store.loc },
            function (results, status) {
                if (status == "OK") {
                    placeMarker(store, results[0].geometry.location);
                } else {
                    return undefined;
                }
            });
    }

    for (var i = 0; i < arrEstablishments.length; i++) {
        convertAddressToLoc(arrEstablishments[i] );
    }
}