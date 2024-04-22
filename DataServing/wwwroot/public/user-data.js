//get domain
const domain = window.location.hostname;

//get user Id
function getUserId() {
    var cookieString = document.cookie;
    var cookies = cookieString.split('; ');
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i].split('=');
        if (cookie[0] === 'user_id') {
            return decodeURIComponent(cookie[1]);
        }
    }
    return null;
}

