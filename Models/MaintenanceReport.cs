using System;

namespace FacilityMaintenanceSystem.Models
{
    /// <summary>
    /// MaintenanceReport model
    /// Covers STEP 9 (Reporting and Continuous Improvement)
    /// </summary>
    public class MaintenanceReport
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; } // Monthly, Quarterly, Yearly, Custom
        public int GeneratedBy { get; set; }
        public int TotalRequests { get; set; }
        public int CompletedRequests { get; set; }
        public decimal AverageResolutionTime { get; set; } // in hours
        public decimal AverageSatisfactionRating { get; set; }
        public string RecurringIssues { get; set; }
        public string RecommendedActions { get; set; }
        public DateTime GeneratedAt { get; set; }

        // Navigation Properties
        public virtual User GeneratedByUser { get; set; }
    }

    /// <summary>
    /// ReportType enum
    /// </summary>
    public enum ReportType
    {
        Monthly,
        Quarterly,
        Yearly,
        Custom
    }
}
