using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataCollection.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.DataAnalysis.Services
{
    public interface IUserProfileDataService
    {
        public Dictionary<string, int> getTopCategoriesForSpecificUser(SearchCriteria criteria, string UserId);
        public Dictionary<string, int> getTopAuthorsForSpecificUser(SearchCriteria criteria, string UserId);
    }
}
