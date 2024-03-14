//constants
const domain = 'x.com'; //to change later
//fetch trending articles data
function fetchTrendingArticlesData(ip) {

    //get element
    trendingListElement = document.getElementById("trendingList")
    ///fetching data
    fetch('/api/Article/trending?domain=' + domain + "&ip=" + ip) //to change later
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(jsonResponse => {
            // Process the JSON data
            const date = [];
            const pageviews = [];

            jsonResponse.forEach(item => {
                var trendingItemElement = document.createElement("li");
                trendingItemElement.textContent = item.postTitle;
                trendingListElement.appendChild(trendingItemElement);
            });
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}