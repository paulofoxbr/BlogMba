using Blog.Data.Data;
using Blog.Data.Dto;
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

        public async Task<int> GetRowsPostAuthorAsync()
        {
            return await _context.Posts.CountAsync();
        }
        public async Task<List<PostAuthorDto>> GetPostAuthorAsync()
        {
            return await _context.Posts
                .Include(navigationPropertyPath: p => p.Author)
                .Select(p => new PostAuthorDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Created = p.Created,
                    Updated = p.Updated,
                    AuthorName = p.Author.Name,
                    AuthorEmail = p.Author.Email,
                    AuthorBio = p.Author.Bio
                })
                .ToListAsync();
        }
        public async Task<ResponsePostAutor> GetPostAuthorAsync(int pageNumber=1,int PageSize=20)
        {
            var rowCounts = await _context.Posts.CountAsync();
            var posts = await _context.Posts
                .Include(navigationPropertyPath: p => p.Author)
                .OrderByDescending(p => p.Created)
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(p => new PostAuthorDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Created = p.Created,
                    Updated = p.Updated,
                    AuthorName = p.Author.Name,
                    AuthorEmail = p.Author.Email,
                    AuthorBio = p.Author.Bio,
                    UserId = p.Author.UserId
                })
                .OrderByDescending(p => p.Created)
                .ToListAsync();

            var response = new ResponsePostAutor(posts,rowCounts,PageSize,pageNumber );
            return response;
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
           return await _context.Posts.FindAsync(id);
        }

        public async Task<PostAuthorDto> GetPostAuthorByIdAsync(int id)
        {
            var postAuthor = await _context.Posts
                .Include(navigationPropertyPath: p => p.Author)
                .Where(p => p.Id == id)
                .Select(p => new PostAuthorDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Created = p.Created,
                    Updated = p.Updated,
                    AuthorName = p.Author.Name,
                    AuthorEmail = p.Author.Email,
                    AuthorBio = p.Author.Bio,
                    UserId = p.Author.UserId
                })
                .FirstOrDefaultAsync();

            return postAuthor;
        }

        public async Task UpdatePostAsync(Post post)
        {
            if (post == null) { throw new ArgumentNullException(nameof(post)); }

            _context.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(PostAuthorDto postAuthor)
        {
            if (postAuthor == null) { throw new ArgumentNullException(nameof(postAuthor)); }

            var postToUpdate = await _context.Posts.FindAsync(postAuthor.Id);
            if (postToUpdate == null) { throw new ArgumentNullException(nameof(postToUpdate)); }

            postToUpdate.Title = postAuthor.Title;
            postToUpdate.Content = postAuthor.Content;
            postToUpdate.Updated = DateTime.Now;

            await UpdatePostAsync(postToUpdate);
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) { throw new ArgumentNullException(nameof(post)); }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PostAuthorDto>> GetPostsWithAuthorsAsync()
        {
            return await _context.Posts
                .Include(navigationPropertyPath: p => p.Author)
                .Select(p => new PostAuthorDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Created = p.Created,
                    Updated = p.Updated,
                    AuthorName = p.Author.Name,
                    AuthorEmail = p.Author.Email,
                    AuthorBio = p.Author.Bio,
                    UserId = p.Author.UserId

                })
                .ToListAsync();
        }
    }
}
