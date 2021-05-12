using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreV2.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string Image { get; set; }

    }
}
