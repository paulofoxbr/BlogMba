﻿using Blog.Data.Dto;
using Blog.Data.Models;
using Blog.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly AuthorService _authorService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public PostsController(PostService postService,AuthorService authorService,SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _postService = postService;
            _authorService = authorService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet("post-autor/{pageNumber:int}/{pageSize:int}")]
        public async Task<ActionResult<ResponsePostAutor>> GetPostAuthor(int pageNumber,int pageSize)
        {
            var posts = await _postService.GetPostAuthorAsync(pageNumber,pageSize);
            return posts;
        }

        [AllowAnonymous]
        [HttpGet("post-por-id/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            return post;

        }

        // o usuario que fez o post também pode deletar um post. como implementar isso?
        [Authorize(Policy = "PostAuthorOrAdminPolicy")]
       // [Authorize(Roles = "Admin")]
        [HttpDelete("delete-post/{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            await _postService.DeletePostAsync(id);
            return Ok();
        }
        [Authorize(Policy = "PostAuthorOrAdminPolicy")]
        [HttpPost("create-post")]
        public async Task<ActionResult> CreatePost(Post post)
        {
            if (post == null)  { return BadRequest(); }

            //todo: verificar se o usuário é um autor
            //            var userId = _userManager.GetUserId(User); // User não tem o ClaimsPrincipal
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            var userId = user.Id;

            var author = await _authorService.GetAuthorByUserId(userId);
            if (author == null) { return Problem("Falha na identificação do usuário/autor."); }

            post.AuthorId = author.Id;
            post.Author = author;
            post.Created = DateTime.Now;

            await _postService.CreatePostAsync(post);
            return Ok();
        }
        [Authorize(Policy = "PostAuthorOrAdminPolicy")]
        [HttpPut("update-post/{id}")]
        public async Task<ActionResult> UpdatePost(int id,Post post)
        {
            await _postService.UpdatePostAsync(post);
            return Ok();
        }

    }
}
