using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataCollection.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.DataAnalysis.Services
{
    /// <summary>
    /// This class is use to get the data of the dashboard statistics 
    /// </summary>
    public class DashboardStatisticsService
    {
        private readonly IMongoCollection<MongoDbPageView> _collection;

        public DashboardStatisticsService(IOptions<DatabaseConnecting> DatabaseSettings)
        {
            //mongo DB connection 
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);
            ///get pageView collection 
            _collection = mongoDatabase.GetCollection<MongoDbPageView>(DatabaseSettings.Value.CollectionName);
        }
        /// <summary>
        /// get total pageviews
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public int getTotalViews(SearchCriteria criteria)
        {
            //Define the aggregation pipeline stages
            //first we need to filter data by domain
            var matchStage = new BsonDocument("$match", new BsonDocument(Constants.DOMAIN, criteria.Domain));

            //get the count of pageviews
            var countStage = new BsonDocument("$count", "TotalPageviews");
            //initialize the pipeline
            var pipeline = new[] { matchStage, countStage };
            //execute the pipeline
            var pipelineResults = _collection.Aggregate<BsonDocument>(pipeline);
            var result = pipelineResults.FirstOrDefault();
            //get total page views
            int totalPageViews = result != null ? result["TotalPageViews"].AsInt32 : 0;
            return totalPageViews;

        }

        public int getTotalAuthors(SearchCriteria criteria)
        {
            //Define the aggregation pipeline stages
            //first we need to filter data by domain
            var matchStage = new BsonDocument("$match", new BsonDocument(Constants.DOMAIN, criteria.Domain));
            //now we need to groupby authors
            var groupStage = new BsonDocument("$group", new BsonDocument
            {
                {"_id",BsonNull.Value},
                {"authors",new BsonDocument("$addToSet",Constants.POST_AUTHOR)}
            });
            //now get the count of the authors
            var projectStage = new BsonDocument("$project",
                new BsonDocument("TotalAuthors",
                new BsonDocument("$size", "$authors")));
            //initialize the pipeline
            var pipeline = new[] { matchStage, groupStage, projectStage };
            var pipelineResults = _collection.Aggregate<BsonDocument>(pipeline);
            var result = pipelineResults.FirstOrDefault();
            int totalAuthors = result != null ? result["TotalAuthors"].AsInt32 : 0;
            return totalAuthors;

        }
    }
}
