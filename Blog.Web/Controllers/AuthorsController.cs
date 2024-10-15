using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data.Data;
using Blog.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Blog.Data.Services;
using Microsoft.AspNetCore.Identity;


namespace Blog.Web.Controllers
{
    [Authorize]
    public class AuthorsController : Controller
    {
        private readonly AuthorService _authorService;
        private readonly SignInManager<IdentityUser>  _signInManager;
        private readonly UserManager<IdentityUser>   _userManager;

        public AuthorsController(AuthorService authorService,SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _authorService = authorService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            return View(await _authorService.GetAuthorsAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _authorService.GetAuthorByIdAsync(id.Value);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Bio,UserId")] Author author)
        {
            if (ModelState.IsValid)
            {
                var userByEmail = await _userManager.FindByEmailAsync(author.Email);
                if (userByEmail == null)
                {
                   ModelState.AddModelError("Email", "e-mail Usuário não encontrado. Um autor deve ser um usuário cadastrado.");
                    return View(author);
                }
                var authorByEmail = await _authorService.GetAuthorByUserEmail(author.Email);
                if (authorByEmail != null)
                {
                    ModelState.AddModelError("Email", $"O e-mail Usuário já está associado ao autor #{authorByEmail.Id}-{ authorByEmail.Name}.");
                    return View(author);
                }

                author.UserId = userByEmail.Id;
                await _authorService.CreateAuthorAsync(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _authorService.GetAuthorByIdAsync(id.Value);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Bio,UserId")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _authorService.UpdateAuthorAsync(author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! _authorService.AuthorExistsAsync(author.Id).Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _authorService.DeleteAuthorAsync(id); 
            return RedirectToAction(nameof(Index));
        }

    }
}
