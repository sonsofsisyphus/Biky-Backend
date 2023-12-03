using Entities;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SocialMediaPostService
    {
        private List<SocialMediaPost> dbSocialMediaPosts;

        public SocialMediaPost? GetPostByPostID(Guid postID)
        {
            SocialMediaPost? post = dbSocialMediaPosts.FirstOrDefault(post => post.PostID == postID);

            return post;
        }

        public List<SocialMediaPost> GetPostByUserID(Guid userID)
        {
            var postList = dbSocialMediaPosts.Where(post => post.AuthorID == userID).ToList();

            return postList;
        }

        //add user validation for post ids
        public Guid AddPost(SocialMediaPostAddRequest socialMediaPostRequest)
        {
            var post = socialMediaPostRequest.ToSocialMediaPost();
            dbSocialMediaPosts.Add(post);
            return post.PostID;
        }

        public void UpdatePost(SocialMediaPost salePost)
        {
            var index = dbSocialMediaPosts.FindIndex(a => a.PostID == salePost.PostID);
            if (index != -1)
            {
                //add checks here for protection against tampering
                //i.e. post time & like count
                dbSocialMediaPosts[index] = salePost;
            }
        }

        public void RemovePost(Guid postID)
        {
            var index = dbSocialMediaPosts.FindIndex(a => a.PostID == postID);
            if (index != -1)
            {
                dbSocialMediaPosts.RemoveAt(index);
            }
        }

        public bool ValidateID(Guid postID)
        {
            return dbSocialMediaPosts.FindIndex(a => a.PostID == postID) != -1;
        }

        public void AddLikeToPost(Like like)
        {
            var index = dbSocialMediaPosts.FindIndex(a => a.PostID == like.PostID);
            dbSocialMediaPosts[index].LikeCount++;
        }

        public void RemoveLikeFromPost(Like like)
        {
            var index = dbSocialMediaPosts.FindIndex(a => a.PostID == like.PostID);
            dbSocialMediaPosts[index].LikeCount--;
        }
    }
}
