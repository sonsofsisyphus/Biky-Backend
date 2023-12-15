using Entities;
using Microsoft.EntityFrameworkCore;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<SalePost> GetFilteredFeed(Dictionary<String, Object> filters)
        {
            try
            {
                var query = _dbConnector.SalePosts.AsQueryable();
                if (filters.ContainsKey("min") && filters["low"] is decimal low)
                {
                    query = query.Where(entity => low >= entity.Price);
                }
                if (filters.ContainsKey("max") && filters["max"] is decimal max)
                {
                    query = query.Where(entity => max >= entity.Price);
                }
                if(filters.ContainsKey("type") && filters["type"] is PostType type)
                {
                    query = query.Where(entity => entity.PostType == type);
                }
                if (filters.ContainsKey("categoryid") && filters["categoryid"] is int categoryid)
                {
                    query = query.Where(entity => entity.CategoryID == categoryid);
                }
                if (filters.ContainsKey("contains") && filters["contains"] is String contains)
                {
                    query = query.Where(entity => EF.Functions.Like(entity.ContentText, $"%{contains}%"));
                }
                return query.ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in getting filtered feed: {ex.Message}");
                return new List<SalePost>();
            }
        }

        public Guid AddPost(SalePostAddRequest post)
        {
            try
            {
                SalePost p = post.ToSalePost();
                _dbConnector.SalePosts.Add(p);
                _dbConnector.SaveChanges();
                if (post.Images != null)
                {
                    foreach (var i in post.Images)
                    {
                        _dbConnector.ImageCollections.Add(new ImageCollection(i, p.PostID));
                    }
                }
                return p.PostID;
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
