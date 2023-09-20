using System.ComponentModel.DataAnnotations;

namespace ProjectChronos.Models.Requests
{
    public class LoginRequestModel
    {
        [Required]
        public string Address { get; set; }

        [Required]
        public string Signature { get; set; }

        public bool RememberMe { get; set; }
    }
}
