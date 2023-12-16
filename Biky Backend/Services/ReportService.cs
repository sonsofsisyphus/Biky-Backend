using Services;

namespace Biky_Backend.Services
{
    public class ReportService
    {
        private readonly DBConnector _dbContext;
        private readonly UserService _userService;

        public ReportService(DBConnector dbContext, UserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }


    }
}
