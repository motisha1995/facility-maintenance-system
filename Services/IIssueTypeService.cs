using System.Collections.Generic;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Interface for IssueType business logic
    /// </summary>
    public interface IIssueTypeService
    {
        IssueType CreateIssueType(IssueType issueType);
        IssueType GetIssueTypeById(int issueTypeId);
        List<IssueType> GetAllIssueTypes();
        List<IssueType> GetAllActiveIssueTypes();
        IssueType GetIssueTypeByName(string typeName);
        void UpdateIssueType(IssueType issueType);
        void DeleteIssueType(int issueTypeId);
        void ActivateIssueType(int issueTypeId);
        void DeactivateIssueType(int issueTypeId);
    }
}
