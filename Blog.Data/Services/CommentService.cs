using Blog.Data.Data;
using Blog.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Services;

public class CommentService
{
    private readonly AppDbContext _context;
    private readonly SqLiteService _sqLiteService;
    public CommentService(AppDbContext appDbContext)
    {
        _context = appDbContext;
        _sqLiteService = new SqLiteService(_context);
    }

    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        if (_sqLiteService.IsSqLite) { comment.Id = await _sqLiteService.GenerateIdAsync<Comment>(); }

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }
}
