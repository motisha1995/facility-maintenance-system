using System;
using System.Collections.Generic;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Interface for AuditLog business logic
    /// Tracks system activity and changes for compliance
    /// </summary>
    public interface IAuditLogService
    {
        void LogAction(int userId, string action, string entityType, int? entityId, string oldValue = null, string newValue = null);
        AuditLog GetAuditLogById(int auditId);
        List<AuditLog> GetAuditLogsByUser(int userId);
        List<AuditLog> GetAuditLogsByEntity(string entityType, int entityId);
        List<AuditLog> GetAuditLogsByDateRange(DateTime startDate, DateTime endDate);
        List<AuditLog> GetAuditLogsByAction(string action);
        List<AuditLog> GetAllAuditLogs();
    }
}
