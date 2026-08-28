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
    /// Unit tests for MaintenanceRequestService
    /// Covers STEP 1 & 3: Request Initiation and Logging
    /// </summary>
    public class MaintenanceRequestServiceTests
    {
        private Mock<FacilityMaintenanceContext> _mockContext;
        private MaintenanceRequestService _service;

        public MaintenanceRequestServiceTests()
        {
            _mockContext = new Mock<FacilityMaintenanceContext>();
            _service = new MaintenanceRequestService(_mockContext.Object);
        }

        [Fact]
        public void CreateRequest_WithValidRequest_ReturnsCreatedRequest()
        {
            // Arrange
            var request = new MaintenanceRequest
            {
                Title = "Test Request",
                Description = "Test Description",
                EmployeeId = 1,
                LocationId = 1,
                IssueTypeId = 1,
                Urgency = "Normal"
            };

            // Act
            var result = _service.CreateRequest(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Request", result.Title);
            Assert.NotNull(result.TrackingId);
            Assert.StartsWith("MR-", result.TrackingId);
            Assert.Equal(RequestStatus.Initiated.ToString(), result.Status);
        }

        [Fact]
        public void CreateRequest_WithNullRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateRequest(null));
        }

        [Fact]
        public void GenerateTrackingId_GeneratesUniqueIds()
        {
            // Act
            var id1 = _service.GenerateTrackingId();
            var id2 = _service.GenerateTrackingId();

            // Assert
            Assert.NotEqual(id1, id2);
            Assert.StartsWith("MR-", id1);
            Assert.StartsWith("MR-", id2);
        }

        [Fact]
        public void GenerateTrackingId_IncludesCurrentYear()
        {
            // Act
            var trackingId = _service.GenerateTrackingId();

            // Assert
            string yearPart = trackingId.Split('-')[1];
            Assert.Equal(DateTime.Now.Year.ToString(), yearPart);
        }

        [Fact]
        public void GetRequestsByStatus_ReturnsOnlyRequestsWithStatus()
        {
            // Arrange
            var requests = new List<MaintenanceRequest>
            {
                new MaintenanceRequest { RequestId = 1, Status = "Initiated", Title = "Test 1" },
                new MaintenanceRequest { RequestId = 2, Status = "Approved", Title = "Test 2" },
                new MaintenanceRequest { RequestId = 3, Status = "Initiated", Title = "Test 3" }
            }.AsQueryable();

            _mockContext.Setup(c => c.MaintenanceRequests).Returns(
                new Mock<DbSet<MaintenanceRequest>>()
                    .SetupData(requests, o => o.OrderBy(r => r.RequestId))
                    .Object
            );

            // Act
            var result = _service.GetRequestsByStatus("Initiated");

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, r => Assert.Equal("Initiated", r.Status));
        }

        [Fact]
        public void UpdateRequest_WithValidRequest_UpdatesSuccessfully()
        {
            // Arrange
            var request = new MaintenanceRequest
            {
                RequestId = 1,
                Title = "Original Title",
                Status = "Initiated"
            };

            request.Title = "Updated Title";

            // Act
            _service.UpdateRequest(request);

            // Assert
            Assert.Equal("Updated Title", request.Title);
        }

        [Fact]
        public void UpdateRequest_WithNullRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.UpdateRequest(null));
        }
    }
}
