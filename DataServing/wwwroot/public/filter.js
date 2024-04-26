
//executed when data is filtered
new Vue({
    el: '#filter', // Corrected to match the HTML element's ID
    data: {
        selectedDate: '30', // Set default value to the value of the first option
        selectedDomain: 'www.almayadeen.net', // Set default value to the value of the first option
        selectedCategory: ''
    },
    methods: {
        filterData() {
            days = parseInt(this.selectedDate)
            executeDashboards(this.selectedDomain, getDate(days), this.selectedCategory)
        }
    }
});

//fetching data and designing the charts
function executeDashboards(domain = "www.almayadeen.net", dateFrom = getDate(30), posttype = "") {
    fetchDashboardStatisticsData(domain, dateFrom, getDate(0), posttype)
    fetchDatePageViewsData(domain, posttype);
    fetchCategoryPageViewsData(domain, dateFrom, getDate(0), posttype);
    fetchAuthorPageViewsData(domain, dateFrom, getDate(0), posttype);
    fetchCountryPageViewsData(domain, dateFrom, getDate(0), posttype);
    fetchPostTypePageViewsdata(domain, dateFrom, getDate(0));
    fetchTagPageViewsdata(domain, dateFrom, getDate(0), posttype);
    fetchArticleTableData(domain, dateFrom, getDate(0), posttype);
}