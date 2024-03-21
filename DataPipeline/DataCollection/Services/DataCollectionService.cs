using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DataPipeline.DataCollection.Models;
using DataPipeline.Helpers.LocationService;
using Wangkanai.Extensions;

namespace DataPipeline.DataCollection.Services
{
    public class DataCollectionService : IDataCollectionService
    {
        private ILocationService _locationService;
        private IMongoCollection<MongoDbPageView> _collection;


        public DataCollectionService(ILocationService locationService, IOptions<DatabaseConnecting> databaseSettings)
        {
            _locationService = locationService;
            setUpMongoDb(databaseSettings.Value);
        }

        public void setUpMongoDb(DatabaseConnecting DatabaseSettings)
        {
            if (string.IsNullOrEmpty(DatabaseSettings.CollectionName))
                throw new Exception(Constants.ERROR_COLLECTION_IS_EMPTY);

            var mongoClient = new MongoClient(DatabaseSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.DatabaseName);

            _collection = mongoDatabase.GetCollection<MongoDbPageView>(DatabaseSettings.CollectionName);
        }

        public MongoDbPageView GetPageViewByPostId(string postId)
        {
            return _collection.Find(x => x.PostId == postId).FirstOrDefault();
        }

        public void LogPageview(MongoDbPageView views)
        {
            ///exceptions to throw 

            //incase ip is null
            if (string.IsNullOrEmpty(views.Ip))
                throw new Exception(Constants.ERROR_IP_IS_EMPTY);

            //incase user id is null
            if (string.IsNullOrEmpty(views.UserId))
                throw new Exception(Constants.ERROR_USER_ID_IS_EMPTY);

            //incase post id is null
            if (string.IsNullOrEmpty(views.PostId))
                throw new Exception(Constants.ERROR_POST_ID_IS_EMPTY);

            //incase browser is null
            if (string.IsNullOrEmpty(views.Browser))
                throw new Exception(Constants.ERROR_BROWSER_IS_EMPTY);

            //incase device is null
            if (string.IsNullOrEmpty(views.Device))
                throw new Exception(Constants.ERROR_DEVICE_IS_EMPTY);

            //incase subscription id is null
            if (string.IsNullOrEmpty(views.Domain))
                throw new Exception(Constants.ERROR_SUBSCRIPTION_ID_IS_EMPTY);

            //incase author is null
            if (string.IsNullOrEmpty(views.PostAuthor))
                throw new Exception(Constants.ERROR_AUTHOR_IS_EMPTY);

            //incase operating is null
            if (string.IsNullOrEmpty(views.Platform))
                throw new Exception(Constants.ERROR_OPERATING_IS_EMPTY);

            //incase operating is null
            if (string.IsNullOrEmpty(views.PostUrl))
                throw new Exception(Constants.ERROR_POST_URL_IS_EMPTY);

            //add country name and code
            Dictionary<string, string> location = _locationService.getCountryInfo(views.Ip);
            views.CountryName = location["CountryName"];
            views.CountryCode = location["CountryCode"];

            //--Set Date on Current Get-->
            DateTime date = DateTime.Now.ToLocalTime();
            views.Date = date;

            //if the formatted date is null it will take the current date
            if (string.IsNullOrEmpty(views.FormattedDate))
                views.FormattedDate = date.ToString("yyyy-MM-dd");

            _collection.InsertOne(views);


        }


    }
}
