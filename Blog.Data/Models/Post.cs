using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [MinLength(5,ErrorMessage ="O tamanho mínimo do Título é de 5 caracteres")]
        [MaxLength(100,ErrorMessage ="O tamanho máximo do Título é de 100 caracteres")]
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        public int AuthorId { get; set; }

    }
}
