using Domain.Abstractions;
using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.EntityConfigurations.Abstractions;

public abstract class BaseEntityTypeConfigurations<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.Property(t => t.CreatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
        builder.Property(t => t.UpdatedAt).HasConversion(d => d != null ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);
        builder.Property(t => t.DeletedAt).HasConversion(d => d != null ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : d, v => v);

        builder.HasOne(x => x.DeleteUser)
            .WithMany()
            .HasForeignKey(e => e.CreateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreateUser)
            .WithMany()
            .HasForeignKey(e => e.UpdateUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UpdateUser)
            .WithMany()
            .HasForeignKey(e => e.DeleteUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
