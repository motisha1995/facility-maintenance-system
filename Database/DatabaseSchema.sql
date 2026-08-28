-- Facility Maintenance System Database Schema
-- This script creates all necessary tables for the 9-step SOP workflow

-- =============================================
-- Create Database
-- =============================================
USE master;
GO

IF DB_ID('FacilityMaintenanceDB') IS NOT NULL
    DROP DATABASE [FacilityMaintenanceDB];
GO

CREATE DATABASE [FacilityMaintenanceDB];
GO

USE [FacilityMaintenanceDB];
GO

-- =============================================
-- 1. Users Table (Authentication & Authorization)
-- =============================================
CREATE TABLE [dbo].[Users] (
    [UserId] INT PRIMARY KEY IDENTITY(1,1),
    [Username] NVARCHAR(100) NOT NULL UNIQUE,
    [Email] NVARCHAR(100) NOT NULL UNIQUE,
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [Department] NVARCHAR(100) NULL,
    [Role] NVARCHAR(50) NOT NULL, -- Employee, Manager, Admin, Technician, Facilities Manager
    [IsActive] BIT DEFAULT 1,
    [PhoneNumber] NVARCHAR(20) NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [UpdatedAt] DATETIME DEFAULT GETDATE()
);

-- =============================================
-- 2. Building/Location Reference Table
-- =============================================
CREATE TABLE [dbo].[Locations] (
    [LocationId] INT PRIMARY KEY IDENTITY(1,1),
    [BuildingName] NVARCHAR(100) NOT NULL,
    [Floor] INT NOT NULL,
    [RoomNumber] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(200) NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE()
);

-- =============================================
-- 3. Issue Types Reference Table
-- =============================================
CREATE TABLE [dbo].[IssueTypes] (
    [IssueTypeId] INT PRIMARY KEY IDENTITY(1,1),
    [TypeName] NVARCHAR(50) NOT NULL UNIQUE, -- Electrical, Plumbing, Furniture, HVAC, etc.
    [Description] NVARCHAR(200) NULL,
    [IsActive] BIT DEFAULT 1,
    [CreatedAt] DATETIME DEFAULT GETDATE()
);

-- =============================================
-- 4. STEP 1 & 3: MaintenanceRequests Table (Request Initiation & Logging)
-- =============================================
CREATE TABLE [dbo].[MaintenanceRequests] (
    [RequestId] INT PRIMARY KEY IDENTITY(1,1),
    [TrackingId] NVARCHAR(50) NOT NULL UNIQUE, -- Auto-generated format: MR-YYYY-XXXXX
    [EmployeeId] INT NOT NULL,
    [LocationId] INT NOT NULL,
    [IssueTypeId] INT NOT NULL,
    [Title] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
    [Urgency] NVARCHAR(20) DEFAULT 'Normal', -- Critical, High, Normal, Low
    [Status] NVARCHAR(50) DEFAULT 'Initiated', -- Initiated, Approved, Assigned, InProgress, Completed, Closed
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [UpdatedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Users]([UserId]),
    FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations]([LocationId]),
    FOREIGN KEY ([IssueTypeId]) REFERENCES [dbo].[IssueTypes]([IssueTypeId])
);

