using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FacilityMaintenanceSystem.Models;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Controllers
{
    /// <summary>
    /// Controller for MaintenanceRequest operations
    /// Handles STEP 1 & 3: Request Initiation and Logging
    /// </summary>
    [Authorize]
    public class MaintenanceRequestController : Controller
    {
        private IMaintenanceRequestService _requestService;
        private ILocationService _locationService;
        private IIssueTypeService _issueTypeService;

        public MaintenanceRequestController(
            IMaintenanceRequestService requestService,
            ILocationService locationService,
            IIssueTypeService issueTypeService)
        {
            _requestService = requestService;
            _locationService = locationService;
            _issueTypeService = issueTypeService;
        }

        // GET: MaintenanceRequest
        [HttpGet]
        public ActionResult Index()
        {
            var requests = _requestService.GetAllRequests();
            return View(requests);
        }

        // GET: MaintenanceRequest/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Locations = new SelectList(_locationService.GetAllLocations(), "LocationId", "FullLocation");
            ViewBag.IssueTypes = new SelectList(_issueTypeService.GetAllActiveIssueTypes(), "IssueTypeId", "TypeName");
            ViewBag.UrgencyLevels = new SelectList(new[] { "Critical", "High", "Normal", "Low" });
            return View();
        }

        // POST: MaintenanceRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MaintenanceRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Set employee ID from current user
                    // request.EmployeeId = GetCurrentUserId();
                    var createdRequest = _requestService.CreateRequest(request);
                    TempData["Success"] = $"Request created successfully with Tracking ID: {createdRequest.TrackingId}";
                    return RedirectToAction("Details", new { id = createdRequest.RequestId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating request: " + ex.Message);
                }
            }

            ViewBag.Locations = new SelectList(_locationService.GetAllLocations(), "LocationId", "FullLocation", request.LocationId);
            ViewBag.IssueTypes = new SelectList(_issueTypeService.GetAllActiveIssueTypes(), "IssueTypeId", "TypeName", request.IssueTypeId);
            return View(request);
        }

        // GET: MaintenanceRequest/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var request = _requestService.GetRequestById(id);
            if (request == null)
                return HttpNotFound();

            return View(request);
        }

        // GET: MaintenanceRequest/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin,FacilitiesAdmin")]
        public ActionResult Edit(int id)
        {
            var request = _requestService.GetRequestById(id);
            if (request == null)
                return HttpNotFound();

            ViewBag.Locations = new SelectList(_locationService.GetAllLocations(), "LocationId", "FullLocation", request.LocationId);
            ViewBag.IssueTypes = new SelectList(_issueTypeService.GetAllActiveIssueTypes(), "IssueTypeId", "TypeName", request.IssueTypeId);
            return View(request);
        }

        // POST: MaintenanceRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,FacilitiesAdmin")]
        public ActionResult Edit(int id, MaintenanceRequest request)
        {
            var existingRequest = _requestService.GetRequestById(id);
            if (existingRequest == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    existingRequest.Title = request.Title;
                    existingRequest.Description = request.Description;
                    existingRequest.Urgency = request.Urgency;
                    existingRequest.LocationId = request.LocationId;
                    existingRequest.IssueTypeId = request.IssueTypeId;

                    _requestService.UpdateRequest(existingRequest);
                    TempData["Success"] = "Request updated successfully";
                    return RedirectToAction("Details", new { id = id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating request: " + ex.Message);
                }
            }

            ViewBag.Locations = new SelectList(_locationService.GetAllLocations(), "LocationId", "FullLocation", request.LocationId);
            ViewBag.IssueTypes = new SelectList(_issueTypeService.GetAllActiveIssueTypes(), "IssueTypeId", "TypeName", request.IssueTypeId);
            return View(request);
        }

        // GET: MaintenanceRequest/MyRequests
        [HttpGet]
        public ActionResult MyRequests()
        {
            // int employeeId = GetCurrentUserId();
            // var requests = _requestService.GetRequestsByEmployee(employeeId);
            var requests = _requestService.GetAllRequests();
            return View(requests);
        }

        // GET: MaintenanceRequest/Filter
        [HttpGet]
        public ActionResult Filter(string status, string urgency, int? locationId, int? issueTypeId)
        {
            var allRequests = _requestService.GetAllRequests();

            if (!string.IsNullOrEmpty(status))
                allRequests = allRequests.Where(r => r.Status == status).ToList();

            if (!string.IsNullOrEmpty(urgency))
                allRequests = allRequests.Where(r => r.Urgency == urgency).ToList();

            if (locationId.HasValue)
                allRequests = allRequests.Where(r => r.LocationId == locationId).ToList();

            if (issueTypeId.HasValue)
                allRequests = allRequests.Where(r => r.IssueTypeId == issueTypeId).ToList();

            ViewBag.Locations = new SelectList(_locationService.GetAllLocations(), "LocationId", "FullLocation", locationId);
            ViewBag.IssueTypes = new SelectList(_issueTypeService.GetAllActiveIssueTypes(), "IssueTypeId", "TypeName", issueTypeId);
            ViewBag.StatusOptions = new SelectList(new[] { "Initiated", "Approved", "Assigned", "InProgress", "Completed", "Closed", "Rejected" }, status);
            ViewBag.UrgencyOptions = new SelectList(new[] { "Critical", "High", "Normal", "Low" }, urgency);

            return View("Index", allRequests);
        }
    }
}
