using DataPipeline.DataAnalysis.Models;

namespace DataPipeline.DataAnalysis.Services
{

    /// <summary>
    /// Is used for data analysis
    /// </summary>
    public interface IDataAnalyticsService
    {
        /// <summary>
        /// this function is used to analyze authors country name pageviews  
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<AuthorPageView> AnalyzeAuthorPageViews(SearchCriteria data);

        /// <summary>
        /// This function is used to analyze top date name pageviews 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<DatePageView> AnalyzePageViewsByDate(SearchCriteria data);

        /// <summary>
        /// This function is used to analyze top category name pageviews 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<CategoryPageView> AnalyzePageViewsByCategory(SearchCriteria data);
        /// <summary>
        /// This function is used to get dashboard statistics data
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public DashboardStatistics getDashboardStatisticsData(SearchCriteria data);

        /// <summary>
        /// This function is used to analyze top country name pageviews 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<CountryNamePageView> AnalyzePageViewsByCountryName(SearchCriteria data, int dataSize);
        /// <summary>
        /// This function is used to get analyze top posttype pageviews
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataSize"></param>
        /// <returns></returns>
        public List<PostTypePageViews> AnalyzePageViewsByPostType(SearchCriteria data);
        /// <summary>
        /// This function is used to analyze tags pageviews
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<TagPageView> AnalyzePageViewsByTag(SearchCriteria data);




    }
}

