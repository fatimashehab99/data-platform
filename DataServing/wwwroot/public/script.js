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

function getDate(days) {
    // Get the current date
    var currentDate = new Date();

    // Get the current date in the format "YYYY-MM-DD"
    var currentDateFormatted = currentDate.toISOString().slice(0, 10);
    // Calculate the date 10 days ago
    var tenDaysAgo = new Date(currentDate);
    tenDaysAgo.setDate(currentDate.getDate() - days);

    // Get the date 10 days ago in the same format
    var tenDaysAgoFormatted = tenDaysAgo.toISOString().slice(0, 10);
    return tenDaysAgoFormatted;

}
//function to format large numbers 
function formatNumbers(number) {
    if (number >= 1000000000) {
        number = (number / 1000000000).toFixed(2);
        if (Number.isInteger(Number(number))) {
            number = Number(number).toLocaleString() + 'B';
        } else {
            number = number.toLocaleString(undefined, { maximumFractionDigits: 2 }) + 'B';
        }
    } else if (number >= 1000000) {
        number = (number / 1000000).toFixed(2);
        if (Number.isInteger(Number(number))) {
            number = Number(number).toLocaleString() + 'M';
        } else {
            number = number.toLocaleString(undefined, { maximumFractionDigits: 2 }) + 'M';
        }
    } else if (number >= 1000) {
        number = (number / 1000).toFixed(2);
        if (Number.isInteger(Number(number))) {
            number = Number(number).toLocaleString() + 'K';
        } else {
            number = number.toLocaleString(undefined, { maximumFractionDigits: 2 }) + 'K';
        }
    }
    return number;
}

///function to create date chart
function createDateChart(ctx2, date, pageviews, publishedArticles) {
    return new Chart(ctx2, {
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
                pointHoverRadius: 5,
                yAxisID: 'y1' // Assigning this dataset to the y1 axis
            }, {
                label: 'Published Articles',
                data: publishedArticles,
                borderColor: 'blue',
                borderWidth: 1,
                backgroundColor: 'rgba(0, 0, 255, 0.5)',
                pointStyle: 'circle',
                pointRadius: 5,
                pointHoverRadius: 5,
                yAxisID: 'y2' // Assigning this dataset to the y2 axis
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
                y1: {
                    beginAtZero: true,
                    position: 'left',
                    title: {
                        display: true,
                        text: 'Page Views',
                        font: {
                            weight: 'bold'
                        }
                    }
                },
                y2: {
                    beginAtZero: true,
                    position: 'right',
                    title: {
                        display: true,
                        text: 'Published Articles',
                        font: {
                            weight: 'bold'
                        }
                    },
                    grid: {
                        drawOnChartArea: false
                    }
                }

            }
        }
    });
}

//function to create category chart 
function createCategoryChart(ctx1, categories, pageviews) {
    return new Chart(ctx1, {
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
let tagsRoot = null;

function createTagsChart(tags) {
    // Check if the root element already exists
    if (!tagsRoot) {
        // Create root element if it doesn't exist
        tagsRoot = am5.Root.new("tagsWordCloud");
        // Set themes
        tagsRoot.setThemes([
            am5themes_Animated.new(tagsRoot)
        ]);
    } else {
        // Clear existing series from the root if it's a filter operation
        tagsRoot.container.children.clear();
    }

    // Add series
    var series = tagsRoot.container.children.push(am5wc.WordCloud.new(tagsRoot, {
        categoryField: "tags",
        valueField: "pageviews",
        maxFontSize: am5.percent(15)
    }));

    // Configure labels
    series.labels.template.setAll({
        fontFamily: "Courier New"
    });

    // Update data
    series.data.setAll(tags);

    // Update data values periodically
    setInterval(function () {
        am5.array.each(series.dataItems, function (dataItem) {
            var value = Math.random() * 65;
            value = value - Math.random() * value;
            dataItem.set("value", value);
            dataItem.set("valueWorking", value);
        });
    }, 5000);
}

//function to create author chart 
function createAuthorChart(ctx3, authors, pageviews) {
    //create the chart
    return new Chart(ctx3, {
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
//function to create postType scatter chart
function createPostTypeChart(posttypeElement, postTypes) {
    // Prepare data for the chart
    const data = {
        datasets: [
            {
                label: 'Total Page Views',
                data: postTypes.map(item => ({ x: Math.random(), y: item.pageViews, r: 10 })), // Increase the 'r' value for larger circles
                borderColor: 'red',
                backgroundColor: 'rgba(255, 99, 132, 0.5)'
            },
            {
                label: 'Total Posts',
                data: postTypes.map(item => ({ x: Math.random(), y: item.posts, r: 10 })), // Increase the 'r' value for larger circles
                borderColor: 'blue',
                backgroundColor: 'rgba(54, 162, 235, 0.5)'
            }
        ],
        labels: postTypes.map(item => item.postType)
    };
    //create the chart
   return new Chart(posttypeElement, {
        type: 'bubble', // Use 'bubble' chart type for scatter plot with variable circle sizes
        data: data,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: 'Total Page Views and Total Posts For Each Post Type'
                }
            }
        },
    });
}
//function to create country  chart
function createCountryChart(ctx4, countries, pageviews) {
    return new Chart(ctx4, {
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



