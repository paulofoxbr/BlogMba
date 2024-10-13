using Blog.Data.Models;
using Microsoft.AspNetCore.Mvc;
namespace Blog.Web.ViewComponets
{
    public class ExibePostViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var post = new Post { AuthorId = 1, Title = "Post 1", Content = "Conteúdo do meu post" };
            return View(post);
        }
    }
}
