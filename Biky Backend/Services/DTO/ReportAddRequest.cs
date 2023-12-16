using Biky_Backend.Entities;
using Entities;
using System.ComponentModel.DataAnnotations;

namespace Biky_Backend.Services.DTO
{
    public class ReportAddRequest
    {
        [Required]
        public Guid AuthorID { get; set; }

        [Required]
        public Guid ReportedID { get; set; }

        [Required]
        public ReportType ReportType { get; set; }

        [Required]
        public string ReportCategory { get; set; }

        [Required]
        public string ReportData { get; set; }

        public Report ToReport()
        {
            return new Report
            {
                ReportID = Guid.NewGuid(),
                AuthorID = AuthorID,
                ReportedID = ReportedID,
                ReportType = ReportType,
                ReportCategory = ReportCategory,
                ReportData = ReportData,
                ReportResponse = "",
                IsClosed = false
            };
        }
    }
}