using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebPosting.Models
{
    /// <summary>
    /// Class representing comments
    /// </summary>
    public class CommentModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id;

        [Display(Name = "Title")]
        public string Title;

        [Display(Name = "Name")]
        public string Name;

        [Display(Name = "Text")]
        public string Text;
    }
}
