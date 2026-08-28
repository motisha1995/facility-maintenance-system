using System;
using System.Collections.Generic;
using System.Linq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Service implementing RequestApproval business logic
    /// </summary>
    public class ApprovalService : IApprovalService
    {
        private FacilityMaintenanceContext _context;

        public ApprovalService(FacilityMaintenanceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// STEP 2: Create approval record for a request
        /// </summary>
        public RequestApproval CreateApproval(RequestApproval approval)
        {
            if (approval == null)
                throw new ArgumentNullException(nameof(approval));

            approval.Status = ApprovalStatus.Pending.ToString();
            approval.CreatedAt = DateTime.Now;

            _context.RequestApprovals.Add(approval);
            _context.SaveChanges();

            return approval;
        }

        public RequestApproval GetApprovalById(int approvalId)
        {
            return _context.RequestApprovals.FirstOrDefault(ra => ra.ApprovalId == approvalId);
        }

        public List<RequestApproval> GetApprovalsByRequest(int requestId)
        {
            return _context.RequestApprovals
                .Where(ra => ra.RequestId == requestId)
                .ToList();
        }

        /// <summary>
        /// Get pending approvals for a department manager
        /// </summary>
        public List<RequestApproval> GetPendingApprovals(int approverId)
        {
            return _context.RequestApprovals
                .Where(ra => ra.ApproverId == approverId && ra.Status == ApprovalStatus.Pending.ToString())
                .ToList();
        }

        /// <summary>
        /// STEP 2: Approve a request
        /// </summary>
        public void ApproveRequest(int approvalId, string comments)
        {
            var approval = GetApprovalById(approvalId);
            if (approval == null)
                throw new InvalidOperationException("Approval not found");

            approval.Status = ApprovalStatus.Approved.ToString();
            approval.Comments = comments;
            approval.ApprovedAt = DateTime.Now;

            // Update main request status
            var request = approval.MaintenanceRequest;
            request.Status = RequestStatus.Approved.ToString();
            request.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        }

        /// <summary>
        /// STEP 2: Reject a request
        /// </summary>
        public void RejectRequest(int approvalId, string comments)
        {
            var approval = GetApprovalById(approvalId);
            if (approval == null)
                throw new InvalidOperationException("Approval not found");

            approval.Status = ApprovalStatus.Rejected.ToString();
            approval.Comments = comments;

            // Update main request status
            var request = approval.MaintenanceRequest;
            request.Status = RequestStatus.Rejected.ToString();
            request.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        }

        /// <summary>
        /// Check if a request is approved
        /// </summary>
        public bool IsRequestApproved(int requestId)
        {
            var approval = _context.RequestApprovals
                .FirstOrDefault(ra => ra.RequestId == requestId && ra.Status == ApprovalStatus.Approved.ToString());

            return approval != null;
        }
    }
}
