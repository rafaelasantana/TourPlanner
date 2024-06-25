window.initializeMap = (mapElement, startLat, startLng, endLat, endLng, routeGeometry) => {
    console.log("Rendering map - ", startLat, startLng, endLat, endLng);

    window.map = L.map('map').setView([startLat, startLng], 8);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(window.map);

    L.marker([startLng, startLat]).addTo(window.map)
        .bindPopup('Start')
        .openPopup();

    L.marker([endLng, endLat]).addTo(window.map)
        .bindPopup('End')
        .openPopup();

    var route = L.polyline(polyline.decode(routeGeometry), {
        color: 'blue',
        weight: 7,
        opacity: 0.9,
        lineJoin: 'round'
    }).addTo(window.map);

    window.map.fitBounds(route.getBounds());

    console.log("Map rendered");
};