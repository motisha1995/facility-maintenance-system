using System;
using System.Collections.Generic;
using System.Linq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Service implementing Attachment business logic
    /// Manages file attachments for requests and maintenance work
    /// </summary>
    public class AttachmentService : IAttachmentService
    {
        private FacilityMaintenanceContext _context;
        private static readonly List<string> AllowedFileTypes = new List<string> 
        { 
            "jpg", "jpeg", "png", "gif", "pdf", "doc", "docx", "xls", "xlsx" 
        };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

        public AttachmentService(FacilityMaintenanceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Upload attachment for a maintenance request
        /// </summary>
        public RequestAttachment UploadRequestAttachment(int requestId, string fileName, string filePath, string fileType, int uploadedBy)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File name and path are required");

            if (!IsValidFileType(fileType))
                throw new InvalidOperationException($"File type '{fileType}' is not allowed");

            var attachment = new RequestAttachment
            {
                RequestId = requestId,
                FileName = fileName,
                FilePath = filePath,
                FileType = fileType,
                UploadedBy = uploadedBy,
                UploadedAt = DateTime.Now
            };

            _context.RequestAttachments.Add(attachment);
            _context.SaveChanges();

            return attachment;
        }

        /// <summary>
        /// Get request attachment by ID
        /// </summary>
        public RequestAttachment GetRequestAttachmentById(int attachmentId)
        {
            return _context.RequestAttachments.FirstOrDefault(ra => ra.AttachmentId == attachmentId);
        }

        /// <summary>
        /// Get all attachments for a request
        /// </summary>
        public List<RequestAttachment> GetRequestAttachments(int requestId)
        {
            return _context.RequestAttachments
                .Where(ra => ra.RequestId == requestId)
                .ToList();
        }

        /// <summary>
        /// Delete request attachment
        /// </summary>
        public void DeleteRequestAttachment(int attachmentId)
        {
            var attachment = GetRequestAttachmentById(attachmentId);
            if (attachment != null)
            {
                _context.RequestAttachments.Remove(attachment);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Upload attachment for maintenance work
        /// </summary>
        public WorkAttachment UploadWorkAttachment(int workId, string fileName, string filePath, string fileType, int uploadedBy)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File name and path are required");

            if (!IsValidFileType(fileType))
                throw new InvalidOperationException($"File type '{fileType}' is not allowed");

            var attachment = new WorkAttachment
            {
                WorkId = workId,
                FileName = fileName,
                FilePath = filePath,
                FileType = fileType,
                UploadedBy = uploadedBy,
                UploadedAt = DateTime.Now
            };

            _context.WorkAttachments.Add(attachment);
            _context.SaveChanges();

            return attachment;
        }

        /// <summary>
        /// Get work attachment by ID
        /// </summary>
        public WorkAttachment GetWorkAttachmentById(int attachmentId)
        {
            return _context.WorkAttachments.FirstOrDefault(wa => wa.WorkAttachmentId == attachmentId);
        }

        /// <summary>
        /// Get all attachments for a work record
        /// </summary>
        public List<WorkAttachment> GetWorkAttachments(int workId)
        {
            return _context.WorkAttachments
                .Where(wa => wa.WorkId == workId)
                .ToList();
        }

        /// <summary>
        /// Delete work attachment
        /// </summary>
        public void DeleteWorkAttachment(int attachmentId)
        {
            var attachment = GetWorkAttachmentById(attachmentId);
            if (attachment != null)
            {
                _context.WorkAttachments.Remove(attachment);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Validate file type
        /// </summary>
        public bool IsValidFileType(string fileType)
        {
            if (string.IsNullOrEmpty(fileType))
                return false;

            return AllowedFileTypes.Contains(fileType.ToLower());
        }

        /// <summary>
        /// Get maximum file size allowed
        /// </summary>
        public long GetFileSizeLimit()
        {
            return MaxFileSize;
        }
    }
}
