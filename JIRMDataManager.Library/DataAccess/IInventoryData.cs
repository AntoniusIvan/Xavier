using JIRMDataManager.Library.Models;

namespace JIRMDataManager.Library.DataAccess
{
    public interface IInventoryData
    {
        Task<List<InventoryModel>> GetInventory();
        Task SaveInventoryRecord(InventoryModel item);
    }
}