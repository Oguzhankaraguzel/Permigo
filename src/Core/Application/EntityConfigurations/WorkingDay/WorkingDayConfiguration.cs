using Application.EntityConfigurations.Abstractions;
using Domain.Entities.Calender;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.EntityConfigurations.WorkingDays;

public sealed class WorkingDayConfiguration
    : BaseEntityTypeConfigurations<WorkingDay>
{
    public override void Configure(EntityTypeBuilder<WorkingDay> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Date)
               .IsRequired();

        builder.Property(x => x.Date)
               .HasConversion<DateOnly>()
               .HasColumnType<DateOnly>("date");

        builder.HasIndex(x => x.Date)
               .IsUnique();                        
    }
}
