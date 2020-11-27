using GroupUp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GroupUp.Data
{
    public class GroupUpContext : IdentityDbContext<User, Role, Guid>
    {
        private readonly ILogger<GroupUpContext> _logger;

        public DbSet<Group> Groups { get; set; }

        public GroupUpContext(
            DbContextOptions<GroupUpContext> options,
            ILogger<GroupUpContext> logger
    
        ) : base(options)
        {
            _logger = logger;
        }

        public override int SaveChanges()
        {
            SetTimestamps();
            return base.SaveChanges();
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SetTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.Ignore(e => e.FullName);
            });
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries().Where(e =>
                (e.Entity is IBaseModel) &&
                (e.State == EntityState.Added || e.State == EntityState.Modified)
            );

            foreach (var entry in entries)
            {
                var now = DateTime.Now;

                switch (entry.State)
                {
                    case EntityState.Added:
                        ((IBaseModel)entry.Entity).CreatedAt = now;
                        ((IBaseModel)entry.Entity).UpdatedAt = now;
                        break;
                    case EntityState.Modified:
                        ((IBaseModel)entry.Entity).UpdatedAt = now;
                        break;
                }
            }
        }
    }
}
