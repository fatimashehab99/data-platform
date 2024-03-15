using DataPipeline.DataAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.DataAnalysis.Services
{
    public interface IDashboardStatisticsService
    {
        /// <summary>
        /// get total pageviews
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public int getTotalPageViews(SearchCriteria criteria);
        /// <summary>
        /// get totalauthors
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public int getTotalAuthors(SearchCriteria criteria);
        /// <summary>
        /// This function is used to get total articles
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public int getTotalArticles(SearchCriteria criteria);
        /// <summary>
        /// This function is used to get total users
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public int getTotalUsers(SearchCriteria criteria);
    }
}
