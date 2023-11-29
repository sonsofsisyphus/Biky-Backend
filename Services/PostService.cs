using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace Services
{
    public class PostService
    {
        private List<Post> dbPosts;

        public Post? GetPostByPostID(Guid postID)
        {
            Post? post = dbPosts.FirstOrDefault(post => post.PostID == postID);

            return post;
        }

        public List<Post> GetPostByUserID(Guid userID)
        {
            var postList = dbPosts.Where(a => a.AuthorID == userID).ToList();

            return postList;
        }
    }
}
