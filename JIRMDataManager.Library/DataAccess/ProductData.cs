using JIRMDataManager.Library.DataAccess.SystemCoreDataAccess;
using JIRMDataManager.Library.Models;

namespace JIRMDataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _sql;

        public ProductData(ISqlDataAccess sql)
        {
            _sql = sql;
        }
        public async Task<List<ProductModel>> GetProducts()
        {
            //SqlDataAccess sql = new SqlDataAccess(_config);

            var output = await _sql.LoadDataStoredProcedureAsync<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "AIRMData23050409BeforeCore");

            return output;
        }

        public async Task<ProductModel> GetProductById(int productId)
        {
            // SqlDataAccess sql = new SqlDataAccess(_config);

            var output = await _sql.LoadDataStoredProcedureAsync<ProductModel, dynamic>("dbo.spProductGetById", new { Id = productId }, "AIRMData23050409BeforeCore");

            // Check if the product was found
            ProductModel product = output.FirstOrDefault();

            // Handle the case where the product is not found
            if (product == null)
            {
                // You might want to throw an exception, return null, or handle it based on your requirements
                // For example:
                // throw new NotFoundException($"Product with ID {productId} not found");
                // or
                // return null;
            }

            return product;
        }

    }
}
