///filter logic
function filter() {
    new Vue({
        el: '#filter', // Corrected to match the HTML element's ID
        data: {
            selectedDate: '1day', // Set default value to the value of the first option
            selectedDomain: 'www.almayadeen.net', // Set default value to the value of the first option
            selectedCategory: ''
        },
        methods: {
            handleSubmit() {
                console.log('Selected Date:', this.selectedDate);
                console.log('Selected Domain:', this.selectedDomain);
                console.log('Selected Category:', this.selectedCategory); // Corrected to match the data property name
            }
        }
    });
}
