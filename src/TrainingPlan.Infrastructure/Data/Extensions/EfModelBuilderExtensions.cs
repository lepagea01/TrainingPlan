using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TrainingPlan.ApplicationCore.Interfaces;

namespace TrainingPlan.Infrastructure.Data.Extensions
{
    public static class EfModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                entityType.Relational().TableName = entityType.DisplayName();
        }

        public static void AddTrackableColumns(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(ITrackable).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreationDate");
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("LastModificationDate");
            }
        }
    }
}