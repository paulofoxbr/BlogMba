using Blog.Data.Data;
using Blog.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Services
{
    public class AuthorService
    {
        private readonly AppDbContext _context;
        private readonly SqLiteService _sqLiteService;

        public AuthorService(AppDbContext appContext)
        {
            _context = appContext;
            _sqLiteService = new SqLiteService(_context);
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await Task.FromResult(_context.Authors.ToList());
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await Task.FromResult(_context.Authors.FirstOrDefault(a => a.Id == id));
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            if (_sqLiteService.IsSqLite) { author.Id = await _sqLiteService.GenerateIdAsync<Author>(); }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(int id)
        {
            var author = await GetAuthorByIdAsync(id);
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AuthorExistsAsync(int id)
        {
            return await Task.FromResult(_context.Authors.Any(a => a.Id == id));
        }

        public async Task<Author?> GetAuthorByUserId(string userId)
        {
            if (userId == null)  {throw new ArgumentNullException(nameof(userId));}

            var author = _context.Authors.FirstOrDefault(a => a.UserId == userId);

            return await Task.FromResult(author);
        }

        public async Task<Author> GetAuthorByUserEmail(string email)
        {
            var author = await _context.Authors.Where(a => a.Email == email).FirstOrDefaultAsync();
            return author ?? new Author();
        }
    }
}
