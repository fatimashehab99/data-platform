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


    }
}