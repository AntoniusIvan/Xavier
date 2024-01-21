using JIRMDataManager.Library.Models;

namespace JIRMDataManager.Library.DataAccess
{
    public interface ISaleData
    {
        Task<List<SaleReportModel>> GetSaleReport();
        Task SaveSale(SaleModel saleInfo, string cashierId);
    }
}