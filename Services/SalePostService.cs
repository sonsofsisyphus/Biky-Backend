using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Services.DTO;

namespace Services
{
    public class SalePostService
    {
        private List<SalePost> dbSalePosts;
        public SalePost? GetPostByPostID(Guid postID)
        {
            SalePost? post = dbSalePosts.FirstOrDefault(post => post.PostID == postID);

            return post;
        }

        public List<SalePost> GetPostByUserID(Guid userID)
        {
            var postList = dbSalePosts.Where(post => post.AuthorID == userID).ToList();

            return postList;
        }

        //add user validation for post ids
        public Guid AddPost(SalePostAddRequest salePostRequest)
        {
            SalePost post = salePostRequest.ToSalePost();
            dbSalePosts.Add(post);
            return post.PostID;
        }

        public void UpdatePost(SalePost salePost)
        {
            var index = dbSalePosts.FindIndex(a => a.PostID == salePost.PostID);
            if (index != -1)
            {
                dbSalePosts[index] = salePost;
            }
        }

        public void RemovePost(Guid postID)
        {
            var index = dbSalePosts.FindIndex(a => a.PostID == postID);
            if (index != -1)
            {
                dbSalePosts.RemoveAt(index);
            }
        }


    }
}
