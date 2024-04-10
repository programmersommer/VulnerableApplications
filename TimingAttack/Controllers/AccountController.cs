using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TimingAttack.Controllers
{
    /// <summary>
    /// Account controller
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountController(UserManager<IdentityUser> userManager,
            IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        /// <summary>
        /// Use this endpoint to add dummy user to database
        /// </summary>
        /// <remarks>
        ///  There are no remarks
        /// </remarks>
        /// <response code="200">Just returns Ok</response>
        [HttpGet]
        public async void AddUser()
        {
            var user = new IdentityUser { UserName = "johndoe", Email = "johndoe@gmail.com" };
            var result = await _userManager.CreateAsync(user, "!QAZ2wsx");
        }

        /// <summary>
        /// Use this endpoint to reset password
        /// </summary>
        /// <remarks>
        ///  Timing arrack could be used together with brute force to get existing logins
        /// </remarks>
        /// <param name="email"></param>
        /// <response code="200">Just returns Ok</response>
        [HttpGet("{email}")]
        public async Task<IActionResult> PasswordReset(string email)
        {
            // with next one random delay it would be hard to guess based on request execution time
            // does user with specified email exist in db or not
            // Random rnd = new Random();
            // Thread.Sleep(rnd.Next(10, 100));

            var user = await _userManager.FindByEmailAsync(email);
            if (user == default)
            {
                // same message in case if email exist in database and in case if not
                return Ok("In case if this address exist in database, mail with link was sent to it");
            }

            // in case if user was found, result would be returned with some delay
            // because next logic execution might take a time

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["code"] = code;

            var uriBuilder = new UriBuilder("https", _configuration["ResetPasswordUrl"], -1, null);
            uriBuilder.Query = query.ToString();
            var callbackUrl = uriBuilder.ToString();

            await _emailSender.SendEmailAsync(email, "Reset Password",
                     $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.");

            return Ok("In case if this address exist in database, mail with link was sent to it");
        }
    }
}
