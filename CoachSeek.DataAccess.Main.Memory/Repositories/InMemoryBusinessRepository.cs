﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Conversion;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.DataAccess.Main.Memory.Repositories
{
    public class InMemoryBusinessRepository : IBusinessRepository
    {
        // Spy behaviour is included
        public bool WasSaveNewBusinessCalled;
        public bool WasSaveBusinessCalled; 

        public static List<DbBusiness> Businesses { get; private set; }


        static InMemoryBusinessRepository()
        {
            Businesses = new List<DbBusiness>();
        }

        public void Clear()
        {
            Businesses.Clear();
        }

        public Domain.Entities.Business Save(NewBusiness newBusiness)
        {
            WasSaveNewBusinessCalled = true;

            var dbBusiness = DbBusinessConverter.Convert(newBusiness);

            Businesses.Add(dbBusiness);
            return newBusiness;
        }

        public Domain.Entities.Business Save(Domain.Entities.Business business)
        {
            WasSaveBusinessCalled = true;

            var dbBusiness = DbBusinessConverter.Convert(business);
            var existingBusiness = Businesses.Single(x => x.Id == dbBusiness.Id);
            var existingIndex = Businesses.IndexOf(existingBusiness);
            Businesses[existingIndex] = dbBusiness;
            var updateBusiness = Businesses.Single(x => x.Id == dbBusiness.Id);
            return CreateBusiness(updateBusiness);
        }

        public Domain.Entities.Business Get(Guid id)
        {
            var dbBusiness = Businesses.FirstOrDefault(x => x.Id == id);
            return CreateBusiness(dbBusiness);
        }

        public Domain.Entities.Business GetByDomain(string domain)
        {
            var dbBusiness = Businesses.FirstOrDefault(x => x.Domain == domain);
            return CreateBusiness(dbBusiness);
        }


        private Domain.Entities.Business CreateBusiness(DbBusiness dbBusiness)
        {
            if (dbBusiness == null)
                return null;

            var locations = Mapper.Map<IEnumerable<DbLocation>, IEnumerable<LocationData>>(dbBusiness.Locations);
            var coaches = Mapper.Map<IEnumerable<DbCoach>, IEnumerable<CoachData>>(dbBusiness.Coaches);
            var services = Mapper.Map<IEnumerable<DbService>, IEnumerable<ServiceData>>(dbBusiness.Services);
            var sessions = Mapper.Map<IEnumerable<DbSingleSession>, IEnumerable<SingleSessionData>>(dbBusiness.Sessions);
            var courses = Mapper.Map<IEnumerable<DbRepeatedSession>, IEnumerable<RepeatedSessionData>>(dbBusiness.Courses);
            var customers = Mapper.Map<IEnumerable<DbCustomer>, IEnumerable<CustomerData>>(dbBusiness.Customers);

            return new Domain.Entities.Business(dbBusiness.Id,
                dbBusiness.Name,
                dbBusiness.Domain,
                locations,
                coaches,
                services,
                sessions,
                courses,
                customers);
        }


        private static BusinessAdminData CreateBusinessAdmin(DbBusinessAdmin dbAdmin)
        {
            return new BusinessAdmin(dbAdmin.Id, dbAdmin.Email,
                                     dbAdmin.FirstName, dbAdmin.LastName,
                                     dbAdmin.Username, dbAdmin.PasswordHash).ToData();
        }

        private static IEnumerable<LocationData> CreateLocations(IEnumerable<DbLocation> dbLocations)
        {
            var locations = new List<LocationData>();

            foreach (var dbLocation in dbLocations)
            {
                var location = new LocationData
                {
                    Id = dbLocation.Id,
                    Name = dbLocation.Name
                };

                locations.Add(location);
            }

            return locations;
        }

        private static IEnumerable<CoachData> CreateCoaches(IEnumerable<DbCoach> dbCoaches)
        {
            var coaches = new List<CoachData>();

            foreach (var dbCoach in dbCoaches)
            {
                var coach = new CoachData
                {
                    Id = dbCoach.Id,
                    FirstName = dbCoach.FirstName,
                    LastName = dbCoach.LastName,
                    Email = dbCoach.Email,
                    Phone = dbCoach.Phone
                };

                coaches.Add(coach);
            }

            return coaches;
        }


        // Only used for tests to add a business while bypassing the validation that occurs using Save.
        public Domain.Entities.Business Add(Domain.Entities.Business business)
        {
            var dbBusiness = DbBusinessConverter.Convert(business);

            Businesses.Add(dbBusiness);
            return business;
        }
    }
}