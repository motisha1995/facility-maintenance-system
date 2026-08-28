using System;
using System.Web.Mvc;
using FacilityMaintenanceSystem.Models;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Controllers
{
    /// <summary>
    /// Controller for Maintenance Work operations
    /// Handles STEP 6: Maintenance Execution
    /// </summary>
    [Authorize(Roles = "Technician,Admin")]
    public class MaintenanceWorkController : Controller
    {
        private IMaintenanceWorkService _workService;
        private IMaintenanceRequestService _requestService;

        public MaintenanceWorkController(
            IMaintenanceWorkService workService,
            IMaintenanceRequestService requestService)
        {
            _workService = workService;
            _requestService = requestService;
        }

        // GET: MaintenanceWork/MyWork
        [HttpGet]
        public ActionResult MyWork()
        {
            // int technicianId = GetCurrentUserId();
            // var work = _workService.GetWorkByTechnician(technicianId);
            var allWork = _workService.GetInProgressWork();
            return View(allWork);
        }

        // GET: MaintenanceWork/Start/5
        [HttpGet]
        public ActionResult Start(int id)
        {
            var work = _workService.GetWorkById(id);
            if (work == null)
                return HttpNotFound();

            return View(work);
        }

        // POST: MaintenanceWork/Start/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Start(int id)
        {
            try
            {
                _workService.StartWork(id);
                TempData["Success"] = "Maintenance work started successfully";
                return RedirectToAction("MyWork");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error starting work: " + ex.Message;
                return RedirectToAction("MyWork");
            }
        }

        // GET: MaintenanceWork/Complete/5
        [HttpGet]
        public ActionResult Complete(int id)
        {
            var work = _workService.GetWorkById(id);
            if (work == null)
                return HttpNotFound();

            return View(work);
        }

        // POST: MaintenanceWork/Complete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complete(int id, string notes, string partsUsed)
        {
            try
            {
                var work = _workService.GetWorkById(id);
                if (work != null)
                {
                    work.PartsUsed = partsUsed;
                    _workService.CompleteWork(id, notes);
                    TempData["Success"] = "Maintenance work completed successfully";
                    return RedirectToAction("MyWork");
                }
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error completing work: " + ex.Message;
                return RedirectToAction("MyWork");
            }
        }

        // GET: MaintenanceWork/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            var work = _workService.GetWorkById(id);
            if (work == null)
                return HttpNotFound();

            return View(work);
        }

        // GET: MaintenanceWork/OverdueWork
        [HttpGet]
        [Authorize(Roles = "FacilitiesManager,Admin")]
        public ActionResult OverdueWork()
        {
            var overdueWork = _workService.GetOverdueWork();
            return View(overdueWork);
        }
    }
}
