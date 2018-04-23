using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingPlan.ApplicationCore.Entities;

namespace TrainingPlan.Infrastructure.Data.Configurations
{
    public class WorkoutEntityTypeConfiguration : IEntityTypeConfiguration<Workout>
    {
        public void Configure(EntityTypeBuilder<Workout> builder)
        {
            builder.Property(w => w.Name)
                .HasDefaultValue("#N\\A")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(w => w.Description)
                .HasDefaultValue("#N\\A")
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}