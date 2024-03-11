using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataAnalysis.Services;
using DataPipeline.DataCollection.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataPipelineTest
{

    public class AnalyticsTest : BaseTest
    {
        /// <summary>
        /// This function is used 
        /// </summary>
        [Test]
        public void AnalyzePageViewsByCategory()
        {
            //generate pageviews
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate pageviews
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();

            //update domain
            pageView1.Domain = domain;
            pageView2.Domain = domain;
            pageView3.Domain = domain;
            pageView4.Domain = domain;

            //update categories
            pageView1.PostCategory = pageView2.PostCategory = pageView4.PostCategory = "News";
            pageView3.PostCategory = "Sport";

            //generate pageviews
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);
            //expected results
            var expectedResults = new List<CategoryPageView>
             {
            new CategoryPageView { Category = "New", PageViews = 3 },
            new CategoryPageView { Category = "Sport", PageViews = 1 },
             };

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };

            //get categories with total pageviews
            var results = _analyticsService.AnalyzePageViewsByCategory(criteria);
            Assert.That(results != null, Is.True);
            //check results
            foreach (var expected in expectedResults)
            {
                var actual = results.Find(c => c.Category == expected.Category);
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.PageViews, Is.EqualTo(expected.PageViews));
            }


        }
        /// <summary>
        /// This function is added to test the results of the AnalyzePageViewsByDate function
        /// </summary>
        [Test]
        public void AnalyzePageViewsByDateTest()
        {
            var domain = "example.com";
            //write logs to mongo db
            //_trackService.LogPageview(pageView1);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };
            var pageviews = _analyticsService.AnalyzePageViewsByDate(criteria);

            Assert.That(pageviews != null, Is.True);
            Assert.That(pageviews.First().PageViews == 2, Is.True);


        }
        [Test]
        public void AnalyticsPostByAuthor()
        {
            var domain = "example.com" + Guid.NewGuid().ToString();


            var author1 = "fatima";

            //generate pageview
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();

            pageView1.PostAuthor = author1;
            pageView1.Domain = domain;


            pageView2.PostAuthor = author1;
            pageView2.Domain = domain;


            //Save data to mongodb
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);



            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
                Author = author1,
                From = DateTime.UtcNow.AddDays(-10),
                To = DateTime.UtcNow
            };

            var authors = _analyticsService.AnalyseByAuthor(criteria);

            Assert.That(authors != null, Is.True);

            Assert.That(authors.First().PageViews == 2, Is.True);
        }
        /// <summary>
        /// This function is used to return the total articles
        /// </summary>
        [Test]
        public void getTotalArticles()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();
            //generate pageviews
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();

            //update post_title
            pageView1.PostTitle = pageView2.PostTitle = "article1";
            pageView3.PostTitle = "article2";
            pageView4.PostTitle = "article3";

            //update domain
            pageView1.Domain = pageView2.Domain= pageView3.Domain= pageView4.Domain=domain;


            //Save data to mongodb
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain
            };
            int totalArticles=_dashboardStatisticsService.getTotalArticles(criteria);
            Assert.That(totalArticles==3, Is.True);

        }
        /// <summary>
        /// This function is used to return the total authors
        /// </summary>
        [Test]
        public void getTotalAuthors()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();
            //generate pageviews
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();

            //update post_title
            pageView1.PostAuthor = pageView2.PostAuthor = "mark";
            pageView3.PostAuthor = "john";
            pageView4.PostAuthor = "sara";

            //update domain
            pageView1.Domain = pageView2.Domain = pageView3.Domain = pageView4.Domain = domain;


            //Save data to mongodb
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain
            };
            int totalAuthors = _dashboardStatisticsService.getTotalAuthors(criteria);
            Assert.That(totalAuthors== 3, Is.True);

        }
        /// <summary>
        /// This function is used to return the total pageviews
        /// </summary>
        [Test]
        public void getTotalPageViews()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();
            //generate pageviews
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();

       
            //update domain
            pageView1.Domain = pageView2.Domain = pageView3.Domain = pageView4.Domain = domain;


            //Save data to mongodb
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain
            };
            int totalPageViews = _dashboardStatisticsService.getTotalPageViews(criteria);
            Assert.That(totalPageViews == 4, Is.True);

        }
        /// <summary>
        /// This function is used to return the total users
        /// </summary>
        [Test]
        public void getTotalUsers()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();
            //generate pageviews
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();


            //update domain
            pageView1.Domain = pageView2.Domain = pageView3.Domain = pageView4.Domain = domain;

            //update userid
            pageView1.UserId = "123";
            pageView2.UserId="1234";
            pageView3.UserId = pageView4.UserId= "12345";



            //Save data to mongodb
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain
            };
            int totalPageViews = _dashboardStatisticsService.getTotalUsers(criteria);
            Assert.That(totalPageViews == 3, Is.True);

        }


    }
}