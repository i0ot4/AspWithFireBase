
// استبدل هذه القيم بتكوين Firebase الخاص بك
var firebaseConfig = {
    apiKey: "AIzaSyBEyD0Y6E-guN-dFVc2EorSprGSlGMq0Ns",
    projectId: "malik-project-419023",
};

// تهيئة Firebase
firebase.initializeApp(firebaseConfig);

let map;
function initMap() {

    map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 15.335416, lng: 44.181946 },
        zoom: 15
    });

    var db = firebase.firestore();

    var tripId = document.getElementById('tripDiracttion').value;

    db.collection("TaxiDriverRequests").doc(tripId).get().then((doc) => {
        if (doc.exists) {
            var trip = doc.data();
            if (trip.CurrentLocation && trip.CurrentLocation.latitude && trip.CurrentLocation.longitude && trip.TargetLocation && trip.TargetLocation.latitude && trip.TargetLocation.longitude) {
                var currentMarker = new google.maps.Marker({
                    position: new google.maps.LatLng(trip.CurrentLocation.latitude, trip.CurrentLocation.longitude),
                    map: map,
                    title: 'Current Location',
                });

                var targetMarker = new google.maps.Marker({
                    position: new google.maps.LatLng(trip.TargetLocation.latitude, trip.TargetLocation.longitude),
                    map: map,
                    title: 'Target Location',
                });

                var directionsService = new google.maps.DirectionsService();
                var directionsRenderer = new google.maps.DirectionsRenderer();
                directionsRenderer.setMap(map);

                var request = {
                    origin: currentMarker.position,
                    destination: targetMarker.position,
                    travelMode: 'DRIVING'
                };

                directionsService.route(request, function (result, status) {
                    if (status == 'OK') {
                        directionsRenderer.setDirections(result);

                        // Get distance and duration
                        var distance = result.routes[0].legs[0].distance.text;
                        var duration = result.routes[0].legs[0].duration.text;

                        // Display distance and duration
                        document.getElementById('getDistance').value = distance;
                        document.getElementById('getDuration').value = duration;
                    }
                });


                directionsService.route(request, function (result, status) {
                    if (status == 'OK') {
                        directionsRenderer.setDirections(result);
                    }
                });
            }
        } else {
            console.log("No such document!");
        }
    }).catch((error) => {
        console.log("Error getting document:", error);
    });

}
