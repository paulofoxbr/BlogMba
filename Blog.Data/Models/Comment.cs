using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Models
{
    public class Comment
    {
        [Key]
            public int Id { get; set; }
            public  string PostComment { get; set; }
            public DateTime Created { get; set; } = DateTime.Now;
            public int PostId { get; set; }
            public int AuthorId { get; set; }
    }
}
