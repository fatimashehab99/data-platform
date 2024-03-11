using DataPipeline.DataAnalysis.Models;

namespace DataPipeline.DataAnalysis.Services
{

    /// <summary>
    /// Is used for data analysis
    /// </summary>
    public interface IDataAnalyticsService
    {
        /// <summary>
        /// get postview count filtered by publisher name during specific date
        /// (group by date)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="SubId"></param>
        /// <param name="pid"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public dynamic AnalyzePost(SearchCriteria data, string SubId, string pid, DateTime start, DateTime end, out AnalyticsErrorEnum error);


        /// <summary>
        /// this function is used to get distinct authors with their page views  
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<AuthorPageView> AnalyseByAuthor(SearchCriteria data);

        /// <summary>
        /// This function is used to get pageviews count on specific date
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<DatePageView> AnalyzePageViewsByDate(SearchCriteria data);

         


    }
}

