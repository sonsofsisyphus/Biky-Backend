using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Like
    {
        public Guid UserID { get; set; }

        public Guid PostID { get; set; }
    }
}
