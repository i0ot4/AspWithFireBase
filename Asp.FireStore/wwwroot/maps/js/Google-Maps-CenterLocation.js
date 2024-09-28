
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

            db.collection("Users").get().then((querySnapshot) => {
                querySnapshot.forEach((doc) => {
                    var center = doc.data();
                    if (center.CenterLocation && center.CenterLocation.latitude && center.CenterLocation.longitude) {
                        var marker = new google.maps.Marker({
                            position: new google.maps.LatLng(center.CenterLocation.latitude, center.CenterLocation.longitude),
                            map: map,
                            title: center.CenterName,
                            label: {
                                text: center.CenterName, // هنا نضيف اسم المركز كتسمية على العلامة
                                color: 'black', // استبدل هذا باللون الذي تريده للنص
                                fontSize: '18px', // استبدل هذا بحجم الخط الذي تريده
                                fontWeight: 'bold' // استبدل هذا بوزن الخط الذي تريده
                            }
                        });
                    }
                });
            });

        }