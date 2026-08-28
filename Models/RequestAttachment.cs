using System;

namespace FacilityMaintenanceSystem.Models
{
    /// <summary>
    /// RequestAttachment model for photos and documentation
    /// </summary>
    public class RequestAttachment
    {
        public int AttachmentId { get; set; }
        public int RequestId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; } // jpg, png, pdf, etc.
        public int UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }

        // Navigation Properties
        public virtual MaintenanceRequest MaintenanceRequest { get; set; }
        public virtual User UploadedByUser { get; set; }
    }
}
