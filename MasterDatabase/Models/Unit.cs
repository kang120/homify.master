using MasterLib.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MasterDatabase.Models
{
    public class Unit : BaseModel
    {
        [Required]
        [StringLength(256)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public long? BlockId { get; set; }
        public Block Block { get; set; }

        [StringLength(256)]
        public string? AddressLine1 { get; set; }

        [StringLength(256)]
        public string? AddressLine2 { get; set; }

        [StringLength(256)]
        public string? AddressLine3 { get; set; }
    }
}
