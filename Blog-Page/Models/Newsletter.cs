using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogNET.Models
{
    public partial class Newsletter
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
