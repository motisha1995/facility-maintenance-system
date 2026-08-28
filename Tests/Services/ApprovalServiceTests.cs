using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Tests.Services
{
    /// <summary>
    /// Unit tests for ApprovalService
    /// Covers STEP 2: Internal Review and Approval
    /// </summary>
    public class ApprovalServiceTests
    {
        private Mock<FacilityMaintenanceContext> _mockContext;
        private ApprovalService _service;

        public ApprovalServiceTests()
        {
            _mockContext = new Mock<FacilityMaintenanceContext>();
            _service = new ApprovalService(_mockContext.Object);
        }

        [Fact]
        public void CreateApproval_WithValidApproval_CreatesSuccessfully()
        {
            // Arrange
            var approval = new RequestApproval
            {
                RequestId = 1,
                ApproverId = 2,
                Status = ApprovalStatus.Pending.ToString()
            };

            // Act
            var result = _service.CreateApproval(approval);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ApprovalStatus.Pending.ToString(), result.Status);
        }

        [Fact]
        public void CreateApproval_WithNullApproval_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateApproval(null));
        }

        [Fact]
        public void ApproveRequest_UpdatesStatusToApproved()
        {
            // Arrange
            var mockRequest = new MaintenanceRequest
            {
                RequestId = 1,
                Status = "Initiated"
            };

            var approval = new RequestApproval
            {
                ApprovalId = 1,
                RequestId = 1,
                Status = ApprovalStatus.Pending.ToString(),
                MaintenanceRequest = mockRequest
            };

            // Act
            _service.ApproveRequest(approval.ApprovalId, "Approved");

            // Assert - Approval status should change
            Assert.Equal(ApprovalStatus.Approved.ToString(), approval.Status);
        }

        [Fact]
        public void RejectRequest_UpdatesStatusToRejected()
        {
            // Arrange
            var mockRequest = new MaintenanceRequest
            {
                RequestId = 1,
                Status = "Initiated"
            };

            var approval = new RequestApproval
            {
                ApprovalId = 1,
                RequestId = 1,
                Status = ApprovalStatus.Pending.ToString(),
                MaintenanceRequest = mockRequest
            };

            // Act
            _service.RejectRequest(approval.ApprovalId, "Not approved");

            // Assert
            Assert.Equal(ApprovalStatus.Rejected.ToString(), approval.Status);
        }

        [Fact]
        public void IsRequestApproved_WithApprovedRequest_ReturnsTrue()
        {
            // Arrange
            var approvals = new List<RequestApproval>
            {
                new RequestApproval { RequestId = 1, Status = ApprovalStatus.Approved.ToString() }
            }.AsQueryable();

            _mockContext.Setup(c => c.RequestApprovals).Returns(
                new Mock<DbSet<RequestApproval>>()
                    .SetupData(approvals, o => o.OrderBy(a => a.ApprovalId))
                    .Object
            );

            // Act
            var result = _service.IsRequestApproved(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsRequestApproved_WithPendingRequest_ReturnsFalse()
        {
            // Arrange
            var approvals = new List<RequestApproval>
            {
                new RequestApproval { RequestId = 1, Status = ApprovalStatus.Pending.ToString() }
            }.AsQueryable();

            _mockContext.Setup(c => c.RequestApprovals).Returns(
                new Mock<DbSet<RequestApproval>>()
                    .SetupData(approvals, o => o.OrderBy(a => a.ApprovalId))
                    .Object
            );

            // Act
            var result = _service.IsRequestApproved(1);

            // Assert
            Assert.False(result);
        }
    }
}
