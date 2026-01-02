using System.ComponentModel.DataAnnotations;

namespace MasterService.Dtos.Master.User
{
    public class UserUpdateDto
    {
        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string? PhoneNo { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
