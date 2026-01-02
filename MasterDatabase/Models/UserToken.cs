using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MasterDatabase.Models
{
    public class UserToken
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public DateTime ExpiredAt { get; set; }
    }
}
