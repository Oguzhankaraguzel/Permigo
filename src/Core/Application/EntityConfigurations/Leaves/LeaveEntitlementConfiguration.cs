using Application.EntityConfigurations.Abstractions;
using Domain.Entities.Leaves;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.EntityConfigurations.Leaves;
public sealed class LeaveEntitlementConfiguration
    : BaseEntityTypeConfigurations<LeaveEntitlement>
{
    public override void Configure(EntityTypeBuilder<LeaveEntitlement> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.Employee)
            .WithMany() 
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(x => x.LeaveType)
            .WithMany(lt => lt.Entitlements) 
            .HasForeignKey(x => x.LeaveTypeId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.Property(x => x.Year)
            .IsRequired();

        builder.Property(x => x.AllocatedDays)
            .IsRequired();

        builder.Property(x => x.UsedDays)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasIndex(x => new { x.EmployeeId, x.LeaveTypeId, x.Year })
            .IsUnique();
    }
}
