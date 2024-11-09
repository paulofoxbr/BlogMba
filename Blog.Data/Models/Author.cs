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
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [StringLength(1000)]
        [Display(Name = "Biografia")]
        [Required(ErrorMessage = "O campo Biografia do autor é obrigatório.")]
        public string Bio { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public ICollection<Post> Posts { get; set; } 

    }
}
