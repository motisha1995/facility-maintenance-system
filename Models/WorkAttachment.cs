using System;

namespace FacilityMaintenanceSystem.Models
{
    /// <summary>
    /// WorkAttachment model for post-maintenance documentation
    /// </summary>
    public class WorkAttachment
    {
        public int WorkAttachmentId { get; set; }
        public int WorkId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public int UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }

        // Navigation Properties
        public virtual MaintenanceWork MaintenanceWork { get; set; }
        public virtual User UploadedByUser { get; set; }
    }
}
