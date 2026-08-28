using System.Collections.Generic;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Interface for User business logic
    /// Manages system users and roles
    /// </summary>
    public interface IUserService
    {
        User CreateUser(User user);
        User GetUserById(int userId);
        User GetUserByUsername(string username);
        List<User> GetAllUsers();
        List<User> GetUsersByRole(string role);
        List<User> GetActiveUsers();
        List<User> GetTechniciansByRole(string role);
        void UpdateUser(User user);
        void DeleteUser(int userId);
        void ActivateUser(int userId);
        void DeactivateUser(int userId);
        bool ValidateUserCredentials(string username, string password);
    }
}
