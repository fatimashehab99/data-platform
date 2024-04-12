
//Tags word cloud
function fetchTagPageViewsdata(domain, dateFrom, dateTo, postType) {
    //fetching data
    fetch("api/Analyze/tags?domain=" + domain + "&date_from=" + dateFrom + "&date_to=" + dateTo + "&postType=" + postType) //to change later
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            // Parse the JSON response
            return response.json();
        })
        .then(data => {
            // Check if data is an array
            if (Array.isArray(data)) {
                // Use the data to create the chart
                createTagsChart(data);
            } else {
                throw new Error('Response data is not an array');
            }
        }).catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

//Dashboard Statistics 
function fetchDashboardStatisticsData(domain) {
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
            pageviewElement.textContent = formatNumbers(jsonResponse.pageViews);
            userElement.textContent = formatNumbers(jsonResponse.users);
            articleElement.textContent = formatNumbers(jsonResponse.articles);
            authorElement.textContent = formatNumbers(jsonResponse.authors);
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}
//Date Chart 
function fetchDatePageViewsData(domain) {

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
function fetchCategoryPageViewsData(domain) {
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
function fetchAuthorPageViewsData(domain) {
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
function fetchCountryPageViewsData(domain) {
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

//PostType chart
function fetchPostTypePageViewsdata(domain, dateFrom, dateTo) {
    //get element
    const posttypeElement = document.getElementById("PostTypeChart");
    //fetching data
    fetch("api/Analyze/posttypes?domain=" + domain + "&date_from=" + dateFrom + "&date_to=" + dateTo) //to change later
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            // Parse the JSON response
            return response.json();
        })
        .then(data => {
            // Check if data is an array
            if (Array.isArray(data)) {
                // Use the data to create the chart
                createPostTypeChart(posttypeElement, data);
            } else {
                throw new Error('Response data is not an array');
            }
        }).catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}