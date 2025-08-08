using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.EntityConfigurations.User;

internal class IdentityUserClaimConfiguration :
    IEntityTypeConfiguration<IdentityUserClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<Guid>> builder)
    {
        builder.ToTable("UserClaims");           
        builder.HasKey(c => c.Id);               
        builder.Property(c => c.ClaimType).HasMaxLength(256);
        builder.Property(c => c.ClaimValue).HasMaxLength(1024);
    }
}
internal class IdentityRoleClaimConfiguration :
    IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
    {
        builder.ToTable("RoleClaims");
        builder.HasKey(rc => rc.Id);
        builder.Property(rc => rc.ClaimType).HasMaxLength(256);
        builder.Property(rc => rc.ClaimValue).HasMaxLength(1024);
    }
}
internal class IdentityUserLoginConfiguration :
    IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
    {
        builder.ToTable("UserLogins");
        builder.HasKey(l => new { l.LoginProvider, l.ProviderKey }); 
        builder.Property(l => l.LoginProvider).HasMaxLength(128);
        builder.Property(l => l.ProviderKey).HasMaxLength(128);
    }
}
internal class IdentityUserRoleConfiguration :
    IEntityTypeConfiguration<IdentityUserRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<Guid>> builder)
    {
        builder.ToTable("UserRoles");
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        builder.HasIndex(ur => ur.RoleId); 
    }
}
internal class IdentityUserTokenConfiguration :
    IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
    {
        builder.ToTable("UserTokens");
        builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
        builder.Property(t => t.LoginProvider).HasMaxLength(128);
        builder.Property(t => t.Name).HasMaxLength(128);
    }
}
