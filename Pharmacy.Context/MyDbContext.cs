using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Context
{
    public class MyDbContext : IdentityDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) :base(options) 
        {
            
        }

        public DbSet<Category> categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== GLOBAL QUERY FILTER (SOFT DELETE) ==========

            //modelBuilder.Entity<Category>().HasQueryFilter(e => !e.IsDeleted);
            //modelBuilder.Entity<Supplier>().HasQueryFilter(e => !e.IsDeleted);
            //modelBuilder.Entity<Medicine>().HasQueryFilter(e => !e.IsDeleted);
            //modelBuilder.Entity<Invoice>().HasQueryFilter(e => !e.IsDeleted);
            //modelBuilder.Entity<InvoiceItem>().HasQueryFilter(e => !e.IsDeleted);



            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var compare = Expression.Equal(property, Expression.Constant(false));
                    var lambda = Expression.Lambda(compare, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            // ========== CASCADE DELETE RELATIONS ==========
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Medicines)
                .WithOne(m => m.Category)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Medicines)
                .WithOne(m => m.Supplier)
                .HasForeignKey(m => m.SupplierId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Medicine>()
                .HasMany(m => m.InvoiceItems)
                .WithOne(ii => ii.Medicine)
                .HasForeignKey(ii => ii.MedicineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.InvoiceItems)
                .WithOne(ii => ii.Invoice)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // ========== DECIMAL PRECISION ==========
            modelBuilder.Entity<Medicine>().Property(m => m.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.Subtotal).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.Tax).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.Discount).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.Total).HasPrecision(18, 2);
            modelBuilder.Entity<InvoiceItem>().Property(ii => ii.UnitPrice).HasPrecision(18, 2);


            modelBuilder.Entity<Category>().HasData(
                   new Category { Id = 1, Name = "Painkillers", Description = "Medicines used to relieve pain", CreatedAt = new DateTime(2025, 1, 1), IsDeleted = false },
                   new Category { Id = 2, Name = "Antibiotics", Description = "Medicines used to treat bacterial infections", CreatedAt = new DateTime(2025, 1, 1), IsDeleted = false },
                   new Category { Id = 3, Name = "Vitamins", Description = "Supplements for vitamins and minerals", CreatedAt = new DateTime(2025, 1, 1), IsDeleted = false }
               );

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { Id = 1, Name = "Moataz", Phone = "01000456050", Address = "Unknown", CreatedAt = new DateTime(2025, 1, 1), IsDeleted = false }
            );

            modelBuilder.Entity<Medicine>().HasData(
                new Medicine { Id = 1, Name = "Paracetamol", Description = "Painkiller tablet", Price = 10, Quantity = 100, CategoryId = 1, SupplierId = 1, CreatedAt = new DateTime(2025, 1, 1), IsDeleted = false },
                new Medicine { Id = 2, Name = "Ibuprofen", Description = "Anti-inflammatory", Price = 15, Quantity = 80, CategoryId = 1, SupplierId = 1, CreatedAt = new DateTime(2025, 1, 1), IsDeleted = false },
                new Medicine { Id = 3, Name = "Aspirin", Description = "Pain and fever relief", Price = 12, Quantity = 90, CategoryId = 1, SupplierId = 1, CreatedAt = new DateTime(2025, 1, 1), IsDeleted = false }
            // أضف باقي الأدوية بنفس الطريقة
            );



        }

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var now = DateTime.UtcNow;

        //    foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        //    {
        //        if (entry.State == EntityState.Added)
        //        {
        //            entry.Entity.CreatedAt = now;
        //            entry.Entity.UpdatedAt = null;
        //        }
        //        else if (entry.State == EntityState.Modified)
        //        {
        //            entry.Entity.UpdatedAt = now;
        //        }
        //    }

        //    return base.SaveChangesAsync(cancellationToken);
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.UpdatedAt = null;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;   // ما نعملش Delete حقيقي
                        entry.Entity.IsDeleted = true;        // نفعّل Soft Delete
                        entry.Entity.UpdatedAt = now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
