using DataPipeline.DataAnalysis.Models;
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
        /// This function is used to test user's top categories function
        /// </summary>
        [Test]
        public void getTopCategoriesForSpecificUser()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();
            var domain2 = "test.com" + Guid.NewGuid().ToString();


            //generate pageviews
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();
            var pageView5 = GeneratePageView();
            var pageView6 = GeneratePageView();
            var pageView7 = GeneratePageView();
            var pageView8 = GeneratePageView();

            //update domain
            pageView1.Domain = pageView2.Domain = pageView3.Domain = pageView4.Domain =
                pageView5.Domain = pageView6.Domain = pageView7.Domain = domain;
            pageView8.Domain = domain2;

            //update user id
            pageView1.UserId = pageView2.UserId = pageView3.UserId = pageView4.UserId =
                pageView5.UserId = pageView6.UserId = "123";
            pageView7.UserId = "2";

            //update categories
            pageView1.PostCategory = pageView2.PostCategory = pageView3.PostCategory = "News";
            pageView4.PostCategory = pageView5.PostCategory = "Sport";
            pageView6.PostCategory = "Fashion";
            pageView7.PostCategory = pageView8.PostCategory = "Technology";

            //generate pageviews
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);
            _trackService.LogPageview(pageView5);
            _trackService.LogPageview(pageView6);
            _trackService.LogPageview(pageView7);
            _trackService.LogPageview(pageView8);

            //expected results
            var expectedResults = new Dictionary<string, int>
            {
                {"News",3 },
                {"Sport",2 },
                {"Fashion",1 }

            };
            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };

            //get results
            var results = _userProfileDataService.getTopCategoriesForSpecificUser(criteria, "123");
            //check results
            CollectionAssert.AreEquivalent(expectedResults, results);
            Assert.That(results != null, Is.True);


        }
        /// <summary>
        /// This function is used to test user's top authors function
        /// </summary>
        [Test]
        public void getTopAuthorsForSpecificUser()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();
            var domain2 = "test.com" + Guid.NewGuid().ToString();


            //generate pageviews
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();
            var pageView5 = GeneratePageView();
            var pageView6 = GeneratePageView();
            var pageView7 = GeneratePageView();
            var pageView8 = GeneratePageView();

            //update domain
            pageView1.Domain = pageView2.Domain = pageView3.Domain = pageView4.Domain =
                pageView5.Domain = pageView6.Domain = pageView7.Domain = domain;
            pageView8.Domain = domain2;

            //update user id
            pageView1.UserId = pageView2.UserId = pageView3.UserId = pageView4.UserId =
                pageView5.UserId = pageView6.UserId = "123";
            pageView7.UserId = "2";

            //update categories
            pageView1.PostAuthor = pageView2.PostAuthor = pageView3.PostAuthor = "fatima";
            pageView4.PostAuthor = pageView5.PostAuthor = "hadi";
            pageView6.PostAuthor = "mhmd";
            pageView7.PostAuthor = pageView8.PostAuthor = "sara";

            //generate pageviews
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);
            _trackService.LogPageview(pageView5);
            _trackService.LogPageview(pageView6);
            _trackService.LogPageview(pageView7);
            _trackService.LogPageview(pageView8);

            //expected results
            var expectedResults = new Dictionary<string, int>
            {
                {"fatima",3 },
                {"hadi",2 },
                {"mhmd",1 }

            };
            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };

            //get results
            var results = _userProfileDataService.getTopAuthorsForSpecificUser(criteria, "123");
            //check results
            CollectionAssert.AreEquivalent(expectedResults, results);
            Assert.That(results != null, Is.True);
        }
        /// <summary>
        /// This functon is used to test getting top user's tags 
        /// </summary>

        [Test]
        public void getTopTagsForSpecificUser()
        {
            var domain = "test.com" + Guid.NewGuid().ToString();


            //generate pageviews
            var pageView1 = GeneratePageView();
            var pageView2 = GeneratePageView();
            var pageView3 = GeneratePageView();
            var pageView4 = GeneratePageView();
            var pageView5 = GeneratePageView();


            //update domain
            pageView1.Domain = pageView2.Domain = pageView3.Domain = pageView4.Domain =
                pageView5.Domain = domain;

            //update tags
            pageView1.PostTags = ["tag 1", "tag 2", "tag 3"];
            pageView2.PostTags = ["tag 1", "tag 2", "tag 3"];
            pageView3.PostTags = ["tag 1", "tag 2", "tag 4"];
            pageView4.PostTags = ["tag 1", "tag 5", "tag 6"];
            pageView3.PostTags = ["tag 1", "tag 2", "tag 4"];

            //update user id
            pageView1.UserId = pageView2.UserId = pageView3.UserId = pageView4.UserId = "123";
            pageView5.UserId = "1234";

            //generate pageviews
            _trackService.LogPageview(pageView1);
            _trackService.LogPageview(pageView2);
            _trackService.LogPageview(pageView3);
            _trackService.LogPageview(pageView4);
            _trackService.LogPageview(pageView5);

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
            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };

            //get results
            var results = _userProfileDataService.getTopTagsForSpecificUser(criteria, "123", 10);
            //check results
            CollectionAssert.AreEquivalent(expectedResults, results);
            Assert.That(results != null, Is.True);
        }
    }
}
