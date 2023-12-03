using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LikeService
    {
        private List<Like> dbLikes;
        private readonly UserService _userService;
        private readonly SocialMediaPostService _socialMediaPostService;

        public LikeService(UserService userService, SocialMediaPostService socialMediaPostService)
        {
            _userService = userService;
            _socialMediaPostService = socialMediaPostService;
        }

        public void AddLike(Like like)
        {
            if(ValidateLike(like))
            {
                dbLikes.Add(like);
                _socialMediaPostService.AddLikeToPost(like);
            }
        }

        public void RemoveLike(Like like)
        {
            if (_userService.ValidateID(like.UserID) && _socialMediaPostService.ValidateID(like.PostID))
            {
                dbLikes.Remove(like);
                _socialMediaPostService.RemoveLikeFromPost(like);
            }
        }

        public bool ValidateLike(Like like)
        {
            if(!_userService.ValidateID(like.UserID))
            {
                throw new ArgumentException("Given UserID doesn't exist.");
            } else if(!_socialMediaPostService.ValidateID(like.PostID))
            {
                throw new ArgumentException("Given PostID doesn't exist.");
            } else if(dbLikes.Contains(like))
            {
                throw new ArgumentException("Given Like already exists.");
            }

            return true;
        }


    }
}
