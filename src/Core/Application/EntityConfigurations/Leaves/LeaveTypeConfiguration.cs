using Application.EntityConfigurations.Abstractions;
using Domain.Entities.Leaves;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.EntityConfigurations.Leaves;

public sealed class LeaveTypeConfiguration
    : BaseEntityTypeConfigurations<LeaveType>
{
    public override void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        base.Configure(builder);


        builder.Property(x => x.Name)
               .HasMaxLength(64)
               .IsRequired();

        builder.HasIndex(x => x.Name)              
               .IsUnique();

        builder.Property(x => x.ApplicableGender)
               .HasConversion<int>();               

        builder.HasMany(x => x.Requests)
               .WithOne(r => r.LeaveType)
               .HasForeignKey(r => r.LeaveTypeId);
    }
}
