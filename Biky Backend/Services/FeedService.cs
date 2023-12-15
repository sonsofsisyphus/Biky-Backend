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

        private List<SocialMediaPostSendRequest> ConvertSocialToSend(List<SocialMediaPost> posts, Guid userID)
        {
            var p = posts.ConvertAll(p => SocialMediaPostSendRequest.ToSocialMediaPostSendRequest(p,
                _likeService.CountLike(p.PostID),
                _likeService.Exists(new LikeRequest()
                {
                    UserID = userID,
                    PostID = p.PostID
                })));
            foreach(var i in p)
            {
                i.Images = _imageCollectionService.GetImagesByPost(i.PostID);
            }
            return p;
        }
        public List<SocialMediaPostSendRequest> GetSocialMediaAll(Guid userID)
        {
            List<SocialMediaPost>? posts = _socialMediaPostService.GetAllFeed(userID);
            return ConvertSocialToSend(posts, userID);
        }

        public List<SocialMediaPostSendRequest> GetSocialMediaFollowings(Guid userID)
        {
            List<SocialMediaPost> posts = _socialMediaPostService.GetFollowingsFeed(userID);
            return ConvertSocialToSend(posts, userID);
        }

        //public List<SocialMediaPostSendRequest> GetSocialMediaFollowing(Guid userID)
    }
}
