using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimingAttack.Data.Entities;

namespace TimingAttack.Data.Configurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.AccountNumber).IsRequired().HasMaxLength(10);
            b.HasIndex(x => x.AccountNumber).IsUnique();
            b.Property(x => x.Balance).HasPrecision(14, 2);
            //b.Property(x => x.RowVersion).IsRowVersion(); // race condition / TOCTOU mitigation
        }
    }
}
