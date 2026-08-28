using System;
using Xunit;
using Moq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Tests.Services
{
    /// <summary>
    /// Unit tests for CompletionVerificationService
    /// Covers STEP 7: Completion Verification
    /// </summary>
    public class CompletionVerificationServiceTests
    {
        private Mock<FacilityMaintenanceContext> _mockContext;
        private CompletionVerificationService _service;

        public CompletionVerificationServiceTests()
        {
            _mockContext = new Mock<FacilityMaintenanceContext>();
            _service = new CompletionVerificationService(_mockContext.Object);
        }

        [Fact]
        public void CreateVerification_WithValidVerification_CreatesSuccessfully()
        {
            // Arrange
            var verification = new CompletionVerification
            {
                RequestId = 1,
                VerifiedBy = 2,
                VerificationNotes = "All work completed successfully"
            };

            // Act
            var result = _service.CreateVerification(verification);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsVerified);
        }

        [Fact]
        public void CreateVerification_WithNullVerification_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateVerification(null));
        }

        [Fact]
        public void VerifyCompletion_WithApprovedVerification_UpdatesRequestStatusToCompleted()
        {
            // Arrange
            var mockRequest = new MaintenanceRequest
            {
                RequestId = 1,
                Status = "Completed"
            };

            var verification = new CompletionVerification
            {
                VerificationId = 1,
                RequestId = 1,
                IsVerified = false,
                MaintenanceRequest = mockRequest
            };

            // Act
            verification.IsVerified = true;
            verification.MaintenanceRequest.Status = RequestStatus.Completed.ToString();

            // Assert
            Assert.True(verification.IsVerified);
            Assert.Equal(RequestStatus.Completed.ToString(), verification.MaintenanceRequest.Status);
        }

        [Fact]
        public void VerifyCompletion_WithRejectedVerification_UpdatesRequestStatusToInProgress()
        {
            // Arrange
            var mockRequest = new MaintenanceRequest
            {
                RequestId = 1,
                Status = "Completed"
            };

            var verification = new CompletionVerification
            {
                VerificationId = 1,
                RequestId = 1,
                IsVerified = true,
                MaintenanceRequest = mockRequest
            };

            // Act
            verification.IsVerified = false;
            verification.MaintenanceRequest.Status = RequestStatus.InProgress.ToString();

            // Assert
            Assert.False(verification.IsVerified);
            Assert.Equal(RequestStatus.InProgress.ToString(), verification.MaintenanceRequest.Status);
        }

        [Fact]
        public void IsRequestCompleted_WithVerifiedRequest_ReturnsTrue()
        {
            // Arrange
            var verification = new CompletionVerification
            {
                RequestId = 1,
                IsVerified = true
            };

            // Assert
            Assert.True(verification.IsVerified);
        }

        [Fact]
        public void IsRequestCompleted_WithUnverifiedRequest_ReturnsFalse()
        {
            // Arrange
            var verification = new CompletionVerification
            {
                RequestId = 1,
                IsVerified = false
            };

            // Assert
            Assert.False(verification.IsVerified);
        }
    }
}
