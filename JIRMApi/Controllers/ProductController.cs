using JIRMDataManager.Library.DataAccess;
using JIRMDataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JIRMApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize(Roles = "Cashier")]
  public class ProductController : ControllerBase
  {
    private readonly IProductData _productData;

    public ProductController(IProductData productData)
    {
      _productData = productData;
    }
    [HttpGet]
    public async Task<List<ProductModel>> Get()
    {

      //ProductData data = new ProductData(_config);

      return await _productData.GetProducts();
    }
  }
}
