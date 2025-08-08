using Domain.Entities.User;
using SharedKernel.Concrete;

namespace Domain.Abstractions;

public class BaseEntity : Entity
{
    public BaseEntity()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }

    public Guid CreateUserId { get; set; }
    public Guid? UpdateUserId { get; set; }
    public Guid? DeleteUserId { get; set; }

    #region Navigation Props
    public virtual AppUser CreateUser { get; set; }
    public virtual AppUser? UpdateUser { get; set; }
    public virtual AppUser? DeleteUser { get; set; }
    #endregion
}
