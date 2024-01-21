using JIRMDataManager.Library.Models;

namespace JIRMDataManager.Library.DataAccess
{
    public interface IProductData
    {
        Task<ProductModel> GetProductById(int productId);
        Task<List<ProductModel>> GetProducts();
    }
}