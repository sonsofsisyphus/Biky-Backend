using Biky_Backend.Services.DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.DTO;

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

        // Method to retrieve a social media post by its ID.
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

        // Method to retrieve all social media posts by a specific user (excluding anonymous posts).
        public List<SocialMediaPost> GetPostByUserID(Guid userID)
        {
            try
            {
                return _dbConnector.SocialMediaPosts
                    .Where(post => post.AuthorID == userID && !post.IsAnonymous).Include(p => p.Author)
                    .OrderByDescending(item => item.PostTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPostByUserID: {ex.Message}");
                return new List<SocialMediaPost>();
            }
        }

        // Method to retrieve all social media posts in the feed.
        public List<SocialMediaPost> GetAllFeed()
        {
            try
            {
                 var p = _dbConnector.SocialMediaPosts
                    .OrderByDescending(item => item.PostTime)
                    .Include(p => p.Author)
                    .ToList();
                
                return p;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllFeed: {ex.Message}");
                return new List<SocialMediaPost>();
            }
        }

        // Method to retrieve social media posts from users that the given user is following (excluding anonymous posts).
        public List<SocialMediaPost> GetFollowingsFeed(Guid userID)
        {
            try
            {
                List<Guid> followings = _userService.GetFollowersByID(userID);
                return _dbConnector.SocialMediaPosts
                    .Where(item => followings.Contains(item.AuthorID) && !item.IsAnonymous)
                    .Include(p => p.Author)
                    .OrderByDescending(item => item.PostTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFollowingsFeed: {ex.Message}");
                return new List<SocialMediaPost>();
            }
        }

        // Method to add a new social media post to the database.
        public Guid AddPost(SocialMediaPostAddRequest post)
        {
            try
            {

                var p = post.ToSocialMediaPost();
                _dbConnector.SocialMediaPosts.Add(p);
                _dbConnector.SaveChanges();
                if (post.Images != null)
                {
                    foreach (var i in post.Images)
                    {
                        _dbConnector.ImageCollections.Add(new ImageCollection(i, p.PostID));
                    }
                }
                _dbConnector.SaveChanges();
                return p.PostID;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddPost: {ex.Message}");
                return Guid.Empty;
            }
        }

        // Method to update an existing social media post.
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

        // Method to remove a social media post by its ID.
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

        // Method to validate if a social media post with the given ID exists.
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

        // Method to get the owner (AuthorID) of a social media post by its ID.
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

        // Method to search for social media posts that contain a specific text in their content.
        public List<SocialMediaPost> GetSearchedFeed(string contains)
        {
            try
            {
                var query = _dbConnector.SocialMediaPosts.AsQueryable();
                if (!string.IsNullOrEmpty(contains))
                {
                    query = query.Where(entity => EF.Functions.Like(entity.ContentText, $"%{contains}%"));
                }
                return query.Include(p => p.Author)
                    .OrderByDescending(item => item.PostTime)
                    .ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in getting searched social media feed: {ex.Message}");
                return new List<SocialMediaPost>();
            }
        }
    }
}
