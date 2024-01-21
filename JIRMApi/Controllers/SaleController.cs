using JIRMDataManager.Library.DataAccess;
using JIRMDataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JIRMApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class SaleController : ControllerBase
  {
    private readonly ISaleData _saleData;

    public SaleController(ISaleData saleData)
    {
      _saleData = saleData;
    }
    [Authorize(Roles = "Cashier")]
    [HttpPost]
    public void Post(SaleModel sale)
    {
      //SaleData data = new SaleData(_config);
      //To Know specifically who is logged in.
      string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//old way - RequestContext.Principal.Identity.GetUserId();

      _saleData.SaveSale(sale, userId);
    }

    [Authorize(Roles = "Admin,Manager")]
    [Route("GetSalesReport")]
    [HttpGet]
    public async Task<List<SaleReportModel>> GetSalesReport()
    {
      //SaleData data = new SaleData(_config);
      return await _saleData.GetSaleReport();
    }
  }
}
