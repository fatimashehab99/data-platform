using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.DataAnalysis.Models
{
    public class ArticlePageView
    {
        public string PostTitle { get; set; }
        public int PostId { get; set; }
        public int PageViews { get; set; }
        public string PostUrl { get; set; }
        public string PostImage { get; set; }
        public string PostAuthor { get; set; }
        public string PostCategory { get; set; }
        public DateTime PublishedDate { get; set; }



    }
}
