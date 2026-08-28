using System.Collections.Generic;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Interface for RequestAssessment business logic
    /// Covers STEP 4 - Initial Assessment and Prioritization
    /// </summary>
    public interface IAssessmentService
    {
        RequestAssessment CreateAssessment(RequestAssessment assessment);
        RequestAssessment GetAssessmentByRequest(int requestId);
        List<RequestAssessment> GetAssessmentsByPriority(string priority);
        List<RequestAssessment> GetCriticalAssessments();
        void UpdateAssessment(RequestAssessment assessment);
        int CalculatePriority(MaintenanceRequest request, RequestAssessment assessment);
    }
}
