using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading.Tasks;
using WebPosting.Models;
using WebPosting.Services;
using WebPosting.Controllers;

namespace WebPosting.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task UserControllerCreateUser()
        {
            var userController = new UserController(new UserService());
            var user = new UserModel { Name = "tttttt", Password = "222", Email = "xxxaxxx1@gmail.com" };

            IActionResult result = await userController.Create(user);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task UserControllerCreateUserWithExistingName()
        {
            var userController = new UserController(new UserService());
            var user = new UserModel { Name = "tttttt", Password = "222", Email = "333a@gmail.com" };

            IActionResult result = await userController.Create(user);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult routeResult = result as RedirectToActionResult;
            Assert.AreEqual(routeResult.ActionName, "SignIn");
        }

        [Test]
        public async Task UserControllerCreateUserWithExistingEmail()
        {
            var userController = new UserController(new UserService());
            var user = new UserModel { Name = "555", Password = "222", Email = "xxxaxxx1@gmail.com" };

            IActionResult result = await userController.Create(user);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult routeResult = result as RedirectToActionResult;
            Assert.AreEqual(routeResult.ActionName, "SignIn");
        }

        [Test]
        public async Task UserControllerCreateUserWithEmptyData()
        {
            var userController = new UserController(new UserService());
            var user = new UserModel { Name = "", Password = "", Email = "222a@gmail.com" };

            IActionResult result = await userController.Create(user);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult routeResult = result as RedirectToActionResult;
            Assert.AreEqual(routeResult.ActionName, "SignIn");
        }

        [Test]
        public async Task UserControllerCreateUserWithNullData()
        {
            var userController = new UserController(new UserService());
            var user = new UserModel { Name = "999", Password = null, Email = null };

            IActionResult result = await userController.Create(user);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            RedirectToActionResult routeResult = result as RedirectToActionResult;
            Assert.AreEqual(routeResult.ActionName, "SignIn");
        }
    }
}