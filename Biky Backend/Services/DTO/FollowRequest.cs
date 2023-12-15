using Entities;

namespace Biky_Backend.Services.DTO
{
    public class FollowRequest
    {
        public Guid FollowerID { get; set; }
        public Guid FollowingID { get; set; }

        public Follow ToFollow()
        {
            return new Follow()
            {
                FollowerID = FollowerID,
                FollowingID = FollowingID,
                FollowID = Guid.NewGuid()
            };
        }
    }
}
