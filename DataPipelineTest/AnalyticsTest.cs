using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataAnalysis.Services;
using DataPipeline.DataCollection.Models;

namespace DataPipelineTest
{

    public class AnalyticsTest : BaseTest
    {

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