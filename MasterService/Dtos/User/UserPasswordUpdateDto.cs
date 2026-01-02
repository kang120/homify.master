using System.ComponentModel.DataAnnotations;

namespace MasterService.Dtos.Master.User
{
    public class UserPasswordUpdateDto
    {
        [StringLength(32)]
        public string Password { get; set; }
    }
}
