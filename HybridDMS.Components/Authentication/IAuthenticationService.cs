using HybridDMS.Components.Models;

namespace HybridDMS.Components.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication);
        Task LogOut();
    }
}