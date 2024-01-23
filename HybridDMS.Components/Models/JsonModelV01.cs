namespace HybridDMS.Components.Models
{
    public class JsonModelV01 
    { 
        public bool Success { get; set; } 
        public string Status { get; set; } 
        public string Msg { get; set; }
        public string ErrorCode { get; set; }
        public string Error { get; set; }
        //public object Content { get; set; }
        //public Dictionary<string, object> Content { get; set; } (For Free Flow Key-Value Pair Data)
    }
}
