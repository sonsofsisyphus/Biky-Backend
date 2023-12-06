using Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class SalePostService
    {
        private readonly DBConnector _dbConnector;

        public SalePostService(DBConnector dbConnector)
        {
            _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));
        }

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

        public List<SalePost> GetPostsByUserID(Guid userID)
        {
            try
            {
                return _dbConnector.SalePosts.Where(post => post.AuthorID == userID).ToList();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error in GetPostsByUserID: {ex.Message}");
                return new List<SalePost>();
            }
        }

        public List<SalePost> GetAllFeed()
        {
            try
            {
                return _dbConnector.SalePosts.OrderByDescending(item => item.PostTime).ToList();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error in GetAllFeed: {ex.Message}");
                return new List<SalePost>();
            }
        }

        public Guid AddPost(SalePost post)
        {
            try
            {
                _dbConnector.SalePosts.Add(post);
                _dbConnector.SaveChanges();
                return post.PostID;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error in AddPost: {ex.Message}");
                return Guid.Empty;
            }
        }

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
                    existingPost.ImagesID = salePost.ImagesID;
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
    }
}
