using System;

namespace FacilityMaintenanceSystem.Models
{
    /// <summary>
    /// AuditLog model for system audit trail
    /// </summary>
    public class AuditLog
    {
        public int AuditId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string EntityType { get; set; }
        public int? EntityId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Timestamp { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
    }
}
