new Vue({
    el: '#filter', // Corrected to match the HTML element's ID
    data: {
        selectedDate: '0', // Set default value to the value of the first option
        selectedDomain: 'www.almayadeen.net', // Set default value to the value of the first option
        selectedCategory: ''
    },
    methods: {
        filterData() {
            days = parseInt(this.selectedDate)
            executeDashboards(this.selectedDomain,getDate(days))
            console.log('Selected Date:', this.selectedDate);
            console.log('Selected Domain:', this.selectedDomain);
            console.log('Selected Category:', this.selectedCategory); // Corrected to match the data property name
        }
    }
});

//fetching data and designing the charts
function executeDashboards(domain = "www.almayadeen.net", dateFrom = getDate(0), posttype = "") {
    console.log(dateFrom)
    //fetchDashboardStatisticsData(domain)
    fetchDatePageViewsData(domain, "");
    fetchCategoryPageViewsData(domain, dateFrom, getDate(0), "");
    fetchAuthorPageViewsData(domain, dateFrom, getDate(0), "");
    fetchCountryPageViewsData(domain, dateFrom, getDate(0), "");
    fetchPostTypePageViewsdata(domain, dateFrom, getDate(0));
    fetchTagPageViewsdata(domain, dateFrom, getDate(0), "");
    fetchDashboardStatisticsData(domain, dateFrom, getDate(0), "");

}