//constants
const domain = 'x.com'; //toDoupdate domain
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
            jsonResponse.forEach(item => {
                //toDO add image
                var trendingItemElement = document.createElement("li");
                trendingItemElement.textContent = item.postTitle;
                trendingItemElement.innerHTML = '<a href="' + item.postUrl + '</a>';
                trendingListElement.appendChild(trendingItemElement);
            });
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

//fetch recommended articles data
function fetchRecommendedArticlesData(userId) {

    //get element
    trendingListElement = document.getElementById("recommendedList")
    ///fetching data
    fetch('/api/Article/recommended?domain=' + domain + "&userId=" + userId) //to change later
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(jsonResponse => {
            // Process the JSON data
            jsonResponse.forEach(item => {

                //toDO add image
                var trendingItemElement = document.createElement("li");
                trendingItemElement.textContent = item.postTitle;
                trendingItemElement.innerHTML = '<a href="' + item.postUrl + '</a>';
                trendingListElement.appendChild(trendingItemElement);
            });
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}