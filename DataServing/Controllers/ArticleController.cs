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
        public ArticleController(IArticlesService articlesService)
        {
            _articlesService = articlesService;
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
            var results = _articlesService.getTrendingArticles(search, ip);
            return Ok(results);

        }
    }
}
