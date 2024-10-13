using Blog.Data.Data;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Services
{
    public class PostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            if (context is null)    { throw new ArgumentNullException(nameof(context));}

            _context = context;
        }

        public async Task CreatePostAsync(Post post)
        {
            if ( post == null) {throw new ArgumentNullException(nameof(post.AuthorId));}

            _context.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task UpdatePostAsync(Post post)
        {
            if (post == null) { throw new ArgumentNullException(nameof(post)); }

            _context.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) { throw new ArgumentNullException(nameof(post)); }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
