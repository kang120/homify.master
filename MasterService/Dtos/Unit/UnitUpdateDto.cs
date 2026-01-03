using System.ComponentModel.DataAnnotations;

namespace MasterService.Dtos.Master.Unit
{
    public class UnitUpdateDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public long? BlockId { get; set; }

        [StringLength(256)]
        public string? AddressLine1 { get; set; }

        [StringLength(256)]
        public string? AddressLine2 { get; set; }

        [StringLength(256)]
        public string? AddressLine3 { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
