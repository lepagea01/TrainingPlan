using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.ApplicationCore.Interfaces;
using TrainingPlan.Infrastructure.Data.Configurations;
using TrainingPlan.Infrastructure.Data.Extensions;

namespace TrainingPlan.Infrastructure.Data
{
    public class TrainingPlanContext : DbContext
    {
        public TrainingPlanContext(DbContextOptions<TrainingPlanContext> options) : base(options)
        {
        }

        public DbSet<Workout> Workouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply global configuration(s):
            modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.AddTrackableColumns();

            // Apply specific configuration(s):
            modelBuilder.ApplyConfiguration(new WorkoutEntityTypeConfiguration());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries<ITrackable>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            // I preferred a two-step approach here in order to avoid testing property
            //  existence when irrelevant (e.g. EntityState.Unchanged...).
            foreach (var entry in entries.Where(e =>
                                                e.Metadata.GetProperties().Any(p => p.Name == "CreationDate") &&
                                                e.Metadata.GetProperties().Any(p => p.Name == "LastModificationDate")))
            {
                var now = DateTime.UtcNow;
                entry.Property("LastModificationDate").CurrentValue = now;
                if (entry.State == EntityState.Added) entry.Property("CreationDate").CurrentValue = now;
            }
        }
    }
}