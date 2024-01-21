using JIRMDataManager.Library.DataAccess;
using JIRMDataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace JIRMApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class InventoryController : ControllerBase
  {
    private readonly IInventoryData _inventoryData;

    public InventoryController(IInventoryData inventoryData)
    {
      _inventoryData = inventoryData;
    }
    [Authorize(Roles = "Admin,Manager")] // OR
    [HttpGet]
    public async Task<List<InventoryModel>> Get()
    {
      return await _inventoryData.GetInventory();

      //InventoryData data = new InventoryData(_config);
      //return data.GetInventory();
    }

    //(OLD)//[Authorize(Roles = "WarehouseWorker")] AND
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public void Post(InventoryModel item)
    {
      _inventoryData.SaveInventoryRecord(item);

      //InventoryData data = new InventoryData(_config);
      //data.SaveInventoryRecord(item);
    }
  }
}
