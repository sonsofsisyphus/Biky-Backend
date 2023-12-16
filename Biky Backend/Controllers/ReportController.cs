using Biky_Backend.ActionFilters;
using Biky_Backend.Entities;
using Biky_Backend.Services;
using Biky_Backend.Services.DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Biky_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly ReportService _reportService;

        public ReportController(ILogger<ReportController> logger, ReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        [HttpPost]
        [Route("Add")]
        [InjectUserId(typeof(ReportAddRequest), "AuthorID")]
        public IActionResult AddReport([FromBody] ReportAddRequest report)
        {
            try
            {
                var result = _reportService.AddReport(report);
                if (result != null)
                    return CreatedAtAction(nameof(GetReportByID), new { result }, result);
                return BadRequest("Cannot create report");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating report.");
                return BadRequest("Error creating report.");
            }
        }

        [HttpGet]
        [Route("GetByID")]
        public IActionResult GetReportByID(Guid reportID)
        {
            try
            {
                var report = _reportService.GetReportByID(reportID);
                if (report != null)
                    return Ok(report);
                return NotFound("Report not found!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting report by ID.");
                return BadRequest("Error getting report by ID.");
            }
        }

        [HttpGet]
        [Route("GetReports")]
        public IActionResult GetReports()
        {
            try
            {
                var reports = _reportService.GetReports();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reports.");
                return BadRequest("Error fetching reports");
            }
        }

        [HttpPost]
        [Route("Close")]
        //[Authorize(Roles = "Admin")]
        public IActionResult CloseReport(ReportCloseRequest report)
        {
            try
            {
                _reportService.CloseReport(report);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing report.");
                return BadRequest("Error closing report");
            }
        }
    }
}
