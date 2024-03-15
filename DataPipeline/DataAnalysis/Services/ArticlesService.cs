﻿using DataPipeline.DataAnalysis.Models;
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
        private readonly IUserProfileDataService _userProfileDataService;
        public ArticlesService(IOptions<DatabaseConnecting> DatabaseSettings, ILocationService locationService, IUserProfileDataService userProfileDataService)
        {
            //mongo DB connection 
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);
            ///get pageView collection 
            _collection = mongoDatabase.GetCollection<MongoDbPageView>(DatabaseSettings.Value.CollectionName);
            _locationService = locationService;
            _userProfileDataService = userProfileDataService;
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
                {"_id","$"+Constants.POST_ID },//group by post title 
                {Constants.POST_TITLE,new  BsonDocument("$first","$"+Constants.POST_TITLE)},
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
                    PostTitle = pResult[Constants.POST_TITLE].AsString, // Get the article title from the _id field
                    PageViews = pResult[Constants.TOTAL_PAGE_VIEWS].AsInt32 // Get total page views
                });
            }

            return results;
        }

        public List<ArticlePageView> getRecommendedArticles(SearchCriteria search, string userId, int dataSize)
        {
            //get user's data

            //toDO save the user profile(user id, location,categories,authors,tags) in memory cache 
            Dictionary<string, int> topCategories = _userProfileDataService.getTopCategoriesForSpecificUser(search, userId);
            Dictionary<string, int> topAuthors = _userProfileDataService.getTopAuthorsForSpecificUser(search, userId);
            Dictionary<string, int> topTags = _userProfileDataService.getTopTagsForSpecificUser(search, userId, 10);

            //match stage 
            var matchFilters = new List<BsonDocument>();

            //filter by domain
            var domainMatchStage= new BsonDocument(new BsonDocument(Constants.MATCH, new BsonDocument(Constants.DOMAIN, search.Domain)));
            //loop over categories
            foreach (var category in topCategories)
            {
                matchFilters.Add(new BsonDocument(Constants.Category, category.Key));
            }
            //loop over authors 
            foreach (var author in topAuthors)
            {
                matchFilters.Add(new BsonDocument(Constants.POST_AUTHOR, author.Key));
            }
            //loop over tags 
            foreach (var tag in topTags)
            {
                matchFilters.Add(new BsonDocument(Constants.POST_TAG, new BsonDocument(Constants.IN, new BsonArray(tag.Key))));
            }

            // Create $or expression for match filters
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument(Constants.OR, new BsonArray(matchFilters)));

            //add limit stage
            var limitStage = new BsonDocument(Constants.LIMIT, dataSize);
            //initialize the pipeline 
            var pipeline = new[] { domainMatchStage,matchStage, limitStage };
            //execute the pipeline
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();

            var results = new List<ArticlePageView>();

            foreach (BsonDocument pResult in pipelineResults)
            {
                results.Add(new ArticlePageView
                {
                    PostTitle = pResult[Constants.POST_TITLE].AsString, // Get the article's title from the _id field
                    PostUrl = pResult[Constants.POST_URL].AsString,//Get article's url
                    PostImage = pResult[Constants.POST_IMAGE]
                    .AsBsonArray.Select(bsonValue => bsonValue.ToString()).ToArray()//Get Image url
                });
            }
            return results;

        }
    }
}
