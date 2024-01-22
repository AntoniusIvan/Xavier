namespace HybridDMS.Components.Catalog;

public record CatalogProduct(
    int Id,
    string Name,
    string Description,
    decimal Price,
    string PictureUri,
    int CatalogBrandId,
    CatalogBrand CatalogBrand,
    int CatalogTypeId,
    CatalogProductType CatalogType);

public record CatalogResult(int PageIndex, int PageSize, int Count, List<CatalogProduct> Data);
public record CatalogBrand(int Id, string Brand);
public record CatalogProductType(int Id, string Type);
