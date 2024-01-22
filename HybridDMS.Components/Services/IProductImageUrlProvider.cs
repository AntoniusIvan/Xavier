using HybridDMS.Components.Catalog;

namespace HybridDMS.Components.Services;

public interface IProductImageUrlProvider
{
    string GetProductImageUrl(CatalogProduct product)
        => GetProductImageUrl(product.Id);

    string GetProductImageUrl(int productId);
}
