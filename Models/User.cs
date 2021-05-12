using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStoreV2.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Address { get; set; }

        public string Image { get; set; }

    }
}
