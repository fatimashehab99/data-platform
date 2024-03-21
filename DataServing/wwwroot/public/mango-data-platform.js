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
        // Parse the metadata script element and extract its content
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
        if (metaData.type === "page") {
            return;
        }
        // Fill the mydata object with metadata
        var mydata = {
            ip: "109.75.64.0",//toDo change ip 
            postid: metaData.postid,
            postcategory: (metaData.classes.find(function (cls) {
                return cls.mapping === "category";
            }) || {}).value || "", //get postCategory from classes meta data
            postauthor: metaData.author,
            posttitle: metaData.title,
            domain: new URL(metaData.url).hostname,
            userId: mangoDataPlatform.getUserId(), // Get or generate user ID from cookie
            posturl: metaData.url,
            postimage: metaData.thumbnail,
            posttags: metaData.keywords.split(","),
            postpublishdate: metaData.published_time,
            postclasses: JSON.stringify(postClasses),
            posttype: (metaData.classes.find(function (cls) {
                return cls.mapping === "posttype";
            }) || {}).value || "", //get postType from classes meta data
        };

        // URL of the API endpoint
        //var apiUrl = 'https://tracking-api.almayadeen.net/api/collect';
        var apiUrl = "https://localhost:7043/api/collect";

        // Send the mydata object to the API endpoint
        mangoDataPlatform.sendData(apiUrl, mydata);
    }
};

// Call the main function to start the process
mangoDataPlatform.main();
