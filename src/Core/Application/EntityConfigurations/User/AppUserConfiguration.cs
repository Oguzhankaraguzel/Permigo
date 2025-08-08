using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.EntityConfigurations.User;
internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.UserName)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(u => u.LastName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(u => u.Gender)            
               .HasConversion<int>()               
               .IsRequired();

    }
}
