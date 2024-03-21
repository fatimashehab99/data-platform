using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using DataPipeline.DataAnalysis.Models;

namespace DataPipeline.DataCollection.Models
{

    /// <summary>
    /// This class is used as a model for data attribute collected during a page view
    /// </summary>
    public class MongoDbPageView
    {

        [Key]
        [Required]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        public string? PostId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public string FormattedDate { get; set; }

        [Required]
        public string Ip { get; set; } = null!;

        [Required]
        public string CountryCode { get; set; } = null!;
        public string CountryName { get; set; } = null!;


        [Required]
        public string Browser { get; set; } = null!;
        [Required]

        public string Platform { get; set; } = null!;

        [Required]
        [BsonElement("Device")]
        public string Device { get; set; } = null!;



        #region Post

        [Required]
        [BsonElement("PostAuthor")]
        public string PostAuthor { get; set; } = null!;


        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PostPublishDate { get; set; }


        [Required]
        public string PostTitle { get; set; } = null!;
        [BsonElement("PostClasses")]
        public List<PostClass> PostClasses { get; set; }
        public string PostType { get; set; } 



        public string[]? PostTags { get; set; }
        public string PostImage { get; set; }
        [BsonElement("PostUrl")]
        public string? PostUrl { get; set; }


        [Required]
        public string PostCategory { get; set; } = null!;


        #endregion


        //domain like mayadeen.net       
        [Required]
        public string Domain { get; set; } = null!;


        [Required]
        public string UserId { get; set; } = null!;


    }
}