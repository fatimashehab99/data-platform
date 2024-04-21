using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataCollection.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipelineTest
{
    public class UserProfileTest : BaseTest
    {
        /// <summary>
        /// This function is used to test if a specific user visted the domain before or not
        /// </summary>
        [Test]
        public void checkUser()
        {
            //set user id
            string userId = Guid.NewGuid().ToString();
            var domain = "test.com" + Guid.NewGuid().ToString();
            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };
            //execute the function
            var results = _userProfileDataService.checkUser(criteria, userId);
            Assert.That(results, Is.False);

        }

        /// <summary>
        /// This function is used to test user's top categories function
        /// </summary>
        [Test]
        public void getTopCategoriesForSpecificUser()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();
            var domain2 = "test.com" + Guid.NewGuid().ToString();


            //generate list of pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(8);
            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain = pageviews[3].Domain =
                pageviews[4].Domain = pageviews[5].Domain = pageviews[6].Domain = domain;
            pageviews[7].Domain = domain2;

            //update user id
            pageviews[0].UserId = pageviews[1].UserId = pageviews[2].UserId = pageviews[3].UserId =
                pageviews[4].UserId = pageviews[5].UserId = "123";
            pageviews[6].UserId = "2";

            //update categories
            pageviews[0].PostCategory = pageviews[1].PostCategory = pageviews[2].PostCategory = "News";
            pageviews[3].PostCategory = pageviews[4].PostCategory = "Sport";
            pageviews[5].PostCategory = "Fashion";
            pageviews[6].PostCategory = pageviews[7].PostCategory = "Technology";

            //save data to mongo db
            savePageViews(pageviews);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };
            // Create an expected dictionary
            var expectedResults = new Dictionary<string, int>
                 {
                       { "News", 3 },
                       { "Sport", 2 },
                       { "Fashion", 1 }
                  };

            //get results
            var results = _userProfileDataService.getTopCategoriesForSpecificUser(criteria, "123");
            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(3);

            results.Should().BeEquivalentTo(expectedResults);
        }
        /// <summary>
        /// This function is used to test user's top authors function
        /// </summary>
        [Test]
        public void getTopAuthorsForSpecificUser()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();
            var domain2 = "test.com" + Guid.NewGuid().ToString();

            //generate list of pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(8);
            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain = pageviews[3].Domain =
                pageviews[4].Domain = pageviews[5].Domain = pageviews[6].Domain = domain;
            pageviews[7].Domain = domain2;

            //update user id
            pageviews[0].UserId = pageviews[1].UserId = pageviews[2].UserId = pageviews[3].UserId =
                pageviews[4].UserId = pageviews[5].UserId = "123";
            pageviews[6].UserId = "2";

            //update categories
            pageviews[0].PostAuthor = pageviews[1].PostAuthor = pageviews[2].PostAuthor = "fatima";
            pageviews[3].PostAuthor = pageviews[4].PostAuthor = "hadi";
            pageviews[5].PostAuthor = "mhmd";
            pageviews[6].PostAuthor = pageviews[7].PostAuthor = "sara";

            //save data to mongo db
            savePageViews(pageviews);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };
            //expected results
            var expectedResults = new Dictionary<string, int>
            {
                {"fatima",3 },
                {"hadi",2 },
                {"mhmd",1 }

            };

            //get results
            var results = _userProfileDataService.getTopAuthorsForSpecificUser(criteria, "123");
            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(3);

            results.Should().BeEquivalentTo(expectedResults);
        }
        /// <summary>
        /// This functon is used to test getting top user's tags 
        /// </summary>

        [Test]
        public void getTopTagsForSpecificUser()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();


            //generate list of pageviews
            List<MongoDbPageView> pageviews = GeneratePageViews(5);
            //update domain
            pageviews[0].Domain = pageviews[1].Domain = pageviews[2].Domain = pageviews[3].Domain =
                pageviews[4].Domain = domain;

            //update tags
            pageviews[0].PostTags = ["tag 1", "tag 2", "tag 3"];
            pageviews[1].PostTags = ["tag 1", "tag 2", "tag 3"];
            pageviews[2].PostTags = ["tag 1", "tag 2", "tag 4"];
            pageviews[3].PostTags = ["tag 1", "tag 5", "tag 6"];
            pageviews[4].PostTags = ["tag 1", "tag 2", "tag 4"];

            //update user id
            pageviews[0].UserId = pageviews[1].UserId = pageviews[2].UserId = pageviews[3].UserId = "123";
            pageviews[4].UserId = "1234";

            //save data to mongo db
            savePageViews(pageviews);

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };

            //expected results
            var expectedResults = new Dictionary<string, int>
            {
                {"tag 1",4 },
                {"tag 2",3 },
                {"tag 3",2 },
                {"tag 4",1 },
                {"tag 5",1 },
                {"tag 6",1 }

            };
            //get results
            var results = _userProfileDataService.getTopTagsForSpecificUser(criteria, "123", 10);
            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(6);

            results.Should().BeEquivalentTo(expectedResults);
        }
    }
}
