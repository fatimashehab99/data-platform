using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.DataAnalysis.Models
{
    public class DatePageView
    {
        public string Date { get; set; }
        public int PageViews { get; set; }
        public int PublishedArticles { get; set; }
    }
}
