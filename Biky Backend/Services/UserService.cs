using Biky_Backend.Services.DTO;
using Entities;
using Services.DTO;

namespace Services
{
    public class UserService
    {
        private readonly DBConnector _dbConnector;
        private readonly NotificationService _notificationService;

        public UserService(DBConnector dbConnector, NotificationService notificationService)
        {
            _dbConnector = dbConnector ?? throw new ArgumentNullException(nameof(dbConnector));
            _notificationService = notificationService;
        }

        public UserSendRequest? GetUserByID(Guid userID)
        {
            try
            {
                return UserSendRequest.ToUserSendRequest(_dbConnector.Users.FirstOrDefault(user => user.UserID == userID));
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserByID: {ex.Message}");
                return null;
            }
        }

        public User? GetUserByNickname(string nickname)
        {
            try
            {
                return _dbConnector.Users.FirstOrDefault(user => user.Nickname == nickname);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserByNickname: {ex.Message}");
                return null;
            }
        }

        public List<Guid> GetFollowersByID(Guid userID)
        {
            try
            {
                return _dbConnector.Follows
                    .Where(a => a.FollowerID == userID)
                    .Select(a => a.FollowingID)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFollowersByID: {ex.Message}");
                return new List<Guid>();
            }
        }

        public int CountFollowersByID(Guid userID)
        {
            try
            {
                return _dbConnector.Follows
                    .Count(a => a.FollowerID == userID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in counting followers: {ex.Message}");
                return -1;
            }
        }

        public List<Guid> GetFollowingsByID(Guid userID)
        {
            try
            {
                return _dbConnector.Follows
                    .Where(a => a.FollowingID == userID)
                    .Select(a => a.FollowerID)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFollowingsByID: {ex.Message}");
                return new List<Guid>();
            }
        }

        public int CountFollowingsByID(Guid userID)
        {
            try
            {
                return _dbConnector.Follows
                    .Count(a => a.FollowingID == userID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in counting followings: {ex.Message}");
                return -1;
            }
        }

        public void AddFollowing(FollowRequest follow)
        {
            try
            {
                if (ValidateFollowing(follow))
                {
                    _notificationService.AddNotification(new DTO.NotificationAddRequest()
                    {
                        ReceiverID = follow.FollowerID,
                        Content = $"{GetUserByID(follow.FollowingID).Nickname} has followed you"
                    });

                    _dbConnector.Follows.Add(follow.ToFollow());
                    _dbConnector.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddFollowing: {ex.Message}");
            }
        }

        public void RemoveFollowing(FollowRequest follow)
        {
            try
            {
                if (_dbConnector.Follows.Any(f =>
                        f.FollowerID == follow.FollowerID && f.FollowingID == follow.FollowingID))
                {
                    var existingFollow = _dbConnector.Follows.FirstOrDefault(f =>
                        f.FollowerID == follow.FollowerID && f.FollowingID == follow.FollowingID);

                    if (existingFollow != null)
                    {
                        _dbConnector.Follows.Remove(existingFollow);
                        _dbConnector.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RemoveFollowing: {ex.Message}");
            }
        }

        public bool ValidateFollowing(FollowRequest follow)
        {
            try
            {
                var followerExists = _dbConnector.Users.Any(user => user.UserID == follow.FollowerID);
                var followingExists = _dbConnector.Users.Any(user => user.UserID == follow.FollowingID);
                var followExists = _dbConnector.Follows.Any(f =>
                    f.FollowerID == follow.FollowerID && f.FollowingID == follow.FollowingID);

                if (!followerExists)
                {
                    throw new ArgumentException("Given followerID doesn't exist.");
                }

                if (!followingExists)
                {
                    throw new ArgumentException("Given followingID doesn't exist.");
                }

                if (followExists)
                {
                    throw new ArgumentException("Given following already exists.");
                }

                if (follow.FollowerID == follow.FollowingID)
                {
                    throw new ArgumentException("An user cannot follow itself.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ValidateFollowing: {ex.Message}");
                return false;
            }
        }

        public bool ValidatePassword(Guid userID, string password)
        {
            try
            {
                var user = _dbConnector.Users.FirstOrDefault(u => u.UserID == userID);
                return user != null && BCrypt.Net.BCrypt.Verify(password, user.Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ValidatePassword: {ex.Message}");
                return false;
            }
        }

        public bool ValidateID(Guid userID)
        {
            try
            {
                return _dbConnector.Users.Any(a => a.UserID == userID);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ValidateID: {ex.Message}");
                return false;
            }
        }

        public bool Register(UserRegisterRequest request)
        {
            try
            {
                if (_dbConnector.Users.Any(u => u.Nickname == request.Nickname))
                {
                    throw new ArgumentException("Nickname is already taken.");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
                
                var newUser = new User
                {
                    UserID = Guid.NewGuid(),
                    UniversityID = request.UniversityID,
                    Nickname = request.Nickname,
                    Email = request.Email,
                    Password = hashedPassword,
                };

                _dbConnector.Users.Add(newUser);
                _dbConnector.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Register: {ex.Message}");
                return false;
            }
        }

        public List<User> GetAllUsers()
        {
            return _dbConnector.Users.ToList();
        }

        public ProfileSendRequest GetProfileByID(Guid userID)
        {
            List<Guid> allSocialMediaPosts = _dbConnector.SocialMediaPosts.Where(p => p.AuthorID == userID).Select(p => p.PostID).ToList();
            int postNumber = allSocialMediaPosts.Count + _dbConnector.SalePosts.Count(p => p.AuthorID == userID);
            int likeNumber = _dbConnector.Likes.Count(l => allSocialMediaPosts.Contains(l.PostID));
            return new ProfileSendRequest(GetUserByID(userID), 
                CountFollowersByID(userID), 
                CountFollowingsByID(userID),
                postNumber,
                likeNumber
                );
        }
    }
}
