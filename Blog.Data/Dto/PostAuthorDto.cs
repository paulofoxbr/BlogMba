using System.ComponentModel;

namespace Blog.Data.Dto
{
    public class PostAuthorDto
    {

        public int Id { get; set; }
        [DisplayName("Título")]
        public string Title { get; set; } = string.Empty;
        [DisplayName("Conteudo do post")]
        public string Content { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
        [DisplayName("Nome do Autor")]
        public string AuthorName { get; set; } = string.Empty;
        [DisplayName("Email do Autor")]
        public string AuthorEmail { get; set; } = string.Empty;
        [DisplayName("Biografia do Autor")]
        public string AuthorBio { get; set; } = string.Empty;
        [DisplayName("Id do usuário")]
        public string UserId { get; set; } = string.Empty;

    }

}