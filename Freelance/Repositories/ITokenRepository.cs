using Microsoft.AspNetCore.Identity;

namespace Freelance.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
