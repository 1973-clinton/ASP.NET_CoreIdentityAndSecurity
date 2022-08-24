using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{
    public class Credential
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
