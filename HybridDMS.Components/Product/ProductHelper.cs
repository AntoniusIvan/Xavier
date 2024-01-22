using HybridDMS.Components.Catalog;

namespace HybridDMS.Components.Product;

public static class ProductHelper
{
    public static string Url(CatalogProduct item)
        => $"item/{item.Id}";
}
