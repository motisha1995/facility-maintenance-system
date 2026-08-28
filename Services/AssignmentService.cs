using System;
using System.Collections.Generic;
using System.Linq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Service implementing RequestAssignment business logic
    /// </summary>
    public class AssignmentService : IAssignmentService
    {
        private FacilityMaintenanceContext _context;

        public AssignmentService(FacilityMaintenanceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// STEP 5: Create assignment for a request
        /// Assign to technician or external vendor and schedule
        /// </summary>
        public RequestAssignment CreateAssignment(RequestAssignment assignment)
        {
            if (assignment == null)
                throw new ArgumentNullException(nameof(assignment));

            assignment.AssignmentStatus = AssignmentStatus.Scheduled.ToString();
            assignment.CreatedAt = DateTime.Now;

            _context.RequestAssignments.Add(assignment);

            // Update request status
            var request = assignment.MaintenanceRequest;
            request.Status = RequestStatus.Assigned.ToString();
            request.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

            return assignment;
        }

        public RequestAssignment GetAssignmentByRequest(int requestId)
        {
            return _context.RequestAssignments
                .FirstOrDefault(ra => ra.RequestId == requestId);
        }

        /// <summary>
        /// Get assignments for a specific technician
        /// </summary>
        public List<RequestAssignment> GetAssignmentsByTechnician(int technicianId)
        {
            return _context.RequestAssignments
                .Where(ra => ra.AssignedTo == technicianId)
                .ToList();
        }

        /// <summary>
        /// Get scheduled assignments within a date range
        /// </summary>
        public List<RequestAssignment> GetScheduledAssignments(DateTime startDate, DateTime endDate)
        {
            return _context.RequestAssignments
                .Where(ra => ra.ScheduledStartDate >= startDate && ra.ScheduledEndDate <= endDate)
                .ToList();
        }

        /// <summary>
        /// Update assignment status (Scheduled, InProgress, Completed)
        /// </summary>
        public void UpdateAssignmentStatus(int assignmentId, string status)
        {
            var assignment = _context.RequestAssignments.FirstOrDefault(ra => ra.AssignmentId == assignmentId);
            if (assignment == null)
                throw new InvalidOperationException("Assignment not found");

            assignment.AssignmentStatus = status;
            _context.SaveChanges();
        }

        /// <summary>
        /// Get overdue assignments (scheduled end date passed)
        /// </summary>
        public List<RequestAssignment> GetOverdueAssignments()
        {
            return _context.RequestAssignments
                .Where(ra => ra.ScheduledEndDate < DateTime.Now && ra.AssignmentStatus != AssignmentStatus.Completed.ToString())
                .ToList();
        }
    }
}
