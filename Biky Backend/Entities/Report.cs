using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Biky_Backend.Entities
{
    public enum ReportType
    {
        COMMENT,
        USER_PROFILE,
        POST
    }

    public class Report
    {
        [Key]
        public Guid ReportID { get; set; }

        public Guid AuthorID { get; set; }

        public ReportType ReportType { get; set; }

        public string ReportCategory { get; set; }

        public string ReportData { get; set; }

        public string ReportResponse { get; set; }

        public bool IsClosed { get; set; }
    }
}
