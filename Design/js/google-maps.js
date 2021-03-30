var myLatlng;
var map;
var marker;

function initGoogleMap() {
    // centeren over vlaanderen
    center = new google.maps.LatLng(51.161162, 4.641145);
    var locations = [
        { lat:51.3135200, lng:4.4231953, name: "Citroën Meyvis"   , loc: "Hoevensebaan 70, 2950 Kapellen", tel: "03 660 09 09", brand: "Citroën" },
        { lat:51.1733764, lng:4.8189937, name: "Citroën Felix"    , loc: "Hemeldonk 4, 2200 Herentals"   , tel: "014 34 70 00", brand: "Citroën" },
        { lat:51.1734740, lng:4.8186030, name: "Peugeot Lavrijsen", loc: "Hemeldonk 4, 2200 Herentals"   , tel: "014 34 70 00", brand: "Peugeot" },
        { lat:51.3532320, lng:4.6651290, name: "Citroën Felix"    , loc: "Heiken 66, 2960 Brecht"        , tel: "03 660 26 00", brand: "Citroën" },
        { lat:51.3532320, lng:4.6651290, name: "Carrosserie Felix", loc: "Heiken 66, 2960 Brecht"        , tel: "03 660 26 00", brand: "Carrosserie" }
    ];

    var mapOptions = {
        zoom: 9,
        center: center,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        scrollwheel: false,
        draggable: false
    };

    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
    var infowindow = new google.maps.InfoWindow();

    var oms = new OverlappingMarkerSpiderfier(map, { 
        markersWontMove: true,
        markersWontHide: true,   
        basicFormatEvents: true,
        keepSpiderfied: true
    });

    function isCurrentPageHomepage () {
        if(document.title == "Felix Groep | de Citroën en tweedehands specialist te Brecht, Mol en Herentals.") {
            return true;
        } else {
            return false;
        }
    }

    function placeMarker( store ) {

        (function() {
            if(!isCurrentPageHomepage()) {
                if(document.title == "Felix Groep | Peugeot"){
                    if(store.brand == "Peugeot") {
                        var marker = new google.maps.Marker({ 
                            position: {lat: store.lat, lng: store.lng }
                            // Optional: custom marker
                            // icon: './images/custom-marker-template.png'
                        });
                    } else {
                        return;
                    }
                } else if (document.title == "Felix Groep | Citroën") {
                    if(store.brand == "Citroën") {
                        var marker = new google.maps.Marker({ 
                            position: {lat: store.lat, lng: store.lng }
                            // Optional: custom marker
                            // icon: './images/custom-marker-template.png'
                        }); 
                    } else {
                        return;
                    }
                }

            } else {
                var marker = new google.maps.Marker({ 
                    position: {lat: store.lat, lng: store.lng }
                    // Optional: custom marker
                    // icon: './images/custom-marker-template.png'
                });  
            }
            // Use spider_click as eventListener to show multiple markers on Click
            google.maps.event.addListener(marker, 'spider_click', function(e) {
              infowindow.setContent(
                "<div class='container-fluid'>" +
                    "<div class='row'>" +
                        "<h4>"+ store.name +"</h4>"+         
                    "</div>"+
                    "<div class='row'>" +
                        "<p> <i class='felix-red fas fa-map-marker-alt' style='width: 25px'></i> "+ store.loc +"</p>"+         
                    "</div>"+
                    "<div class='row'>" +
                        "<p> <i class='felix-red fas fa-phone'  style='width: 25px'></i> "+ store.tel +"</p>"+         
                    "</div>"+
                    "<div class='row'>" +
                        "<p> <i class='felix-red fas fa-tag' style='width: 25px'></i> "+ store.brand +"</p>"+         
                    "</div>"+
                "</div>"
              );
              infowindow.open(map, marker);
            });
            oms.addMarker(marker);
        })();
    }

    for(var i=0; i<locations.length; i++) {
        placeMarker( locations[i] );
    }
}
google.maps.event.addDomListener(window, 'load', initGoogleMap);
