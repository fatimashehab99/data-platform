# Data Platform Analytics

### Outline
### I. Introduction
 Overview of Data Platform Analytics

### II. Collect API Usage <br/>

 A. Integration of Track Script <br/>
 B. Parameters for tracking data <br/>
 C. Example of Track Script <br/>
 D. Accessing analyzed data <br/>


### III. Analytics Visualization <br/>
   
 A. Analyze Page Views By Date <br/>
 B. Analyze Page Views By Categories <br/>
 C. Analyze Page Views By Authors <br/>
 D. Analyze Page Views By Countries <br/>
 E. Analyze Page Views By Post Type <br/>
 F. Analyze Page Views By Post Tags <br/>
 
### IV. Conclusion

- Summary of Data Platform Analytics

___

## 1. Overview
Data Platform Analytics System is a comprehensive data anlytics tool with three main parts. First, it gathers important data from metadata websites using carefully made scripts and stores it efficiently with MongoDB. Then, it uses ASP.NET and Mongo DB to create strong pipelines for processing the data smoothly. After that, it lets users easily get the data they need for visualization through simple APIs. Finally, it creates eye-catching charts and graphs using HTML, CSS,JavaScript and Chart.js, making the data easy to understand. Overall, this system helps users collect, analyze, and display data in a straightforward and interactive manner, aiding them in making informed decisions.


## 2. Collect API Usage 

To utilize the analytics functionality, it is necessary to integrate the mango-analytics-script  into your website. This script will collect the necessary data to generate the dynamic analytics charts and provide a comprehensive representation of the website's traffic.

