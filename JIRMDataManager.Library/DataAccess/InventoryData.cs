
using JIRMDataManager.Library.DataAccess.SystemCoreDataAccess;
using JIRMDataManager.Library.Models;
using Microsoft.Extensions.Configuration;

namespace JIRMDataManager.Library.DataAccess
{
    public class InventoryData : IInventoryData
    {
        private readonly IConfiguration _config;
        private readonly ISqlDataAccess _sql;

        public InventoryData(IConfiguration config, ISqlDataAccess sql)
        {
            _config = config;
            _sql = sql;
        }

        public async Task<List<InventoryModel>> GetInventory()
        {
            //SqlDataAccess sql = new SqlDataAccess(_config);

            var output = await _sql.LoadDataStoredProcedureAsync<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "AIRMData23050409BeforeCore");

            return output;
        }

        public async Task SaveInventoryRecord(InventoryModel item)
        {
            //SqlDataAccess sql = new SqlDataAccess(_config);

            _sql.SaveDataStoredProcedure("dbo.spInventory_Insert", item, "AIRMData23050409BeforeCore");

            //return true;
        }
    }
}
