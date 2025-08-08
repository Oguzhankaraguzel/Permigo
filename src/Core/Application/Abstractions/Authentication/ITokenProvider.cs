using Domain.Entities.User;

namespace Application.Abstractions.Authentications;

public interface ITokenProvider
{
    string GenerateToken(AppUser user, IList<string>? roles, DateTime? expiration = null);
}