The mango-analytics-script takes parameters in the body named as the following:
* PostId $~~~~~~~~~~~~~~~~~~~~~~~~~$*(This is responsible for handling the Post Id Post's article)*
* PostCategory $~~~~~~~~~~~$              *(This is responsible for handling the Post category Post's article)*
* PostAuthor $~~~~~~~~~~~~~~~~~~~~~~$                    *(This is responsible for handling the Author of the Post's article)*
* PostTitle $~~~~~~~~~~~~~~~~~~~~$                 *(This is responsible for handling the Post title of the Post's article)*
* Domain $~~~~~$          *(This is responsible for handling the Website Domain Name)*
* UserId $~~~~~~~~~~~~~~~~~~~~~~$                    *(This is responsible for handling the distinct users accessing this Website)*
* PostUrl $~~~~~~~~~~~~~~~~~~~~~~$                    *(This is responsible for handling the url of the Post's article)*
* PostImage $~~~~~~~~~~~$              *(This is responsible for handling the Post image Post's article)*
* PostTags $~~~~~~~~~~~$              *(This is responsible for handling the Post tags Post's article)*
* PostPublishedDate $~~~~~~~~~~~$              *(This is responsible for handling the Post published date Post's article)*
* PostClasses $~~~~~~~~~~~$              *(This is responsible for handling the Post classes Post's article)*
* PostType $~~~~~~~~~~~$              *(This is responsible for handling the Post type Post's article)*
 

>Those parameters will allow you to track specific information about your website's visitors

>To track your website traffic using Mango Analytics, you need to add the following Track script inside any page on your website:
```bash
<script>
var mangoDataPlatform = {
    // Function to retrieve a cookie value by name
    getCookie: function (name) {
        var cookieString = document.cookie;
        var cookies = cookieString.split('; ');
        for (var i = 0; i < cookies.length; i++) {
            var cookie = cookies[i].split('=');
            if (cookie[0] === name) {
                return decodeURIComponent(cookie[1]);
            }
        }
        return null;
    },

    // Function to generate a unique user ID
    generateUserId: function () {
        return 'user_' + Date.now() + '_' + Math.floor(Math.random() * 10000);
    },

    // Function to set a cookie with a given name, value, and expiration date
    setCookie: function (name, value, days) {
        var expires = new Date();
        expires.setTime(expires.getTime() + (days * 24 * 60 * 60 * 1000));
        document.cookie = name + '=' + encodeURIComponent(value) + ';expires=' + expires.toUTCString() + ';path=/';
    },

    // Function to send a POST request with JSON data
    sendData: function (url, data) {
        var xhr = new XMLHttpRequest();
        xhr.open('POST', url, true);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    console.log('Data sent successfully');
                } else {
                    console.error('Error sending data:', xhr.status);
                }
            }
        };
        xhr.send(JSON.stringify(data));
    },

    // Function to get or generate user ID
    getUserId: function () {
        var userId = mangoDataPlatform.getCookie('user_id');
        if (userId) {
            return userId;
        } else {
            var newUserId = mangoDataPlatform.generateUserId();
            mangoDataPlatform.setCookie('user_id', newUserId, 365);
            return newUserId;
        }
    },

    // Main function to fetch metadata, fill mydata object, and send data to API
    main: function () {
        var scriptElement = document.getElementById('tawsiyat-metadata');
        if (!scriptElement) {
            console.error('Metadata script element not found');
            return;
        }
        var scriptContent = scriptElement.textContent.trim();
        var metaData = JSON.parse(scriptContent);
        var postClasses = [];
        metaData.classes.forEach(function (item) {
            postClasses.push({ "mapping": item.mapping, "value": item.value });
        });
        //validations
        
        if (metaData.type === "page") {
            return;//data will not be collected if the type is page
        }
        
        if (metaData.thumbnail == null || metaData.thumbnail === "") {
            return;//data will not be collected if the post image is empty
        }
        //collect post tags incase it has no data it will return null and not empty array
        var postTags = metaData.keywords.trim() !== "" ? metaData.keywords.split(",") : null;

        var posttype = (metaData.classes.find(function (cls) {
            return cls.mapping === "posttype";// Get post type
        }) || {}).value || ""; // Get postType from classes meta data

        //to get data from articles only
        if (!posttype || posttype === 'الصفحات')
            return;
        var postCategory = (metaData.classes.find(function (cls) {
            return cls.mapping === "category";
        }) || {}).value || ""; // Get postCategory from classes meta data

        if (postCategory === "") {
            postCategory = posttype;// If postCategory is empty, set it to postType
        }

        // Fill the mydata object with metadata
        var mydata = {
            ip: "",//toDo change ip 
            postid: metaData.postid,
            postcategory: postCategory,
            postauthor: metaData.author,
            posttitle: metaData.title,
            domain: new URL(metaData.url).hostname,
            userId: mangoDataPlatform.getUserId(), // Get or generate user ID from cookie
            posturl: metaData.url,
            postimage: metaData.thumbnail,
            posttags: postTags,
            postpublishdate: metaData.published_time,
            postclasses: JSON.stringify(postClasses),
            posttype: posttype
        };
        
        // URL of the API endpoint
        var apiUrl = 'https://tracking-api.almayadeen.net/api/collect';
        //var apiUrl = "https://localhost:7043/api/collect";

        // Send the mydata object to the API endpoint
        mangoDataPlatform.sendData(apiUrl, mydata);
    }
};
// Call the main function to start the process
mangoDataPlatform.main();
</script>
```

>Once the Track Script is added into your website's page, you will initiate traffic tracking and have access to the analyzed data in a visually appealing and organized manner via the dynamic Analytics Page.

## 3. Analytics Page 
At Data Platform Analytics we not only provide APIs for data reporting and tracking but also offer comprehensive data integration services. Our on-demand data reports are visually represented in charts and tables that are easy to use and understand. The Analytics page presents a dynamic representation of recorded data with various visualizations such as charts and tables, displaying the stored records in a comprehensive manner. To make the experience even more user-friendly, we have added dynamic filtering options to the page, allowing users to customize their view and focus on the data that is relevant to them

### A.
![image](https://github.com/fatimashehab99/data-platform/blob/main/assets/1.png))
![image](https://github.com/fatimashehab99/data-platform/blob/main/assets/2.png))
![image](https://github.com/fatimashehab99/data-platform/blob/main/assets/3.png))
![image](https://github.com/fatimashehab99/data-platform/blob/main/assets/4.png))


>In conclusion, Data Platform Analytics is a comprehensive solution for data reporting and visualization, offering both powerful APIs and visually appealing chart displays. The project continues to evolve and improve, aiming to provide even more insights and value to its users. We hope you find Data Platform Analytics helpful and we welcome any feedback or suggestions. 

## Happy Tracking!

## Happy Analytics!

