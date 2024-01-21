using HybridCisBCRM.Components.Models;

namespace HybridCisBCRM.Components.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication);
        Task LogOut();
    }
}