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
//get date from 10 days
var tenDaysAgoDate = getTenDaysAgoDate()

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

}///function to create date chart
function createDateChart(ctx2, date, pageviews) {
    new Chart(ctx2, {
        type: 'line',
        data: {
            labels: date,
            datasets: [{
                label: 'PageViews',
                data: pageviews,
                borderColor: 'red',
                borderWidth: 1,
                backgroundColor: 'rgba(255, 0, 0, 0.5)',
                pointStyle: 'circle',
                pointRadius: 5,
                pointHoverRadius: 5
            }]
        },
        options: {
            responsive: true,
            scales: {
                x: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Date',
                        font: {
                            weight: 'bold'
                        }
                    }
                },
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'PageViews',
                        font: {
                            weight: 'bold'
                        }
                    }
                }
            }
        }
    });
}

//function to create category chart 
function createCategoryChart(ctx1, categories, pageviews) {
    new Chart(ctx1, {
        type: 'bar',
        data: {
            labels: categories,
            datasets: [{
                label: "PageViews",
                data: pageviews,
                backgroundColor: backGoundColors,
                borderColor: backGoundColors,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                x: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Top Categories',
                        font: {
                            weight: 'bold'
                        }
                    }
                },
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'PageViews',
                        font: {
                            weight: 'bold'
                        }
                    }
                }
            }
        }
    });
}

//function to create author chart 
function createAuthorChart(ctx3, authors, pageviews) {
    //create the chart
    new Chart(ctx3, {
        type: 'pie',
        data: {
            labels: authors,
            datasets: [{
                label: 'pageviews',
                data: pageviews,
                backgroundColor: backGoundColors
            }]
        },
        options: {
            plugins: {
                legend: { position: 'left' },
                title: {
                    display: true,
                    text: "Top Authors",
                    font: {
                        weight: 'bold'
                    }
                }
            },
            aspectRatio: 1, // Aspect ratio of the chart (width/height)
            maintainAspectRatio: false // Whether to maintain aspect ratio when resizing
        }
    });
}
    //function to create country  chart
 function createCountryChart(ctx4, countries, pageviews) {
        new Chart(ctx4, {
            type: 'bar',
            data: {
                labels: countries,
                datasets: [{
                    axis: 'y',
                    label: 'PageViews',
                    data: pageviews,
                    fill: false,
                    backgroundColor: backGoundColors,
                    borderColor: backGoundColors,
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    x: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'PageViews',
                            font: {
                                weight: 'bold'
                            }
                        }
                    }
                },
                plugins: {
                    title: {
                        display: true,
                        text: "Top Countries",
                        font: {
                            weight: 'bold'
                        }
                    }
                },
                indexAxis: 'y'
            }
        });
    }
    
