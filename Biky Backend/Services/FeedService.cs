using Services;
using Entities;
using Services.DTO;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;
using Biky_Backend.Services.DTO;

namespace Biky_Backend.Services
{
    public class FeedService
    {
        private readonly DBConnector _dbConnector;
        private readonly SocialMediaPostService _socialMediaPostService;
        private readonly SalePostService _salePostService;
        private readonly LikeService _likeService;
        private readonly ImageCollectionService _imageCollectionService;

        public FeedService(DBConnector dbConnector,
            SocialMediaPostService socialMediaPostService,
            SalePostService salePostService,
            LikeService likeService,
            ImageCollectionService imageCollectionService)
        {
            _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));
            _socialMediaPostService = socialMediaPostService ?? throw new ArgumentNullException(nameof(socialMediaPostService));
            _salePostService = salePostService ?? throw new ArgumentNullException(nameof(salePostService));
            _likeService = likeService ?? throw new ArgumentNullException(nameof(likeService));
            _imageCollectionService = imageCollectionService ?? throw new ArgumentNullException(nameof(imageCollectionService));
        }

        // Helper method to convert social media posts to sendable format with additional details.
        private List<SocialMediaPostSendRequest> ConvertSocialToSend(List<SocialMediaPost> posts, Guid userID)
        {
            var p = posts.ConvertAll(p => SocialMediaPostSendRequest.ToSocialMediaPostSendRequest(p,
                _likeService.CountLike(p.PostID),
                _likeService.Exists(new LikeRequest()
                {
                    UserID = userID,
                    PostID = p.PostID
                })));
            foreach (var i in p)
            {
                i.Images = _imageCollectionService.GetImagesByPost(i.PostID);
            }
            return p;
        }

        // Helper method to convert social media posts to sendable format without additional details.
        private List<SocialMediaPostSendRequest> ConvertSocialToSend(List<SocialMediaPost> posts)
        {
            var p = posts.ConvertAll(p => SocialMediaPostSendRequest.ToSocialMediaPostSendRequest(p,
                _likeService.CountLike(p.PostID),
                false));
            foreach (var i in p)
            {
                i.Images = _imageCollectionService.GetImagesByPost(i.PostID);
            }
            return p;
        }

        // Helper method to convert sale posts to sendable format with additional details.
        private List<SalePostSendRequest> ConvertSaleToSend(List<SalePost> posts)
        {
            var p = posts.ConvertAll(p => SalePostSendRequest.ToSalePostSendRequest(p));
            foreach (var i in p)
            {
                i.Images = _imageCollectionService.GetImagesByPost(i.PostID);
            }
            return p;
        }

        // Method to retrieve all social media posts for a user.
        public List<SocialMediaPostSendRequest> GetSocialMediaAll(Guid userID)
        {
            List<SocialMediaPost>? posts = _socialMediaPostService.GetAllFeed();
            return ConvertSocialToSend(posts, userID);
        }

        // Method to retrieve social media posts for a guest user.
        public List<SocialMediaPostSendRequest> GetSocialMediaGuest()
        {
            List<SocialMediaPost>? posts = _socialMediaPostService.GetAllFeed();
            return ConvertSocialToSend(posts);
        }

        // Method to retrieve social media posts from users the current user is following.
        public List<SocialMediaPostSendRequest> GetSocialMediaFollowings(Guid userID)
        {
            List<SocialMediaPost> posts = _socialMediaPostService.GetFollowingsFeed(userID);
            return ConvertSocialToSend(posts, userID);
        }

        // Method to retrieve social media posts based on content search.
        public List<SocialMediaPostSendRequest> GetSocialMediaByContent(string contains, Guid userID)
        {
            List<SocialMediaPost> posts = _socialMediaPostService.GetSearchedFeed(contains);
            return ConvertSocialToSend(posts, userID);
        }

        // Method to retrieve social media posts for a specific user's profile.
        public List<SocialMediaPostSendRequest> GetSocialMediaUser(Guid seeing, Guid profile)
        {
            List<SocialMediaPost> posts = _socialMediaPostService.GetPostByUserID(profile);
            return ConvertSocialToSend(posts, seeing);
        }

        // Method to retrieve all sale posts.
        public List<SalePostSendRequest> GetSaleAll()
        {
            List<SalePost>? posts = _salePostService.GetAllFeed();
            return ConvertSaleToSend(posts);
        }

        // Method to retrieve sale posts from users the current user is following.
        public List<SalePostSendRequest> GetSaleFollowings(Guid userID)
        {
            List<SalePost>? posts = _salePostService.GetFollowingsFeed(userID);
            return ConvertSaleToSend(posts);
        }

        // Method to retrieve filtered sale posts based on criteria.
        public List<SalePostSendRequest> GetSaleFiltered(SaleFilter filters) {
            List<SalePost>? posts = _salePostService.GetFilteredFeed(filters);
            return ConvertSaleToSend(posts);
        }

        // Method to retrieve sale posts for a specific user's profile
        public List<SalePostSendRequest> GetSaleUser(Guid profile)
        {
            List<SalePost> posts = _salePostService.GetPostsByUserID(profile);
            return ConvertSaleToSend(posts);
        }
    }
}