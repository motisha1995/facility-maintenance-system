using System;
using System.Web.Mvc;
using FacilityMaintenanceSystem.Models;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Controllers
{
    /// <summary>
    /// Controller for Completion Verification operations
    /// Handles STEP 7: Completion Verification
    /// </summary>
    [Authorize(Roles = "FacilitiesManager,Admin")]
    public class VerificationController : Controller
    {
        private ICompletionVerificationService _verificationService;
        private IMaintenanceRequestService _requestService;
        private IMaintenanceWorkService _workService;

        public VerificationController(
            ICompletionVerificationService verificationService,
            IMaintenanceRequestService requestService,
            IMaintenanceWorkService workService)
        {
            _verificationService = verificationService;
            _requestService = requestService;
            _workService = workService;
        }

        // GET: Verification/PendingVerifications
        [HttpGet]
        public ActionResult PendingVerifications()
        {
            var allRequests = _requestService.GetAllRequests();
            var completedRequests = allRequests.FindAll(r => r.Status == "Completed");
            return View(completedRequests);
        }

        // GET: Verification/Verify/5
        [HttpGet]
        public ActionResult Verify(int id)
        {
            var request = _requestService.GetRequestById(id);
            if (request == null)
                return HttpNotFound();

            var verification = _verificationService.GetVerificationByRequest(id);
            if (verification == null)
            {
                verification = new CompletionVerification { RequestId = id };
            }

            return View(verification);
        }

        // POST: Verification/Verify/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(int id, bool isVerified, string notes)
        {
            try
            {
                var verification = _verificationService.GetVerificationByRequest(id);

                if (verification == null)
                {
                    verification = new CompletionVerification
                    {
                        RequestId = id,
                        // VerifiedBy = GetCurrentUserId(),
                        VerificationNotes = notes
                    };
                    _verificationService.CreateVerification(verification);
                }

                _verificationService.VerifyCompletion(verification.VerificationId, isVerified, notes);

                if (isVerified)
                    TempData["Success"] = "Request verified and completed successfully";
                else
                    TempData["Success"] = "Request sent back for rework";

                return RedirectToAction("PendingVerifications");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error verifying request: " + ex.Message;
                return RedirectToAction("Verify", new { id = id });
            }
        }

        // GET: Verification/FailedVerifications
        [HttpGet]
        public ActionResult FailedVerifications()
        {
            var failedVerifications = _verificationService.GetFailedVerifications();
            return View(failedVerifications);
        }
    }
}
