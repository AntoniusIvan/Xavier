using System.ComponentModel.DataAnnotations;

namespace HybridDMS.Components.Models.AccountManagement
{
    public class DtoFrontRegistration
    {
        public string usr_cd { get; set; }
        public string full_nm { get; set; }
        public string disp_nm { get; set; }
        public string login { get; set; }
        public string pwd { get; set; }
        public string eml_addr { get; set; }
        public string empl_id { get; set; }
        public string pos_cd { get; set; }
        public byte auth_cd { get; set; }
        public byte used_auth_ver { get; set; }
        public string notes { get; set; }
        public string cre_by { get; set; }
        // ... Add the rest of the fields you want to allow registration for
    }

}
