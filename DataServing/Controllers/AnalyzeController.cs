using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataAnalysis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        ///This API is used to get top categories pageviews 
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
            var results = _dataAnalyticsService.AnalyzePageViewsByCategory(search, 10);
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
            var results = _dataAnalyticsService.AnalyseByAuthor(search, 10);
            return Ok(results);
        }
        /// <summary>
        /// This API is used to get pageviews by date
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
            var results = _dataAnalyticsService.AnalyzePageViewsByDate(search, date, 10);
            return Ok(results);
        }
        /// <summary>
        /// This API is used to get pageviews by countries
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        [HttpGet("countries")]
        public IActionResult AnalyzePageViewsbyCountryName([FromQuery(Name = "domain")] string domain)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain
            };
            var results = _dataAnalyticsService.AnalyzePageViewsByCountryName(search, 10);
            return Ok(results);
        }
        /// <summary>
        /// This API is used to get pageviews and total post with respect to posttypes
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        [HttpGet("posttypes")]
        public IActionResult AnalyzePageViewsByPostType([FromQuery(Name = "domain")] string domain,
                                                        [FromQuery(Name = "date_from")] string dateFrom,
                                                        [FromQuery(Name = "date_to")] string dateTo)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain,
                DateFrom = dateFrom,
                DateTo = dateTo,
                Size=10
            };
            var results = _dataAnalyticsService.AnalyzePageViewsByPostType(search);
            return Ok(results);
        }
    }
}
