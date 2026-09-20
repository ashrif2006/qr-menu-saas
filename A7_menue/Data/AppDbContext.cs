using A7_menue.Models;
using Microsoft.EntityFrameworkCore;

namespace A7_menue.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<MenuItemTranslation> MenuItemTranslations { get; set; }
    public DbSet<MenuItemVariant> MenuItemVariants { get; set; }
    public DbSet<MenuItemVariantTranslation> MenuItemVariantTranslations { get; set; }

    public DbSet<User> users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===== Tenant =====
        modelBuilder.Entity<Tenant>()
            .HasIndex(t => t.Slug)
            .IsUnique(); 

        // ===== MenuCategory =====
        modelBuilder.Entity<MenuCategory>()
            .HasOne(c => c.Tenant)
            .WithMany(t => t.Categories)
            .HasForeignKey(c => c.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        // ===== CategoryTranslation =====
        modelBuilder.Entity<CategoryTranslation>()
            .HasOne(ct => ct.Category)
            .WithMany(c => c.Translations)
            .HasForeignKey(ct => ct.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CategoryTranslation>()
            .HasIndex(ct => new { ct.CategoryId, ct.LanguageCode })
            .IsUnique(); 

        // ===== MenuItem =====
        modelBuilder.Entity<MenuItem>()
            .HasOne(mi => mi.Tenant)
            .WithMany()
            .HasForeignKey(mi => mi.TenantId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<MenuItem>()
            .HasOne(mi => mi.Category)
            .WithMany(c => c.MenuItems)
            .HasForeignKey(mi => mi.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuItem>()
            .Property(mi => mi.Price)
            .HasColumnType("decimal(10,2)");

        // ===== MenuItemTranslation =====
        modelBuilder.Entity<MenuItemTranslation>()
            .HasOne(mit => mit.MenuItem)
            .WithMany(mi => mi.Translations)
            .HasForeignKey(mit => mit.MenuItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MenuItemTranslation>()
            .HasIndex(mit => new { mit.MenuItemId, mit.LanguageCode })
            .IsUnique();

        // ===== MenuItemVariant =====
        modelBuilder.Entity<MenuItemVariant>()
            .HasOne(v => v.MenuItem)
            .WithMany(mi => mi.Variants)
            .HasForeignKey(v => v.MenuItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MenuItemVariant>()
            .Property(v => v.Price)
            .HasColumnType("decimal(10,2)");

        // ===== MenuItemVariantTranslation =====
        modelBuilder.Entity<MenuItemVariantTranslation>()
            .HasOne(vt => vt.Variant)
            .WithMany(v => v.Translations)
            .HasForeignKey(vt => vt.VariantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MenuItemVariantTranslation>()
            .HasIndex(vt => new { vt.VariantId, vt.LanguageCode })
            .IsUnique();

        //====User===
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasOne(u => u.Tenant)
            .WithMany()
            .HasForeignKey(U => U.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
