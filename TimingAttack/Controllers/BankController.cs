using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TimingAttack.Data;
using TimingAttack.Data.Entities;

namespace TimingAttack.Controllers
{
    /// <summary>
    /// Account controller
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class BankController : ControllerBase
    {
        private readonly DemoDBContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        public BankController(DemoDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
