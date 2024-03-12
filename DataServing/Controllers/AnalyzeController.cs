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
        /// <summary>
        /// This api is used to get the dashboard data
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        [HttpGet("dashboard")]

        public IActionResult GetDashboardData([FromQuery(Name = "domain")] string domain)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain
            };
            var results = _dataAnalyticsService.getDashboardStatisticsData(search);
            return Ok(results);

        }
        /// <summary>
        /// This api is used to get authors with pageviews
        /// </summary>
        /// <returns></returns>
        [HttpGet("authors")]
        public IActionResult getAnalyticsPostByAuthor([FromQuery(Name = "domain")] string domain)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain
            };
            var results = _dataAnalyticsService.AnalyseByAuthor(search);
            return Ok(results);
        }
        /// <summary>
        /// This function is used to get pageviews by date
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        [HttpGet("date")]
        public IActionResult AnalyzePageViewsByDate([FromQuery(Name = "domain")] string domain,
                                                    [FromQuery(Name = "date")] string date)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain
            };
            var results = _dataAnalyticsService.AnalyzePageViewsByDate(search,date);
            return Ok(results);
        }
    }
}
