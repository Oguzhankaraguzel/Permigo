using Application.EntityConfigurations.Abstractions;
using Domain.Entities.Leaves;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.EntityConfigurations.Leaves;
public sealed class LeaveRequestConfiguration
    : BaseEntityTypeConfigurations<LeaveRequest>
{
    public override void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Status)
               .HasConversion<int>()
               .IsRequired();

        builder.Property(x => x.StartDate)
               .HasColumnType("date")
               .IsRequired();

        builder.Property(x => x.EndDate)
               .HasColumnType("date")
               .IsRequired();

        builder.Property(x => x.DurationDays)
               .IsRequired();

        builder.Property(x => x.Reason)
               .HasMaxLength(512);

        builder.HasOne(x => x.LeaveType)
               .WithMany(t => t.Requests)
               .HasForeignKey(x => x.LeaveTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RequestingEmployee)
               .WithMany()
               .HasForeignKey(x => x.RequestingEmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Approver)
               .WithMany()
               .HasForeignKey(x => x.ApproverId)
               .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(x => x.RequestingEmployeeId);
    }
}
