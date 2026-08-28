using System;
using System.Web.Mvc;
using FacilityMaintenanceSystem.Models;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Controllers
{
    /// <summary>
    /// Controller for Feedback operations
    /// Handles STEP 8: Closure and Feedback
    /// </summary>
    [Authorize]
    public class FeedbackController : Controller
    {
        private IFeedbackService _feedbackService;
        private IMaintenanceRequestService _requestService;

        public FeedbackController(
            IFeedbackService feedbackService,
            IMaintenanceRequestService requestService)
        {
            _feedbackService = feedbackService;
            _requestService = requestService;
        }

        // GET: Feedback/Survey/5
        [HttpGet]
        public ActionResult Survey(int id)
        {
            var request = _requestService.GetRequestById(id);
            if (request == null)
                return HttpNotFound();

            // Check if already has feedback
            var existingFeedback = _feedbackService.GetFeedbackByRequest(id);
            if (existingFeedback != null)
            {
                return RedirectToAction("Details", new { id = id });
            }

            var feedback = new RequestFeedback { RequestId = id };
            ViewBag.Ratings = new SelectList(new[] { 1, 2, 3, 4, 5 });
            return View(feedback);
        }

        // POST: Feedback/Survey/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Survey(int id, RequestFeedback feedback)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    feedback.RequestId = id;
                    // feedback.SubmittedBy = GetCurrentUserId();
                    _feedbackService.CreateFeedback(feedback);
                    _feedbackService.ClosRequest(id);

                    TempData["Success"] = "Thank you for your feedback. Request has been closed.";
                    return RedirectToAction("Details", new { id = id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error submitting feedback: " + ex.Message);
                }
            }

            ViewBag.Ratings = new SelectList(new[] { 1, 2, 3, 4, 5 });
            return View(feedback);
        }

        // GET: Feedback/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var feedback = _feedbackService.GetFeedbackByRequest(id);
            if (feedback == null)
                return HttpNotFound();

            return View(feedback);
        }

        // GET: Feedback/Summary
        [HttpGet]
        [Authorize(Roles = "Admin,FacilitiesManager")]
        public ActionResult Summary()
        {
            var avgSatisfaction = _feedbackService.GetAverageSatisfactionRating();
            var avgQuality = _feedbackService.GetAverageQualityRating();
            var avgTimeliness = _feedbackService.GetAverageTimelinessRating();
            var avgProfessionalism = _feedbackService.GetAverageProfessionalismRating();

            ViewBag.AverageSatisfaction = Math.Round(avgSatisfaction, 2);
            ViewBag.AverageQuality = Math.Round(avgQuality, 2);
            ViewBag.AverageTimeliness = Math.Round(avgTimeliness, 2);
            ViewBag.AverageProfessionalism = Math.Round(avgProfessionalism, 2);

            return View();
        }
    }
}
