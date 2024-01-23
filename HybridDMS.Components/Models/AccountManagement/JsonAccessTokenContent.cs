namespace HybridDMS.Components.Models.AccountManagement
{
    public class JsonAccessTokenContent : JsonModelV01
    {
        public new AccessTokenContent Content { get; set; }
    }

    public class AccessTokenContent
    {
        public string AccessToken { get; set; }
        public string UserCode { get; set; }
    }
}
