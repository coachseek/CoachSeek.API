﻿using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.DataAccess.Conversion
{
    public static class DbBusinessConverter
    {
        public static DbBusiness Convert(Business business)
        {
            return new DbBusiness
            {
                Id = business.Id,
                Name = business.Name,
                Domain = business.Domain,
                Admin = Convert(business.Id, business.Admin),
                Locations = Convert(business.Id, business.Locations),
                Coaches = Convert(business.Id, business.Coaches)
            };
        }

        private static DbBusinessAdmin Convert(Guid businessId, BusinessAdminData adminData)
        {
            return new DbBusinessAdmin
            {
                Id = adminData.Id,
                BusinessId = businessId,
                FirstName = adminData.FirstName,
                LastName = adminData.LastName,
                Email = adminData.Email,
                Username = adminData.Username,
                PasswordHash = adminData.PasswordHash
            };
        }

        private static List<DbLocation> Convert(Guid businessId, IEnumerable<Location> locations)
        {
            return locations.Select(location => new DbLocation
            {
                Id = location.Id, 
                BusinessId = businessId, 
                Name = location.Name
            }).ToList();
        }

        private static List<DbCoach> Convert(Guid businessId, IEnumerable<Coach> coaches)
        {
            return coaches.Select(coach => new DbCoach
            {
                Id = coach.Id,
                BusinessId = businessId,
                FirstName = coach.FirstName,
                LastName = coach.LastName,
                Email = coach.Email,
                Phone = coach.Phone
            }).ToList();
        }
    }
}