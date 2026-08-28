using System.Data.Entity;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Data
{
    /// <summary>
    /// Entity Framework DbContext for Facility Maintenance System
    /// Manages all database operations and relationships
    /// </summary>
    public class FacilityMaintenanceContext : DbContext
    {
        public FacilityMaintenanceContext() : base("name=FacilityMaintenanceDB")
        {
            Configuration.LazyLoadingEnabled = true;
        }

        // DbSet for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<IssueType> IssueTypes { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<RequestAttachment> RequestAttachments { get; set; }
        public DbSet<RequestApproval> RequestApprovals { get; set; }
        public DbSet<RequestAssessment> RequestAssessments { get; set; }
        public DbSet<RequestAssignment> RequestAssignments { get; set; }
        public DbSet<MaintenanceWork> MaintenanceWork { get; set; }
        public DbSet<WorkAttachment> WorkAttachments { get; set; }
        public DbSet<CompletionVerification> CompletionVerifications { get; set; }
        public DbSet<RequestFeedback> RequestFeedback { get; set; }
        public DbSet<MaintenanceReport> MaintenanceReports { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>()
                .HasMany(u => u.SubmittedRequests)
                .WithRequired(mr => mr.Employee)
                .HasForeignKey(mr => mr.EmployeeId)
                .WillCascadeOnDelete(false);

            // Configure MaintenanceRequest entity
            modelBuilder.Entity<MaintenanceRequest>()
                .HasMany(mr => mr.Attachments)
                .WithRequired(ra => ra.MaintenanceRequest)
                .HasForeignKey(ra => ra.RequestId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<MaintenanceRequest>()
                .HasMany(mr => mr.Approvals)
                .WithRequired(ra => ra.MaintenanceRequest)
                .HasForeignKey(ra => ra.RequestId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<MaintenanceRequest>()
                .HasMany(mr => mr.MaintenanceWork)
                .WithRequired(mw => mw.MaintenanceRequest)
                .HasForeignKey(mw => mw.RequestId)
                .WillCascadeOnDelete(true);

            // Configure indexes
            modelBuilder.Entity<MaintenanceRequest>()
                .Property(mr => mr.TrackingId)
                .IsRequired();

            // Set precision for decimal fields
            modelBuilder.Entity<MaintenanceWork>()
                .Property(mw => mw.LaborHours)
                .HasPrecision(5, 2);

            modelBuilder.Entity<RequestFeedback>()
                .Property(rf => rf.SatisfactionRating)
                .IsRequired();
        }
    }
}
