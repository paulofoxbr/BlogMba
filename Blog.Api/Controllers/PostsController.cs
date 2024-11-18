using Blog.Data.Dto;
using Blog.Data.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        public PostsController(PostService postService)
        {
            _postService = postService;
        }
        [HttpGet("post-autor/{pageNumber:int}/{pageSize:int}")]
        //[HttpGet("post-autor")]

        public async Task<ActionResult<ResponsePostAutor>> GetPostAuthor(int pageNumber,int pageSize)
        {
            var posts = await _postService.GetPostAuthorAsync(pageNumber,pageSize);
            return posts;
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public string GetPost(int id)
        {
            return "value";
        }

    }
}
