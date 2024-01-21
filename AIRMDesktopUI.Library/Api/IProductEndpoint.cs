using AIRMDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIRMDesktopUI.Library.Api
{
  public interface IProductEndpoint
  {
    Task<List<ProductModel>> GetAll();
  }
}