using System.Net.Http.Json;
using System.Web;
using HybridDMS.Components.Catalog;
using HybridDMS.Components.Helpers;

namespace HybridDMS.Components.Services;

public class CatalogService(HttpClient httpClient)
{
    private readonly string remoteServiceBaseUrl = "api/v1/catalog/";
    private const string ApiUrlBase = "api/v1/Catalog";

    public Task<CatalogProduct?> GetCatalogProduct(int id)
    {
        var uri2 = UriHelper.CombineUri(GlobalSetting.Instance.GatewayShoppingEndpoint, $"{ApiUrlBase}/products");


        var uri = $"{remoteServiceBaseUrl}products/{id}";
        return httpClient.GetFromJsonAsync<CatalogProduct>(uri);
    }

    public async Task<CatalogResult> GetCatalogProducts(int pageIndex, int pageSize, int? brand, int? type)
    {
        var uri2 = UriHelper.CombineUri(GlobalSetting.Instance.GatewayShoppingEndpoint, $"{ApiUrlBase}/products");

        var uri = GetAllCatalogProductsUri(remoteServiceBaseUrl, pageIndex, pageSize, brand, type);
        var result = await httpClient.GetFromJsonAsync<CatalogResult>(uri);
        return result!;
    }

    public async Task<List<CatalogProduct>> GetCatalogProducts(IEnumerable<int> ids)
    {
        var uri = $"{remoteServiceBaseUrl}products/by?ids={string.Join("&ids=", ids)}";
        var result = await httpClient.GetFromJsonAsync<List<CatalogProduct>>(uri);
        return result!;
    }

    public Task<CatalogResult> GetCatalogProductsWithSemanticRelevance(int page, int take, string text)
    {
        var url = $"{remoteServiceBaseUrl}products/withsemanticrelevance/{HttpUtility.UrlEncode(text)}?pageIndex={page}&pageSize={take}";
        var result = httpClient.GetFromJsonAsync<CatalogResult>(url);
        return result!;
    }

    public async Task<IEnumerable<CatalogBrand>> GetBrands()
    {
        var uri = $"{remoteServiceBaseUrl}catalogBrands";
        var result = await httpClient.GetFromJsonAsync<CatalogBrand[]>(uri);
        return result!;
    }

    public async Task<IEnumerable<CatalogProductType>> GetTypes()
    {
        var uri = $"{remoteServiceBaseUrl}catalogTypes";
        var result = await httpClient.GetFromJsonAsync<CatalogProductType[]>(uri);
        return result!;
    }

    private static string GetAllCatalogProductsUri(string baseUri, int pageIndex, int pageSize, int? brand, int? type)
    {
        string filterQs;

        if (type.HasValue)
        {
            var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
            filterQs = $"/type/{type.Value}/brand/{brandQs}";

        }
        else if (brand.HasValue)
        {
            var brandQs = brand.HasValue ? brand.Value.ToString() : string.Empty;
            filterQs = $"/type/all/brand/{brandQs}";
        }
        else
        {
            filterQs = string.Empty;
        }

        return $"{baseUri}products{filterQs}?pageIndex={pageIndex}&pageSize={pageSize}";
    }
}
