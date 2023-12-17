using Biky_Backend.Services.DTO;
using Entities;
using Services;

namespace Biky_Backend.Services
{
    public class ReportService
    {
        private readonly DBConnector _dbContext;

        public ReportService(DBConnector dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to add a new report to the database.
        public Guid AddReport(ReportAddRequest report)
        {
            try
            {
                var r = report.ToReport();
                if (ValidateReport(report))
                {
                    _dbContext.Reports.Add(r);
                    _dbContext.SaveChanges();
                }
                return r.ReportID;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddReport: {ex.Message}");
                return Guid.Empty;
            }
        }

        // Method to retrieve a report by its ID.
        public Report? GetReportByID(Guid reportID)
        {
            return _dbContext.Reports.FirstOrDefault(r => r.ReportID == reportID);
        }

        // Private method to validate a report before adding it to the database.
        private bool ValidateReport(ReportAddRequest report)
        {
            try
            {
                // Check if the reported entity (comment, user profile, post) exists.
                var reportedExists = report.ReportType switch
                {
                    ReportType.COMMENT => _dbContext.Comments.Any(c => c.CommentID == report.ReportedID),
                    ReportType.USER_PROFILE => _dbContext.Users.Any(u => u.UserID == report.ReportedID),
                    ReportType.POST => _dbContext.Posts.Any(p => p.PostID == report.ReportedID),
                    _ => false
                };

                // Check if a similar report already exists for the same author and reported entity.
                var reportExists = _dbContext.Reports.Any(r =>
                    r.AuthorID == report.AuthorID && r.ReportedID == report.ReportedID);

                if (reportExists)
                {
                    throw new ArgumentException("Given following already exists.");
                }
                if (!reportedExists)
                {
                    throw new ArgumentException("Given reported cannot be found.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ValidateReport: {ex.Message}");
                return false;
            }
        }

        // Method to retrieve a list of reports and convert them to ReportSendRequest objects.
        public List<ReportSendRequest> GetReports()
        {
            return _dbContext.Reports.Select(ReportSendRequest.ToReportSendRequest).ToList();
        }

        // Method to close a report and set the report response.
        public void CloseReport(ReportCloseRequest report)
        {
            try
            {
                var existingReport = _dbContext.Reports.Find(report.reportID);
                
                if (existingReport == null)
                    throw new ArgumentNullException("Report is not found.");

                existingReport.IsClosed = true;
                existingReport.ReportResponse = report.reportResponse;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CloseReport: {ex.Message}");
            }
        }
    }
}
