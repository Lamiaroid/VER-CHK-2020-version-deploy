using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebPosting.Models;
using WebPosting.Services;

namespace WebPosting.Controllers
{
    /// <summary>
    /// Main controller for working with articles and comments
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ArticleService db;

        public HomeController(ArticleService context)
        {
            db = context;
        }

        /// <summary>
        /// Display home page
        /// </summary>
        /// <param name="filterName">Param for choosing filtering articles method</param>
        /// <param name="searchingQuery">String for filtering</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(string filterName, string searchingQuery)
        {
            var articles = await db.GetArticles(filterName, searchingQuery);
            var model = new IndexViewModel { Articles = articles, Comments = null };
            return View(model);
        }

        /// <summary>
        /// Create new article (requires authorization)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create new article  (requires authorization)
        /// </summary>
        /// <param name="article">Article to create</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ArticleModel article)
        {
            if (article != null && !String.IsNullOrEmpty(article.Category) && !String.IsNullOrEmpty(article.Content) 
                    && !String.IsNullOrEmpty(article.Title))
            { 
                if (ModelState.IsValid)
                {
                    ArticleModel articleModel = await db.GetArticle(article.Title);
                    if (articleModel == null)
                    {
                        await db.Create(article);
                        return RedirectToAction("Index");
                    }
                }

                return View(article);
            }

            return View();
        }

        /// <summary>
        /// Remove article (requires authorization)
        /// </summary>
        /// <param name="articleTitle">Article title to remove</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Remove(string articleTitle)
        {
            if (!String.IsNullOrEmpty(articleTitle))
            {
                await db.Remove(articleTitle);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Display page with article data to change (requires authorization)
        /// </summary>
        /// <param name="articleTitle">Article title to edit</param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> DisplayEditArticlePage(string articleTitle)
        {
            if (!String.IsNullOrEmpty(articleTitle))
            {
                ArticleModel article = await db.GetArticle(articleTitle);
                return View(article);
            }

            return View();
        }

        /// <summary>
        /// Edit article (requires authorization)
        /// </summary>
        /// <param name="article">Article to edit</param>
        /// <param name="articleTitle">Article title to edit</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(ArticleModel article, string articleTitle)
        {
            if (!String.IsNullOrEmpty(article.Category) && !String.IsNullOrEmpty(article.Content)
                    && !String.IsNullOrEmpty(articleTitle))
            {
                if (ModelState.IsValid)
                {
                    await db.Update(article, articleTitle);                  
                }            
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Show article (requires authorization)
        /// </summary>
        /// <param name="articleTitle">Article title to show</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ShowArticle(string articleTitle)
        {
            if (!String.IsNullOrEmpty(articleTitle))
            {
                var articles = await db.GetArticles("FullTitle", articleTitle);
                var comments = await db.GetComments(articleTitle);
                var model = new IndexViewModel { Articles = articles, Filter = null, Comments = comments };
                return View(model);
            }

            return View();
        }

        /// <summary>
        /// Post a comment (requires authorization)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult PostComment()
        {
            return View();
        }

        /// <summary>
        /// Post a comment (requires authorization)
        /// </summary>
        /// <param name="userName">User posting comment</param>
        /// <param name="articleTitle">Article where comment will be posted</param>
        /// <param name="commentText">Comment text</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostComment(string userName, string articleTitle, string commentText)
        {
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(articleTitle) 
                    && !String.IsNullOrEmpty(commentText))
            {
                CommentModel comment = new CommentModel();
                comment.Name = userName;
                comment.Title = articleTitle;
                comment.Text = commentText;

                await db.CreateComment(comment);
            }

            return RedirectToAction("ShowArticle", "Home", new { articleTitle });
        }
    }
}
