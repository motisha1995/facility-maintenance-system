using System.Collections.Generic;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Interface for Location business logic
    /// </summary>
    public interface ILocationService
    {
        Location CreateLocation(Location location);
        Location GetLocationById(int locationId);
        List<Location> GetAllLocations();
        List<Location> GetLocationsByBuilding(string buildingName);
        List<Location> GetLocationsByFloor(int floor);
        void UpdateLocation(Location location);
        void DeleteLocation(int locationId);
    }
}
