
//Tags word cloud
function fetchTagPageViewsdata(domain, dateFrom, dateTo, postType) {
    //fetching data
    fetch("api/Analyze/tags?domain=" + domain + "&date_from=" + dateFrom + "&date_to=" + dateTo + "&posttype=" + postType) //to change later
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
function fetchDashboardStatisticsData(domain, dateFrom, dateTo, posttype) {
    //get elements
    pageviewElement = document.getElementById("pageviews");
    userElement = document.getElementById("users");
    articleElement = document.getElementById("articles");
    authorElement = document.getElementById("authors");

    ///fetching data
    fetch('/api/Analyze/dashboard?domain=' + domain + "&date_from=" + dateFrom + "&date_to=" + dateTo + "&posttype=" + posttype) //to change later
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
function fetchDatePageViewsData(domain, posttype) {

    //get element
    const dateElement = document.getElementById('DateChart').getContext('2d');
    date = getDate(10)
    //fetching data
    fetch('/api/Analyze/date?domain=' + domain + "&date=" + date + "&posttype=" + posttype) //to change later
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
            const publishedArticles = [];

            jsonResponse.forEach(item => {
                date.push(item.date);
                pageviews.push(item.pageViews);
                publishedArticles.push(item.publishedArticles);
            });

            //check if the hcart exists before
            if (dateElement.chart) {
                (dateElement.chart).destroy()
            }
            //create the chart
            const dateChart = createDateChart(dateElement, date, pageviews, publishedArticles);
            dateElement.chart = dateChart
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

//Category Chart
function fetchCategoryPageViewsData(domain, dateFrom, dateTo, postType) {
    const categoryElement = document.getElementById('CategoryChart').getContext('2d');

    fetch('/api/Analyze/categories?domain=' + domain + "&date_from=" + dateFrom + "&date_to=" + dateTo + "&posttype=" + postType) //to change later
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

            //check if the hcart exists before
            if (categoryElement.chart) {
                (categoryElement.chart).destroy()
            }
            ///create the chart
            const categoryChart = createCategoryChart(categoryElement, categories, pageviews)
            categoryElement.chart = categoryChart

        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}
//Author Chart
function fetchAuthorPageViewsData(domain, dateFrom, dateTo, postType) {
    //get element
    const authorElement = document.getElementById('AuthorChart').getContext('2d');
    //fetching data
    fetch('/api/Analyze/authors?domain=' + domain + "&date_from=" + dateFrom + "&date_to=" + dateTo + "&posttype=" + postType) //to change later
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
            if (authorElement.chart) {
                authorElement.chart.destroy()
            }
            //create the chart
            const authorChart = createAuthorChart(authorElement, authors, pageviews)
            authorElement.chart = authorChart

        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}
//Country Chart
function fetchCountryPageViewsData(domain, dateFrom, dateTo, posttype) {
    //get element
    const countryElement = document.getElementById('CountryNameChart');
    //fetching data
    fetch('/api/Analyze/countries?domain=' + domain + "&date_from=" + dateFrom + "&date_to=" + dateTo + "&posttype=" + posttype) //to change later
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

            //check if chart exist before
            if (countryElement.chart) {
                countryElement.chart.destroy()
            }

            //create the chart
            const countryChart = createCountryChart(countryElement, countries, pageviews)
            countryElement.chart = countryChart

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
                //check if chart exist before
                if (posttypeElement.chart) {
                    posttypeElement.chart.destroy()
                }
                //create the chart
                // Use the data to create the chart
                const posttypeChart = createPostTypeChart(posttypeElement, data);
                posttypeElement.chart = posttypeChart

            } else {
                throw new Error('Response data is not an array');
            }
        }).catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}