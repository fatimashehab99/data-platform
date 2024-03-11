using DataPipeline.DataCollection.Models;
using Microsoft.Extensions.Options;

namespace DataPipeline.DataCollection.Services
{

    /// <summary>
    /// Is used for data collection
    /// </summary>
    public interface IDataCollectionService
    {

        /// <summary>
        /// TrackRecord
        /// </summary>
        /// <param name="views"></param>
        /// <param name="DatabaseSettings"></param>
        public void LogPageview(MongoDbPageView view);


        /// <summary>
        /// GetPageViewByPostId
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="DatabaseSettings"></param>
        /// <returns></returns>
        public MongoDbPageView GetPageViewByPostId(string postId);


        /// <summary>
        /// to be used by the test to setup an invalid connection and collection names
        /// </summary>
        /// <param name="DatabaseSettings"></param>
        public void setUpMongoDb(DatabaseConnecting DatabaseSettings);

    }
}
