using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.DataAnalysis.Models
{
    /// <summary>
    /// This class is used to read data from mongo database 
    /// </summary>
    public class SearchCriteria
    {
        public string? Author { get; set; }
        public string? Category { get; set; }
        public string? Domain { get; set; }
        public string CountryName { get; set; }
        public int Size { get; set; }
        public string? PostType { get; set; }
       
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

    }
}
