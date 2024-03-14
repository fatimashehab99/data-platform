using DataPipeline.DataAnalysis.Models;

namespace DataPipelineTest
{

    public class AnalyticsTest : BaseTest
    {
        /// <summary>
        /// This function is used to test category pageviews function
        /// </summary>
        [Test]
        public void AnalyzePageViewsByCategory()
        {
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
            new CategoryPageView { Category = "News", PageViews = 3 },
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
            string date = "2024-03-05";
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate pageviews
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();
            var pageView5 = GeneratePageView();
            var pageView6 = GeneratePageView();

            //update domain
            pageView1.Domain = pageView2.Domain =
                pageView3.Domain = pageView4.Domain =
                pageView5.Domain = pageView6.Domain = domain;

            //update date
            pageView1.Formatted_Date = pageView2.Formatted_Date = pageView3.Formatted_Date = "2024-03-09";
            pageView4.Formatted_Date = pageView5.Formatted_Date = "2024-03-07";
            pageView6.Formatted_Date = "2024-03-06";

            //Save data to mongodb
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);
            _trackService.LogPageview(pageView5);
            _trackService.LogPageview(pageView6);


            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };
            //expected results
            var expectedResults = new List<DatePageView>
             {
            new DatePageView { Date = "2024-03-09", PageViews = 3 },
            new DatePageView { Date = "2024-03-07", PageViews = 2 },
            new DatePageView { Date = "2024-03-06", PageViews = 1 },
             };

            var pageviews = _analyticsService.AnalyzePageViewsByDate(criteria, date);

            Assert.That(pageviews != null, Is.True);

            //check results
            foreach (var expected in expectedResults)
            {
                var actual = pageviews.Find(c => c.Date == expected.Date);
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.PageViews, Is.EqualTo(expected.PageViews));
            }


        }
        [Test]
        public void AnalyticsPostByAuthor()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();


            var author1 = "fatima";
            var author2 = "sara";
            var author3 = "john";

            //generate pageview
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();
            var pageView5 = GeneratePageView();

            //update domain
            pageView1.Domain = pageView2.Domain =
                pageView3.Domain = pageView4.Domain = pageView5.Domain = domain;

            //update authors
            pageView1.PostAuthor = pageView2.PostAuthor = author1;
            pageView3.PostAuthor = author2;
            pageView4.PostAuthor = pageView5.PostAuthor = author3;

            //Save data to mongodb
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);
            _trackService.LogPageview(pageView5);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };

            //expected results
            var expectedResults = new List<AuthorPageView>
             {
            new AuthorPageView { Author = "fatima", PageViews = 2 },
            new AuthorPageView { Author = "sara", PageViews = 1 },
            new AuthorPageView { Author = "john", PageViews = 2 },
             };

            var authors = _analyticsService.AnalyseByAuthor(criteria);

            Assert.That(authors != null, Is.True);

            //check results
            foreach (var expected in expectedResults)
            {
                var actual = authors.Find(c => c.Author == expected.Author);
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.PageViews, Is.EqualTo(expected.PageViews));
            }
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
            int totalArticles = _dashboardStatisticsService.getTotalArticles(criteria);
            Assert.That(totalArticles == 3, Is.True);

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
            Assert.That(totalAuthors == 3, Is.True);

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
            pageView2.UserId = "1234";
            pageView3.UserId = pageView4.UserId = "12345";



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
        /// <summary>
        /// This function is used test AnalyzeLocationPageViews function
        /// </summary>
        [Test]
        public void AnalyzeLocationPageViews()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate pageviews
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();
            var pageView5 = GeneratePageView();

            //update domain
            pageView1.Domain = pageView2.Domain = pageView3.Domain = pageView4.Domain = pageView5.Domain = domain;

            //update ips 
            pageView1.Ip = pageView2.Ip = "102.129.65.0";
            pageView3.Ip = pageView4.Ip = pageView5.Ip = "109.172.22.0";

            //save data to mongo db
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);
            _trackService.LogPageview(pageView5);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain
            };
            //expected results
            var expectedResults = new List<CountryNamePageView>
             {
            new CountryNamePageView { CountryName = "Congo Republic", PageViews = 2 },
            new CountryNamePageView { CountryName = "Lebanon", PageViews = 3 },
             };

            var locations = _analyticsService.AnalyzePageViewsByCountryName(criteria);

            Assert.That(locations != null, Is.True);

            //check results
            foreach (var expected in expectedResults)
            {
                var actual = locations.Find(c => c.CountryName == expected.CountryName);
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.PageViews, Is.EqualTo(expected.PageViews));
            }


        }


    }
}