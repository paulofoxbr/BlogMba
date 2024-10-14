using Blog.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Services
{
    public class CommentService
    {
        private readonly AppDbContext _context;
        public CommentService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
    }
}
