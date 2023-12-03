using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SocialMediaPost : Post
    {
        public bool IsAnonymous { get; set; }

        public int LikeCount { get; set; }
    }
}
