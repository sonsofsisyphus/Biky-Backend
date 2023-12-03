using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Comment
    {
        public Guid CommentID { get; set; }

        public Guid AuthorID { get; set; }

        public string Content { get; set; }

        public DateTime PostTime { get; set; }
    }
}
