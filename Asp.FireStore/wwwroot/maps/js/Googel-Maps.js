/*

var map;
var markers = [];
var directionsService;
var directionsRenderer;

function initMap() {
    directionsService = new google.maps.DirectionsService();
    directionsRenderer = new google.maps.DirectionsRenderer();

    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 15.335416, lng: 44.181946 },
        zoom: 16
    });

    directionsRenderer.setMap(map);

    // Add a click event handler to the map.
    google.maps.event.addListener(map, 'click', function (event) {
        // Limit the number of markers on the map to two (for current and target locations).
        if (markers.length >= 2) {
            return;
        }



        // Add a new marker to the map at the clicked location.
        var marker = new google.maps.Marker({
            position: event.latLng,
            map: map
        });

        markers.push(marker);

        // Update the form inputs for current and target locations.
        if (markers.length == 1) {
            document.getElementById('currentLocation').value = event.latLng.lat() + ', ' + event.latLng.lng();
        } else if (markers.length == 2) {
            document.getElementById('targetLocation').value = event.latLng.lat() + ', ' + event.latLng.lng();

            // Calculate and display the route.
            calculateAndDisplayRoute(directionsService, directionsRenderer);
        }
    });
}

function calculateAndDisplayRoute(directionsService, directionsRenderer) {
    directionsService.route(
        {
            origin: markers[0].getPosition(),
            destination: markers[1].getPosition(),
            travelMode: 'DRIVING'
        },
        function (response, status) {
            if (status === 'OK') {
                directionsRenderer.setDirections(response);

                // Get distance and duration
                var distance = response.routes[0].legs[0].distance.text;
                var duration = response.routes[0].legs[0].duration.text;

                // Display distance and duration
                document.getElementById('distance').value = distance;
                document.getElementById('duration').value = duration;
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });
}

*/