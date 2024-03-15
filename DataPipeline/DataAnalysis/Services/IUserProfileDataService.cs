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
        /// <summary>
        /// This function is used to get top categories interested by specific usr
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Dictionary<string, int> getTopCategoriesForSpecificUser(SearchCriteria criteria, string UserId);
        /// <summary>
        /// This function is used to get top authors interested by specific usr
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Dictionary<string, int> getTopAuthorsForSpecificUser(SearchCriteria criteria, string UserId);
        /// <summary>
        /// This function is used to get top tags for specific user
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Dictionary<string, int> getTopTagsForSpecificUser(SearchCriteria criteria, string UserId, int dataSize);


    }
}
