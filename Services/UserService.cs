using System;
using System.Collections.Generic;
using System.Linq;
using FacilityMaintenanceSystem.Data;
using FacilityMaintenanceSystem.Models;

namespace FacilityMaintenanceSystem.Services
{
    /// <summary>
    /// Service implementing User business logic
    /// Manages system users, authentication, and authorization
    /// </summary>
    public class UserService : IUserService
    {
        private FacilityMaintenanceContext _context;

        public UserService(FacilityMaintenanceContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        public User CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Check if username already exists
            if (_context.Users.Any(u => u.Username == user.Username))
                throw new InvalidOperationException("Username already exists");

            user.IsActive = true;
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        /// <summary>
        /// Get users by role
        /// </summary>
        public List<User> GetUsersByRole(string role)
        {
            if (string.IsNullOrEmpty(role))
                return new List<User>();

            return _context.Users
                .Where(u => u.Role == role)
                .ToList();
        }

        /// <summary>
        /// Get all active users
        /// </summary>
        public List<User> GetActiveUsers()
        {
            return _context.Users
                .Where(u => u.IsActive)
                .ToList();
        }

        /// <summary>
        /// Get technicians by role (Technician or Contractor)
        /// </summary>
        public List<User> GetTechniciansByRole(string role)
        {
            return _context.Users
                .Where(u => u.Role == role && u.IsActive)
                .ToList();
        }

        /// <summary>
        /// Update user
        /// </summary>
        public void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.UpdatedAt = DateTime.Now;
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        /// <summary>
        /// Delete user
        /// </summary>
        public void DeleteUser(int userId)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Activate user account
        /// </summary>
        public void ActivateUser(int userId)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                user.IsActive = true;
                UpdateUser(user);
            }
        }

        /// <summary>
        /// Deactivate user account
        /// </summary>
        public void DeactivateUser(int userId)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                user.IsActive = false;
                UpdateUser(user);
            }
        }

        /// <summary>
        /// Validate user credentials
        /// Note: In production, use proper password hashing (BCrypt, PBKDF2, etc.)
        /// </summary>
        public bool ValidateUserCredentials(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            var user = GetUserByUsername(username);
            if (user == null || !user.IsActive)
                return false;

            // TODO: Implement proper password hash validation
            // This is a placeholder - implement actual password hashing in production
            return true;
        }
    }
}
