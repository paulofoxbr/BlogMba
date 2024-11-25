using Blog.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Services;

public class SqLiteService
{
    private readonly AppDbContext _context;
    public bool IsSqLite { get; private set; } = false;

    public SqLiteService(AppDbContext context)
    {
        if (context is null) { throw new ArgumentNullException(nameof(context)); }

        _context = context;
        IsSqLite = _context.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite";
    }



    public async Task<int> GenerateIdAsync<T>() where T : class
    {
        if (IsSqLite)
        {
            var maxId = await _context.Set<T>().MaxAsync(x => EF.Property<int>(x, "Id"));
            return maxId + 1;
        }

        throw new NotSupportedException("ID generation is only supported for SQLite.");
    }
}
