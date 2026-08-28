# Facility Maintenance Request System

A comprehensive end-to-end ASP.NET MVC web application for managing office facility maintenance requests following a structured 9-step SOP (Standard Operating Procedure).

## Overview

This system automates and tracks facility maintenance requests from initiation through closure, covering all 9 steps of the maintenance workflow:

1. Request Initiation
2. Internal Review and Approval
3. Logging and Categorization
4. Initial Assessment and Prioritization
5. Assignment and Scheduling
6. Maintenance Execution
7. Completion Verification
8. Closure and Feedback
9. Reporting and Continuous Improvement

## Features

- **User Roles:** Employee, Department Manager, Facilities Admin, Technician, Facilities Manager
- **Request Management:** Create, track, and manage maintenance requests
- **Workflow Status:** Automated workflow tracking (Initiated → Approved → Assigned → Completed → Closed)
- **Priority System:** Automatic prioritization based on safety, urgency, and operational impact
- **Notifications:** Real-time notifications for request status changes
- **Document Uploads:** Support for photos and maintenance documentation
- **Reporting:** Analytics and trend analysis for maintenance activities
- **SLA Tracking:** Monitor service level agreements

## Technology Stack

- **Framework:** ASP.NET MVC (.NET Framework)
- **Database:** SQL Server
- **ORM:** Entity Framework
- **Frontend:** HTML5, CSS3, Bootstrap, jQuery
- **Authentication:** ASP.NET Identity

## Project Structure

```
facility-maintenance-system/
├── Models/                    # Data models
├── Controllers/              # MVC Controllers
├── Views/                    # Views (CSHTML)
├── Database/                 # Database scripts and migrations
├── Services/                 # Business logic layer
├── Utilities/                # Helper classes
├── Scripts/                  # JavaScript files
├── Content/                  # CSS and styling
└── Web.config               # Configuration file
```

## Getting Started

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2+
- SQL Server 2016+

### Installation

1. Clone the repository
```bash
git clone https://github.com/motisha1995/facility-maintenance-system.git
```

2. Open the solution in Visual Studio

3. Update the database connection string in `Web.config`

4. Run database migrations
```bash
Update-Database
```

5. Build and run the application

## Database Schema

The system uses the following main entities:
- **Users** - System users with different roles
- **MaintenanceRequests** - Core request records
- **RequestApprovals** - Approval workflow
- **RequestAssignments** - Task assignments
- **MaintenanceWork** - Execution records
- **RequestFeedback** - User satisfaction feedback
- **Reports** - Maintenance analytics

## Workflow Steps

### Step 1: Request Initiation
- Employees submit maintenance requests with location, issue type, and description
- Attachments (photos) can be uploaded

### Step 2: Internal Review and Approval
- Department managers review requests
- Approve, reject, or request more information

### Step 3: Logging and Categorization
- System automatically assigns tracking ID
- Categorizes by type, urgency, and location

### Step 4: Initial Assessment and Prioritization
- Facilities manager conducts assessment
- Sets priority level (Critical, High, Medium, Low)

### Step 5: Assignment and Scheduling
- Assign to internal staff or external vendor
- Schedule maintenance work

### Step 6: Maintenance Execution
- Technician performs the work
- Documents activities and issues

### Step 7: Completion Verification
- Final inspection and verification
- Post-maintenance documentation

### Step 8: Closure and Feedback
- Request is closed
- Requester provides satisfaction feedback

### Step 9: Reporting and Continuous Improvement
- Analytics dashboard shows trends
- Recurring issues identified
- Preventive maintenance plans adjusted

## User Roles

- **Employee:** Submit requests, view own requests, provide feedback
- **Department Manager:** Approve/reject requests from their department
- **Facilities Admin:** Log requests, manage categorization
- **Technician:** Execute maintenance, document work
- **Facilities Manager:** Prioritize, assign tasks, view analytics

## API Endpoints (Future Enhancement)

The system can be extended with RESTful APIs for mobile access and third-party integrations.

## Contributing

1. Create a feature branch
2. Commit your changes
3. Push to the branch
4. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues or questions, please open an issue in the GitHub repository.

---

**Status:** Project Setup Complete ✅
**Last Updated:** 2026-08-28
