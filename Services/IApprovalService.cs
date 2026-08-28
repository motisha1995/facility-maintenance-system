using System.Collections.Generic;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Interface for RequestApproval business logic
    /// Covers STEP 2 - Internal Review and Approval
    /// </summary>
    public interface IApprovalService
    {
        RequestApproval CreateApproval(RequestApproval approval);
        RequestApproval GetApprovalById(int approvalId);
        List<RequestApproval> GetApprovalsByRequest(int requestId);
        List<RequestApproval> GetPendingApprovals(int approverId);
        void ApproveRequest(int approvalId, string comments);
        void RejectRequest(int approvalId, string comments);
        bool IsRequestApproved(int requestId);
    }
}
