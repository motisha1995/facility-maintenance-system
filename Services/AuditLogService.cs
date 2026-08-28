using System;
using System.Collections.Generic;
using System.Linq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Service implementing AuditLog business logic
    /// Tracks all system activity for compliance and security audit trails
    /// </summary>
    public class AuditLogService : IAuditLogService
    {
        private FacilityMaintenanceContext _context;

        public AuditLogService(FacilityMaintenanceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Log an action (Create, Update, Delete, Approve, Reject, etc.)
        /// </summary>
        public void LogAction(int userId, string action, string entityType, int? entityId, string oldValue = null, string newValue = null)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    UserId = userId,
                    Action = action,
                    EntityType = entityType,
                    EntityId = entityId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    Timestamp = DateTime.Now
                };

                _context.AuditLogs.Add(auditLog);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log to error handler instead of throwing
                // This prevents audit logging from breaking the application
                System.Diagnostics.Debug.WriteLine("Audit log error: " + ex.Message);
            }
        }

        /// <summary>
        /// Get audit log by ID
        /// </summary>
        public AuditLog GetAuditLogById(int auditId)
        {
            return _context.AuditLogs.FirstOrDefault(al => al.AuditId == auditId);
        }

        /// <summary>
        /// Get all audit logs for a specific user
        /// </summary>
        public List<AuditLog> GetAuditLogsByUser(int userId)
        {
            return _context.AuditLogs
                .Where(al => al.UserId == userId)
                .OrderByDescending(al => al.Timestamp)
                .ToList();
        }

        /// <summary>
        /// Get all audit logs for a specific entity
        /// </summary>
        public List<AuditLog> GetAuditLogsByEntity(string entityType, int entityId)
        {
            if (string.IsNullOrEmpty(entityType))
                return new List<AuditLog>();

            return _context.AuditLogs
                .Where(al => al.EntityType == entityType && al.EntityId == entityId)
                .OrderByDescending(al => al.Timestamp)
                .ToList();
        }

        /// <summary>
        /// Get audit logs within a date range
        /// </summary>
        public List<AuditLog> GetAuditLogsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.AuditLogs
                .Where(al => al.Timestamp >= startDate && al.Timestamp <= endDate)
                .OrderByDescending(al => al.Timestamp)
                .ToList();
        }

        /// <summary>
        /// Get audit logs for a specific action type
        /// </summary>
        public List<AuditLog> GetAuditLogsByAction(string action)
        {
            if (string.IsNullOrEmpty(action))
                return new List<AuditLog>();

            return _context.AuditLogs
                .Where(al => al.Action == action)
                .OrderByDescending(al => al.Timestamp)
                .ToList();
        }

        /// <summary>
        /// Get all audit logs (with optional pagination in production)
        /// </summary>
        public List<AuditLog> GetAllAuditLogs()
        {
            return _context.AuditLogs
                .OrderByDescending(al => al.Timestamp)
                .Take(10000) // Limit to prevent memory issues
                .ToList();
        }
    }
}
