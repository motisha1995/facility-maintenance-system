using System;
using System.Collections.Generic;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Interface for MaintenanceRequest business logic
    /// Covers STEP 1, 3 - Request Initiation and Logging
    /// </summary>
    public interface IMaintenanceRequestService
    {
        // STEP 1: Request Initiation
        MaintenanceRequest CreateRequest(MaintenanceRequest request);
        MaintenanceRequest GetRequestById(int requestId);
        List<MaintenanceRequest> GetRequestsByEmployee(int employeeId);
        List<MaintenanceRequest> GetAllRequests();
        void UpdateRequest(MaintenanceRequest request);
        void DeleteRequest(int requestId);

        // STEP 3: Logging and Categorization
        string GenerateTrackingId();
        List<MaintenanceRequest> GetRequestsByStatus(string status);
        List<MaintenanceRequest> GetRequestsByLocation(int locationId);
        List<MaintenanceRequest> GetRequestsByIssueType(int issueTypeId);
        List<MaintenanceRequest> GetRequestsByUrgency(string urgency);
    }
}
