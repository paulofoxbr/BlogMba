using Blog.Data.Dto;
using Blog.Data.Models;
using Blog.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Api.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<ResponsePostAutor>> GetPostAuthor(int pageNumber,int pageSize)
        {
            var posts = await _postService.GetPostAuthorAsync(pageNumber,pageSize);
            return posts;
        }

        [HttpGet("post-por-id/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            return post;

        }

        // o usuario que fez o post também pode deletar um post. como implementar isso?
        [Authorize(Policy = "AuthorOrAdmin")]
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-post/{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            await _postService.DeletePostAsync(id);
            return Ok();
        }

    }
}
