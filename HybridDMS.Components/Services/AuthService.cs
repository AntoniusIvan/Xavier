

using HybridDMS.Components.Models.AccountManagement;

public class AuthService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public class AccessTokenContent
    {
        public string AccessToken { get; set; }
        public string UserCode { get; set; }
    }
    public AuthService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    //public async Task<JsonAccessTokenContent> AuthenticateAsync(DtoFrontRegistration registrationModel)
    //{
    //    using (HttpClient client = _httpClientFactory.CreateClient())
    //    {
    //        string apiUrl = "shttps://ijcowa.azurewebsites.net/token";

    //        // Create an object with the required properties from DtoFrontRegistration
    //        var requestData = new
    //        {
    //            usr_cd = "string",
    //            full_nm = "string",
    //            disp_nm = "string",
    //            login = registrationModel.login,
    //            pwd = registrationModel.pwd,
    //            eml_addr = "string",
    //            empl_id = "string",
    //            pos_cd = "string",
    //            auth_cd = 0,
    //            used_auth_ver = 0,
    //            notes = "string",
    //            cre_by = "string"
    //        };

    //        // Make a POST request
    //        HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, requestData);

    //        if (response.IsSuccessStatusCode)
    //        {
    //            JsonAccessTokenContent result = await response.Content.ReadFromJsonAsync<JsonAccessTokenContent>();
    //            return result;
    //        }
    //        else
    //        {
    //            // Handle error case
    //            // You might want to throw an exception or return a specific error object here
    //            return null;
    //        }
    //    }
    //}
}
