using System;
using System.Collections.Generic;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Interface for RequestAssignment business logic
    /// Covers STEP 5 - Assignment and Scheduling
    /// </summary>
    public interface IAssignmentService
    {
        RequestAssignment CreateAssignment(RequestAssignment assignment);
        RequestAssignment GetAssignmentByRequest(int requestId);
        List<RequestAssignment> GetAssignmentsByTechnician(int technicianId);
        List<RequestAssignment> GetScheduledAssignments(DateTime startDate, DateTime endDate);
        void UpdateAssignmentStatus(int assignmentId, string status);
        List<RequestAssignment> GetOverdueAssignments();
    }
}
