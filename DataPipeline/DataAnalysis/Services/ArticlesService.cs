﻿using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataCollection.Models;
using DataPipeline.Helpers.LocationService;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataPipeline.DataAnalysis.Services
{
    public class ArticlesService : IArticlesService
    {
        private readonly IMongoCollection<MongoDbPageView> _collection;
        private readonly ILocationService _locationService;
        private readonly IUserProfileDataService _userProfileDataService;
        private readonly IMemoryCache _cache;
        public ArticlesService(IOptions<DatabaseConnecting> DatabaseSettings, ILocationService locationService,
            IUserProfileDataService userProfileDataService, IMemoryCache cache)
        {
            //mongo DB connection 
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);
            ///get pageView collection 
            _collection = mongoDatabase.GetCollection<MongoDbPageView>(DatabaseSettings.Value.CollectionName);
            _locationService = locationService;
            _userProfileDataService = userProfileDataService;
            _cache = cache;
        }

        public List<ArticlePageView> getTrendingArticles(SearchCriteria criteria, string Ip, int dataSize)
        {
            //get 3 days ago date

            DateTime pastDate = (DateTime.Now).AddDays(-3);// Subtract 3 days from the current date
            string date = pastDate.ToString("yyyy-MM-dd");// Format the date as "yyyy-MM-dd"

            //get location
            Dictionary<string, string> location = _locationService.getCountryInfo(Ip);

            string countryName = location["CountryName"];

            //filtering stage 
            var matchStage = new BsonDocument(Constants.MATCH, new BsonDocument
            {
                {Constants.DOMAIN,criteria.Domain },
                {Constants.COUNTRY_NAME,countryName},
                { Constants.FORMATTED_DATE, new BsonDocument(Constants.GREATER, date) },
                { Constants.POST_TITLE, new BsonDocument(Constants.NOT, BsonNull.Value) }
            });

            //grouping by stage (post id)
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument {
                {Constants.ID,"$"+Constants.POST_ID },//group by post id
                {Constants.POST_TITLE,new  BsonDocument(Constants.FIRST,"$"+Constants.POST_TITLE)},
                {Constants.POST_URL,new  BsonDocument(Constants.FIRST,"$"+Constants.POST_URL)},
                {Constants.POST_IMAGE,new  BsonDocument(Constants.FIRST,"$"+Constants.POST_IMAGE)},
                {Constants.POST_AUTHOR,new  BsonDocument(Constants.FIRST,"$"+Constants.POST_AUTHOR)},
                {Constants.CATEGORY,new  BsonDocument(Constants.FIRST,"$"+Constants.CATEGORY)},
                {Constants.PUBLISHED_DATE,new  BsonDocument(Constants.FIRST,"$"+Constants.PUBLISHED_DATE)},
                {Constants.TOTAL_PAGE_VIEWS,new BsonDocument(Constants.SUM,1) }
            });

            //sorting stage 
            var orderByStage = new BsonDocument(Constants.SORT, new BsonDocument(Constants.TOTAL_PAGE_VIEWS, -1));

            //limit stage
            var limitStage = new BsonDocument(Constants.LIMIT, dataSize);
            //initialize the pipeline 
            var pipeline = new[] { matchStage, groupStage, orderByStage, limitStage };
            //execute the pipeline
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();

            var results = new List<ArticlePageView>();

            foreach (BsonDocument pResult in pipelineResults)
            {
                results.Add(new ArticlePageView
                {
                    PostId = pResult[Constants.ID].AsString,
                    PostTitle = pResult[Constants.POST_TITLE].AsString, // Get the article title from the _id field
                    PageViews = pResult[Constants.TOTAL_PAGE_VIEWS].AsInt32, // Get total page views
                    PostUrl = pResult[Constants.POST_URL].AsString,//Get article's url
                    PostImage = pResult[Constants.POST_IMAGE].AsString,//Get Image url
                    PostAuthor = pResult[Constants.POST_AUTHOR].AsString,
                    PostCategory = pResult[Constants.CATEGORY].AsString,
                    PublishedDate = (pResult[Constants.PUBLISHED_DATE]).AsDateTime
                });
            }

            return results;
        }

        public List<ArticlePageView> getRecommendedArticles(SearchCriteria search, string userId, string Ip, int dataSize)
        {
            //variables initialization 
            Dictionary<string, int>? topCategories = null;
            Dictionary<string, int>? topAuthors = null;
            Dictionary<string, int>? topTags = null;

            //set unique cache key for specific user id and domain
            var cacheKey = $"User_{userId}_Domain_{search.Domain}";

            bool isUserExist = _cache.TryGetValue(cacheKey, out isUserExist);

            //if user's data doesn't exist it will be created and added to the memory
            if (!isUserExist)
            {
                //get top categories , author and tags for the user 
                topCategories = _userProfileDataService.getTopCategoriesForSpecificUser(search, userId);
                topAuthors = _userProfileDataService.getTopAuthorsForSpecificUser(search, userId);
                topTags = _userProfileDataService.getTopTagsForSpecificUser(search, userId, 10);
                //get location
                Dictionary<string, string> location = _locationService.getCountryInfo(Ip);

                //create user data
                UserData user = new UserData()
                {
                    UserId = userId,
                    Domain = search.Domain,
                    CountryName = location["CountryName"],
                    TopCategories = topCategories,
                    TopAuthors = topAuthors,
                    TopTags = topTags
                };

                //add data to the cache
                _cache.Set(cacheKey, user, TimeSpan.FromDays(1));
            }
            //get user data from the cache
            UserData userData = (UserData)_cache.Get(cacheKey);
            topCategories = userData.TopCategories;
            topAuthors = userData.TopAuthors;
            topTags = userData.TopTags;


            //match stage 
            var matchFilters = new List<BsonDocument>();

            //filter by domain
            var domainMatchStage = new BsonDocument(new BsonDocument(Constants.MATCH, new BsonDocument(Constants.DOMAIN, search.Domain)));
            //loop over categories
            foreach (var category in topCategories)
            {
                matchFilters.Add(new BsonDocument(Constants.CATEGORY, category.Key));
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

            //grouping by stage (post id)
            var groupStage = new BsonDocument(Constants.GROUP, new BsonDocument {
                {Constants.ID,"$"+Constants.POST_ID },//group by post id
                {Constants.POST_TITLE,new  BsonDocument(Constants.FIRST,"$"+Constants.POST_TITLE)},
                {Constants.POST_URL,new  BsonDocument(Constants.FIRST,"$"+Constants.POST_URL)},
                {Constants.POST_IMAGE,new  BsonDocument(Constants.FIRST,"$"+Constants.POST_IMAGE)},
                {Constants.POST_AUTHOR,new  BsonDocument(Constants.FIRST,"$"+Constants.POST_AUTHOR)},
                {Constants.CATEGORY,new  BsonDocument(Constants.FIRST,"$"+Constants.CATEGORY)},
                {Constants.PUBLISHED_DATE,new  BsonDocument(Constants.FIRST,"$"+Constants.PUBLISHED_DATE)},
                {Constants.TOTAL_PAGE_VIEWS,new BsonDocument(Constants.SUM,1) }
            });


            //add limit stage
            var limitStage = new BsonDocument(Constants.LIMIT, dataSize);

            //initialize the pipeline 
            var pipeline = new[] { domainMatchStage, matchStage, groupStage, limitStage };
            //execute the pipeline
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();

            var results = new List<ArticlePageView>();

            foreach (BsonDocument pResult in pipelineResults)
            {
                results.Add(new ArticlePageView
                {
                    PostId = pResult[Constants.ID].AsString,
                    PostTitle = pResult[Constants.POST_TITLE].AsString, // Get the article title from the _id field
                    PageViews = pResult[Constants.TOTAL_PAGE_VIEWS].AsInt32, // Get total page views
                    PostUrl = pResult[Constants.POST_URL].AsString,//Get article's url
                    PostImage = pResult[Constants.POST_IMAGE].AsString,//Get Image url
                    PostAuthor = pResult[Constants.POST_AUTHOR].AsString,
                    PostCategory = pResult[Constants.CATEGORY].AsString,
                    PublishedDate = (pResult[Constants.PUBLISHED_DATE]).AsDateTime
                });
            }
            return results;

        }
    }
}
