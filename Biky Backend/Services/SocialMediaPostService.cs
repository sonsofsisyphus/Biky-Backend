﻿using Entities;

namespace Services
{
    public class SocialMediaPostService
    {
        private readonly DBConnector _dbConnector;
        private readonly UserService _userService;

        public SocialMediaPostService(DBConnector dbConnector, UserService userService)
        {
            _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public SocialMediaPost? GetPostByPostID(Guid postID)
        {
            try
            {
                return _dbConnector.SocialMediaPosts.Find(postID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPostByPostID: {ex.Message}");
                return null;
            }
        }

        public List<SocialMediaPost> GetPostByUserID(Guid userID)
        {
            try
            {
                return _dbConnector.SocialMediaPosts
                    .Where(post => post.AuthorID == userID)
                    .OrderByDescending(item => item.PostTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPostByUserID: {ex.Message}");
                return new List<SocialMediaPost>();
            }
        }

        public List<SocialMediaPost> GetAllFeed()
        {
            try
            {
                return _dbConnector.SocialMediaPosts
                    .OrderByDescending(item => item.PostTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllFeed: {ex.Message}");
                return new List<SocialMediaPost>();
            }
        }

        public List<SocialMediaPost> GetFollowingsFeed(Guid userID)
        {
            try
            {
                List<Guid> followings = _userService.GetFollowingsByID(userID);
                return _dbConnector.SocialMediaPosts
                    .Where(item => followings.Contains(item.AuthorID))
                    .OrderByDescending(item => item.PostTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFollowingsFeed: {ex.Message}");
                return new List<SocialMediaPost>();
            }
        }

        public Guid AddPost(SocialMediaPost post)
        {
            try
            {
                _dbConnector.SocialMediaPosts.Add(post);
                _dbConnector.SaveChanges();
                return post.PostID;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddPost: {ex.Message}");
                return Guid.Empty;
            }
        }

        public Guid? UpdatePost(SocialMediaPost updatedPost)
        {
            try
            {
                var existingPost = _dbConnector.SocialMediaPosts.Find(updatedPost.PostID);
                if (existingPost != null)
                {
                    existingPost.AuthorID = updatedPost.AuthorID;
                    existingPost.ContentText = updatedPost.ContentText;
                    existingPost.PostTime = updatedPost.PostTime;
                    existingPost.ImagesID = updatedPost.ImagesID;
                    existingPost.IsAnonymous = updatedPost.IsAnonymous;

                    _dbConnector.SaveChanges();
                }

                return existingPost?.PostID;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdatePost: {ex.Message}");
                return null;
            }
        }

        public bool RemovePost(Guid postID)
        {
            try
            {
                var postToRemove = _dbConnector.SocialMediaPosts.Find(postID);
                if (postToRemove != null)
                {
                    _dbConnector.SocialMediaPosts.Remove(postToRemove);
                    _dbConnector.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RemovePost: {ex.Message}");
                return false;
            }
        }

        public bool ValidateID(Guid postID)
        {
            try
            {
                return _dbConnector.SocialMediaPosts.Any(a => a.PostID == postID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ValidateID: {ex.Message}");
                return false;
            }
        }

        public Guid PostOwner(Guid postID)
        {
            try
            {
                var post = _dbConnector.SocialMediaPosts.SingleOrDefault(a => a.PostID == postID);
                return post?.AuthorID ?? Guid.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PostOwner: {ex.Message}");
                return Guid.Empty;
            }
        }
    }
}