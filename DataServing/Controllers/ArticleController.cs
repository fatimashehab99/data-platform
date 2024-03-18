using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataAnalysis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataServing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticlesService _articlesService;
        private readonly IUserProfileDataService _userProfileDataService;
        public ArticleController(IArticlesService articlesService, IUserProfileDataService userProfileDataService)
        {
            _articlesService = articlesService;
            _userProfileDataService = userProfileDataService;
        }
        /// <summary>
        /// This API is used to get trending articles
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("trending")]
        public IActionResult getTrendingArticles([FromQuery(Name = "domain")] string domain,
                                                    [FromQuery(Name = "ip")] string ip)

        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain
            };
            var results = _articlesService.getTrendingArticles(search, ip, 4);
            return Ok(results);

        }
        /// <summary>
        /// This API is used to get recommended articles
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("recommended")]
        public IActionResult getRecommendedArticles([FromQuery(Name = "domain")] string domain,
                                                    [FromQuery(Name = "userId")] string userId, [FromQuery(Name = "Ip")] string Ip)
        {
            //toDo get ip
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain
            };
            //check user if exists
            bool userExist = _userProfileDataService.checkUser(search, userId);
            List<ArticlePageView> results;
            //incase user does not exists it will return trending articles 
            results = !userExist
             //incase the user has no recommended articles it will get the trending articles
             ? _articlesService.getTrendingArticles(search, Ip, 4) // ToDo: Change IP
             : _articlesService.getRecommendedArticles(search, userId, Ip, 4);
            return Ok(results);
        }
    }
}
