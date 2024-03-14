using DataPipeline.DataAnalysis.Models;

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
                pageView5.Domain = pageView6.Domain = pageView7.Domain = pageView8.Domain = domain;

            //update ip 
            pageView1.Ip = pageView2.Ip = pageView3.Ip = pageView4.Ip = pageView5.Ip =
                pageView6.Ip = "109.75.64.0";
            pageView7.Ip = pageView8.Ip = "102.129.65.0";

            //update post id
            pageView1.PostId = pageView2.PostId = "1";
            pageView3.PostId = pageView4.PostId = pageView5.PostId = "2";
            pageView6.PostId = pageView7.PostId = pageView8.PostId = "3";

            //update post title
            pageView1.PostTitle = pageView2.PostTitle = "article 1";
            pageView3.PostTitle = pageView4.PostTitle = pageView5.PostTitle = "article 2";
            pageView6.PostTitle = pageView7.PostTitle = pageView8.PostTitle = "article 3";


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
            var expectedResults = new List<ArticlePageView>
             {
            new ArticlePageView { PostTitle = "article 1", PageViews = 2 },
            new ArticlePageView { PostTitle = "article 2", PageViews = 3 },
             };

            //Read data from mongodb
            SearchCriteria criteria = new()
            {
                Domain = domain,
            };

            //get articles with page views
            var results = _articlesService.getTrendingArticles(criteria,"109.75.64.0");
            Assert.That(results != null, Is.True);
            //check results
            foreach (var expected in expectedResults)
            {
                var actual = results.Find(c => c.PostTitle == expected.PostTitle);
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.PageViews, Is.EqualTo(expected.PageViews));
            }





        }
    }
}
