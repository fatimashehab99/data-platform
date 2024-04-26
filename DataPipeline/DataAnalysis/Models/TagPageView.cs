using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.DataAnalysis.Models
{
    public class TagPageView
    {
        public string tags { get; set; }
        public int pageviews { get; set; }
    }
}
