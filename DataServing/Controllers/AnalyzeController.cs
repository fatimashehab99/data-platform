using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataAnalysis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataServing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzeController : ControllerBase
    {
        private readonly IDataAnalyticsService _dataAnalyticsService;
        public AnalyzeController(IDataAnalyticsService dataAnalyticsService)
        {
            _dataAnalyticsService = dataAnalyticsService;
        }
        /// <summary>
        /// get top categories pageviews api
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        [HttpGet("categories")]
        public IActionResult GetTopCategories([FromQuery(Name = "domain")] string domain)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain
            };
            var results = _dataAnalyticsService.AnalyzePageViewsByCategory(search);
            return Ok(results);
        }
    }
}
