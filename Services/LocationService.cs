using System;
using System.Collections.Generic;
using System.Linq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Service implementing Location business logic
    /// Manages building locations for maintenance requests
    /// </summary>
    public class LocationService : ILocationService
    {
        private FacilityMaintenanceContext _context;

        public LocationService(FacilityMaintenanceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new location
        /// </summary>
        public Location CreateLocation(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            location.CreatedAt = DateTime.Now;
            _context.Locations.Add(location);
            _context.SaveChanges();

            return location;
        }

        /// <summary>
        /// Get location by ID
        /// </summary>
        public Location GetLocationById(int locationId)
        {
            return _context.Locations.FirstOrDefault(l => l.LocationId == locationId);
        }

        /// <summary>
        /// Get all locations
        /// </summary>
        public List<Location> GetAllLocations()
        {
            return _context.Locations.ToList();
        }

        /// <summary>
        /// Get locations by building name
        /// </summary>
        public List<Location> GetLocationsByBuilding(string buildingName)
        {
            if (string.IsNullOrEmpty(buildingName))
                return new List<Location>();

            return _context.Locations
                .Where(l => l.BuildingName.Contains(buildingName))
                .ToList();
        }

        /// <summary>
        /// Get locations by floor number
        /// </summary>
        public List<Location> GetLocationsByFloor(int floor)
        {
            return _context.Locations
                .Where(l => l.Floor == floor)
                .ToList();
        }

        /// <summary>
        /// Update location
        /// </summary>
        public void UpdateLocation(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            _context.Entry(location).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Delete location
        /// </summary>
        public void DeleteLocation(int locationId)
        {
            var location = GetLocationById(locationId);
            if (location != null)
            {
                _context.Locations.Remove(location);
                _context.SaveChanges();
            }
        }
    }
}
