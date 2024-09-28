using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="O campo [0] é obrigatório.")]
        [MaxLength( 100,ErrorMessage = "O campo [0] é limitado a 100 caracteres.")]
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
       

    }
}
