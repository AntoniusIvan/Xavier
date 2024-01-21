using System.Threading.Tasks;
using AIRMDesktopUI.Library.Models;

namespace AIRMDesktopUI.Library.Api
{
    public interface ISaleEndpoint
    {
        Task PostSale(SaleModel sale);
    }
}