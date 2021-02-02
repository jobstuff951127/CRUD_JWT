using Model;

namespace Controller.Security
{
    public interface IJwtGenerator
    {
         public string CreateToken(AppUser user);
    }
}