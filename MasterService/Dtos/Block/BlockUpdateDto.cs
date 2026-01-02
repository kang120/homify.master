using System.ComponentModel.DataAnnotations;

namespace MasterService.Dtos.Master.Block
{
    public class BlockUpdateDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
