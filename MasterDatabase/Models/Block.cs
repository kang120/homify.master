using MasterLib.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MasterDatabase.Models
{
    public class Block : BaseModel
    {
        [Required]
        [StringLength(256)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public List<Unit> Units { get; set; }
    }
}
