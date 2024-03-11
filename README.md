# Mango Analytics

### Outline
### I. Introduction
- Overview of Mango Analytics

### II. Tracking API Usage <br/>

 A. Integration of Track Script <br/>
 B. Parameters for tracking data <br/>
 C. Example of Track Script <br/>
 D. Accessing analyzed data <br/>


### III. Analytics Visualization <br/>
   
 A. On-demand data reports <br/>
 B. Dynamic representation of recorded data <br/>
 C. Visualizations: Charts and Tables <br/>
 D. Dynamic filtering options <br/>
 E. Page Views Smooth Analytical Chart <br/>
 F. Website's Authors Analytical Chart <br/>
 G. Websites Visitor's Analytical Location Chart <br/>
 H. Page Views Column Analytical Chart <br/>
 I. Dynamic Table with server-side features <br/>
 J. Filters applied to charts and table <br/>


### IV. Conclusion

- Summary of Mango Analytics

___

## Overview
Mango Analytics is a project that provides reporting and tracking APIs along with full visual display of data in charts. It is a tool that allows users to collect and analyze data, create reports, and visualize the results in a clear and interactive manner. The goal of Mango Analytics is to help users make informed decisions based on data-driven insights by presenting data in an easy-to-understand format.

## 1. Tracking API Usage 

To utilize the analytics functionality, it is necessary to integrate the Track Script into your website. This script will collect the necessary data to generate the dynamic analytics charts and provide a comprehensive representation of the website's traffic.

The Mango Analytics script takes parameters in the body named as the following:
* PostId $~~~~~~~~~~~~~~~~~~~~~~~~~$*(This is responsible for handling the Post Id or Page Id)*
* postCategory $~~~~~~~~~~~$              *(This is responsible for handling the Post category or Page category)*
* Author $~~~~~~~~~~~~~~~~~~~~~~$                    *(This is responsible for handling the Author of the Post)*
* postTitle $~~~~~~~~~~~~~~~~~~~~$                 *(This is responsible for handling the Post title or Page title)*
* subscriptionName $~~~~~$          *(This is responsible for handling the Website Domain Name)*
* subscriptionId $~~~~~~~~~~$            *(This is responsible for handling the Website Subscription unique Id)*
* userId $~~~~~~~~~~~~~~~~~~~~~~$                    *(This is responsible for handling the distinct users accessing this Website)*
 

>Those parameters will allow you to track specific information about your website's visitors.The following example shows the needed parameters.

```bash

var mydata = 
{ 
          postid:'129t214h38q2a34',
          postcategory:'News',
          author:'MohamadHassan',
          posttitle:'Mango Analytics Impact',
          subscriptionName:'MangoAnalytics',
          subscriptionId='219ee194-0866-43cd-8318-18c514bfec0f',
          userId:'313aa122-0666-46cd-8468-68e514beecvt'
};


```

>To track your website traffic using Mango Analytics, you need to add the following Track script inside any page on your website:
```bash
<script>
               var mydata = 
                   { 
                     postid:PageId,
                     postcategory:PageCategory,
                     author:PageAuthor,
                     posttitle:Title,
                     subscriptionName:SubscriptionName,
                     subscriptionId=subscriptionId,
                     userId:userID
                   };
                var url = 'https://tracking-api.mangopulse.net/api/Track';
                $.ajax({
                    type: 'POST',
                    url: url,
                    data:JSON.stringify(mydata),
                    contentType: 'application/json',
                    success: function () {console.log('Tracking')},
                    error: function (error) { console.log(error); }
                });
</script>
```

>Once the Track Script is added into your website's page, you will initiate traffic tracking and have access to the analyzed data in a visually appealing and organized manner via the dynamic Analytics Page.

## 2. Analytics Page 
At mango Analytics we not only provide APIs for data reporting and tracking but also offer comprehensive data integration services. Our on-demand data reports are visually represented in charts and tables that are easy to use and understand. The Analytics page presents a dynamic representation of recorded data with various visualizations such as charts and tables, displaying the stored records in a comprehensive manner. To make the experience even more user-friendly, we have added dynamic filtering options to the page, allowing users to customize their view and focus on the data that is relevant to them

### A. Page Views Smooth Analytical Chart

<img src="https://user-images.githubusercontent.com/107344706/220045152-4fb41aaf-bdd9-4445-b728-7562807be71f.png" style=" width:900px ; height:500px "  >

___

### B. Website's Authors Analytical Chart
<img src="https://user-images.githubusercontent.com/107344706/216351108-26b8ff68-f842-45e2-88f5-79f80342ed7c.png" style=" width:600px ; height:500px "  >

___

### C. Websites Visitor's Analytical Location Chart
<img src="https://user-images.githubusercontent.com/107344706/216351325-570e4cea-91f2-481e-ace8-366d8992a321.png" style=" width:600px ; height:500px "  >

___

### D. Page Views Column Analytical Chart

<img src="https://user-images.githubusercontent.com/107344706/220045479-fc39da73-5293-493f-a020-78aaf9d7ea87.png" style=" width:1100px ; height:450px "  >

___

### E. Recorded Data displayed in a dynamic Table that supports serverside (pagination, search,and sorting)
<img src="https://user-images.githubusercontent.com/107344706/216351731-f18c0a2f-4bd8-4dfd-a830-5e3053abc3d7.png" style=" width:900px ; height:500px "  >

___

### F. Filters that are applied on all displayed charts and given table

<img src="https://user-images.githubusercontent.com/107344706/220044746-ddc2ab69-18ab-4173-a8d2-4e6a70c34550.png" style=" width:900px ; height:450px "  >

___

### G. Map Visualization Chart
<img src="https://user-images.githubusercontent.com/107344706/220044125-9a789ca2-5981-425b-9747-9d0e50291879.png" style=" width:1000px ; height:400px "  >

___

>In conclusion, Mango Analytics is a comprehensive solution for data reporting and visualization, offering both powerful APIs and visually appealing chart displays. The project continues to evolve and improve, aiming to provide even more insights and value to its users. We hope you find Mango Analytics helpful and we welcome any feedback or suggestions. 

## Happy Tracking!

## Happy Analytics!

<a href="https://imgflip.com/i/7a1bt3"><img src="https://i.imgflip.com/7a1bt3.jpg" title="made at imgflip.com"/></a><div><a href="https://imgflip.com/memegenerator">from Imgflip Meme Generator</a></div>
