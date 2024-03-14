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
        public int getTotalPageViews(SearchCriteria criteria);
        public int getTotalAuthors(SearchCriteria criteria);
        public int getTotalArticles(SearchCriteria criteria);
        public int getTotalUsers(SearchCriteria criteria);
    }
}
