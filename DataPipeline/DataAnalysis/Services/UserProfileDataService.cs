﻿using Amazon.Runtime.Internal.Transform;
using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataCollection.Models;
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
    public class UserProfileDataService : IUserProfileDataService
    {
        private readonly IMongoCollection<MongoDbPageView> _collection;
        public UserProfileDataService(IOptions<DatabaseConnecting> DatabaseSettings)
        {
            //mongo DB connection 
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);
            ///get pageView collection 
            _collection = mongoDatabase.GetCollection<MongoDbPageView>(DatabaseSettings.Value.CollectionName);
        }
        /// <summary>
        /// This function is used to get top categories interested by specific usr
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Dictionary<string, int> getTopCategoriesForSpecificUser(SearchCriteria criteria, string UserId)
        {
            // filtering stage
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
        {
            { Constants.DOMAIN, criteria.Domain },
             {Constants.USERID,UserId },
            {Constants.Category, new BsonDocument(Constants.NULL, BsonNull.Value) }
        });
            //grouping by category
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument
        {
            { "_id", "$"+Constants.Category }, // Group by author
            { Constants.TOTAL_PAGE_VIEWS, new BsonDocument(Constants.SUM, 1) } // Sum the PageView values for each author
        });
            //order by stage
            var orderByStage = new BsonDocument(Constants.SORT, new BsonDocument(Constants.TOTAL_PAGE_VIEWS, -1));
            //limit by 10
            var limitStage = new BsonDocument(Constants.LIMIT, 10);

            var pipeline = new[] { matchStage, groupStage, orderByStage, limitStage };

            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();
            //save the results in a dictionary
            Dictionary<string, int> results = new Dictionary<string, int>();
            foreach (BsonDocument pipelineResult in pipelineResults)
            {
                results.Add(pipelineResult["_id"].AsString, pipelineResult[Constants.TOTAL_PAGE_VIEWS].AsInt32);
            }
            return results;
        }
        /// <summary>
        /// This function is used to get top authors interested by specific usr
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Dictionary<string, int> getTopAuthorsForSpecificUser(SearchCriteria criteria, string UserId)
        {
            //data filtering 
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
        {
            { Constants.DOMAIN, criteria.Domain },
             {Constants.USERID,UserId },
            {Constants.POST_AUTHOR, new BsonDocument(Constants.NULL, BsonNull.Value) }
        });

            //grouping by author
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument
        {
            { "_id", "$"+Constants.POST_AUTHOR }, // Group by author
            { Constants.TOTAL_PAGE_VIEWS, new BsonDocument(Constants.SUM, 1) } // Sum the PageView values for each author
        });
            //sorting stage
            var orderByStage = new BsonDocument(Constants.SORT, new BsonDocument(Constants.TOTAL_PAGE_VIEWS, -1));
            //limit by 10
            var limitStage = new BsonDocument(Constants.LIMIT, 10);

            var pipeline = new[] { matchStage, groupStage, orderByStage, limitStage };

            //save the results in a dictionary
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();
            Dictionary<string, int> results = new Dictionary<string, int>();
            foreach (BsonDocument pipelineResult in pipelineResults)
            {
                results.Add(pipelineResult["_id"].AsString, pipelineResult[Constants.TOTAL_PAGE_VIEWS].AsInt32);
            }
            return results;
        }
    }
}