using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.DataAnalysis.Models
{
    public class DashboardStatistics
    {
        public int pageViews { get; set; }
        public int authors { get; set; }
        public int articles { get; set; }
        public int users { get; set; }
    }
}
