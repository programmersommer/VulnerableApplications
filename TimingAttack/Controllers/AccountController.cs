using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TimingAttack.Data;
using TimingAttack.Data.Entities;

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
        private readonly DemoDBContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountController(UserManager<IdentityUser> userManager, DemoDBContext context,
            IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        /// <summary>
        /// Use this endpoint to add dummy user to database
        /// </summary>
        /// <remarks>
        ///  There are no remarks
        /// </remarks>
        /// <param name="name">The username</param>
        /// <param name="email">The email address</param>
        /// <param name="password">The password</param>
        /// <response code="200">Just returns Ok</response>
        /// <response code="400">Request is invalid (e.g. unsafe password).</response>
        [HttpPost]
        public async Task<IActionResult> AddUser(string name, string email, string password)
        {
            var user = new IdentityUser { UserName = name, Email = email };
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        /// <summary>
        /// Authenticates a user and issues a JWT access token.
        /// </summary>
        /// <param name="req">The login credentials (email and password).</param>
        /// <returns>A JWT access token to send as a <c>Bearer</c> token in the <c>Authorization</c> header.</returns>
        /// <response code="200">Authentication succeeded. The response body is the access token.</response>
        /// <response code="401">The email or password is incorrect.</response>
        [HttpPost]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> Login(LoginRequest req)
        {
            var email = req.Email.Trim().ToLowerInvariant();
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null ||
                _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, req.Password) == PasswordVerificationResult.Failed)
                return Unauthorized();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(30),
                Claims = new Dictionary<string, object> { ["sub"] = user.Id.ToString(), ["email"] = user.Email },
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            });

            return token;
        }

        /// <summary>
        /// Use this endpoint to reset password
        /// </summary>
        /// <remarks>
        ///  Timing arrack could be used together with brute force to get existing logins
        /// </remarks>
        /// <param name="email"></param>
        /// <response code="200">Just returns Ok</response>
        [HttpPost("{email}")]
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

            // vulnerable to Host Header Attacks
            var uriBuilder = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port.Value, null)
            {
                Query = query.ToString()
            };

            var callbackUrl = uriBuilder.ToString();

            await _emailSender.SendEmailAsync(email, "Reset Password",
                     $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.");

            return Ok("In case if this address exist in database, mail with link was sent to it");
        }


        /// <summary>
        /// Use this endpoint to create new Bank Account
        /// </summary>
        /// <remarks>
        ///  Would be used in future for race condition / TOCTOU attack demonstration
        /// </remarks>
        /// <param name="number">Bank account number</param>   
        /// <param name="balance">Current balance</param>
        /// <response code="200">Just returns Ok</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost]
        public async Task<string> AddBankAccount(string number, decimal balance)
        {
            var bankAccount = new BankAccount { AccountNumber = number, Balance = balance };

            try
            {
                _context.BankAccounts.Add(bankAccount);
                await _context.SaveChangesAsync();
                return bankAccount.AccountNumber;
            }
            catch (DbUpdateException ex)
            {
                // bad practice is to return information about real exception
                throw new InvalidOperationException($"Account '{number}' could not be created (it may already exist).", ex);
            }
        }

        /// <summary>
        /// This endpoint is used for race condition / TOCTOU attack demonstration. 
        /// If send 2 or more requests in parallel, it would be possible to withdraw more money than available on the account.
        /// </summary>
        /// <param name="accountNumber">Bank account number</param>   
        /// <param name="amount">Amount to be withdrawn from account for some payment</param>
        /// <response code="200">Just returns Ok</response>
        /// <response code="400">Not enough money in the account</response>
        [HttpPost]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Charge(string accountNumber, decimal amount)
        {
            // could be mitigation for SQLite. For else Databases you can consider RowVersion
            //await using var transaction = await _context.Database.BeginTransactionAsync();

            var account = await _context.BankAccounts
                .FirstAsync(a => a.AccountNumber == accountNumber);

            if (account.Balance < amount)
                return BadRequest("Insufficient money in the account");

            // Simulate some processing, for example does User eligable to withdraw money (e.g. check if account is blocked, etc.)
            await Task.Delay(5000);

            account.Balance -= amount;

            await _context.SaveChangesAsync();
            //await transaction.CommitAsync();

            return Ok();
        }

    }
}
