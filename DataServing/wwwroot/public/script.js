const pieChartColors = [
    'rgba(255, 99, 132, 0.5)', // Red
    'rgba(255, 132, 99, 0.5)', // Light Red
    'rgba(99, 255, 132, 0.5)', // Green
    'rgba(132, 255, 99, 0.5)', // Light Green
    'rgba(99, 132, 255, 0.5)', // Blue
    'rgba(132, 99, 255, 0.5)', // Light Blue
    'rgba(255, 255, 99, 0.5)', // Yellow
    'rgba(99, 255, 255, 0.5)', // Cyan
    'rgba(255, 99, 255, 0.5)', // Magenta
    'rgba(192, 192, 192, 0.5)' // Gray
];


function getTenDaysAgoDate() {
    // Get the current date
    var currentDate = new Date();

    // Get the current date in the format "YYYY-MM-DD"
    var currentDateFormatted = currentDate.toISOString().slice(0, 10);
    // Calculate the date 10 days ago
    var tenDaysAgo = new Date(currentDate);
    tenDaysAgo.setDate(currentDate.getDate() - 10);

    // Get the date 10 days ago in the same format
    var tenDaysAgoFormatted = tenDaysAgo.toISOString().slice(0, 10);
    return tenDaysAgoFormatted;

}


