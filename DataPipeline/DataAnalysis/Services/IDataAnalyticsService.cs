using DataPipeline.DataAnalysis.Models;

namespace DataPipeline.DataAnalysis.Services
{

    /// <summary>
    /// Is used for data analysis
    /// </summary>
    public interface IDataAnalyticsService
    {
        /// <summary>
        /// this function is used to get distinct authors with their page views  
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<AuthorPageView> AnalyseByAuthor(SearchCriteria data, int dataSize);

        /// <summary>
        /// This function is used to get pageviews count on specific date
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<DatePageView> AnalyzePageViewsByDate(SearchCriteria data, string date, int dataSize);

        /// <summary>
        /// This function is used to get top category page views 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<CategoryPageView> AnalyzePageViewsByCategory(SearchCriteria data, int dataSize);
        /// <summary>
        /// This function is used to get dashboard statistics data
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DashboardStatistics getDashboardStatisticsData(SearchCriteria data);

        /// <summary>
        /// This function is used to get top locations pageviews 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<CountryNamePageView> AnalyzePageViewsByCountryName(SearchCriteria data, int dataSize);
        /// <summary>
        /// This function is used to get top post type with total page views and posts
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataSize"></param>
        /// <returns></returns>
        public List<PostTypePageViews> AnalyzePageViewsByPostType(SearchCriteria data);
        



    }
}

