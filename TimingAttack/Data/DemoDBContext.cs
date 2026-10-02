using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimingAttack.Data.Entities;


namespace TimingAttack.Data
{
    public class DemoDBContext : IdentityDbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
                                  => options.UseSqlite("Data Source=DemoDB.db");
    }
}
