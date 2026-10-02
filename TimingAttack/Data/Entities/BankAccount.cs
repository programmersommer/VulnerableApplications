using System.ComponentModel.DataAnnotations;

namespace TimingAttack.Data.Entities
{
    public class BankAccount
    {
        [Key]
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
