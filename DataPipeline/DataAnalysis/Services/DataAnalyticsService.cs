using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataCollection.Models;
using LinqKit;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataPipeline.DataAnalysis.Services
{


    public class DataAnalyticsService : IDataAnalyticsService
    {
        private readonly IMongoCollection<MongoDbPageView> _collection;
        private readonly IDashboardStatisticsService _dashboardStatisticsService;


        public DataAnalyticsService(IOptions<DatabaseConnecting> DatabaseSettings, IDashboardStatisticsService dashboardStatisticsService)
        {
            //mongo DB connection 
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);
            ///get pageView collection 
            _collection = mongoDatabase.GetCollection<MongoDbPageView>(DatabaseSettings.Value.CollectionName);
            _dashboardStatisticsService = dashboardStatisticsService;
        }

        public List<AuthorPageView> AnalyseByAuthor(SearchCriteria criteria, int dataSize)
        {
            // Define the aggregation pipeline stages
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
             {
               { Constants.DOMAIN, criteria.Domain },
                { Constants.POST_AUTHOR, new BsonDocument(Constants.NOT, BsonNull.Value) }
                     });

            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument
                        {
                            { Constants.ID, "$" + Constants.POST_AUTHOR }, // Group by author
                            { Constants.TOTAL_PAGE_VIEWS, new BsonDocument(Constants.SUM, 1) } // Sum the PageView values for each author
                        });
            //order by total pageviews
            var orderByStage = new BsonDocument(Constants.SORT, new BsonDocument(Constants.TOTAL_PAGE_VIEWS, -1));
            //limit 10 records
            var limitStage = new BsonDocument(Constants.LIMIT, 10);

            var pipeline = new[] { matchStage, groupStage, orderByStage, limitStage };

            List<BsonDocument> pResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();

            var results = new List<AuthorPageView>();

            foreach (BsonDocument pResult in pResults)
            {
                results.Add(new AuthorPageView
                {
                    Author = pResult[Constants.ID].AsString, // Get the author from the _id field
                    PageViews = pResult[Constants.TOTAL_PAGE_VIEWS].AsInt32 // Get the PageView value
                });
            }

            return results;
        }
        public List<DatePageView> AnalyzePageViewsByDate(SearchCriteria criteria, string date, int dataSize)
        {
            //Define the aggregation pipeline stages
            //first we need to filter data by domain
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
                {
                  { Constants.DOMAIN, criteria.Domain },
                  { Constants.FORMATTED_DATE, new BsonDocument(Constants.GREATER, date) }
                   });
            //now we need to get the count of page views for each day
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument
            {
                {Constants.ID,"$"+Constants.FORMATTED_DATE},//group by date
                {Constants.TOTAL_PAGE_VIEWS,new BsonDocument(Constants.SUM,1) }//count the pageviews

            });
            ///now we need to order the results by date
            var orderByStage = new BsonDocument(Constants.SORT, new BsonDocument(Constants.TOTAL_PAGE_VIEWS, -1));
            //limit 10 records
            var limitStage = new BsonDocument(Constants.LIMIT, dataSize);
            //initialize the pipeline
            var pipeline = new[] { matchStage, groupStage, limitStage, orderByStage };
            //execute the pipeline then store the results in list 
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();
            var results = new List<DatePageView>();
            //loop over the results to store them in DatePageView List
            foreach (BsonDocument pipelineResult in pipelineResults)
            {
                results.Add(new DatePageView
                {
                    Date = pipelineResult[Constants.ID].AsString,
                    PageViews = pipelineResult[Constants.TOTAL_PAGE_VIEWS].AsInt32

                });
            }
            return results;

        }

        public List<CategoryPageView> AnalyzePageViewsByCategory(SearchCriteria criteria, int dataSize)
        {
            //Define the aggregation pipeline stages
            //first we need to filter data by domain
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
                {
                  { Constants.DOMAIN, criteria.Domain },
                  {Constants.Category,new BsonDocument(Constants.NOT, BsonNull.Value) }
                   });
            //now we need to get the count of pageviews for each category 
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument
            {
                {Constants.ID,"$" + Constants.Category },//group by date
                {Constants.TOTAL_PAGE_VIEWS,new BsonDocument(Constants.SUM,1) }//count the pageviews

            });
            var orderByStage = new BsonDocument(Constants.SORT, new BsonDocument(Constants.TOTAL_PAGE_VIEWS, -1));
            var limitStage = new BsonDocument(Constants.LIMIT, dataSize);
            //initialize the pipeline
            var pipeline = new[] { matchStage, groupStage, orderByStage, limitStage };
            //execute the pipeline then store the results in list 
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();
            var results = new List<CategoryPageView>();
            //loop over the results to store them in DatePageView List
            foreach (BsonDocument pipelineResult in pipelineResults)
            {
                results.Add(new CategoryPageView
                {
                    Category = pipelineResult[Constants.ID].AsString,
                    PageViews = pipelineResult[Constants.TOTAL_PAGE_VIEWS].AsInt32

                });
            }
            return results;
        }
        public List<CountryNamePageView> AnalyzePageViewsByCountryName(SearchCriteria criteria, int dataSize)
        {
            // Define the aggregation pipeline stages
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
             {
               { Constants.DOMAIN, criteria.Domain },
                { Constants.COUNTRY_NAME, new BsonDocument(Constants.NOT, BsonNull.Value) }
                     });
            //now we need to get the count of pageviews for each country name
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument
            {
                {Constants.ID,"$" + Constants.COUNTRY_NAME },//group by date
                {Constants.TOTAL_PAGE_VIEWS,new BsonDocument(Constants.SUM,1) }//count the pageviews

            });
            var orderByStage = new BsonDocument(Constants.SORT, new BsonDocument(Constants.TOTAL_PAGE_VIEWS, -1));
            var limitStage = new BsonDocument(Constants.LIMIT, dataSize);
            //initialize the pipeline
            var pipeline = new[] { matchStage, groupStage, orderByStage, limitStage };
            //execute the pipeline then store the results in list 
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();
            var results = new List<CountryNamePageView>();
            //loop over the results to store them in DatePageView List
            foreach (BsonDocument pipelineResult in pipelineResults)
            {
                results.Add(new CountryNamePageView
                {
                    CountryName = pipelineResult[Constants.ID].AsString,
                    PageViews = pipelineResult[Constants.TOTAL_PAGE_VIEWS].AsInt32

                });
            }
            return results;
        }
        public DashboardStatistics getDashboardStatisticsData(SearchCriteria criteria)
        {
            int authors = _dashboardStatisticsService.getTotalAuthors(criteria);
            int pageViews = _dashboardStatisticsService.getTotalPageViews(criteria);
            int articles = _dashboardStatisticsService.getTotalArticles(criteria);
            int users = _dashboardStatisticsService.getTotalUsers(criteria);
            DashboardStatistics data = new DashboardStatistics()
            {
                authors = authors,
                pageViews = pageViews,
                articles = articles,
                users = users
            };
            return data;

        }

        public List<PostTypePageViews> AnalyzePageViewsByPostType(SearchCriteria criteria)
        {
            // Define the aggregation pipeline stages
            //we need to filter by date , domain and posttype
            var matchStage = new BsonDocument("$match", new BsonDocument
            {
            { Constants.DOMAIN, criteria.Domain },
            {
               Constants.FORMATTED_DATE, new  BsonDocument
               {
                { Constants.GREATER, criteria.DateFrom },
                { Constants.SMALLER, criteria.DateTo }
                }
                  }
                    });
            //we need to group by post type and get total pageviews and posts
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument
            {
                {Constants.ID,"$" + Constants.POST_TYPE },//group by date
                {Constants.TOTAL_PAGE_VIEWS,new BsonDocument(Constants.SUM,1) },//count the pageviews
                {Constants.POSTS,new BsonDocument(Constants.ADD_TO_SET,"$"+Constants.POST_ID) } //get array of post to be able to get the toal posts

            });

            //we need to get the total posts count
            var projectStage = new BsonDocument(Constants.PROJECT, new BsonDocument
            {
                {"_id",0 },
                {Constants.POST_TYPE,"$"+Constants.ID },
                {Constants.TOTAL_PAGE_VIEWS,1 },
                {Constants.TOTAL_POSTS,new BsonDocument(Constants.SIZE,"$"+Constants.POSTS) }
            });
            var limitStage = new BsonDocument(Constants.LIMIT, criteria.Size);

            //initialize the pipeline
            var pipeline = new[] { matchStage, groupStage, projectStage, limitStage };
            //execute the pipeline then store the results in list 
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();
            var results = new List<PostTypePageViews>();
            //loop over the results to store them in DatePageView List
            foreach (BsonDocument pipelineResult in pipelineResults)
            {
                results.Add(new PostTypePageViews
                {
                    PostType = pipelineResult[Constants.POST_TYPE].AsString,
                    PageViews = pipelineResult[Constants.TOTAL_PAGE_VIEWS].AsInt32,
                    Posts = pipelineResult[Constants.TOTAL_POSTS].AsInt32
                });
            }
            return results;



        }
    }
}
