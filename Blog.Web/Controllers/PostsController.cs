using Blog.Data.Models;
using Blog.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    // [Route("posts")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly PostService _postService;

        // GET: Posts
        public PostsController(PostService postService)
        {
            _postService = postService;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View( await _postService.GetPostsAsync());
        }

        //// GET: Posts/detalhes/5
        [AllowAnonymous]
        [HttpGet("Details/{id:int}")]
        public async Task<ActionResult> Details(int id)
        {
            return View( await _postService.GetPostByIdAsync(id));
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
        public async Task<ActionResult> Create([Bind("Id,Title,Content,Created,Updated,AuthorId")] Post post )
        {
            if (!ModelState.IsValid)  { return View(post); }
            try
            {

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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostController1/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id,Title,Content,Created,Updated,AuthorId")] Post post)
        {
            if (id != post.Id) { return NotFound(); }
            if (!ModelState.IsValid) { return View(post); }

            try
            {
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
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
