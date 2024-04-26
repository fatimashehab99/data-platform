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
            List<MongoDbPageView> pageviews = GeneratePageViews(4);

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

            //generate list of pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(4);

            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = domain;

            //update date
            pageviews[0].FormattedDate = pageviews[1].FormattedDate = "2024-04-12";
            pageviews[3].FormattedDate = "2024-04-25";
            pageviews[2].FormattedDate = "2024-03-01";

            //update authors
            pageviews[0].PostAuthor = pageviews[1].PostAuthor = pageviews[2].PostAuthor = "fatima";
            pageviews[3].PostAuthor = "sara";

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
            var results = _analyticsService.AnalyzeAuthorPageViews(criteria);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            results[0].Author.Should().Be("fatima");
            results[0].PageViews.Should().Be(2);

            results[1].Author.Should().Be("sara");
            results[1].PageViews.Should().Be(1);
        }
        /// <summary>
        /// This function is used to return the total articles
        /// </summary>
        [Test]
        public void getTotalArticles()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(4);

            //update post id
            for (int i = 0; i < pageviews.Count; i++)
            {
                pageviews[i].PostId = i.ToString();
            }

            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = domain;

            //update date
            pageviews[0].FormattedDate = pageviews[1].FormattedDate = "2024-04-12";
            pageviews[2].FormattedDate = "2024-04-25";
            pageviews[3].FormattedDate = "2024-03-01";

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

            int results = _dashboardStatisticsService.getTotalArticles(criteria);

            Assert.That(results == 3, Is.True);

        }

        /// <summary>
        /// This function is used to return the total authors
        /// </summary>
        [Test]
        public void getTotalAuthors()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(4);

            //update post_title
            pageviews[0].PostAuthor = pageviews[1].PostAuthor = "mark";
            pageviews[2].PostAuthor = "john";
            pageviews[3].PostAuthor = "sara";

            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = domain;

            //update date
            pageviews[0].FormattedDate = pageviews[1].FormattedDate = "2024-04-12";
            pageviews[2].FormattedDate = "2024-04-25";
            pageviews[3].FormattedDate = "2024-03-01";

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

            int results = _dashboardStatisticsService.getTotalAuthors(criteria);
            Assert.That(results == 2, Is.True);

        }
        /// <summary>
        /// This function is used to return the total pageviews
        /// </summary>
        [Test]
        public void getTotalPageViews()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(4);


            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = domain;

            //update date
            pageviews[0].FormattedDate = pageviews[1].FormattedDate = "2024-04-12";
            pageviews[2].FormattedDate = "2024-04-25";
            pageviews[3].FormattedDate = "2024-03-01";


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

            int results = _dashboardStatisticsService.getTotalPageViews(criteria);
            Assert.That(results == 3, Is.True);

        }
        /// <summary>
        /// This function is used to return the total users
        /// </summary>
        [Test]
        public void getTotalUsers()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(4);


            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = domain;

            //update date
            pageviews[0].FormattedDate = pageviews[1].FormattedDate = "2024-04-12";
            pageviews[2].FormattedDate = "2024-04-25";
            pageviews[3].FormattedDate = "2024-03-01";

            //update user id
            pageviews[0].UserId = pageviews[1].UserId = "123";
            pageviews[2].UserId = "1234";
            pageviews[3].UserId = "12345";


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

            int results = _dashboardStatisticsService.getTotalUsers(criteria);
            Assert.That(results == 2, Is.True);

        }
        /// <summary>
        /// This function is used test AnalyzeLocationPageViews function
        /// </summary>
        [Test]
        public void AnalyzeLocationPageViews()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(5);

            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = pageviews[4].Domain = domain;

            //update ips 
            pageviews[0].Ip = pageviews[1].Ip = "102.129.65.0";
            pageviews[2].Ip = pageviews[3].Ip = pageviews[4].Ip = "109.172.22.0";

            //update date
            pageviews[4].FormattedDate = pageviews[1].FormattedDate = pageviews[2].FormattedDate = "2024-04-05";
            pageviews[3].FormattedDate = "2024-04-20";
            pageviews[0].FormattedDate = "2024-03-01";

            //save data to mongo db
            savePageViews(pageviews);

            //Read data from mongodb
            SearchCriteria search = new()
            {
                Domain = domain,
                DateFrom = "2024-04-01",
                DateTo = "2024-04-25",
                Size = 10
            };
            var results = _analyticsService.AnalyzePageViewsByCountryName(search);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            results[0].CountryName.Should().Be("Lebanon");
            results[0].PageViews.Should().Be(3);

            results[1].CountryName.Should().Be("Congo Republic");
            results[1].PageViews.Should().Be(1);

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
            pageviews[4].PostType = "infographs";
            pageviews[2].PostType = pageviews[3].PostType = pageviews[1].PostType= "videos";

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
            results.Should().HaveCount(2);

            results[0].PostType.Should().Be("videos");
            results[0].PageViews.Should().Be(3);
            results[0].Posts.Should().Be(3);

            results[1].PostType.Should().Be("news");
            results[1].PageViews.Should().Be(1);
            results[1].Posts.Should().Be(1);


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
            pageviews[1].PostTags = ["Lebanon"];
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
            results.Should().HaveCount(3);

            results[0].tags.Should().Be("Lebanon");
            results[0].pageviews.Should().Be(3);

            results[1].tags.Should().Be("Palestine");
            results[1].pageviews.Should().Be(2);

            results[2].tags.Should().Be("South");
            results[2].pageviews.Should().Be(1);


        }
        /// <summary>
        /// This test is used to get top articles pageviews
        /// </summary>
        [Test]
        public void AnalyzeTopArticlesPageViews()
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

            //update post ID
            pageviews[0].PostId = pageviews[1].PostId = pageviews[2].PostId = "1";
             pageviews[4].PostId = "2";
            pageviews[3].PostId = "3";


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
            var results = _analyticsService.GetTopPageViewsArticles(search);
            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            results[0].PostId.Should().Be("1");
            results[0].PageViews.Should().Be(3);

            results[1].PostId.Should().Be("3");
            results[1].PageViews.Should().Be(1);

        }
    }
}