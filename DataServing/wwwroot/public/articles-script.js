﻿//fetch trending articles data
function fetchTrendingArticlesData() {
    new Vue({
        el: '#trending',
        data: {
            trendingItems: []
        },
        mounted() {
            // Fetch data from API
            fetch(`/api/Article/trending?domain=${domain}`) //to change later
                .then(response => response.json())
                .then(data => {
                    this.trendingItems = data;
                })
                .catch(error => {
                    console.error('Error fetching recommended items:', error);
                });
        }
    });
}

//fetch recommended articles data
function fetchRecommendedArticlesData(userId) {
    new Vue({
        el: '#recommended',
        data: {
            recommendedItems: []
        },
        mounted() {
            // Fetch data from API
            fetch(`/api/Article/recommended?domain=${domain}&userId=${userId}`) //to change later
                .then(response => response.json())
                .then(data => {
                    this.recommendedItems = data;
                })
                .catch(error => {
                    console.error('Error fetching recommended items:', error);
                });
        }
    });
}