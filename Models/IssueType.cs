using System;
using System.Collections.Generic;

namespace FacilityMaintenanceSystem.Models
{
    /// <summary>
    /// IssueType model for categorizing maintenance issues
    /// </summary>
    public class IssueType
    {
        public int IssueTypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}
