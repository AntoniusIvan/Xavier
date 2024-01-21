using HybridDMS.Components.Catalog;

namespace HybridDMS.Components.Item;

public static class ItemHelper
{
    public static string Url(CatalogItem item)
        => $"item/{item.Id}";
}
