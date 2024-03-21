using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataAnalysis.Services;
using DataPipeline.DataCollection.Models;
using DataPipeline.DataCollection.Services;
using DataPipeline.Helpers.LocationService;
using DataServing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Wangkanai.Detection.Services;

namespace DataServing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectController : ControllerBase
    {
        private readonly IDataCollectionService _service;
        public readonly IDetectionService detectionService;
        public readonly IMemoryCache _cache;
        public readonly IUserProfileDataService _userProfileDataService;
        public readonly ILocationService _locationService;


        public CollectController(IDataCollectionService service, IDetectionService _detectionService, IMemoryCache cache,
            IUserProfileDataService userProfileDataService, ILocationService locationService)
        {
            _service = service;
            detectionService = _detectionService;
            _cache = cache;
            _userProfileDataService = userProfileDataService;
            _locationService = locationService;
        }


        //Track the current page/post Viewer 
        [HttpPost]
        public ActionResult<MongoDbPageView> Track([FromBody] RequestData data)
        {
            //read ip
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            //deserialize post classes to save the data into mongo db
            var classes = JsonConvert.DeserializeObject<List<PostClass>>(data.PostClasses);

            // map request data to MongoDb pageview (data conversion/data modeling)
            MongoDbPageView views = new()
            {
                Domain = data.Domain,

                Ip = string.IsNullOrEmpty(data.IP) ? remoteIpAddress : data.IP,

                //Data Cleaning / Parsing user agent Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36
                //https://github.com/wangkanai/wangkanai/tree/main/Detection
                Browser = detectionService.Browser.Name.ToString(),
                Platform = detectionService.Platform.Name.ToString(),
                Device = detectionService.Device.Type.ToString(),

                PostAuthor = data.PostAuthor,
                PostId = data.PostId,
                PostTitle = data.PostTitle,
                PostCategory = data.PostCategory,
                PostTags = data.PostTags,
                PostPublishDate = data.PostPublishDate,
                PostImage = data.PostImage,
                PostUrl = data.PostUrl,
                PostClasses = classes,
                PostType = data.PostType

            };

            views.UserId = data.UserId;

            _service.LogPageview(views);

            //cache user data 
            //set unique cache key for specific user id and domain
            var cacheKey = $"User_{data.UserId}_Domain_{data.Domain}";
            SearchCriteria search = new SearchCriteria()
            {
                Domain = data.Domain
            };
            //get top categories , author and tags for the user 
            Dictionary<string, int> topCategories = _userProfileDataService.getTopCategoriesForSpecificUser(search, data.UserId);
            Dictionary<string, int> topAuthors = _userProfileDataService.getTopAuthorsForSpecificUser(search, data.UserId);
            Dictionary<string, int> topTags = _userProfileDataService.getTopTagsForSpecificUser(search, data.UserId, 10);
            Dictionary<string, string> location = _locationService.getCountryInfo(views.Ip);

            //create user data
            UserData userData = new UserData()
            {
                UserId = data.UserId,
                Domain = data.Domain,
                TopCategories = topCategories,
                CountryName = location["CountryName"],
                TopAuthors = topAuthors,
                TopTags = topTags
            };
            //add data to the cache
            _cache.Set(cacheKey, userData, TimeSpan.FromDays(1));

            return views;

        }
    }



}