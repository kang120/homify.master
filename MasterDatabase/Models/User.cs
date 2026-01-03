using MasterLib.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MasterDatabase.Models
{
    public class User : BaseModel
    {
        [Required]
        public Guid UserId { get; set; }

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

        [JsonIgnore]
        public string? Password { get; set; }

        [JsonIgnore]
        public List<UserToken> UserTokens { get; set; }
    }
}
