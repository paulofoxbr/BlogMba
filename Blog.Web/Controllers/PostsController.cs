using Blog.Data.Data;
using Blog.Data.Dto;
using Blog.Data.Models;
using Blog.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Web.Controllers
{
    // [Route("posts")]
    //[Authorize(Roles = "ADMIN")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly PostService _postService;
        private readonly AuthorService _authorService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        // GET: Posts
        public PostsController(PostService postService,AuthorService authorService ,SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _postService = postService;
            _signInManager = signInManager;
            _userManager = userManager;
            _authorService = authorService;

        }

        [AllowAnonymous]
        public async Task<ActionResult> Index(int pagina=1)
        {
            var result = await _postService.GetPostAuthorAsync(pagina,10 );
            return View(result);
        }

        //// GET: Posts/detalhes/5
        [AllowAnonymous]
        [HttpGet("Details/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            return View(await _postService.GetPostAuthorByIdAsync(id));
        }

        //// GET: Posts/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Title,Content")] Post post )
        {
            ModelState.Remove("AuthorId");
            ModelState.Remove("Author");
            if (!ModelState.IsValid)  
            { 
                return View(post); 
            }
            try
            {
                //post.Content = SanitizeHtml(post.Content);
                var author = await _authorService.GetAuthorByUserId( _userManager.GetUserId(User));
                post.AuthorId = author.Id;
                post.Author = author;
                if (author == null) 
                {
                    ModelState.AddModelError("Email", "Usuário não encontrado como um autor.");
                    return View(post);
                }
                await _postService.CreatePostAsync(post);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(post);
            }
        }

        // GET: PostController1/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<ActionResult> Edit(int id)
        {
            if (User.Identity.IsAuthenticated == false) { return RedirectToAction("Login", "Account"); }
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin= User.IsInRole("ADMIN");
            var isAdmin2= await _userManager.IsInRoleAsync(await _userManager.FindByIdAsync(userid), "ADMIN");

            var postAuthor = await _postService.GetPostAuthorByIdAsync(id);
            if (isAdmin || userid==postAuthor.UserId) 
                return View(postAuthor);
            else
            {
                return RedirectToAction(nameof(Index));
              //  return RedirectToAction("AccessDenied", "Account");
            }
            
        }

        // POST: PostController1/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Title,Content,Created,Updated,AuthorId")] PostAuthorDto post)
        {
            if (id != post.Id) { return NotFound(); }
            if (!ModelState.IsValid) { return View(post); }

            try
            {
                await _postService.UpdatePostAsync(post);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(post);
            }
        }

        // GET: PostController1/Delete/5
        [HttpGet("Delete/{id:int}")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostController1/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _postService.DeletePostAsync(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
