using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Helpers.LocationService
{
    public interface ILocationService
    {
        /// <summary>
        /// this function is used to get country name from ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Dictionary<string, string> getCountryInfo(string ip);
    }
}
