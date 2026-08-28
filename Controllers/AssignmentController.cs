using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FacilityMaintenanceSystem.Models;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Controllers
{
    /// <summary>
    /// Controller for Assignment operations
    /// Handles STEP 5: Assignment and Scheduling
    /// </summary>
    [Authorize(Roles = "FacilitiesCoordinator,Admin")]
    public class AssignmentController : Controller
    {
        private IAssignmentService _assignmentService;
        private IMaintenanceRequestService _requestService;
        private IUserService _userService;

        public AssignmentController(
            IAssignmentService assignmentService,
            IMaintenanceRequestService requestService,
            IUserService userService)
        {
            _assignmentService = assignmentService;
            _requestService = requestService;
            _userService = userService;
        }

        // GET: Assignment/Assign/5
        [HttpGet]
        public ActionResult Assign(int id)
        {
            var request = _requestService.GetRequestById(id);
            if (request == null)
                return HttpNotFound();

            var technicians = _userService.GetTechniciansByRole("Technician");
            ViewBag.Technicians = new SelectList(technicians, "UserId", "FullName");

            var assignment = new RequestAssignment { RequestId = id };
            return View(assignment);
        }

        // POST: Assignment/Assign/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(RequestAssignment assignment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // assignment.FacilitiesCoordinatorId = GetCurrentUserId();
                    _assignmentService.CreateAssignment(assignment);
                    TempData["Success"] = "Request assigned successfully";
                    return RedirectToAction("ScheduledAssignments");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error assigning request: " + ex.Message);
                }
            }

            var technicians = _userService.GetTechniciansByRole("Technician");
            ViewBag.Technicians = new SelectList(technicians, "UserId", "FullName", assignment.AssignedTo);
            return View(assignment);
        }

        // GET: Assignment/ScheduledAssignments
        [HttpGet]
        public ActionResult ScheduledAssignments()
        {
            var assignments = _assignmentService.GetScheduledAssignments(
                DateTime.Now.AddDays(-30),
                DateTime.Now.AddDays(30));

            return View(assignments);
        }

        // GET: Assignment/TechnicianWorkload/5
        [HttpGet]
        public ActionResult TechnicianWorkload(int id)
        {
            var assignments = _assignmentService.GetAssignmentsByTechnician(id);
            ViewBag.TechnicianId = id;
            return View(assignments);
        }

        // GET: Assignment/OverdueAssignments
        [HttpGet]
        public ActionResult OverdueAssignments()
        {
            var overdueAssignments = _assignmentService.GetOverdueAssignments();
            return View(overdueAssignments);
        }

        // POST: Assignment/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int id, string status)
        {
            try
            {
                _assignmentService.UpdateAssignmentStatus(id, status);
                TempData["Success"] = "Assignment status updated successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating status: " + ex.Message;
            }

            return RedirectToAction("ScheduledAssignments");
        }
    }
}
