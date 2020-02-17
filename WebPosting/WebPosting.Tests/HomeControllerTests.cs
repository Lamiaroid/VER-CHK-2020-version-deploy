using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;
using WebPosting.Models;
using WebPosting.Services;
using WebPosting.Controllers;

namespace WebPosting.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task HomeControllerCreateArticle()
        {
            var homeController = new HomeController(new ArticleService());
            var article = new ArticleModel { Title = "111", Category = "222", Content = "111", CreatedUser="Me" };

            IActionResult result = await homeController.Create(article);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult routeResult = result as RedirectToActionResult;
            Assert.AreEqual(routeResult.ActionName, "Index");
        }

        [Test]
        public async Task HomeControllerEditArticle()
        {
            var homeController = new HomeController(new ArticleService());
            var article = new ArticleModel { Category = "333", Content = "222" };
            var articleTitle = "111";

            IActionResult result = await homeController.Edit(article, articleTitle);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult routeResult = result as RedirectToActionResult;
            Assert.AreEqual(routeResult.ActionName, "Index");
        }

        [Test]
        public async Task HomeControllerRemoveArticle()
        {
            var homeController = new HomeController(new ArticleService());
            var articleTitle = "111";

            IActionResult result = await homeController.Remove(articleTitle);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult routeResult = result as RedirectToActionResult;
            Assert.AreEqual(routeResult.ActionName, "Index");
        }

        [Test]
        public async Task HomeControllerCreateArticleWithEmptyTitle()
        {
            var homeController = new HomeController(new ArticleService());
            var article = new ArticleModel { Title = "", Category = "222", Content = "111", CreatedUser = "Me" };

            IActionResult result = await homeController.Create(article);
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task HomeControllerCreateArticleWithNullArticle()
        {
            var homeController = new HomeController(new ArticleService());

            IActionResult result = await homeController.Create(null);
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}