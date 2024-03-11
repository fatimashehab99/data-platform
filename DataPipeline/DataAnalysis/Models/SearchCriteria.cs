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
        public string? Sort { get; set; }
        public string? Order { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public string? Device { get; set; }
        public string? Domain { get; set; }
        public int PageNumber { get; set; }
        public int Size { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }
}
