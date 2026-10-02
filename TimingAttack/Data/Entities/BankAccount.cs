using System;

namespace TimingAttack.Data.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }

        /// <summary>
        /// Doesn't work for SQLite, but work with else databases(
        /// </summary>
        //public byte[] RowVersion { get; set; } = null!; // race condition / TOCTOU mitigation
    }
}
