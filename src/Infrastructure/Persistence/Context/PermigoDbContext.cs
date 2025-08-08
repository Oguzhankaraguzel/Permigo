using Application.Abstractions.Data;
using Application.Marker;
using Domain.Entities.Calender;
using Domain.Entities.Leaves;
using Domain.Entities.User;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstraction;
using SharedKernel.Concrete;

namespace Persistence.Context;

public sealed class PermigoDbContext(DbContextOptions<PermigoDbContext> options, IPublisher publisher)
    : IdentityDbContext<AppUser,AppRole,Guid>(options), IApplicationDbContext
{
    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<AppRole> AppRoles { get; set; }

    public DbSet<WorkingDay> WorkingDays { get; set; }

    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    public DbSet<LeaveType> LeaveTypes { get; set; }

    public DbSet<LeaveEntitlement> LeaveEntitlements { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(IApplicationMarker).Assembly);

        builder.HasDefaultSchema(Schemas.Default);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}
