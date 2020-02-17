using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebPosting.Models;
using WebPosting.Services;

namespace WebPosting.Controllers
{
    /// <summary>
    /// Controller providing operations to work with users
    /// </summary>
    public class UserController : Controller
    {
        private readonly UserService db;

        public UserController(UserService context)
        {
            db = context;
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="userModel">User to create</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(UserModel userModel)
        {
            if (!String.IsNullOrEmpty(userModel.Email) && !String.IsNullOrEmpty(userModel.Name)
                    && !String.IsNullOrEmpty(userModel.Password))
            {
                if (ModelState.IsValid)
                {
                    UserModel user = await db.GetUser(userModel, true);
                    if (user == null)
                    {
                        user = await db.GetUser(userModel, false);
                        if (user == null)
                        {
                            await db.Create(userModel);
                            await Authenticate(userModel.Name);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            return RedirectToAction("SignIn", "User");
        }

        /// <summary>
        /// Sign in
        /// </summary>
        /// <returns></returns>
        public IActionResult SignIn()
        {
            return View();
        }

        /// <summary>
        /// Log in
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        /// <summary>
        /// Log in
        /// </summary>
        /// <param name="userModel">User to log in</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(UserModel userModel)
        {
            if (!String.IsNullOrEmpty(userModel.Name) && !String.IsNullOrEmpty(userModel.Password))
            {
                if (ModelState.IsValid)
                {
                    IEnumerable<UserModel> people = await db.GetUsers(userModel.Name);
                    UserModel user = people.ToList().FirstOrDefault(u => u.Name == userModel.Name && u.Password == userModel.Password);
                    if (user != null)
                    {
                        await Authenticate(user.Name);
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Wrong name and(or) password");
                }
            }

            return View(userModel);
        }

        /// <summary>
        /// Logout from account
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Restore the forgotten password
        /// </summary>
        /// <returns></returns>
        public IActionResult RestorePassword()
        {
            return View();
        }

        /// <summary>
        /// Restore the forgotten password
        /// </summary>
        /// <param name="userModel">User to send password</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RestorePassword(UserModel userModel)
        {
            if (!String.IsNullOrEmpty(userModel.Email))
            {
                UserModel user = await db.GetUser(userModel, true);

                if (user != null)
                {
                    await db.SendPasswordByEmail(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            
            return View();
        }

        /// <summary>
        /// Authentification through cookies
        /// </summary>
        /// <param name="userName">User name to auth</param>
        /// <returns></returns>
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}