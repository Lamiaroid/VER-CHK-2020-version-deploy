using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebPosting.Controllers.Authorization
{
    /// <summary>
    /// Controller for getting various data
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        /// <summary>
        /// Receive users current name (login)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("getlogin")]
        public IActionResult GetLogin()
        {
            return Ok($"{User.Identity.Name}");
        }
    }
}