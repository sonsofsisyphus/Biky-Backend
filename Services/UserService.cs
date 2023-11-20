using Entities;
namespace Services
{
    public class UserService
    {
        private List<User> users = new List<User>
        {
             new User
            {
                UserID = Guid.Parse("c71ae2ef-dcfc-419c-a6c5-5858716cb5bd"),
                UniversityID = "123456",
                Nickname = "user1",
                Email = "user1@example.com"
            },
            new User
            {
                UserID = Guid.Parse("5f2d78d7-a387-49e9-b5a5-899f91ffe2f9"),
                UniversityID = "789012",
                Nickname = "user2",
                Email = "user2@example.com"
            },
            new User
            {
                UserID = Guid.Parse("2cd9834f-6911-44a8-8080-7a313867ff0f"),
                UniversityID = "232356",
                Nickname = "user3",
                Email = "user3@example.com"
            },
        } ;

        private List<Following> followings = new List<Following>
        {
            new Following
            {
                followerID = Guid.Parse("c71ae2ef-dcfc-419c-a6c5-5858716cb5bd"),
                followingID = Guid.Parse("5f2d78d7-a387-49e9-b5a5-899f91ffe2f9")
            },
            new Following
            {
                followerID = Guid.Parse("c71ae2ef-dcfc-419c-a6c5-5858716cb5bd"),
                followingID = Guid.Parse("2cd9834f-6911-44a8-8080-7a313867ff0f")
            }
        };

        public User? GetUserByID(Guid userID)
        {
            User? user = users.FirstOrDefault(user => user.UserID == userID);

            return user;
        }

        public List<Guid> GetFollowersByID(Guid userID)
        {
            var followerList = followings.Where(a => a.followerID == userID).Select(a => a.followingID).ToList();

            return followerList;
        }

        public List<Guid> GetFollowingsByID(Guid userID)
        {
            var followingList = followings.Where(a => a.followingID == userID).Select(a => a.followerID).ToList();

            return followingList;
        }

        public void AddFollowing(Following following)
        {
            if(ValidateFollowing(following))
            {
                followings.Add(following);
            }

        }

        private bool ValidateFollowing(Following following)
        {
            if(users.FirstOrDefault(user => user.UserID == following.followerID) == null)
            {
                throw new ArgumentException("Given followerID doesn't exist.");
            }
            else if (users.FirstOrDefault(user => user.UserID == following.followingID) == null)
            {
                throw new ArgumentException("Given followingID doesn't exist.");
            }
            else if (followings.Contains(following))
            {
                throw new ArgumentException("Given following already exists.");
            } 
             if(following.followerID == following.followingID)
            {
                throw new ArgumentException("An user cannot follow itself.");
            }
            return true;
        }
    }
}