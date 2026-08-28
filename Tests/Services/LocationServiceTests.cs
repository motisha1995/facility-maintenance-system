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
    /// Unit tests for LocationService
    /// </summary>
    public class LocationServiceTests
    {
        private Mock<FacilityMaintenanceContext> _mockContext;
        private LocationService _service;

        public LocationServiceTests()
        {
            _mockContext = new Mock<FacilityMaintenanceContext>();
            _service = new LocationService(_mockContext.Object);
        }

        [Fact]
        public void CreateLocation_WithValidLocation_CreatesSuccessfully()
        {
            // Arrange
            var location = new Location
            {
                BuildingName = "Building A",
                Floor = 1,
                RoomNumber = "101"
            };

            // Act
            var result = _service.CreateLocation(location);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Building A", result.BuildingName);
        }

        [Fact]
        public void CreateLocation_WithNullLocation_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.CreateLocation(null));
        }

        [Fact]
        public void GetLocationsByBuilding_ReturnsLocationsForBuilding()
        {
            // Arrange
            var locations = new List<Location>
            {
                new Location { LocationId = 1, BuildingName = "Building A", Floor = 1 },
                new Location { LocationId = 2, BuildingName = "Building A", Floor = 2 },
                new Location { LocationId = 3, BuildingName = "Building B", Floor = 1 }
            }.AsQueryable();

            _mockContext.Setup(c => c.Locations).Returns(
                new Mock<DbSet<Location>>()
                    .SetupData(locations, o => o.OrderBy(l => l.LocationId))
                    .Object
            );

            // Act
            var result = _service.GetLocationsByBuilding("Building A");

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, l => Assert.Equal("Building A", l.BuildingName));
        }

        [Fact]
        public void GetLocationsByFloor_ReturnsLocationsForFloor()
        {
            // Arrange
            var locations = new List<Location>
            {
                new Location { LocationId = 1, BuildingName = "Building A", Floor = 1 },
                new Location { LocationId = 2, BuildingName = "Building B", Floor = 1 },
                new Location { LocationId = 3, BuildingName = "Building A", Floor = 2 }
            }.AsQueryable();

            _mockContext.Setup(c => c.Locations).Returns(
                new Mock<DbSet<Location>>()
                    .SetupData(locations, o => o.OrderBy(l => l.LocationId))
                    .Object
            );

            // Act
            var result = _service.GetLocationsByFloor(1);

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, l => Assert.Equal(1, l.Floor));
        }
    }
}
