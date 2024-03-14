using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataCollection.Models;
using DataPipeline.Helpers.LocationService;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataPipeline.DataAnalysis.Services
{
    public class ArticlesService : IArticlesService
    {
        private readonly IMongoCollection<MongoDbPageView> _collection;
        private readonly ILocationService _locationService;
        public ArticlesService(IOptions<DatabaseConnecting> DatabaseSettings, ILocationService locationService)
        {
            //mongo DB connection 
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);
            ///get pageView collection 
            _collection = mongoDatabase.GetCollection<MongoDbPageView>(DatabaseSettings.Value.CollectionName);
            _locationService = locationService;

        }

        public List<ArticlePageView> getTrendingArticles(SearchCriteria criteria, string Ip)
        {
            //get 3 days ago date

            DateTime pastDate = (DateTime.Now).AddDays(-3);// Subtract 3 days from the current date
            string date = pastDate.ToString("yyyy-MM-dd");// Format the date as "yyyy-MM-dd"

            //get country name
            string countryName = _locationService.getCountryName(Ip);

            //filtering stage 
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
            {
                {Constants.DOMAIN,criteria.Domain },
                {Constants.COUNTRY_NAME,countryName},
                { Constants.FORMATTED_DATE, new BsonDocument(Constants.GREATER, date) },
                { Constants.POST_TITLE, new BsonDocument(Constants.NULL, BsonNull.Value) }
            });

            //grouping by stage (post title)
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument {
                {"_id","$"+Constants.POST_TITLE },//group by post title 
                {Constants.TOTAL_PAGE_VIEWS,new BsonDocument(Constants.SUM,1) }
            });

            //sorting stage 
            var orderByStage = new BsonDocument(Constants.SORT, new BsonDocument(Constants.TOTAL_PAGE_VIEWS, -1));

            //limit stage
            var limitStage = new BsonDocument(Constants.LIMIT, 4);
            //initialize the pipeline 
            var pipeline = new[] { matchStage, groupStage, orderByStage, limitStage };
            //execute the pipeline
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();

            var results = new List<ArticlePageView>();

            foreach (BsonDocument pResult in pipelineResults)
            {
                results.Add(new ArticlePageView
                {
                    PostTitle = pResult["_id"].AsString, // Get the article title from the _id field
                    PageViews = pResult[Constants.TOTAL_PAGE_VIEWS].AsInt32 // Get total page views
                });
            }

            return results;
        }



    }
}
