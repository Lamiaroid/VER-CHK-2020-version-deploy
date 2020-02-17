using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebPosting.Models
{
    /// <summary>
    /// Class representing articles
    /// </summary>
    public class ArticleModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "CreatedUser")]
        public string CreatedUser { get; set; }

        [Display(Name = "CreatedDate")]
        public string CreatedDate { get; set; }
   
        [Display(Name = "Category")]
        public string Category { get; set; }
   
        [Display(Name = "Content")]
        public string Content { get; set; }
    }
}
