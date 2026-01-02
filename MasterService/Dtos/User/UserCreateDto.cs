using System.ComponentModel.DataAnnotations;

namespace MasterService.Dtos.Master.User
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(256)]
        public string Username { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string? PhoneNo { get; set; }
    }
}
