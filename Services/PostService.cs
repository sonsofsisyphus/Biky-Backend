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
        //all of this might be useless code keeping it just in case
        private List<Post> dbPosts;
        private readonly SalePostService _salePostService;
        private readonly SocialMediaPostService _socialMediaPostService;

        public PostService(SalePostService salePostService, SocialMediaPostService socialMediaPostService)
        { 
            _salePostService = salePostService;
            _socialMediaPostService = socialMediaPostService;
        }



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
