using System.ComponentModel.DataAnnotations;

namespace MasterService.Dtos.Master.Block
{
    public class BlockCreateDto
    {
        [Required]
        [StringLength(256)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
