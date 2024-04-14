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
    public class DashboardStatisticsService : IDashboardStatisticsService
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

        public int getTotalPageViews(SearchCriteria criteria)
        {
            //Define the aggregation pipeline stages

            //first we need to filter data by domain
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
             {
               { Constants.DOMAIN, criteria.Domain },
                 {Constants.FORMATTED_DATE, new  BsonDocument{
                    { Constants.GREATER, criteria.DateFrom },
                    { Constants.SMALLER, criteria.DateTo }}}
                   });
            ///filter by posttype
            if (!string.IsNullOrEmpty(criteria.PostType))
            {
                matchStage[Constants.MATCH].AsBsonDocument.Add(Constants.POST_TYPE, new BsonDocument
                {
                 { Constants.REGEX, criteria.PostType }
                 });
            }
            
            //get the count of pageviews
            var countStage = new BsonDocument(Constants.COUNT, Constants.TOTAL_PAGE_VIEWS);
            //initialize the pipeline
            var pipeline = new[] { matchStage, countStage };
            //execute the pipeline
            var pipelineResults = _collection.Aggregate<BsonDocument>(pipeline);
            var result = pipelineResults.FirstOrDefault();
            //get total page views
            int totalPageViews = result != null ? result[Constants.TOTAL_PAGE_VIEWS].AsInt32 : 0;
            return totalPageViews;

        }

        public int getTotalAuthors(SearchCriteria criteria)
        {
            //Define the aggregation pipeline stages
            //first we need to filter data by domain
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
             {
               { Constants.DOMAIN, criteria.Domain },
                { Constants.POST_AUTHOR, new BsonDocument(Constants.NOT, BsonNull.Value) },
                 {Constants.FORMATTED_DATE, new  BsonDocument{
                    { Constants.GREATER, criteria.DateFrom },
                    { Constants.SMALLER, criteria.DateTo }}}
                   });
            ///filter by posttype
            if (!string.IsNullOrEmpty(criteria.PostType))
            {
                matchStage[Constants.MATCH].AsBsonDocument.Add(Constants.POST_TYPE, new BsonDocument
                {
                 { Constants.REGEX, criteria.PostType }
                 });
            }
            //now we need to groupby authors
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument
            {
                {Constants.ID,BsonNull.Value},
                {Constants.AUTHORS,new BsonDocument(Constants.ADD_TO_SET,"$"+Constants.POST_AUTHOR)}
            });
            //now get the count of the authors
            var projectStage = new BsonDocument(Constants.PROJECT,
                new BsonDocument(Constants.TOTAL_AUTHORS,
                new BsonDocument(Constants.SIZE, "$" + Constants.AUTHORS)));
            //initialize the pipeline
            var pipeline = new[] { matchStage, groupStage, projectStage };
            var pipelineResults = _collection.Aggregate<BsonDocument>(pipeline);
            var result = pipelineResults.FirstOrDefault();
            int totalAuthors = result != null ? result[Constants.TOTAL_AUTHORS].AsInt32 : 0;
            return totalAuthors;

        }

        public int getTotalArticles(SearchCriteria criteria)
        {
            //Define the aggregation pipeline stages

            //first we need to filter data by domain
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
             {
               { Constants.DOMAIN, criteria.Domain },
                 {Constants.FORMATTED_DATE, new  BsonDocument{
                    { Constants.GREATER, criteria.DateFrom },
                    { Constants.SMALLER, criteria.DateTo }}}
                   });
            ///filter by posttype
            if (!string.IsNullOrEmpty(criteria.PostType))
            {
                matchStage[Constants.MATCH].AsBsonDocument.Add(Constants.POST_TYPE, new BsonDocument
                {
                 { Constants.REGEX, criteria.PostType }
                 });
            }
            //now we need to group by post id
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument
            {
                {Constants.ID,BsonNull.Value},
                {Constants.ARTICLES,new BsonDocument(Constants.ADD_TO_SET,"$"+Constants.POST_ID)}
            });
            //now get the count of the authors
            var projectStage = new BsonDocument(Constants.PROJECT,
                new BsonDocument(Constants.TOTAL_ARTICLES,
                new BsonDocument(Constants.SIZE, "$" + Constants.ARTICLES)));
            //initialize the pipeline
            var pipeline = new[] { matchStage, groupStage, projectStage };
            var pipelineResults = _collection.Aggregate<BsonDocument>(pipeline);
            var result = pipelineResults.FirstOrDefault();
            int totalArticles = result != null ? result[Constants.TOTAL_ARTICLES].AsInt32 : 0;
            return totalArticles;

        }

        public int getTotalUsers(SearchCriteria criteria)
        {
            //Define the aggregation pipeline stages

            //first we need to filter data by domain
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
             {
               { Constants.DOMAIN, criteria.Domain },
                 {Constants.FORMATTED_DATE, new  BsonDocument{
                    { Constants.GREATER, criteria.DateFrom },
                    { Constants.SMALLER, criteria.DateTo }}}
                   });
            ///filter by posttype
            if (!string.IsNullOrEmpty(criteria.PostType))
            {
                matchStage[Constants.MATCH].AsBsonDocument.Add(Constants.POST_TYPE, new BsonDocument
                {
                 { Constants.REGEX, criteria.PostType }
                 });
            }
            //now we need to group by post title
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument
            {
                {Constants.ID,BsonNull.Value},
                {Constants.USERS,new BsonDocument(Constants.ADD_TO_SET,"$"+Constants.USERID)}
            });
            //now get the count of the authors
            var projectStage = new BsonDocument(Constants.PROJECT,
                new BsonDocument(Constants.TOTAL_USERS,
                new BsonDocument(Constants.SIZE, "$" + Constants.USERS)));
            //initialize the pipeline
            var pipeline = new[] { matchStage, groupStage, projectStage };
            var pipelineResults = _collection.Aggregate<BsonDocument>(pipeline);
            var result = pipelineResults.FirstOrDefault();
            int totalUsers = result != null ? result[Constants.TOTAL_USERS].AsInt32 : 0;
            return totalUsers;

        }
    }


}
