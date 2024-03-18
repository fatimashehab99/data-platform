using DataPipeline.DataAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.DataAnalysis.Services
{
    /// <summary>
    /// This function is used to get trending articles
    /// </summary>
    public interface IArticlesService
    {
        public List<ArticlePageView> getTrendingArticles(SearchCriteria criteria, string IP, int dataSize);
        public List<ArticlePageView> getRecommendedArticles(SearchCriteria search, string userId, string ip, int dataSize);
    }
}
