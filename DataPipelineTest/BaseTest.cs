using DataPipeline;
using DataPipeline.DataAnalysis.Services;
using DataPipeline.DataCollection.Models;
using DataPipeline.DataCollection.Services;
using DataPipeline.Helpers.LocationService;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using FluentAssertions;
using FluentAssertions.Specialized;
using System.Drawing;


namespace DataPipelineTest
{
    /// <summary>
    /// This class is used to write rhe common functions
    /// btwn tests
    /// </summary>
    public class BaseTest
    {
        private ServiceProvider _serviceProvider;



        public IDataCollectionService _trackService;
        public ILocationService _locationService;
        public IDataAnalyticsService _analyticsService;
        public IDashboardStatisticsService _dashboardStatisticsService;
        public IArticlesService _articlesService;
        public IUserProfileDataService _userProfileDataService;
        public IMemoryCache _cache;


        [OneTimeSetUp]
        public void SetUp()
        {
            SetupTest();
        }
        private T GetService<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public void SetupTest()
        {
            var services = new ServiceCollection();
            services.Configure<DatabaseConnecting>
            (con =>
            {
                con.ConnectionString = "mongodb://localhost:27017";
                con.DatabaseName = "mangopulse-analytics";
                con.CollectionName = "MangoAnalyticsDBTests";
            }
            );
            services.AddHttpContextAccessor();

            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IDataCollectionService, DataCollectionService>();
            services.AddTransient<IDataAnalyticsService, DataAnalyticsService>();
            services.AddTransient<IDashboardStatisticsService, DashboardStatisticsService>();
            services.AddTransient<IArticlesService, ArticlesService>();
            services.AddTransient<IUserProfileDataService, UserProfileDataService>();
            services.AddTransient<IMemoryCache, MemoryCache>();


            _serviceProvider = services.BuildServiceProvider();

            _analyticsService = GetService<IDataAnalyticsService>();
            _trackService = GetService<IDataCollectionService>();
            _locationService = GetService<ILocationService>();
            _dashboardStatisticsService = GetService<IDashboardStatisticsService>();
            _articlesService = GetService<IArticlesService>();
            _userProfileDataService = GetService<IUserProfileDataService>();
            _cache = GetService<IMemoryCache>();
        }
        /// <summary>
        /// This function is used  to test the database connection
        /// </summary>
        [Test]
        public void testDatabaseConnection()
        {
            //create a new database collection
            var invalidMongoConnection = new DatabaseConnecting()
            {
                ConnectionString = "test",
                CollectionName = "test"
            };


            var exeption = Assert.Throws<MongoConfigurationException>(() =>
            _trackService.setUpMongoDb(invalidMongoConnection));
            Assert.That(exeption.Message, Is.EqualTo("The connection string 'test' is not valid."));

        }
        /// <summary>
        /// This function is used to check if collection exists or not
        /// </summary>
        [Test]
        public void if_no_collection_name_code_should_throw_exception()
        {

            var exception = Assert.Throws<Exception>(()
                => _trackService.setUpMongoDb(new DatabaseConnecting()));

            Assert.That(exception.Message, Is.EqualTo(Constants.ERROR_COLLECTION_IS_EMPTY));

        }
        /// <summary>
        /// This function is used to generate a new page view
        /// </summary>
        /// <returns></returns>
        public MongoDbPageView GeneratePageView()
        {
            MongoDbPageView page = new MongoDbPageView();

            page.PostAuthor = "demo";
            page.Ip = "109.75.64.0";
            page.UserId = "123";
            page.PostId = Guid.NewGuid().ToString();
            page.PostTitle = "Post Title Mango";
            page.PostCategory = "Mango Category";
            page.Domain = "Sub Name";
            page.Domain = "102-123-123";
            page.Browser = "chrome";
            page.Device = "iphone";
            page.Platform = "operating";
            page.PostUrl = "testUrl.com";
            page.PostImage = "image";
            page.PostTags = ["tag1", "tag2", "tag3"];
            page.PostType = "news";
            //--Set Date on Current Get-->
            page.Date = DateTime.Now.ToLocalTime();
            return page;
        }
        /// <summary>
        /// This function is used to generate list of page views
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<MongoDbPageView> GeneratePageViews(int size)
        {
            List<MongoDbPageView> pageviews = new List<MongoDbPageView>();
            for (int i = 0; i < size; i++)
            {
                pageviews.Add(GeneratePageView());
            }
            return pageviews;
        }
        /// <summary>
        /// This function is used to save a list of page views
        /// </summary>
        /// <param name="pageviews"></param>
        public void savePageViews(List<MongoDbPageView> pageviews)
        {
            for (int i = 0; i < pageviews.Count; i++)
            {
                _trackService.LogPageview(pageviews[i]);
            }

        }




        [TearDown]
        [OneTimeTearDown]
        public void TearDown()
        {
            _serviceProvider.Dispose();
            _cache.Dispose();

        }

    }
}
