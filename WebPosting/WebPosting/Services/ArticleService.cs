using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebPosting.Models;

namespace WebPosting.Services
{
    /// <summary>
    /// Service to work with articles
    /// </summary>
    public class ArticleService
    {
        const int MaxArticleContentLength = 2000;
        const int MaxCommentLength = 200;

        IMongoCollection<ArticleModel> Articles;
        IMongoCollection<CommentModel> Comments;

        public ArticleService()
        {
            string connectionString = "mongodb://127.0.0.1:27017/WebPosting";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            Articles = database.GetCollection<ArticleModel>("Articles");
            Comments = database.GetCollection<CommentModel>("Comments");
        }
        
        /// <summary>
        /// Get all existing articles using filter
        /// </summary>
        /// <param name="filter">Type of filter to use</param>
        /// <param name="searchingQuery">String for filtering</param>
        /// <returns></returns>
        public async Task<IEnumerable<ArticleModel>> GetArticles(string filter, string searchingQuery)
        {
            var builder = new FilterDefinitionBuilder<ArticleModel>();
            var filtrator = builder.Empty;

            if (filter == "FullTitle" && !String.IsNullOrEmpty(searchingQuery))
            {
                filtrator = filtrator & builder.Regex("Title", new BsonRegularExpression("^" + searchingQuery + "$"));
            }
            if (filter == "PartialTitle" && !String.IsNullOrEmpty(searchingQuery))
            {
                filtrator = filtrator & builder.Regex("Title", new BsonRegularExpression(searchingQuery));
            }
            if (filter == "CreatedUser" && !String.IsNullOrEmpty(searchingQuery))
            {
                filtrator = filtrator & builder.Regex("CreatedUser", new BsonRegularExpression("^" + searchingQuery + "$"));
            }
            if (filter == "Category" && !String.IsNullOrEmpty(searchingQuery))
            {
                filtrator = filtrator & builder.Regex("Category", new BsonRegularExpression("^" + searchingQuery + "$"));
            }

            return await Articles.Find(filtrator).ToListAsync();
        }

        /// <summary>
        /// Get all existing comments on article
        /// </summary>
        /// <param name="searchingQuery">String for filtering</param>
        /// <returns></returns>
        public async Task<IEnumerable<CommentModel>> GetComments(string searchingQuery)
        {
            var builder = new FilterDefinitionBuilder<CommentModel>();
            var filter = builder.Empty;

            if (!String.IsNullOrEmpty(searchingQuery))
            {
                filter = filter & builder.Regex("Title", new BsonRegularExpression("^" + searchingQuery + "$"));
            }

            return await Comments.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Get a single article
        /// </summary>
        /// <param name="articleTitle">Article title</param>
        /// <returns></returns>
        public async Task<ArticleModel> GetArticle(string articleTitle)
        {
            return await Articles.Find(a => a.Title == articleTitle).FirstOrDefaultAsync();
        }
        
        /// <summary>
        /// Create a new article
        /// </summary>
        /// <param name="article">Article model</param>
        /// <returns></returns>
        public async Task Create(ArticleModel article)
        {
            if (article.Content.Length > MaxArticleContentLength)
            {
                return;
            }
            else
            {
                article.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                await Articles.InsertOneAsync(article);
            }
        }

        /// <summary>
        /// Create new comment
        /// </summary>
        /// <param name="comment">Comment model</param>
        /// <returns></returns>
        public async Task CreateComment(CommentModel comment)
        {
            if (comment.Text.Length > MaxCommentLength)
            {
                return;
            }
            else
            {
                await Comments.InsertOneAsync(comment);
            }
        }
        
        /// <summary>
        /// Update article
        /// </summary>
        /// <param name="article">Article model to take data from</param>
        /// <param name="articleTitle">Article title to update</param>
        /// <returns></returns>
        public async Task Update(ArticleModel article, string articleTitle)
        {
            if (article.Content.Length > MaxArticleContentLength)
            {
                return;
            }
            else
            {
                var filter = Builders<ArticleModel>.Filter.Eq("Title", articleTitle);
                var update1 = Builders<ArticleModel>.Update.Set("Content", article.Content);
                var update2 = Builders<ArticleModel>.Update.Set("Category", article.Category);

                await Articles.UpdateOneAsync(filter, update1);
                await Articles.UpdateOneAsync(filter, update2);
            }
        }

        /// <summary>
        /// Remove one article from data base
        /// </summary>
        /// <param name="articleTitle">Article title</param>
        /// <returns></returns>
        public async Task Remove(string articleTitle)
        {
            await Articles.DeleteOneAsync(a => a.Title == articleTitle);
            await Comments.DeleteManyAsync(a => a.Title == articleTitle);
        }
    }
}