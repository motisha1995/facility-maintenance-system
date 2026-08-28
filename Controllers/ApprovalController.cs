using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FacilityMaintenanceSystem.Models;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Controllers
{
    /// <summary>
    /// Controller for Request Approval operations
    /// Handles STEP 2: Internal Review and Approval
    /// </summary>
    [Authorize(Roles = "DepartmentManager,Admin")]
    public class ApprovalController : Controller
    {
        private IApprovalService _approvalService;
        private IMaintenanceRequestService _requestService;

        public ApprovalController(
            IApprovalService approvalService,
            IMaintenanceRequestService requestService)
        {
            _approvalService = approvalService;
            _requestService = requestService;
        }

        // GET: Approval/PendingApprovals
        [HttpGet]
        public ActionResult PendingApprovals()
        {
            // int approverId = GetCurrentUserId();
            // var pendingApprovals = _approvalService.GetPendingApprovals(approverId);
            var allRequests = _requestService.GetAllRequests();
            var pendingRequests = allRequests.Where(r => r.Status == "Initiated").ToList();
            return View(pendingRequests);
        }

        // GET: Approval/Review/5
        [HttpGet]
        public ActionResult Review(int id)
        {
            var request = _requestService.GetRequestById(id);
            if (request == null)
                return HttpNotFound();

            return View(request);
        }

        // POST: Approval/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id, string comments)
        {
            try
            {
                var request = _requestService.GetRequestById(id);
                if (request == null)
                    return HttpNotFound();

                // Create approval record
                var approval = new RequestApproval
                {
                    RequestId = id,
                    // ApproverId = GetCurrentUserId(),
                    Status = "Pending",
                    Comments = comments
                };

                _approvalService.CreateApproval(approval);
                _approvalService.ApproveRequest(approval.ApprovalId, comments);

                TempData["Success"] = "Request approved successfully";
                return RedirectToAction("PendingApprovals");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error approving request: " + ex.Message;
                return RedirectToAction("Review", new { id = id });
            }
        }

        // POST: Approval/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reject(int id, string comments)
        {
            try
            {
                var request = _requestService.GetRequestById(id);
                if (request == null)
                    return HttpNotFound();

                // Create approval record
                var approval = new RequestApproval
                {
                    RequestId = id,
                    // ApproverId = GetCurrentUserId(),
                    Status = "Pending",
                    Comments = comments
                };

                _approvalService.CreateApproval(approval);
                _approvalService.RejectRequest(approval.ApprovalId, comments);

                TempData["Success"] = "Request rejected. Feedback sent to requester.";
                return RedirectToAction("PendingApprovals");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error rejecting request: " + ex.Message;
                return RedirectToAction("Review", new { id = id });
            }
        }

        // GET: Approval/ApprovedRequests
        [HttpGet]
        public ActionResult ApprovedRequests()
        {
            var allRequests = _requestService.GetAllRequests();
            var approvedRequests = allRequests.Where(r => r.Status == "Approved").ToList();
            return View(approvedRequests);
        }
    }
}
