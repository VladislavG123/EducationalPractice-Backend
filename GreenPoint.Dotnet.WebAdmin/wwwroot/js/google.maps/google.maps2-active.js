function initMap() {

    var myLatlng = {lat: 51.171887, lng: 71.42959};

    map6 = document.getElementById('map6')
    
    json = map6.dataset.spots
    
    list = JSON.parse(json)
    
    console.log(list)
    
    var map6 = new google.maps.Map(map6, {
        zoom: 2,
        center: myLatlng
    });

    for (const el of list) {
        var marker = new google.maps.Marker({
            position: {
                lat: el.Latitude, 
                lng: el.Langitude
            },
            map: map6,
            title: el.Title
        });
    }
    
    

}
	  
     
 

