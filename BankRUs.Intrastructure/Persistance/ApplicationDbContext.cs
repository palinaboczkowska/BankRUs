using BankRUs.Domain.Entities;
using BankRUs.Intrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Persistance;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<BankAccount> BankAccounts { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.SocialSecurityNumber)
            .IsUnique();

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // ⭐ Precision for BankAccount
        builder.Entity<BankAccount>(entity =>
        {
            entity.Property(x => x.Balance)
                  .HasPrecision(18, 2);
        });

        // ⭐ Precision for Transaction
        builder.Entity<Transaction>(entity =>
        {
            entity.Property(t => t.Amount)
                  .HasPrecision(18, 2);

            entity.Property(t => t.BalanceAfter)
                  .HasPrecision(18, 2);
        });

        builder.Entity<BankAccount>()
           .HasOne<ApplicationUser>()                 
           .WithMany(u => u.BankAccounts)           
           .HasForeignKey(a => a.UserId)  
           .HasPrincipalKey(u => u.Id); 
    }
}

