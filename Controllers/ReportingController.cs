using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Controllers
{
    /// <summary>
    /// Controller for Reporting operations
    /// Handles STEP 9: Reporting and Continuous Improvement
    /// </summary>
    [Authorize(Roles = "Admin,FacilitiesManager")]
    public class ReportingController : Controller
    {
        private IReportingService _reportingService;

        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        // GET: Reporting/Dashboard
        [HttpGet]
        public ActionResult Dashboard()
        {
            var totalCompleted = _reportingService.GetTotalRequestsCompleted();
            var totalOpen = _reportingService.GetTotalRequestsOpen();
            var avgResolutionTime = _reportingService.GetAverageResolutionTime();
            var issueBreakdown = _reportingService.GetIssueTypeBreakdown();
            var statusBreakdown = _reportingService.GetRequestsByStatus();
            var recommendations = _reportingService.GenerateRecommendations();

            ViewBag.TotalCompleted = totalCompleted;
            ViewBag.TotalOpen = totalOpen;
            ViewBag.AverageResolutionTime = Math.Round(avgResolutionTime, 2);
            ViewBag.IssueBreakdown = issueBreakdown;
            ViewBag.StatusBreakdown = statusBreakdown;
            ViewBag.Recommendations = recommendations;

            return View();
        }

        // GET: Reporting/MonthlyReport
        [HttpGet]
        public ActionResult MonthlyReport()
        {
            var report = _reportingService.GenerateMonthlyReport();
            return View("ReportDetails", report);
        }

        // GET: Reporting/QuarterlyReport
        [HttpGet]
        public ActionResult QuarterlyReport()
        {
            var report = _reportingService.GenerateQuarterlyReport();
            return View("ReportDetails", report);
        }

        // GET: Reporting/YearlyReport
        [HttpGet]
        public ActionResult YearlyReport()
        {
            var report = _reportingService.GenerateYearlyReport();
            return View("ReportDetails", report);
        }

        // GET: Reporting/CustomReport
        [HttpGet]
        public ActionResult CustomReport()
        {
            return View();
        }

        // POST: Reporting/CustomReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                var report = _reportingService.GenerateCustomReport(startDate, endDate);
                return View("ReportDetails", report);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error generating report: " + ex.Message;
                return View();
            }
        }

        // GET: Reporting/ReportHistory
        [HttpGet]
        public ActionResult ReportHistory()
        {
            var reports = _reportingService.GetReportHistory();
            return View(reports);
        }

        // GET: Reporting/RecurringIssues
        [HttpGet]
        public ActionResult RecurringIssues()
        {
            var issues = _reportingService.IdentifyRecurringIssues();
            return View(issues);
        }

        // GET: Reporting/Recommendations
        [HttpGet]
        public ActionResult Recommendations()
        {
            var recommendations = _reportingService.GenerateRecommendations();
            return View(recommendations);
        }
    }
}
