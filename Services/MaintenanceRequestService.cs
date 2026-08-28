using System;
using System.Collections.Generic;
using System.Linq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Service implementing MaintenanceRequest business logic
    /// </summary>
    public class MaintenanceRequestService : IMaintenanceRequestService
    {
        private FacilityMaintenanceContext _context;

        public MaintenanceRequestService(FacilityMaintenanceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// STEP 1: Create a new maintenance request
        /// </summary>
        public MaintenanceRequest CreateRequest(MaintenanceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.TrackingId = GenerateTrackingId();
            request.Status = RequestStatus.Initiated.ToString();
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;

            _context.MaintenanceRequests.Add(request);
            _context.SaveChanges();

            return request;
        }

        /// <summary>
        /// Generate unique tracking ID in format MR-YYYY-XXXXX
        /// </summary>
        public string GenerateTrackingId()
        {
            var year = DateTime.Now.Year;
            var count = _context.MaintenanceRequests
                .Where(mr => mr.TrackingId.StartsWith($"MR-{year}"))
                .Count();

            return $"MR-{year}-{(count + 1).ToString("D5")}";
        }

        public MaintenanceRequest GetRequestById(int requestId)
        {
            return _context.MaintenanceRequests.FirstOrDefault(mr => mr.RequestId == requestId);
        }

        public List<MaintenanceRequest> GetRequestsByEmployee(int employeeId)
        {
            return _context.MaintenanceRequests
                .Where(mr => mr.EmployeeId == employeeId)
                .ToList();
        }

        public List<MaintenanceRequest> GetAllRequests()
        {
            return _context.MaintenanceRequests.ToList();
        }

        public void UpdateRequest(MaintenanceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.UpdatedAt = DateTime.Now;
            _context.Entry(request).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteRequest(int requestId)
        {
            var request = GetRequestById(requestId);
            if (request != null)
            {
                _context.MaintenanceRequests.Remove(request);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// STEP 3: Get requests by status for categorization
        /// </summary>
        public List<MaintenanceRequest> GetRequestsByStatus(string status)
        {
            return _context.MaintenanceRequests
                .Where(mr => mr.Status == status)
                .ToList();
        }

        public List<MaintenanceRequest> GetRequestsByLocation(int locationId)
        {
            return _context.MaintenanceRequests
                .Where(mr => mr.LocationId == locationId)
                .ToList();
        }

        public List<MaintenanceRequest> GetRequestsByIssueType(int issueTypeId)
        {
            return _context.MaintenanceRequests
                .Where(mr => mr.IssueTypeId == issueTypeId)
                .ToList();
        }

        public List<MaintenanceRequest> GetRequestsByUrgency(string urgency)
        {
            return _context.MaintenanceRequests
                .Where(mr => mr.Urgency == urgency)
                .ToList();
        }
    }
}
