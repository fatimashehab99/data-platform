
//constants
const domain = 'www.almayadeen.net'; //to change later

//Dashboard Statistics 
function fetchDashboardStatisticsData() {
    //get elements
    pageviewElement = document.getElementById("pageviews");
    userElement = document.getElementById("users");
    articleElement = document.getElementById("articles");
    authorElement = document.getElementById("authors");

    ///fetching data
    fetch('/api/Analyze/dashboard?domain=' + domain) //to change later
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(jsonResponse => {
            // Process the JSON data
            pageviewElement.textContent = jsonResponse.pageViews;
            userElement.textContent = jsonResponse.users;
            articleElement.textContent = jsonResponse.articles;
            authorElement.textContent = jsonResponse.authors;

        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}
//Date Chart 
function fetchDatePageViewsData() {

    //get element
    const dateElement = document.getElementById('DateChart').getContext('2d');

    //fetching data
    fetch('/api/Analyze/date?domain=' + domain + "&date=" + tenDaysAgoDate) //to change later
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
                date.push(item.date);
                pageviews.push(item.pageViews);
            });
            //create the chart
            createDateChart(dateElement, date, pageviews);
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

//Category Chart
function fetchCategoryPageViewsData() {
    const categoryElement = document.getElementById('CategoryChart').getContext('2d');

    fetch('/api/Analyze/categories?domain=' + domain) //to change later
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(jsonResponse => {
            // Process the JSON data
            const categories = [];
            const pageviews = [];

            jsonResponse.forEach(item => {
                categories.push(item.category);
                pageviews.push(item.pageViews);
            });

            ///create the chart
            createCategoryChart(categoryElement, categories, pageviews)

        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}
//Author Chart
function fetchAuthorPageViewsData() {
    //get element
    const authorElement = document.getElementById('AuthorChart').getContext('2d');
    //fetching data
    fetch('/api/Analyze/authors?domain=' + domain) //to change later
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(jsonResponse => {
            // Process the JSON data
            const authors = [];
            const pageviews = [];

            jsonResponse.forEach(item => {
                authors.push(item.author);
                pageviews.push(item.pageViews);
            });
            //create the chart
            createAuthorChart(authorElement, authors, pageviews)
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}
//Country Chart
function fetchCountryPageViewsData() {
    //get element
    const countryElement = document.getElementById('CountryNameChart');
    //fetching data
    fetch('/api/Analyze/countries?domain=' + domain) //to change later
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(jsonResponse => {
            // Process the JSON data
            const countries = [];
            const pageviews = [];

            jsonResponse.forEach(item => {
                countries.push(item.countryName);
                pageviews.push(item.pageViews);
            });
            //create the chart
            createCountryChart(countryElement, countries, pageviews)

        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}