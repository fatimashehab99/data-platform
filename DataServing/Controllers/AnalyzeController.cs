﻿using DataPipeline.DataAnalysis.Models;
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
        ///This API is used to get categories pageviews analysis
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        [HttpGet("categories")]
        public IActionResult GetTopCategories([FromQuery(Name = "domain")] string domain,
                                                        [FromQuery(Name = "date_from")] string dateFrom,
                                                        [FromQuery(Name = "date_to")] string dateTo,
                                                        [FromQuery(Name = "posttype")] string? posttype)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain,
                DateFrom = dateFrom,
                DateTo = dateTo,
                PostType = posttype,
                Size = 10
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

        public IActionResult GetDashboardData([FromQuery(Name = "domain")] string domain,
                                                        [FromQuery(Name = "date_from")] string dateFrom,
                                                        [FromQuery(Name = "date_to")] string dateTo,
                                                        [FromQuery(Name = "posttype")] string? posttype)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain,
                DateFrom = dateFrom,
                DateTo = dateTo,
                PostType = posttype
            };
            var results = _dataAnalyticsService.getDashboardStatisticsData(search);
            return Ok(results);

        }
        /// <summary>
        /// This api is used to get authors pageviews analysis
        /// </summary>
        /// <returns></returns>
        [HttpGet("authors")]
        public IActionResult getAnalyticsPostByAuthor([FromQuery(Name = "domain")] string domain,
                                                      [FromQuery(Name = "date_from")] string dateFrom,
                                                      [FromQuery(Name = "date_to")] string dateTo,
                                                      [FromQuery(Name = "posttype")] string? posttype)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain,
                DateFrom = dateFrom,
                DateTo = dateTo,
                PostType = posttype,
                Size = 10
            };
            var results = _dataAnalyticsService.AnalyzeAuthorPageViews(search);
            return Ok(results);
        }
        /// <summary>
        /// This API is used to get date pageviews analysis
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        [HttpGet("date")]
        public IActionResult AnalyzePageViewsByDate([FromQuery(Name = "domain")] string domain,
                                                    [FromQuery(Name = "date")] string date,
                                                    [FromQuery(Name = "posttype")] string? posttype)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain,
                DateFrom = date,
                Size = 10,
                PostType = posttype
            };
            var results = _dataAnalyticsService.AnalyzePageViewsByDate(search);
            return Ok(results);
        }
        /// <summary>
        /// This API is usedto get countries pageviews analysis
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        [HttpGet("countries")]
        public IActionResult AnalyzePageViewsbyCountryName([FromQuery(Name = "domain")] string domain,
                                                           [FromQuery(Name = "date_from")] string dateFrom,
                                                           [FromQuery(Name = "date_to")] string dateTo,
                                                           [FromQuery(Name = "posttype")] string? posttype)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain,
                DateFrom = dateFrom,
                DateTo = dateTo,
                PostType = posttype,
                Size = 10
            };
            var results = _dataAnalyticsService.AnalyzePageViewsByCountryName(search);
            return Ok(results);
        }
        /// <summary>
        /// This API is used to get posttypes pageviews analysis
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
                Size = 10
            };
            var results = _dataAnalyticsService.AnalyzePageViewsByPostType(search);
            return Ok(results);
        }
        /// <summary>
        /// This API is used to get tags pageviews analysis
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        [HttpGet("tags")]
        public IActionResult AnalyzePageViewsByTags([FromQuery(Name = "domain")] string domain,
                                                        [FromQuery(Name = "posttype")] string? posttype,
                                                        [FromQuery(Name = "date_from")] string dateFrom,
                                                        [FromQuery(Name = "date_to")] string dateTo)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain,
                DateFrom = dateFrom,
                DateTo = dateTo,
                PostType = posttype,
                Size = 50
            };
            var results = _dataAnalyticsService.AnalyzePageViewsByTag(search);
            return Ok(results);
        }
        /// <summary>
        /// This function is used to get top page views articles
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="posttype"></param>
        /// <returns></returns>
        [HttpGet("articles")]
        public IActionResult AnalyzeArticlesPageViews([FromQuery(Name = "domain")] string domain,
                                                   [FromQuery(Name = "date_from")] string dateFrom,
                                                   [FromQuery(Name = "date_to")] string dateTo,
                                                   [FromQuery(Name = "posttype")] string? posttype)
        {
            SearchCriteria search = new SearchCriteria()
            {
                Domain = domain,
                DateFrom = dateFrom,
                DateTo = dateTo,
                PostType = posttype,
                Size = 10
            };
            var results = _dataAnalyticsService.GetTopPageViewsArticles(search);
            return Ok(results);
        }
    }
}
