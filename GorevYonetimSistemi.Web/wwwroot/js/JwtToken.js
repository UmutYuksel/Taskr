// Cookie'den token almak için bir fonksiyon
function getJwtTokenFromCookie() {
    var name = "AuthToken=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

// JWT token'ını al
var token = getJwtTokenFromCookie();

// Eğer token varsa, bunu başlığa ekle
if (token) {
    // API'ye istek gönder
    fetch('http://localhost:5113/api/user/', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`, // token'ı header'a ekle
            'Content-Type': 'application/json'
        }
    })
    .then(response => response.json())
    .then(data => {
        console.log(data);
    })
    .catch(error => console.error('Error:', error));
}
