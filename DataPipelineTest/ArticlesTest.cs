using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataCollection.Models;
using FluentAssertions;

namespace DataPipelineTest
{
    public class ArticlesTest : BaseTest
    {
        /// <summary>
        /// This test is used to test trending articles
        /// </summary>
        [Test]
        public void getTrendingArticles()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate list of pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(8);


            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = pageviews[4].Domain = pageviews[5].Domain = pageviews[6].Domain =
                pageviews[7].Domain = domain;

            //update ip m
            pageviews[0].Ip = pageviews[1].Ip = pageviews[2].Ip = pageviews[3].Ip = pageviews[4].Ip = "109.75.64.0";
            pageviews[5].Ip = pageviews[6].Ip = pageviews[7].Ip = "102.129.65.0";

            //update post id
            pageviews[0].PostId = pageviews[1].PostId = "1";
            pageviews[2].PostId = pageviews[3].PostId = pageviews[4].PostId = "2";
            pageviews[5].PostId = pageviews[6].PostId = pageviews[7].PostId = "3";

            //save data to mongo db
            savePageViews(pageviews);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };

            //get articles with page views
            var results = _articlesService.getTrendingArticles(criteria, "109.75.64.0", 4);
            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(2);

            results[0].PostId.Should().Be("2");
            results[0].PageViews.Should().Be(3);

            results[1].PostId.Should().Be("1");
            results[1].PageViews.Should().Be(2);
        }
        /// <summary>
        /// This function is used to test the get recommendation articles API
        /// </summary>
        [Test]
        public void getRecommendedArticles()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();

            //generate list of pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(6);


            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain =
                pageviews[3].Domain = pageviews[4].Domain = pageviews[5].Domain = domain;

            //update user id

            pageviews[0].UserId = pageviews[1].UserId = pageviews[2].UserId = pageviews[3].UserId;
            pageviews[4].UserId = pageviews[5].UserId = "1234";

            //update categories
            pageviews[0].PostCategory = pageviews[1].PostCategory = pageviews[2].PostCategory = "News";
            pageviews[3].PostCategory = pageviews[4].PostCategory = pageviews[5].PostCategory = "Sport";


            //update authors
            pageviews[0].PostAuthor = pageviews[1].PostAuthor = pageviews[3].PostAuthor = "fatima";
            pageviews[2].PostAuthor = pageviews[4].PostAuthor = "sara";
            pageviews[5].PostAuthor = "fadi";

            //update tags 
            pageviews[0].PostTags = ["war", "south"];
            pageviews[1].PostTags = ["war", "palestine"];
            pageviews[2].PostTags = ["war", "lebanon"];
            pageviews[3].PostTags = ["war", "lebanon", "south"];
            pageviews[4].PostTags = ["football", "basketball"];
            pageviews[5].PostTags = ["barcelona"];

            //update post id
            for (int i = 0; i < pageviews.Count; i++)
            {
                pageviews[i].PostId = i.ToString();
            }

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };

            //save data to mongo db
            savePageViews(pageviews);

            //get user's data
            Dictionary<string, int> topCategories = _userProfileDataService.getTopCategoriesForSpecificUser(criteria, "123");
            Dictionary<string, int> topAuthors = _userProfileDataService.getTopAuthorsForSpecificUser(criteria, "123");
            Dictionary<string, int> topTags = _userProfileDataService.getTopTagsForSpecificUser(criteria, "123", 10);

            //execute the function
            var results = _articlesService.getRecommendedArticles(criteria, "123", "109.75.64.0", 4);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(4);

        }
    }
}
