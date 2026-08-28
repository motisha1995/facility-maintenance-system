using System;
using System.Collections.Generic;
using System.Linq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Service implementing IssueType business logic
    /// Manages maintenance issue categories
    /// </summary>
    public class IssueTypeService : IIssueTypeService
    {
        private FacilityMaintenanceContext _context;

        public IssueTypeService(FacilityMaintenanceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new issue type
        /// </summary>
        public IssueType CreateIssueType(IssueType issueType)
        {
            if (issueType == null)
                throw new ArgumentNullException(nameof(issueType));

            issueType.IsActive = true;
            issueType.CreatedAt = DateTime.Now;
            _context.IssueTypes.Add(issueType);
            _context.SaveChanges();

            return issueType;
        }

        /// <summary>
        /// Get issue type by ID
        /// </summary>
        public IssueType GetIssueTypeById(int issueTypeId)
        {
            return _context.IssueTypes.FirstOrDefault(it => it.IssueTypeId == issueTypeId);
        }

        /// <summary>
        /// Get all issue types
        /// </summary>
        public List<IssueType> GetAllIssueTypes()
        {
            return _context.IssueTypes.ToList();
        }

        /// <summary>
        /// Get all active issue types
        /// </summary>
        public List<IssueType> GetAllActiveIssueTypes()
        {
            return _context.IssueTypes
                .Where(it => it.IsActive)
                .ToList();
        }

        /// <summary>
        /// Get issue type by name
        /// </summary>
        public IssueType GetIssueTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            return _context.IssueTypes
                .FirstOrDefault(it => it.TypeName == typeName);
        }

        /// <summary>
        /// Update issue type
        /// </summary>
        public void UpdateIssueType(IssueType issueType)
        {
            if (issueType == null)
                throw new ArgumentNullException(nameof(issueType));

            _context.Entry(issueType).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Delete issue type
        /// </summary>
        public void DeleteIssueType(int issueTypeId)
        {
            var issueType = GetIssueTypeById(issueTypeId);
            if (issueType != null)
            {
                _context.IssueTypes.Remove(issueType);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Activate issue type (make available for new requests)
        /// </summary>
        public void ActivateIssueType(int issueTypeId)
        {
            var issueType = GetIssueTypeById(issueTypeId);
            if (issueType != null)
            {
                issueType.IsActive = true;
                UpdateIssueType(issueType);
            }
        }

        /// <summary>
        /// Deactivate issue type (prevent new requests)
        /// </summary>
        public void DeactivateIssueType(int issueTypeId)
        {
            var issueType = GetIssueTypeById(issueTypeId);
            if (issueType != null)
            {
                issueType.IsActive = false;
                UpdateIssueType(issueType);
            }
        }
    }
}
