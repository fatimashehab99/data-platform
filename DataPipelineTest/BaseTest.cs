using DataPipeline;
using DataPipeline.DataAnalysis.Services;
using DataPipeline.DataCollection.Models;
using DataPipeline.DataCollection.Services;
using DataPipeline.Helpers.LocationService;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                con.CollectionName = "MangoAnalyticsDB";
            }
            );
            services.AddHttpContextAccessor();

            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IDataCollectionService, DataCollectionService>();
            services.AddTransient<IDataAnalyticsService, DataAnalyticsService>();
            services.AddTransient<IDashboardStatisticsService, DashboardStatisticsService>();

            _serviceProvider = services.BuildServiceProvider();

            _analyticsService = GetService<IDataAnalyticsService>();
            _trackService = GetService<IDataCollectionService>();
            _locationService = GetService<ILocationService>();
            _dashboardStatisticsService=GetService<IDashboardStatisticsService>();
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
            //--Set Date on Current Get-->
            page.Date = DateTime.Now.ToLocalTime();
            return page;
        }



        [TearDown]
        [OneTimeTearDown]
        public void TearDown()
        {
            _serviceProvider.Dispose();
        }

    }
}
