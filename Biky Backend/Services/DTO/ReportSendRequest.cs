using Entities;

namespace Biky_Backend.Services.DTO
{
    public class ReportSendRequest
    {
        public Guid ReportID { get; set; }

        public Guid AuthorID { get; set; }

        public Guid ReportedID { get; set; }

        public ReportType ReportType { get; set; }

        public string ReportCategory { get; set; }

        public string ReportData { get; set; }

        public string ReportResponse { get; set; }

        public bool IsClosed { get; set; }

        public static ReportSendRequest ToReportSendRequest(Report r)
        {
            return new ReportSendRequest
            {
                ReportID = r.ReportID,
                AuthorID = r.AuthorID,
                ReportedID = r.ReportID,
                ReportType = r.ReportType,
                ReportCategory = r.ReportCategory,
                ReportData = r.ReportData,
                ReportResponse = r.ReportResponse,
                IsClosed = r.IsClosed
            };
        }
    }
}
