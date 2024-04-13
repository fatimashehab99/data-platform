using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataCollection.Models;
using FluentAssertions;

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

            //generate list of pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(5);

            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = domain;

            //update date
            pageviews[0].FormattedDate = pageviews[1].FormattedDate = "2024-04-12";
            pageviews[3].FormattedDate = "2024-04-25";
            pageviews[2].FormattedDate = "2024-03-01";

            //update categories
            pageviews[0].PostCategory = pageviews[1].PostCategory = pageviews[2].PostCategory = "News";
            pageviews[3].PostCategory = "Sport";

            //generate pageviews
            savePageViews(pageviews);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
                DateFrom = "2024-04-01",
                DateTo = "2024-05-01",
                Size = 10
            };

            //get categories with total pageviews
            var results = _analyticsService.AnalyzePageViewsByCategory(criteria);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            results[0].Category.Should().Be("News");
            results[0].PageViews.Should().Be(2);

            results[1].Category.Should().Be("Sport");
            results[1].PageViews.Should().Be(1);

        }
        /// <summary>
        /// This function is added to test the results of the AnalyzePageViewsByDate function
        /// </summary>
        [Test]
        public void AnalyzePageViewsByDateTest()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate list of pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(5);

            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = pageviews[4].Domain = domain;

            //update date
            pageviews[0].FormattedDate = pageviews[1].FormattedDate = pageviews[2].FormattedDate = "2024-04-12";
            pageviews[3].FormattedDate = "2024-04-11";
            pageviews[4].FormattedDate = "2024-03-01";

            //update post type
            pageviews[0].PostType = pageviews[1].PostType = pageviews[2].PostType = pageviews[4].PostType = "news";
            pageviews[2].PostType = "sport";

            //update post id
            for (int i = 0; i < pageviews.Count; i++)
            {
                pageviews[i].PostId = i.ToString();
            }

            //save data to mongo db
            savePageViews(pageviews);

            SearchCriteria search = new()
            {
                Domain = domain,
                DateFrom = "2024-04-02",
                Size = 10,
                PostType = "news"
            };

            //get results
            var results = _analyticsService.AnalyzePageViewsByDate(search);
            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            results[0].Date.Should().Be("2024-04-12");
            results[0].PageViews.Should().Be(2);

            results[1].Date.Should().Be("2024-04-11");
            results[1].PageViews.Should().Be(1);
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

            var authors = _analyticsService.AnalyseByAuthor(criteria, 10);

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

            var locations = _analyticsService.AnalyzePageViewsByCountryName(criteria, 10);

            Assert.That(locations != null, Is.True);

            //check results
            foreach (var expected in expectedResults)
            {
                var actual = locations.Find(c => c.CountryName == expected.CountryName);
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.PageViews, Is.EqualTo(expected.PageViews));
            }

        }

        /// <summary>
        /// This function is used to check teh results of the get posttype API
        /// </summary>
        [Test]
        public void AnalyzePageViewsByPostType()
        {

            var domain = "test.com" + Guid.NewGuid().ToString();
            //generate list of pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(5);

            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = pageviews[4].Domain = domain;

            //update date
            pageviews[0].FormattedDate = pageviews[1].FormattedDate = pageviews[2].FormattedDate = "2024-04-05";
            pageviews[3].FormattedDate = "2024-04-20";
            pageviews[4].FormattedDate = "2024-03-01";
            //update postType
            pageviews[0].PostType = "news";
            pageviews[1].PostType = "articles";
            pageviews[4].PostType = "infographs";
            pageviews[2].PostType = pageviews[3].PostType = "videos";

            //update post id
            for (int i = 0; i < pageviews.Count; i++)
            {
                pageviews[i].PostId = i.ToString();
            }


            //save data to mongo db
            savePageViews(pageviews);


            SearchCriteria search = new()
            {
                Domain = domain,
                DateFrom = "2024-04-01",
                DateTo = "2024-04-25",
                Size = 10
            };

            var results = _analyticsService.AnalyzePageViewsByPostType(search);
            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(3);

            results[0].PostType.Should().Be("videos");
            results[0].PageViews.Should().Be(2);
            results[0].Posts.Should().Be(2);

            results[1].PostType.Should().Be("news");
            results[1].PageViews.Should().Be(1);
            results[1].Posts.Should().Be(1);

            results[2].PostType.Should().Be("articles");
            results[2].PageViews.Should().Be(1);
            results[2].Posts.Should().Be(1);

        }
        /// <summary>
        /// This function is used to check the results of the get tags pageviews API
        /// </summary>
        [Test]
        public void AnalyzePageViewsByTags()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate list of pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(5);

            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = pageviews[4].Domain = domain;

            //update date
            pageviews[0].FormattedDate = pageviews[1].FormattedDate = pageviews[2].FormattedDate = "2024-04-05";
            pageviews[3].FormattedDate = "2024-04-20";
            pageviews[4].FormattedDate = "2024-03-01";

            //update post type
            pageviews[0].PostType = pageviews[1].PostType = pageviews[2].PostType = pageviews[4].PostType = "news";
            pageviews[3].PostType = "sport";

            //update tags
            pageviews[0].PostTags = ["Lebanon", "Palestine", "South"];
            pageviews[1].PostTags = ["Lebanon", "war"];
            pageviews[2].PostTags = ["Lebanon", "Palestine"];
            pageviews[3].PostTags = ["Lebanon"];
            pageviews[4].PostTags = ["war"];

            //save data to mongo db
            savePageViews(pageviews);

            SearchCriteria search = new()
            {
                Domain = domain,
                DateFrom = "2024-04-01",
                DateTo = "2024-04-25",
                Size = 50,
                PostType = "news"
            };

            //get results
            var results = _analyticsService.AnalyzePageViewsByTag(search);
            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(4);

            results[0].tags.Should().Be("Lebanon");
            results[0].pageviews.Should().Be(3);

            results[1].tags.Should().Be("Palestine");
            results[1].pageviews.Should().Be(2);

            results[2].tags.Should().Be("South");
            results[2].pageviews.Should().Be(1);

            results[3].tags.Should().Be("war");
            results[3].pageviews.Should().Be(1);


        }
    }
}