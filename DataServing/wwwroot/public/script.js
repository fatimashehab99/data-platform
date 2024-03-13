const backGoundColors = [
    'rgba(255, 99, 132, 0.2)',
    'rgba(255, 159, 64, 0.2)',
    'rgba(255, 205, 86, 0.2)',
    'rgba(75, 192, 192, 0.2)',
    'rgba(54, 162, 235, 0.2)',
    'rgba(153, 102, 255, 0.2)',
    'rgba(201, 203, 207, 0.2)',
    'rgba(255, 0, 0, 0.2)',   
    'rgba(0, 255, 0, 0.2)',   
    'rgba(0, 0, 255, 0.2)'       
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


