using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace TimingAttack.Entities
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class DemoDBContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
                                  => options.UseSqlite("Data Source=DemoDB.db");
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
