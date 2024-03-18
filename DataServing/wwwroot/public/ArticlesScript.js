//constants
const domain = 'x.com'; //toDoupdate domain
//fetch trending articles data
function fetchTrendingArticlesData(ip) {

    //get element
    trendingListElement = document.getElementById("trendingList")
    ///fetching data
    fetch(`/api/Article/trending?domain=${domain}&ip=${ip}`) //to change later
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(jsonResponse => {
            jsonResponse.forEach(item => {
                //toDO add image

                //add post title
                var trendingItemElement = document.createElement("li");
                trendingItemElement.textContent = item.postTitle;
                trendingListElement.appendChild(trendingItemElement);

                //add post url
                var trendingLink = document.createElement("a");
                trendingLink.href = item.postUrl;
                trendingLink.textContent = "Read More";
                trendingListElement.appendChild(trendingLink);

            });
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

//fetch recommended articles data
function fetchRecommendedArticlesData(userId) {

    //get element
    recommendedListElement = document.getElementById("recommendedList")
    ///fetching data
    fetch(`/api/Article/recommended?domain=${domain}&userId=${userId}&Ip=109.75.64.0`) //to change later
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

                //add post title
                var recommendedItemElement = document.createElement("li");
                recommendedItemElement.textContent = item.postTitle;
                recommendedListElement.appendChild(recommendedItemElement);

                //add post url
                var recommendedLink = document.createElement("a");
                recommendedLink.href = item.postUrl;
                recommendedLink.textContent = "Read More";
                recommendedListElement.appendChild(recommendedLink);

                // Append li element to ul element
            });
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}