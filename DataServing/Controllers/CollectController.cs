using DataPipeline.DataCollection.Models;
using DataPipeline.DataCollection.Services;
using DataServing.Models;
using Microsoft.AspNetCore.Mvc;
using Wangkanai.Detection.Services;

namespace DataServing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectController : ControllerBase
    {
        private readonly IDataCollectionService _service;
        public readonly IDetectionService detectionService;

        public CollectController(IDataCollectionService service, IDetectionService _detectionService)
        {
            _service = service;
            detectionService = _detectionService;
        }


        //Track the current page/post Viewer 
        [HttpPost]
        public ActionResult<MongoDbPageView> Track([FromBody] RequestData data)
        {

            //read ip
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();


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
                PostImage=data.PostImage,
                PostUrl=data.PostUrl,
            };

            views.UserId = data.UserId;

            _service.LogPageview(views);

            return views;

        }
    }



}