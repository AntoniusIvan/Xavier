using JIRMDataManager.Library.Models;

namespace JIRMDataManager.Library.DataAccess
{
    public interface IUserData
    {
        Task<List<UserModel>> GetUserById(string Id);
    }
}