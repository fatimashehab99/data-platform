namespace DataServing.Models
{
    public class RequestData
    {

        public string? IP { get; set; }


        //Post Data
        public string? PostAuthor { get; set; }
        public string PostTitle { get; set; }
        public string PostId { get; set; }
        public string PostCategory { get; set; }
        public string PostImage { get; set; }
        public string? PostUrl { get; set; }
        public DateTime PostPublishDate { get; set; }
        public string[]? PostTags { get; set; }
        public string PostClasses { get; set; }

        public string Domain { get; set; }
        public string UserId { get; set; }

    }
}
