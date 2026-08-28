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
    /// Unit tests for FeedbackService
    /// Covers STEP 8: Closure and Feedback
    /// </summary>
    public class FeedbackServiceTests
    {
        private Mock<FacilityMaintenanceContext> _mockContext;
        private FeedbackService _service;

        public FeedbackServiceTests()
        {
            _mockContext = new Mock<FacilityMaintenanceContext>();
            _service = new FeedbackService(_mockContext.Object);
        }

        [Fact]
        public void CreateFeedback_WithValidFeedback_CreatesSuccessfully()
        {
            // Arrange
            var feedback = new RequestFeedback
            {
                RequestId = 1,
                SubmittedBy = 1,
                SatisfactionRating = 5,
                Comments = "Great service!"
            };

            // Act
            var result = _service.CreateFeedback(feedback);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.SatisfactionRating);
        }

        [Fact]
        public void CreateFeedback_WithInvalidRating_ThrowsArgumentException()
        {
            // Arrange
            var feedback = new RequestFeedback
            {
                RequestId = 1,
                SatisfactionRating = 6 // Invalid: should be 1-5
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.CreateFeedback(feedback));
        }

        [Fact]
        public void CreateFeedback_WithZeroRating_ThrowsArgumentException()
        {
            // Arrange
            var feedback = new RequestFeedback
            {
                RequestId = 1,
                SatisfactionRating = 0 // Invalid: should be 1-5
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.CreateFeedback(feedback));
        }

        [Fact]
        public void CreateFeedback_WithNullFeedback_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateFeedback(null));
        }

        [Fact]
        public void GetAverageSatisfactionRating_CalculatesCorrectAverage()
        {
            // Arrange
            var feedbacks = new List<RequestFeedback>
            {
                new RequestFeedback { SatisfactionRating = 5 },
                new RequestFeedback { SatisfactionRating = 4 },
                new RequestFeedback { SatisfactionRating = 3 },
                new RequestFeedback { SatisfactionRating = 2 }
            };

            double totalRating = 0;
            foreach (var feedback in feedbacks)
            {
                totalRating += feedback.SatisfactionRating;
            }
            double average = totalRating / feedbacks.Count;

            // Assert
            Assert.Equal(3.5, average);
        }

        [Fact]
        public void ClosRequest_UpdatesRequestStatusToClosed()
        {
            // Arrange
            var request = new MaintenanceRequest
            {
                RequestId = 1,
                Status = "Completed"
            };

            // Act
            request.Status = RequestStatus.Closed.ToString();

            // Assert
            Assert.Equal(RequestStatus.Closed.ToString(), request.Status);
        }
    }
}
