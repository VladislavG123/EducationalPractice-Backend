function initMap() {
    var myLatlng = {lat: 51.171887, lng: 71.42959};
    setMap(myLatlng)
}

function setMap(myLatlng) {
    const map = new google.maps.Map(document.getElementById("map6"), {
        zoom: 2,
        center: myLatlng,
    });

// Create the initial InfoWindow.
    let infoWindow = new google.maps.InfoWindow({
        content: "Нажмите, для выбора точки",
        position: myLatlng,
    });
    infoWindow.open(map);

// Configure the click listener.
    map.addListener("click", (mapsMouseEvent) => {
        // Close the current InfoWindow.
        infoWindow.close();

        // Create a new InfoWindow.
        infoWindow = new google.maps.InfoWindow({
            position: mapsMouseEvent.latLng,
        });

        document.getElementById("googlemap_lat")
            .value = mapsMouseEvent.latLng.toJSON().lat

        document.getElementById("googlemap_lng")
            .value = mapsMouseEvent.latLng.toJSON().lng


        infoWindow.setContent(
            "Место новой проблемной точки"
        );
        infoWindow.open(map);
    });
}
    
 

