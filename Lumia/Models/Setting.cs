using System.ComponentModel.DataAnnotations;

namespace Lumia.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [StringLength(maximumLength:50)]
        public string? Key { get; set; }
        [StringLength(maximumLength:200)]
        public string Value { get; set; }
    }
}
