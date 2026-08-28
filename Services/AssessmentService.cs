using System;
using System.Collections.Generic;
using System.Linq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Service implementing RequestAssessment business logic
    /// </summary>
    public class AssessmentService : IAssessmentService
    {
        private FacilityMaintenanceContext _context;

        public AssessmentService(FacilityMaintenanceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// STEP 4: Create assessment for a request
        /// Facilities Manager assesses safety, impact, and sets priority
        /// </summary>
        public RequestAssessment CreateAssessment(RequestAssessment assessment)
        {
            if (assessment == null)
                throw new ArgumentNullException(nameof(assessment));

            assessment.AssessedAt = DateTime.Now;
            _context.RequestAssessments.Add(assessment);
            _context.SaveChanges();

            return assessment;
        }

        public RequestAssessment GetAssessmentByRequest(int requestId)
        {
            return _context.RequestAssessments
                .FirstOrDefault(ra => ra.RequestId == requestId);
        }

        /// <summary>
        /// Get assessments by priority level
        /// </summary>
        public List<RequestAssessment> GetAssessmentsByPriority(string priority)
        {
            return _context.RequestAssessments
                .Where(ra => ra.Priority == priority)
                .ToList();
        }

        /// <summary>
        /// Get all critical priority assessments
        /// </summary>
        public List<RequestAssessment> GetCriticalAssessments()
        {
            return _context.RequestAssessments
                .Where(ra => ra.Priority == PriorityLevel.Critical.ToString())
                .ToList();
        }

        public void UpdateAssessment(RequestAssessment assessment)
        {
            if (assessment == null)
                throw new ArgumentNullException(nameof(assessment));

            _context.Entry(assessment).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Calculate priority based on safety risk and urgency
        /// </summary>
        public int CalculatePriority(MaintenanceRequest request, RequestAssessment assessment)
        {
            int priorityScore = 0;

            // Safety risk (highest weight)
            if (assessment.SafetyRisk)
                priorityScore += 40;

            // Urgency level
            switch (request.Urgency)
            {
                case "Critical":
                    priorityScore += 30;
                    break;
                case "High":
                    priorityScore += 20;
                    break;
                case "Normal":
                    priorityScore += 10;
                    break;
                case "Low":
                    priorityScore += 5;
                    break;
            }

            // Operational impact
            if (!string.IsNullOrEmpty(assessment.OperationalImpact))
                priorityScore += 20;

            return priorityScore;
        }
    }
}
