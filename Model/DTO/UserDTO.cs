using System.ComponentModel.DataAnnotations;

namespace Model.DTO
{
    public class UserDTO
    {
        public string Token { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}