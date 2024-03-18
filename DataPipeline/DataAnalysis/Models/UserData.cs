using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.DataAnalysis.Models
{
    public class UserData
    {
        public string UserId { get; set; }
        public string CountryName { get; set; }
        public string Domain { get; set; }
        public Dictionary<string, int> TopAuthors { get; set; }
        public Dictionary<string, int> TopCategories { get; set; }
        public Dictionary<string, int> TopTags { get; set; }

    }
}
