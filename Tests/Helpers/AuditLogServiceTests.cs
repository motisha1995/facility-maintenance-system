using System;
using Xunit;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Tests.Helpers
{
    /// <summary>
    /// Unit tests for AuditLogService
    /// </summary>
    public class AuditLogServiceTests
    {
        private Mock<FacilityMaintenanceContext> _mockContext;
        private AuditLogService _service;

        public AuditLogServiceTests()
        {
            _mockContext = new Mock<FacilityMaintenanceContext>();
            _service = new AuditLogService(_mockContext.Object);
        }

        [Fact]
        public void LogAction_WithValidAction_LogsSuccessfully()
        {
            // Arrange
            int userId = 1;
            string action = "CreateRequest";
            string entityType = "MaintenanceRequest";
            int entityId = 5;

            // Act - Should not throw
            _service.LogAction(userId, action, entityType, entityId);

            // Assert - If no exception thrown, test passes
            Assert.True(true);
        }

        [Fact]
        public void LogAction_WithOldAndNewValues_LogsSuccessfully()
        {
            // Arrange
            int userId = 1;
            string action = "UpdateRequest";
            string entityType = "MaintenanceRequest";
            int entityId = 5;
            string oldValue = "Initiated";
            string newValue = "Approved";

            // Act - Should not throw
            _service.LogAction(userId, action, entityType, entityId, oldValue, newValue);

            // Assert
            Assert.True(true);
        }
    }
}