-- =============================================
-- 5. Request Attachments (Photos & Documentation)
-- =============================================
CREATE TABLE [dbo].[RequestAttachments] (
    [AttachmentId] INT PRIMARY KEY IDENTITY(1,1),
    [RequestId] INT NOT NULL,
    [FileName] NVARCHAR(255) NOT NULL,
    [FilePath] NVARCHAR(MAX) NOT NULL,
    [FileType] NVARCHAR(50), -- jpg, png, pdf, etc.
    [UploadedBy] INT NOT NULL,
    [UploadedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([RequestId]) REFERENCES [dbo].[MaintenanceRequests]([RequestId]) ON DELETE CASCADE,
    FOREIGN KEY ([UploadedBy]) REFERENCES [dbo].[Users]([UserId])
);

-- =============================================
-- 6. STEP 2: RequestApprovals Table (Internal Review & Approval)
-- =============================================
CREATE TABLE [dbo].[RequestApprovals] (
    [ApprovalId] INT PRIMARY KEY IDENTITY(1,1),
    [RequestId] INT NOT NULL,
    [ApproverId] INT NOT NULL, -- Department Manager
    [Status] NVARCHAR(20) DEFAULT 'Pending', -- Pending, Approved, Rejected
    [Comments] NVARCHAR(MAX) NULL,
    [ApprovedAt] DATETIME NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([RequestId]) REFERENCES [dbo].[MaintenanceRequests]([RequestId]),
    FOREIGN KEY ([ApproverId]) REFERENCES [dbo].[Users]([UserId])
);

-- =============================================
-- 7. STEP 4: RequestAssessment Table (Initial Assessment & Prioritization)
-- =============================================
CREATE TABLE [dbo].[RequestAssessments] (
    [AssessmentId] INT PRIMARY KEY IDENTITY(1,1),
    [RequestId] INT NOT NULL,
    [FacilitiesManagerId] INT NOT NULL,
    [Priority] NVARCHAR(20) DEFAULT 'Medium', -- Critical, High, Medium, Low
    [SafetyRisk] BIT DEFAULT 0,
    [OperationalImpact] NVARCHAR(200) NULL,
    [EstimatedDuration] INT NULL, -- in hours
    [AssessmentNotes] NVARCHAR(MAX) NULL,
    [AssessedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([RequestId]) REFERENCES [dbo].[MaintenanceRequests]([RequestId]),
    FOREIGN KEY ([FacilitiesManagerId]) REFERENCES [dbo].[Users]([UserId])
);

-- =============================================
-- 8. STEP 5: RequestAssignments Table (Assignment & Scheduling)
-- =============================================
CREATE TABLE [dbo].[RequestAssignments] (
    [AssignmentId] INT PRIMARY KEY IDENTITY(1,1),
    [RequestId] INT NOT NULL,
    [AssignedTo] INT NOT NULL, -- Technician or Contractor ID
    [FacilitiesCoordinatorId] INT NOT NULL,
    [ScheduledStartDate] DATETIME NOT NULL,
    [ScheduledEndDate] DATETIME NOT NULL,
    [AssignmentStatus] NVARCHAR(50) DEFAULT 'Scheduled', -- Scheduled, InProgress, Completed
    [IsExternalVendor] BIT DEFAULT 0,
    [VendorName] NVARCHAR(100) NULL,
    [VendorContactInfo] NVARCHAR(200) NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([RequestId]) REFERENCES [dbo].[MaintenanceRequests]([RequestId]),
    FOREIGN KEY ([AssignedTo]) REFERENCES [dbo].[Users]([UserId]),
    FOREIGN KEY ([FacilitiesCoordinatorId]) REFERENCES [dbo].[Users]([UserId])
);

-- =============================================
-- 9. STEP 6: MaintenanceWork Table (Maintenance Execution)
-- =============================================
CREATE TABLE [dbo].[MaintenanceWork] (
    [WorkId] INT PRIMARY KEY IDENTITY(1,1),
    [RequestId] INT NOT NULL,
    [TechnicianId] INT NOT NULL,
    [ActualStartDate] DATETIME NULL,
    [ActualEndDate] DATETIME NULL,
    [WorkDescription] NVARCHAR(MAX) NULL,
    [PartsUsed] NVARCHAR(MAX) NULL,
    [LaborHours] DECIMAL(5,2) NULL,
    [WorkStatus] NVARCHAR(50) DEFAULT 'NotStarted', -- NotStarted, InProgress, Completed
    [Issues] NVARCHAR(MAX) NULL,
    [Notes] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    [UpdatedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([RequestId]) REFERENCES [dbo].[MaintenanceRequests]([RequestId]),
    FOREIGN KEY ([TechnicianId]) REFERENCES [dbo].[Users]([UserId])
);

-- =============================================
-- 10. WorkAttachments (Post-Maintenance Documentation)
-- =============================================
CREATE TABLE [dbo].[WorkAttachments] (
    [WorkAttachmentId] INT PRIMARY KEY IDENTITY(1,1),
    [WorkId] INT NOT NULL,
    [FileName] NVARCHAR(255) NOT NULL,
    [FilePath] NVARCHAR(MAX) NOT NULL,
    [FileType] NVARCHAR(50),
    [UploadedBy] INT NOT NULL,
    [UploadedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([WorkId]) REFERENCES [dbo].[MaintenanceWork]([WorkId]) ON DELETE CASCADE,
    FOREIGN KEY ([UploadedBy]) REFERENCES [dbo].[Users]([UserId])
);

-- =============================================
-- 11. STEP 7: CompletionVerification Table (Completion Verification)
-- =============================================
CREATE TABLE [dbo].[CompletionVerifications] (
    [VerificationId] INT PRIMARY KEY IDENTITY(1,1),
    [RequestId] INT NOT NULL,
    [VerifiedBy] INT NOT NULL, -- Facilities Manager
    [InspectionDate] DATETIME NOT NULL,
    [IsVerified] BIT DEFAULT 0,
    [VerificationNotes] NVARCHAR(MAX) NULL,
    [Issues] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([RequestId]) REFERENCES [dbo].[MaintenanceRequests]([RequestId]),
    FOREIGN KEY ([VerifiedBy]) REFERENCES [dbo].[Users]([UserId])
);

-- =============================================
-- 12. STEP 8: RequestFeedback Table (Closure & Feedback)
-- =============================================
CREATE TABLE [dbo].[RequestFeedback] (
    [FeedbackId] INT PRIMARY KEY IDENTITY(1,1),
    [RequestId] INT NOT NULL,
    [SubmittedBy] INT NOT NULL, -- Original requester
    [SatisfactionRating] INT NOT NULL, -- 1-5 scale
    [Comments] NVARCHAR(MAX) NULL,
    [WorkQuality] INT NULL, -- 1-5
    [Timeliness] INT NULL, -- 1-5
    [Professionalism] INT NULL, -- 1-5
    [SubmittedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([RequestId]) REFERENCES [dbo].[MaintenanceRequests]([RequestId]),
    FOREIGN KEY ([SubmittedBy]) REFERENCES [dbo].[Users]([UserId])
);

-- =============================================
-- 13. STEP 9: MaintenanceReports & Analytics
-- =============================================
CREATE TABLE [dbo].[MaintenanceReports] (
    [ReportId] INT PRIMARY KEY IDENTITY(1,1),
    [ReportName] NVARCHAR(200) NOT NULL,
    [ReportType] NVARCHAR(50), -- Monthly, Quarterly, Yearly, Custom
    [GeneratedBy] INT NOT NULL,
    [TotalRequests] INT,
    [CompletedRequests] INT,
    [AverageResolutionTime] DECIMAL(10,2), -- in hours
    [AverageSatisfactionRating] DECIMAL(3,2),
    [RecurringIssues] NVARCHAR(MAX),
    [RecommendedActions] NVARCHAR(MAX),
    [GeneratedAt] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([GeneratedBy]) REFERENCES [dbo].[Users]([UserId])
);

-- =============================================
-- 14. AuditLog (System Audit Trail)
-- =============================================
CREATE TABLE [dbo].[AuditLog] (
    [AuditId] INT PRIMARY KEY IDENTITY(1,1),
    [UserId] INT NOT NULL,
    [Action] NVARCHAR(200) NOT NULL,
    [EntityType] NVARCHAR(50),
    [EntityId] INT,
    [OldValue] NVARCHAR(MAX) NULL,
    [NewValue] NVARCHAR(MAX) NULL,
    [Timestamp] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId])
);

-- =============================================
-- Create Indexes for Performance
-- =============================================
CREATE INDEX IX_MaintenanceRequests_Status ON [dbo].[MaintenanceRequests]([Status]);
CREATE INDEX IX_MaintenanceRequests_EmployeeId ON [dbo].[MaintenanceRequests]([EmployeeId]);
CREATE INDEX IX_MaintenanceRequests_LocationId ON [dbo].[MaintenanceRequests]([LocationId]);
CREATE INDEX IX_MaintenanceRequests_CreatedAt ON [dbo].[MaintenanceRequests]([CreatedAt]);
CREATE INDEX IX_RequestApprovals_RequestId ON [dbo].[RequestApprovals]([RequestId]);
CREATE INDEX IX_RequestAssignments_RequestId ON [dbo].[RequestAssignments]([RequestId]);
CREATE INDEX IX_MaintenanceWork_RequestId ON [dbo].[MaintenanceWork]([RequestId]);
CREATE INDEX IX_RequestFeedback_RequestId ON [dbo].[RequestFeedback]([RequestId]);

-- =============================================
-- Insert Default Data
-- =============================================

-- Issue Types
INSERT INTO [dbo].[IssueTypes] ([TypeName], [Description]) VALUES
('Electrical', 'Electrical system issues'),
('Plumbing', 'Water and plumbing issues'),
('HVAC', 'Heating, ventilation, air conditioning'),
('Furniture', 'Furniture damage or maintenance'),
('Painting', 'Wall painting and touch-ups'),
('Cleaning', 'Deep cleaning and sanitation'),
('Door Lock', 'Door locks and access systems'),
('IT Infrastructure', 'Network, servers, infrastructure');

-- Sample Locations
INSERT INTO [dbo].[Locations] ([BuildingName], [Floor], [RoomNumber], [Description]) VALUES
('Building A', 1, '101', 'Main Conference Room'),
('Building A', 1, '102', 'Reception Area'),
('Building A', 2, '201', 'Accounting Department'),
('Building A', 2, '202', 'HR Department'),
('Building B', 1, '101', 'IT Server Room'),
('Building B', 1, '102', 'Cafeteria');

PRINT 'Database schema created successfully!';
