using JIRMDataManager.Library.DataAccess.SystemCoreDataAccess;
using JIRMDataManager.Library.Models;

namespace JIRMDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _sql;

        public UserData(ISqlDataAccess sql)
        {
            _sql = sql;
        }
        public async Task<List<UserModel>> GetUserById(string Id)
        {

            var output = await _sql.LoadDataStoredProcedureAsync<UserModel, dynamic>("dbo.spUserLookup", new { Id }, "AIRMData23050409BeforeCore");

            return output;
        }
    }
}
