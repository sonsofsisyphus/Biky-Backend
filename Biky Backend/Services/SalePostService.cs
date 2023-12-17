using Biky_Backend.Services.DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
using Services.DTO;

namespace Services
{
    public class SalePostService
    {
        private readonly DBConnector _dbConnector;
        private readonly UserService _userService;

        public SalePostService(DBConnector dbConnector, UserService userService)
        {
            _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));
            _userService = userService;
        }

        // Method to retrieve a sale post by its ID.
        public SalePost? GetPostByPostID(Guid postID)
        {
            try
            {
                return _dbConnector.SalePosts.FirstOrDefault(post => post.PostID == postID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPostByPostID: {ex.Message}");
                return null;
            }
        }

        // Method to retrieve all sale posts by a specific user.
        public List<SalePost> GetPostsByUserID(Guid userID)
        {
            try
            {
                return _dbConnector.SalePosts.Where(post => post.AuthorID == userID).Include(p => p.Author).ToList();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error in GetPostsByUserID: {ex.Message}");
                return new List<SalePost>();
            }
        }

        // Method to retrieve all sale posts in the feed.
        public List<SalePost> GetAllFeed()
        {
            try
            {
                var p = _dbConnector.SalePosts
                   .OrderByDescending(item => item.PostTime)
                   .Include(p => p.Author)
                   .ToList();

                return p;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllFeed: {ex.Message}");
                return new List<SalePost>();
            }
        }

        // Method to retrieve sale posts from users that the given user is following.
        public List<SalePost> GetFollowingsFeed(Guid userID)
        {
            try
            {
                List<Guid> followings = _userService.GetFollowingsByID(userID);
                return _dbConnector.SalePosts
                    .Where(item => followings.Contains(item.AuthorID))
                    .Include(p => p.Author)
                    .OrderByDescending(item => item.PostTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFollowingsFeed: {ex.Message}");
                return new List<SalePost>();
            }
        }

        // Method to retrieve sale posts based on filtering criteria.
        public List<SalePost> GetFilteredFeed(SaleFilter filters)
        {
            try
            {
                var query = _dbConnector.SalePosts.AsQueryable();
                if (filters.min != null)
                {
                    query = query.Where(entity => filters.min <= entity.Price);
                }
                if (filters.max != null)
                {
                    query = query.Where(entity => filters.max >= entity.Price);
                }
                if(filters.type != null)
                {
                    query = query.Where(entity => entity.PostType == filters.type);
                }
                if (filters.categoryid != null)
                {
                    query = query.Where(entity => entity.CategoryID == filters.categoryid);
                }
                if (filters.contains != null)
                {
                    query = query.Where(entity => EF.Functions.Like(entity.ContentText, $"%{filters.contains}%"));
                }
                return query.Include(p => p.Author)
                    .OrderByDescending(item => item.PostTime)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in getting filtered feed: {ex.Message}");
                return new List<SalePost>();
            }
        }

        // Method to add a new sale post to the database.
        public Guid AddPost(SalePostAddRequest post)
        {
            try
            {
                var p = post.ToSalePost();
                _dbConnector.SalePosts.Add(p);
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

        // Method to update an existing sale post.
        public Guid? UpdatePost(SalePost salePost)
        {
            try
            {
                var existingPost = _dbConnector.SalePosts.Find(salePost.PostID);
                if (existingPost != null)
                {
                    existingPost.AuthorID = salePost.AuthorID;
                    existingPost.ContentText = salePost.ContentText;
                    existingPost.PostTime = salePost.PostTime;
                    existingPost.PostType = salePost.PostType;
                    existingPost.Price = salePost.Price;
                    existingPost.Category = salePost.Category;

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

        // Method to remove a sale post by its ID.
        public bool RemovePost(Guid postID)
        {
            try
            {
                var postToRemove = _dbConnector.SalePosts.Find(postID);
                if (postToRemove != null)
                {
                    _dbConnector.SalePosts.Remove(postToRemove);
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

        // Method to validate if a sale post with the given ID exists.
        public bool ValidateID(Guid postID)
        {
            try
            {
                return _dbConnector.SalePosts.Any(a => a.PostID == postID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ValidateID: {ex.Message}");
                return false;
            }
        }

        // Method to get the owner (AuthorID) of a sale post by its ID.
        public Guid PostOwner(Guid postID)
        {
            try
            {
                var post = _dbConnector.SalePosts.SingleOrDefault(a => a.PostID == postID);
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
