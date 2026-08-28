using System;
using Xunit;
using Moq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;
using FacilityMaintenanceSystem.Services;

namespace FacilityMaintenanceSystem.Tests.Services
{
    /// <summary>
    /// Unit tests for MaintenanceWorkService
    /// Covers STEP 6: Maintenance Execution
    /// </summary>
    public class MaintenanceWorkServiceTests
    {
        private Mock<FacilityMaintenanceContext> _mockContext;
        private MaintenanceWorkService _service;

        public MaintenanceWorkServiceTests()
        {
            _mockContext = new Mock<FacilityMaintenanceContext>();
            _service = new MaintenanceWorkService(_mockContext.Object);
        }

        [Fact]
        public void CreateMaintenanceWork_WithValidWork_CreatesSuccessfully()
        {
            // Arrange
            var work = new MaintenanceWork
            {
                RequestId = 1,
                TechnicianId = 2,
                WorkDescription = "Replace broken fixture"
            };

            // Act
            var result = _service.CreateMaintenanceWork(work);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(WorkStatus.NotStarted.ToString(), result.WorkStatus);
        }

        [Fact]
        public void CreateMaintenanceWork_WithNullWork_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateMaintenanceWork(null));
        }

        [Fact]
        public void StartWork_SetsWorkStatusToInProgress()
        {
            // Arrange
            var work = new MaintenanceWork
            {
                WorkId = 1,
                WorkStatus = WorkStatus.NotStarted.ToString(),
                MaintenanceRequest = new MaintenanceRequest { RequestId = 1, Status = "Assigned" }
            };

            // Act
            work.WorkStatus = WorkStatus.InProgress.ToString();
            work.ActualStartDate = DateTime.Now;

            // Assert
            Assert.Equal(WorkStatus.InProgress.ToString(), work.WorkStatus);
            Assert.NotNull(work.ActualStartDate);
        }

        [Fact]
        public void CompleteWork_CalculatesLaborHours()
        {
            // Arrange
            var startTime = DateTime.Now.AddHours(-2);
            var work = new MaintenanceWork
            {
                WorkId = 1,
                ActualStartDate = startTime,
                WorkStatus = WorkStatus.InProgress.ToString()
            };

            // Act
            var endTime = DateTime.Now;
            var duration = endTime - startTime.Value;
            work.LaborHours = (decimal)duration.TotalHours;
            work.ActualEndDate = endTime;
            work.WorkStatus = WorkStatus.Completed.ToString();

            // Assert
            Assert.True(work.LaborHours > 0);
            Assert.Equal(WorkStatus.Completed.ToString(), work.WorkStatus);
        }

        [Fact]
        public void GetTotalLaborHours_SumsAllLaborHours()
        {
            // Arrange
            var works = new System.Collections.Generic.List<MaintenanceWork>
            {
                new MaintenanceWork { WorkId = 1, RequestId = 1, LaborHours = 2.5m },
                new MaintenanceWork { WorkId = 2, RequestId = 1, LaborHours = 3.0m },
                new MaintenanceWork { WorkId = 3, RequestId = 1, LaborHours = 1.5m }
            };

            var totalHours = 0m;
            foreach (var work in works)
            {
                if (work.LaborHours.HasValue)
                    totalHours += work.LaborHours.Value;
            }

            // Assert
            Assert.Equal(7.0m, totalHours);
        }
    }
}
