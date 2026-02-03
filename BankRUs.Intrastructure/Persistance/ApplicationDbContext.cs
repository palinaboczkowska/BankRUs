using BankRUs.Domain.Entities;
using BankRUs.Intrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankRUs.Intrastructure.Persistance;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<BankAccount> BankAccounts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.SocialSecurityNumber)
            .IsUnique();

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Entity<BankAccount>(builder =>
        {
            builder.Property(x => x.Balance)
              .HasPrecision(18, 2);
        });
        builder.Entity<BankAccount>()
           .HasOne<ApplicationUser>()                 
           .WithMany(u => u.BankAccounts)           
           .HasForeignKey(a => a.UserId)  
           .HasPrincipalKey(u => u.Id); 
    }
}

