using System;
using System.Collections.Generic;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Interface for Attachment business logic
    /// Manages file attachments for requests and work
    /// </summary>
    public interface IAttachmentService
    {
        // Request Attachments
        RequestAttachment UploadRequestAttachment(int requestId, string fileName, string filePath, string fileType, int uploadedBy);
        RequestAttachment GetRequestAttachmentById(int attachmentId);
        List<RequestAttachment> GetRequestAttachments(int requestId);
        void DeleteRequestAttachment(int attachmentId);

        // Work Attachments
        WorkAttachment UploadWorkAttachment(int workId, string fileName, string filePath, string fileType, int uploadedBy);
        WorkAttachment GetWorkAttachmentById(int attachmentId);
        List<WorkAttachment> GetWorkAttachments(int workId);
        void DeleteWorkAttachment(int attachmentId);

        // General
        bool IsValidFileType(string fileType);
        long GetFileSizeLimit();
    }
}
