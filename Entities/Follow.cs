using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Follow
    {
        [ForeignKey("User")]
        public Guid FollowerID { get; set; }

        [ForeignKey("User")]
        public Guid FollowingID { get; set; }

        public virtual User Follower { get; set; }

        public virtual User Following { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(Follow))
            {
                return false;
            }
            Follow cFollow = (Follow)obj;

            return FollowerID == cFollow.FollowerID && FollowingID == cFollow.FollowingID;
        }
    }
}