using System;
using Xunit;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Tests.Helpers
{
    /// <summary>
    /// Unit tests for ValidationHelper
    /// </summary>
    public class ValidationHelperTests
    {
        private ValidationHelper _validator;

        public ValidationHelperTests()
        {
            _validator = new ValidationHelper();
        }

        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("invalid.email", false)]
        [InlineData("", false)]
        [InlineData("user@domain.co.uk", true)]
        public void ValidateEmail_WithVariousInputs_ReturnsExpectedResult(string email, bool expected)
        {
            // Act
            var result = _validator.ValidateEmail(email);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123-456-7890", true)]
        [InlineData("(123) 456-7890", true)]
        [InlineData("+1-123-456-7890", true)]
        [InlineData("123", false)]
        [InlineData("", false)]
        public void ValidatePhoneNumber_WithVariousInputs_ReturnsExpectedResult(string phone, bool expected)
        {
            // Act
            var result = _validator.ValidatePhoneNumber(phone);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("jpg", true)]
        [InlineData("pdf", true)]
        [InlineData("exe", false)]
        [InlineData("zip", false)]
        public void ValidateFileType_WithVariousInputs_ReturnsExpectedResult(string fileType, bool expected)
        {
            // Act
            var result = _validator.ValidateFileType(fileType);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Critical", true)]
        [InlineData("High", true)]
        [InlineData("Normal", true)]
        [InlineData("Low", true)]
        [InlineData("Urgent", false)]
        [InlineData("", false)]
        public void ValidateUrgencyLevel_WithVariousInputs_ReturnsExpectedResult(string urgency, bool expected)
        {
            // Act
            var result = _validator.ValidateUrgencyLevel(urgency);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Critical", true)]
        [InlineData("High", true)]
        [InlineData("Medium", true)]
        [InlineData("Low", true)]
        [InlineData("Urgent", false)]
        public void ValidatePriorityLevel_WithVariousInputs_ReturnsExpectedResult(string priority, bool expected)
        {
            // Act
            var result = _validator.ValidatePriorityLevel(priority);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(5, true)]
        [InlineData(3, true)]
        [InlineData(0, false)]
        [InlineData(6, false)]
        public void ValidateRating_WithVariousInputs_ReturnsExpectedResult(int rating, bool expected)
        {
            // Act
            var result = _validator.ValidateRating(rating);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetValidationMessage_ReturnsCorrectMessage()
        {
            // Act
            var message = _validator.GetValidationMessage("Email");

            // Assert
            Assert.NotEmpty(message);
            Assert.Contains("email", message.ToLower());
        }

        [Fact]
        public void GetValidationMessage_WithInvalidField_ReturnsGenericMessage()
        {
            // Act
            var message = _validator.GetValidationMessage("UnknownField");

            // Assert
            Assert.Equal("Invalid input", message);
        }
    }
}
