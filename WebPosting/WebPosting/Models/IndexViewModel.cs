using System.Collections.Generic;

namespace WebPosting.Models
{
    /// <summary>
    /// Class representing data for correct pages view
    /// </summary>
    public class IndexViewModel
    {
        public FilterViewModel Filter { get; set; }

        public IEnumerable<ArticleModel> Articles { get; set; }

        public IEnumerable<CommentModel> Comments { get; set; }
    }
}
