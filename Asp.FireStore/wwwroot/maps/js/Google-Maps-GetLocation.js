// استبدل هذه القيم بتكوين Firebase الخاص بك
var firebaseConfig = {
    apiKey: "AIzaSyBEyD0Y6E-guN-dFVc2EorSprGSlGMq0Ns",
    projectId: "malik-project-419023",
};

// تهيئة Firebase
firebase.initializeApp(firebaseConfig);

let map;
function initMap() {

    var db = firebase.firestore();

    var tripId = document.getElementById('tripDiracttion').value;

    db.collection("MechanicRequests").doc(tripId).get().then((doc) => {
        if (doc.exists) {
            var trip = doc.data();
            if (trip.CurrentLocation && trip.CurrentLocation.latitude && trip.CurrentLocation.longitude) {
                var currentMarker = new google.maps.Marker({
                    position: new google.maps.LatLng(trip.CurrentLocation.latitude, trip.CurrentLocation.longitude),
                    map: map,
                    title: 'Current Location',
                });

                map = new google.maps.Map(document.getElementById('map'), {
                    center: { lat: trip.CurrentLocation.latitude, lng: trip.CurrentLocation.longitude },
                    zoom: 15
                });

                currentMarker.setMap(map);
            }
        } else {
            db.collection("TrafficPoliceRequests").doc(tripId).get().then((doc) => {
                if (doc.exists) {
                    var trip = doc.data();
                    if (trip.CurrentLocation && trip.CurrentLocation.latitude && trip.CurrentLocation.longitude) {
                        var currentMarker = new google.maps.Marker({
                            position: new google.maps.LatLng(trip.CurrentLocation.latitude, trip.CurrentLocation.longitude),
                            map: map,
                            title: 'Current Location',
                        });

                        map = new google.maps.Map(document.getElementById('map'), {
                            center: { lat: trip.CurrentLocation.latitude, lng: trip.CurrentLocation.longitude },
                            zoom: 15
                        });

                        currentMarker.setMap(map);
                    }
                } else {
                    console.log("No such document!");
                }
            }).catch((error) => {
                console.log("Error getting document:", error);
            });
        }
    }).catch((error) => {
        console.log("Error getting document:", error);
    });

}
