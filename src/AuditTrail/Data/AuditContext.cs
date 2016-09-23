using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditTrail.Data
{
    public class AuditContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
        }

        public override int SaveChanges()
        {
            var changedEntities = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).ToList();
            var now = DateTime.UtcNow;

            foreach (var changedEntity in changedEntities)
            {
                var entityName = changedEntity.Entity.GetType().Name;
                var primaryKey = changedEntity.Metadata.FindPrimaryKey();

                foreach (var prop in  changedEntity.Metadata.GetProperties())
                {
                    var originalValue = changedEntity.Property(prop.Name).OriginalValue.ToString();
                    var currentValue = changedEntity.Property(prop.Name).CurrentValue.ToString();
                    if (originalValue != currentValue)
                    {
                        AuditLog log = new AuditLog()
                        {
                            EntityName = entityName,
                            PrimaryKeyValue = primaryKey.ToString(),
                            PropertyName = prop.Name,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            DateChanged = now
                        };
                        AuditLogs.Add(log);
                    }
                }
            }
            return base.SaveChanges();
        }

    }
}
