using System.ComponentModel.DataAnnotations;

namespace Biky_Backend.Services.DTO
{
    public class ReportCloseRequest
    {
        [Required]
        public Guid reportID { get; set; }

        public string reportResponse { get; set; }
    }
}
