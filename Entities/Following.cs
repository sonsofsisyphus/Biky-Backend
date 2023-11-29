using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Following
    {
        public Guid FollowerID {get; set; }

        public Guid FollowingID { get; set; }

        //followingID follows followerID.

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(Following))
            {
                return false;
            }
            Following cFollowing = (Following)obj;

            return FollowerID == cFollowing.FollowerID && FollowingID == cFollowing.FollowingID;
        }


    }
}
