﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbBusinessRepository : IBusinessRepository
    {
        private SqlConnection _connection;

        protected virtual string ConnectionStringKey { get { return "BusinessDatabase"; } } 

        private SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
                    _connection = new SqlConnection(connectionString);
                }

                return _connection;
            }
        }


        public BusinessData GetBusiness(Guid businessId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Business_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    return new BusinessData
                    {
                        Id = reader.GetGuid(1),
                        Name = reader.GetString(2),
                        Domain = reader.GetString(3)
                    };
                }

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public BusinessData AddBusiness(Business business)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("Business_Create", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 100, "Name"));
                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar, 100, "Domain"));

                command.Parameters[0].Value = business.Id;
                command.Parameters[1].Value = business.Name;
                command.Parameters[2].Value = business.Domain;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    return new BusinessData
                    {
                        Id = reader.GetGuid(1),
                        Name = reader.GetString(2),
                        Domain = reader.GetString(3)
                    };
                }

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public bool IsAvailableDomain(string domain)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Business_GetByDomain]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar, 100, "Domain"));
                command.Parameters[0].Value = domain;
                
                reader = command.ExecuteReader();

                return !reader.HasRows;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }


        public IList<LocationData> GetAllLocations(Guid businessId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Location_GetAll]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;

                reader = command.ExecuteReader();

                var locations = new List<LocationData>();

                while (reader.Read())
                    locations.Add(ReadLocationData(reader));

                return locations;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public LocationData GetLocation(Guid businessId, Guid locationId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Location_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@locationGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[1].Value = locationId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadLocationData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public LocationData AddLocation(Guid businessId, Location location)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("Location_Create", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@locationGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 100, "Name"));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = location.Id;
                command.Parameters[2].Value = location.Name;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadLocationData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public LocationData UpdateLocation(Guid businessId, Location location)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("Location_Update", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@locationGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 100, "Name"));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = location.Id;
                command.Parameters[2].Value = location.Name;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadLocationData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }


        public IList<CoachData> GetAllCoaches(Guid businessId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Coach_GetAll]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;

                reader = command.ExecuteReader();

                var coaches = new List<CoachData>();

                while (reader.Read())
                    coaches.Add(ReadCoachData(reader));

                return coaches;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public CoachData GetCoach(Guid businessId, Guid coachId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Coach_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@coachGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[1].Value = coachId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadCoachData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public CoachData AddCoach(Guid businessId, Coach coach)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("Coach_Create", Connection) {CommandType = CommandType.StoredProcedure};

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@coachGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar, 50, "FirstName"));
                command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar, 50, "LastName"));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 100, "Email"));
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar, 50, "Phone"));
                command.Parameters.Add(new SqlParameter("@mondayIsAvailable", SqlDbType.Bit, 0, "MondayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@mondayStartTime", SqlDbType.NChar, 5, "MondayStartTime"));
                command.Parameters.Add(new SqlParameter("@mondayFinishTime", SqlDbType.NChar, 5, "MondayFinishTime"));
                command.Parameters.Add(new SqlParameter("@tuesdayIsAvailable", SqlDbType.Bit, 0, "TuesdayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@tuesdayStartTime", SqlDbType.NChar, 5, "TuesdayStartTime"));
                command.Parameters.Add(new SqlParameter("@tuesdayFinishTime", SqlDbType.NChar, 5, "TuesdayFinishTime"));
                command.Parameters.Add(new SqlParameter("@wednesdayIsAvailable", SqlDbType.Bit, 0, "WednesdayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@wednesdayStartTime", SqlDbType.NChar, 5, "WednesdayStartTime"));
                command.Parameters.Add(new SqlParameter("@wednesdayFinishTime", SqlDbType.NChar, 5, "WednesdayFinishTime"));
                command.Parameters.Add(new SqlParameter("@thursdayIsAvailable", SqlDbType.Bit, 0, "ThursdayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@thursdayStartTime", SqlDbType.NChar, 5, "ThursdayStartTime"));
                command.Parameters.Add(new SqlParameter("@thursdayFinishTime", SqlDbType.NChar, 5, "ThursdayFinishTime"));
                command.Parameters.Add(new SqlParameter("@fridayIsAvailable", SqlDbType.Bit, 0, "FridayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@fridayStartTime", SqlDbType.NChar, 5, "FridayStartTime"));
                command.Parameters.Add(new SqlParameter("@fridayFinishTime", SqlDbType.NChar, 5, "FridayFinishTime"));
                command.Parameters.Add(new SqlParameter("@saturdayIsAvailable", SqlDbType.Bit, 0, "SaturdayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@saturdayStartTime", SqlDbType.NChar, 5, "SaturdayStartTime"));
                command.Parameters.Add(new SqlParameter("@saturdayFinishTime", SqlDbType.NChar, 5, "SaturdayFinishTime"));
                command.Parameters.Add(new SqlParameter("@sundayIsAvailable", SqlDbType.Bit, 0, "SundayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@sundayStartTime", SqlDbType.NChar, 5, "SundayStartTime"));
                command.Parameters.Add(new SqlParameter("@sundayFinishTime", SqlDbType.NChar, 5, "SundayFinishTime"));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = coach.Id;
                command.Parameters[2].Value = coach.FirstName;
                command.Parameters[3].Value = coach.LastName;
                command.Parameters[4].Value = coach.Email;
                command.Parameters[5].Value = coach.Phone;
                // Monday
                command.Parameters[6].Value = coach.WorkingHours.Monday.IsAvailable;
                command.Parameters[7].Value = coach.WorkingHours.Monday.StartTime;
                command.Parameters[8].Value = coach.WorkingHours.Monday.FinishTime;
                // Tuesday
                command.Parameters[9].Value = coach.WorkingHours.Tuesday.IsAvailable;
                command.Parameters[10].Value = coach.WorkingHours.Tuesday.StartTime;
                command.Parameters[11].Value = coach.WorkingHours.Tuesday.FinishTime;
                // Wednesday
                command.Parameters[12].Value = coach.WorkingHours.Wednesday.IsAvailable;
                command.Parameters[13].Value = coach.WorkingHours.Wednesday.StartTime;
                command.Parameters[14].Value = coach.WorkingHours.Wednesday.FinishTime;
                // Thursday
                command.Parameters[15].Value = coach.WorkingHours.Thursday.IsAvailable;
                command.Parameters[16].Value = coach.WorkingHours.Thursday.StartTime;
                command.Parameters[17].Value = coach.WorkingHours.Thursday.FinishTime;
                // Friday
                command.Parameters[18].Value = coach.WorkingHours.Friday.IsAvailable;
                command.Parameters[19].Value = coach.WorkingHours.Friday.StartTime;
                command.Parameters[20].Value = coach.WorkingHours.Friday.FinishTime;
                // Saturday
                command.Parameters[21].Value = coach.WorkingHours.Saturday.IsAvailable;
                command.Parameters[22].Value = coach.WorkingHours.Saturday.StartTime;
                command.Parameters[23].Value = coach.WorkingHours.Saturday.FinishTime;
                // Sunday
                command.Parameters[24].Value = coach.WorkingHours.Sunday.IsAvailable;
                command.Parameters[25].Value = coach.WorkingHours.Sunday.StartTime;
                command.Parameters[26].Value = coach.WorkingHours.Sunday.FinishTime;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadCoachData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public CoachData UpdateCoach(Guid businessId, Coach coach)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("Coach_Update", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@coachGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar, 50, "FirstName"));
                command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar, 50, "LastName"));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 100, "Email"));
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar, 50, "Phone"));
                command.Parameters.Add(new SqlParameter("@mondayIsAvailable", SqlDbType.Bit, 0, "MondayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@mondayStartTime", SqlDbType.NChar, 5, "MondayStartTime"));
                command.Parameters.Add(new SqlParameter("@mondayFinishTime", SqlDbType.NChar, 5, "MondayFinishTime"));
                command.Parameters.Add(new SqlParameter("@tuesdayIsAvailable", SqlDbType.Bit, 0, "TuesdayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@tuesdayStartTime", SqlDbType.NChar, 5, "TuesdayStartTime"));
                command.Parameters.Add(new SqlParameter("@tuesdayFinishTime", SqlDbType.NChar, 5, "TuesdayFinishTime"));
                command.Parameters.Add(new SqlParameter("@wednesdayIsAvailable", SqlDbType.Bit, 0, "WednesdayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@wednesdayStartTime", SqlDbType.NChar, 5, "WednesdayStartTime"));
                command.Parameters.Add(new SqlParameter("@wednesdayFinishTime", SqlDbType.NChar, 5, "WednesdayFinishTime"));
                command.Parameters.Add(new SqlParameter("@thursdayIsAvailable", SqlDbType.Bit, 0, "ThursdayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@thursdayStartTime", SqlDbType.NChar, 5, "ThursdayStartTime"));
                command.Parameters.Add(new SqlParameter("@thursdayFinishTime", SqlDbType.NChar, 5, "ThursdayFinishTime"));
                command.Parameters.Add(new SqlParameter("@fridayIsAvailable", SqlDbType.Bit, 0, "FridayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@fridayStartTime", SqlDbType.NChar, 5, "FridayStartTime"));
                command.Parameters.Add(new SqlParameter("@fridayFinishTime", SqlDbType.NChar, 5, "FridayFinishTime"));
                command.Parameters.Add(new SqlParameter("@saturdayIsAvailable", SqlDbType.Bit, 0, "SaturdayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@saturdayStartTime", SqlDbType.NChar, 5, "SaturdayStartTime"));
                command.Parameters.Add(new SqlParameter("@saturdayFinishTime", SqlDbType.NChar, 5, "SaturdayFinishTime"));
                command.Parameters.Add(new SqlParameter("@sundayIsAvailable", SqlDbType.Bit, 0, "SundayIsAvailable"));
                command.Parameters.Add(new SqlParameter("@sundayStartTime", SqlDbType.NChar, 5, "SundayStartTime"));
                command.Parameters.Add(new SqlParameter("@sundayFinishTime", SqlDbType.NChar, 5, "SundayFinishTime"));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = coach.Id;
                command.Parameters[2].Value = coach.FirstName;
                command.Parameters[3].Value = coach.LastName;
                command.Parameters[4].Value = coach.Email;
                command.Parameters[5].Value = coach.Phone;
                // Monday
                command.Parameters[6].Value = coach.WorkingHours.Monday.IsAvailable;
                command.Parameters[7].Value = coach.WorkingHours.Monday.StartTime;
                command.Parameters[8].Value = coach.WorkingHours.Monday.FinishTime;
                // Tuesday
                command.Parameters[9].Value = coach.WorkingHours.Tuesday.IsAvailable;
                command.Parameters[10].Value = coach.WorkingHours.Tuesday.StartTime;
                command.Parameters[11].Value = coach.WorkingHours.Tuesday.FinishTime;
                // Wednesday
                command.Parameters[12].Value = coach.WorkingHours.Wednesday.IsAvailable;
                command.Parameters[13].Value = coach.WorkingHours.Wednesday.StartTime;
                command.Parameters[14].Value = coach.WorkingHours.Wednesday.FinishTime;
                // Thursday
                command.Parameters[15].Value = coach.WorkingHours.Thursday.IsAvailable;
                command.Parameters[16].Value = coach.WorkingHours.Thursday.StartTime;
                command.Parameters[17].Value = coach.WorkingHours.Thursday.FinishTime;
                // Friday
                command.Parameters[18].Value = coach.WorkingHours.Friday.IsAvailable;
                command.Parameters[19].Value = coach.WorkingHours.Friday.StartTime;
                command.Parameters[20].Value = coach.WorkingHours.Friday.FinishTime;
                // Saturday
                command.Parameters[21].Value = coach.WorkingHours.Saturday.IsAvailable;
                command.Parameters[22].Value = coach.WorkingHours.Saturday.StartTime;
                command.Parameters[23].Value = coach.WorkingHours.Saturday.FinishTime;
                // Sunday
                command.Parameters[24].Value = coach.WorkingHours.Sunday.IsAvailable;
                command.Parameters[25].Value = coach.WorkingHours.Sunday.StartTime;
                command.Parameters[26].Value = coach.WorkingHours.Sunday.FinishTime;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadCoachData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }


        public IList<ServiceData> GetAllServices(Guid businessId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Service_GetAll]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;

                reader = command.ExecuteReader();

                var services = new List<ServiceData>();

                while (reader.Read())
                    services.Add(ReadServiceData(reader));

                return services;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public ServiceData GetService(Guid businessId, Guid serviceId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Service_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@serviceGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[1].Value = serviceId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadServiceData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public ServiceData AddService(Guid businessId, Service service)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("Service_Create", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@serviceGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 100, "Name"));
                command.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar, 500, "Description"));
                command.Parameters.Add(new SqlParameter("@duration", SqlDbType.SmallInt, 2, "Duration"));
                command.Parameters.Add(new SqlParameter("@studentCapacity", SqlDbType.TinyInt, 1, "StudentCapacity"));
                command.Parameters.Add(new SqlParameter("@isOnlineBookable", SqlDbType.Bit, 0, "IsOnlineBookable"));
                command.Parameters.Add(new SqlParameter("@sessionCount", SqlDbType.TinyInt, 1, "SessionCount"));
                command.Parameters.Add(new SqlParameter("@repeatFrequency", SqlDbType.Char, 1, "RepeatFrequency"));
                command.Parameters.Add(new SqlParameter("@sessionPrice", SqlDbType.Decimal, 19, "SessionPrice"));
                command.Parameters.Add(new SqlParameter("@coursePrice", SqlDbType.Decimal, 19, "CoursePrice"));
                command.Parameters.Add(new SqlParameter("@colour", SqlDbType.Char, 12, "Colour"));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = service.Id;
                command.Parameters[2].Value = service.Name;
                command.Parameters[3].Value = service.Description;
                command.Parameters[4].Value = service.Timing == null ? null : service.Timing.Duration;
                command.Parameters[5].Value = service.Booking == null ? null : service.Booking.StudentCapacity;
                command.Parameters[6].Value = service.Booking == null ? null : service.Booking.IsOnlineBookable;
                command.Parameters[7].Value = service.Repetition.SessionCount;
                command.Parameters[8].Value = service.Repetition.RepeatFrequency;
                command.Parameters[9].Value = service.Pricing == null ? null : service.Pricing.SessionPrice;
                command.Parameters[10].Value = service.Pricing == null ? null : service.Pricing.CoursePrice;
                command.Parameters[11].Value = service.Presentation == null ? null : service.Presentation.Colour;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadServiceData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public ServiceData UpdateService(Guid businessId, Service service)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("Service_Update", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@serviceGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 100, "Name"));
                command.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar, 500, "Description"));
                command.Parameters.Add(new SqlParameter("@duration", SqlDbType.SmallInt, 2, "Duration"));
                command.Parameters.Add(new SqlParameter("@studentCapacity", SqlDbType.TinyInt, 1, "StudentCapacity"));
                command.Parameters.Add(new SqlParameter("@isOnlineBookable", SqlDbType.Bit, 0, "IsOnlineBookable"));
                command.Parameters.Add(new SqlParameter("@sessionCount", SqlDbType.TinyInt, 1, "SessionCount"));
                command.Parameters.Add(new SqlParameter("@repeatFrequency", SqlDbType.Char, 1, "RepeatFrequency"));
                command.Parameters.Add(new SqlParameter("@sessionPrice", SqlDbType.Decimal, 19, "SessionPrice"));
                command.Parameters.Add(new SqlParameter("@coursePrice", SqlDbType.Decimal, 19, "CoursePrice"));
                command.Parameters.Add(new SqlParameter("@colour", SqlDbType.Char, 12, "Colour"));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = service.Id;
                command.Parameters[2].Value = service.Name;
                command.Parameters[3].Value = service.Description;
                command.Parameters[4].Value = service.Timing == null ? null : service.Timing.Duration;
                command.Parameters[5].Value = service.Booking == null ? null : service.Booking.StudentCapacity;
                command.Parameters[6].Value = service.Booking == null ? null : service.Booking.IsOnlineBookable;
                command.Parameters[7].Value = service.Repetition.SessionCount;
                command.Parameters[8].Value = service.Repetition.RepeatFrequency;
                command.Parameters[9].Value = service.Pricing == null ? null : service.Pricing.SessionPrice;
                command.Parameters[10].Value = service.Pricing == null ? null : service.Pricing.CoursePrice;
                command.Parameters[11].Value = service.Presentation == null ? null : service.Presentation.Colour;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadServiceData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }


        public IList<CustomerData> GetAllCustomers(Guid businessId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Customer_GetAll]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;

                reader = command.ExecuteReader();

                var customers = new List<CustomerData>();

                while (reader.Read())
                    customers.Add(ReadCustomerData(reader));

                return customers;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public CustomerData GetCustomer(Guid businessId, Guid customerId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Customer_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[1].Value = customerId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadCustomerData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public CustomerData AddCustomer(Guid businessId, Customer customer)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("Customer_Create", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar, 50, "FirstName"));
                command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar, 50, "LastName"));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 100, "Email"));
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar, 50, "Phone"));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = customer.Id;
                command.Parameters[2].Value = customer.FirstName;
                command.Parameters[3].Value = customer.LastName;
                command.Parameters[4].Value = customer.Email;
                command.Parameters[5].Value = customer.Phone;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadCustomerData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public CustomerData UpdateCustomer(Guid businessId, Customer customer)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("Customer_Update", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar, 50, "FirstName"));
                command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar, 50, "LastName"));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 100, "Email"));
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar, 50, "Phone"));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = customer.Id;
                command.Parameters[2].Value = customer.FirstName;
                command.Parameters[3].Value = customer.LastName;
                command.Parameters[4].Value = customer.Email;
                command.Parameters[5].Value = customer.Phone;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadCustomerData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }


        public IList<SingleSessionData> GetAllStandaloneSessions(Guid businessId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Session_GetAllStandaloneSessions]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;

                reader = command.ExecuteReader();

                var sessions = new List<SingleSessionData>();

                while (reader.Read())
                    sessions.Add(ReadSessionData(reader));

                return sessions;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public IList<SingleSessionData> GetAllSessions(Guid businessId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Session_GetAllSessions]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;

                reader = command.ExecuteReader();

                var sessions = new List<SingleSessionData>();

                while (reader.Read())
                    sessions.Add(ReadSessionData(reader));

                return sessions;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public SingleSessionData GetSession(Guid businessId, Guid sessionId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Session_GetSessionByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@sessionGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[1].Value = sessionId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadSessionData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public SingleSessionData AddSession(Guid businessId, StandaloneSession session)
        {
            try
            {
                Connection.Open();

                return AddSessionData(businessId, session);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }

        public SingleSessionData UpdateSession(Guid businessId, SingleSession session)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("Session_UpdateSession", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@sessionGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@parentGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@locationGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@coachGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@serviceGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@startTime", SqlDbType.Time));
                command.Parameters.Add(new SqlParameter("@duration", SqlDbType.SmallInt));
                command.Parameters.Add(new SqlParameter("@studentCapacity", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@isOnlineBookable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@sessionCount", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@repeatFrequency", SqlDbType.Char));
                command.Parameters.Add(new SqlParameter("@sessionPrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@coursePrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@colour", SqlDbType.Char));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = session.Id;
                command.Parameters[2].Value = session.ParentId;
                command.Parameters[3].Value = session.Location.Id;
                command.Parameters[4].Value = session.Coach.Id;
                command.Parameters[5].Value = session.Service.Id;
                command.Parameters[6].Value = null;                     // Not storing name yet. It's built up from location, coach, service etc.
                command.Parameters[7].Value = session.Timing.StartDate;
                command.Parameters[8].Value = session.Timing.StartTime;
                command.Parameters[9].Value = session.Timing.Duration;
                command.Parameters[10].Value = session.Booking.StudentCapacity;
                command.Parameters[11].Value = session.Booking.IsOnlineBookable;
                command.Parameters[12].Value = 1;       // SessionCount
                command.Parameters[13].Value = null;    // RepeatFrequency
                command.Parameters[14].Value = session.Pricing.SessionPrice;
                command.Parameters[15].Value = null;    // CoursePrice
                command.Parameters[16].Value = session.Presentation.Colour;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadSessionData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        private bool OpenConnection()
        {
            var wasAlreadyOpen = Connection.State == ConnectionState.Open;
            if (!wasAlreadyOpen)
                Connection.Open();

            return wasAlreadyOpen;
        }

        private void CloseConnection(bool wasAlreadyOpen)
        {
            if (Connection != null && !wasAlreadyOpen)
                Connection.Close();
        }

        public void DeleteSession(Guid businessId, Guid sessionId)
        {
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Session_DeleteByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@sessionGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = sessionId;

                command.ExecuteNonQuery();
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }


        public RepeatedSessionData GetCourse(Guid businessId, Guid courseId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Session_GetCourseByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@courseGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[1].Value = courseId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadCourseAndSessionsData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public RepeatedSessionData AddCourse(Guid businessId, RepeatedSession course)
        {
            try
            {
                Connection.Open();

                AddCourseData(businessId, course);

                foreach (var session in course.Sessions)
                    AddSessionData(businessId, session);

                return GetCourse(businessId, course.Id);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }

        public RepeatedSessionData UpdateCourse(Guid businessId, RepeatedSession course)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Session_UpdateCourse]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@courseGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@locationGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@coachGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@serviceGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@startTime", SqlDbType.Time));
                command.Parameters.Add(new SqlParameter("@duration", SqlDbType.SmallInt));
                command.Parameters.Add(new SqlParameter("@studentCapacity", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@isOnlineBookable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@sessionCount", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@repeatFrequency", SqlDbType.Char));
                command.Parameters.Add(new SqlParameter("@sessionPrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@coursePrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@colour", SqlDbType.Char));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = course.Id;
                command.Parameters[2].Value = course.Location.Id;
                command.Parameters[3].Value = course.Coach.Id;
                command.Parameters[4].Value = course.Service.Id;
                command.Parameters[5].Value = null;                     // Not storing name yet. It's built up from location, coach, service etc.
                command.Parameters[6].Value = course.Timing.StartDate;
                command.Parameters[7].Value = course.Timing.StartTime;
                command.Parameters[8].Value = course.Timing.Duration;
                command.Parameters[9].Value = course.Booking.StudentCapacity;
                command.Parameters[10].Value = course.Booking.IsOnlineBookable;
                command.Parameters[11].Value = course.Repetition.SessionCount;
                command.Parameters[12].Value = course.Repetition.RepeatFrequency;
                command.Parameters[13].Value = course.Pricing.SessionPrice;
                command.Parameters[14].Value = course.Pricing.CoursePrice;
                command.Parameters[15].Value = course.Presentation.Colour;

                command.ExecuteNonQuery();

                foreach (var session in course.Sessions)
                    UpdateSession(businessId, session);

                return GetCourse(businessId, course.Id);
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        public void DeleteCourse(Guid businessId, Guid courseId)
        {
            DeleteSession(businessId, courseId);
        }


        public SingleSessionBookingData GetSessionBooking(Guid businessId, Guid sessionBookingId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Booking_GetSessionBookingByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = sessionBookingId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadSessionBookingData(reader);

                return null;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public SingleSessionBookingData AddSessionBooking(Guid businessId, SingleSessionBooking booking)
        {
            try
            {
                Connection.Open();

                return AddSessionBookingData(businessId, booking);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }

        public CourseBookingData AddCourseBooking(Guid businessId, CourseBooking courseBooking)
        {
            try
            {
                Connection.Open();

                AddCourseBookingData(businessId, courseBooking);

                foreach (var sessionBooking in courseBooking.SessionBookings)
                    AddSessionBookingData(businessId, sessionBooking);

                return GetCourseBooking(businessId, courseBooking.Id);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }

        private SingleSessionBookingData AddSessionBookingData(Guid businessId, SingleSessionBooking booking)
        {
            SqlDataReader reader = null;
            try
            {
                var command = new SqlCommand("Booking_CreateSessionBooking", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@parentGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@sessionGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@paymentStatus", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@hasAttended", SqlDbType.Bit));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = booking.Id;
                command.Parameters[2].Value = booking.ParentId;
                command.Parameters[3].Value = booking.Session.Id;
                command.Parameters[4].Value = booking.Customer.Id;
                command.Parameters[5].Value = booking.PaymentStatus;
                command.Parameters[6].Value = booking.HasAttended;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadSessionBookingData(reader);

                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private void AddCourseBookingData(Guid businessId, CourseBooking courseBooking)
        {
            var command = new SqlCommand("Booking_CreateCourseBooking", Connection) { CommandType = CommandType.StoredProcedure };

            command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@courseGuid", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@paymentStatus", SqlDbType.NVarChar));
            command.Parameters.Add(new SqlParameter("@hasAttended", SqlDbType.Bit));

            command.Parameters[0].Value = businessId;
            command.Parameters[1].Value = courseBooking.Id;
            command.Parameters[2].Value = courseBooking.Course.Id;
            command.Parameters[3].Value = courseBooking.Customer.Id;
            command.Parameters[4].Value = courseBooking.PaymentStatus;
            command.Parameters[5].Value = courseBooking.HasAttended;

            command.ExecuteNonQuery();
        }

        public CourseBookingData GetCourseBooking(Guid businessId, Guid courseBookingId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Booking_GetCourseBookingByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = courseBookingId;


                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadCourseAndSessionsBookingData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        private SingleSessionBookingData ReadSessionBookingData(SqlDataReader reader)
        {
            var id = reader.GetGuid(2);
            var parentId = reader.GetNullableGuid(3);
            var sessionId = reader.GetGuid(4);
            var sessionName = reader.GetString(5);
            var customerId = reader.GetGuid(6);
            var customerName = reader.GetString(7);
            var paymentStatus = reader.GetNullableString(8);
            var hasAttended = reader.GetNullableBool(9);

            return new SingleSessionBookingData
            {
                Id = id,
                ParentId = parentId,
                Session = new SessionKeyData
                {
                    Id = sessionId,
                    Name = sessionName
                },
                Customer = new CustomerKeyData
                {
                    Id = customerId,
                    Name = customerName
                },
                PaymentStatus = paymentStatus,
                HasAttended = hasAttended
            };
        }

        //private CourseBookingData ReadCourseBookingData(SqlDataReader reader)
        //{
        //    var id = reader.GetGuid(2);
        //    var parentId = reader.GetNullableGuid(3);
        //    var sessionId = reader.GetGuid(4);
        //    var sessionName = reader.GetString(5);
        //    var customerId = reader.GetGuid(6);
        //    var customerName = reader.GetString(7);

        //    return new SingleSessionBookingData
        //    {
        //        Id = id,
        //        ParentId = parentId,
        //        Session = new SessionKeyData
        //        {
        //            Id = sessionId,
        //            Name = sessionName
        //        },
        //        Customer = new CustomerKeyData
        //        {
        //            Id = customerId,
        //            Name = customerName
        //        }
        //    };
        //}

        public void DeleteBooking(Guid businessId, Guid bookingId)
        {
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Booking_DeleteByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = bookingId;

                command.ExecuteNonQuery();
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }


        public IList<CustomerBookingData> GetCustomerBookingsBySessionId(Guid businessId, Guid sessionId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Booking_GetCustomerBookingsBySessionId]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@sessionGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = sessionId;

                reader = command.ExecuteReader();

                var customerBookings = new List<CustomerBookingData>();

                while (reader.Read())
                    customerBookings.Add(ReadCustomerBookingData(reader));

                return customerBookings;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        public IList<CustomerBookingData> GetCustomerBookingsByCourseId(Guid businessId, Guid courseId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Booking_GetCustomerBookingsByCourseId]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@courseGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = courseId;

                reader = command.ExecuteReader();

                var customerBookings = new List<CustomerBookingData>();

                while (reader.Read())
                    customerBookings.Add(ReadCustomerBookingData(reader));

                return customerBookings;
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }


        private LocationData ReadLocationData(SqlDataReader reader)
        {
            return new LocationData
            {
                Id = reader.GetGuid(2),
                Name = reader.GetString(3)
            };
        }

        private CoachData ReadCoachData(SqlDataReader reader)
        {
            return new CoachData
            {
                Id = reader.GetGuid(2),
                FirstName = reader.GetString(3),
                LastName = reader.GetString(4),
                Email = reader.GetString(5),
                Phone = reader.GetString(6),
                WorkingHours = new WeeklyWorkingHoursData
                {
                    Monday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(7),
                        StartTime = reader.GetNullableStringTrimmed(8),
                        FinishTime = reader.GetNullableStringTrimmed(9)
                    },
                    Tuesday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(10),
                        StartTime = reader.GetNullableStringTrimmed(11),
                        FinishTime = reader.GetNullableStringTrimmed(12)
                    },
                    Wednesday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(13),
                        StartTime = reader.GetNullableStringTrimmed(14),
                        FinishTime = reader.GetNullableStringTrimmed(15)
                    },
                    Thursday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(16),
                        StartTime = reader.GetNullableStringTrimmed(17),
                        FinishTime = reader.GetNullableStringTrimmed(18)
                    },
                    Friday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(19),
                        StartTime = reader.GetNullableStringTrimmed(20),
                        FinishTime = reader.GetNullableStringTrimmed(21)
                    },
                    Saturday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(22),
                        StartTime = reader.GetNullableStringTrimmed(23),
                        FinishTime = reader.GetNullableStringTrimmed(24)
                    },
                    Sunday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(25),
                        StartTime = reader.GetNullableStringTrimmed(26),
                        FinishTime = reader.GetNullableStringTrimmed(27)
                    }
                }
            };
        }

        private ServiceData ReadServiceData(SqlDataReader reader)
        {
            var service = new ServiceData
            {
                Id = reader.GetGuid(2),
                Name = reader.GetString(3),
                Description = reader.GetNullableString(4)
            };

            var duration = reader.GetNullableInt16(5);
            if (duration.IsExisting())
                service.Timing = new ServiceTimingData { Duration = duration };

            var studentCapacity = reader.GetNullableByte(6);
            var isOnlineBookable = reader.GetNullableBool(7);
            if (studentCapacity.IsExisting() || isOnlineBookable.IsExisting())
                service.Booking = new ServiceBookingData { StudentCapacity = studentCapacity, IsOnlineBookable = isOnlineBookable };

            service.Repetition = new RepetitionData { SessionCount = reader.GetByte(8), RepeatFrequency = reader.GetNullableString(9) };

            var sessionPrice = reader.GetNullableDecimal(10);
            var coursePrice = reader.GetNullableDecimal(11);
            if (sessionPrice.IsExisting() || coursePrice.IsExisting())
                service.Pricing = new RepeatedSessionPricingData { SessionPrice = sessionPrice, CoursePrice = coursePrice };

            var colour = reader.GetNullableStringTrimmed(12);
            if (colour.IsExisting())
                service.Presentation = new PresentationData { Colour = colour };

            return service;
        }

        private CustomerData ReadCustomerData(SqlDataReader reader)
        {
            return new CustomerData
            {
                Id = reader.GetGuid(2),
                FirstName = reader.GetString(3),
                LastName = reader.GetString(4),
                Email = reader.GetNullableString(5),
                Phone = reader.GetNullableString(6)
            };
        }

        private SingleSessionData ReadSessionData(SqlDataReader reader)
        {
            var id = reader.GetGuid(2);
            var parentId = reader.GetNullableGuid(3);
            var locationId = reader.GetGuid(4);
            var locationName = reader.GetString(5);
            var coachId = reader.GetGuid(6);
            var coachFirstName = reader.GetString(7);
            var coachLastName = reader.GetString(8);
            var serviceId = reader.GetGuid(9);
            var serviceName = reader.GetString(10);
            var name = reader.GetNullableString(11);
            var startDate = reader.GetDateTime(12).ToString("yyyy-MM-dd");
            var startTime = reader.GetTimeSpan(13).ToString(@"h\:mm");
            var duration = reader.GetInt16(14);
            var studentCapacity = reader.GetByte(15);
            var isOnlineBookable = reader.GetBoolean(16);
            var sessionCount = reader.GetByte(17);
            var repeatFrequency = reader.GetNullableStringTrimmed(18);
            var sessionPrice = reader.GetNullableDecimal(19);   // Nullable because for course session this can be null.
            var coursePrice = reader.GetNullableDecimal(20);
            var colour = reader.GetNullableStringTrimmed(21);

            if (sessionCount != 1)
                throw new InvalidOperationException("Single session must have only a single session.");
            if (repeatFrequency != null)
                throw new InvalidOperationException("Single session must not have repeatFrequency.");
            if (coursePrice != null)
                throw new InvalidOperationException("Single session must not have a coursePrice.");

            return new SingleSessionData
            {
                Id = id,
                ParentId = parentId,
                Location = new LocationKeyData { Id = locationId, Name = locationName},
                Coach = new CoachKeyData { Id = coachId, Name = string.Format("{0} {1}", coachFirstName, coachLastName)},
                Service = new ServiceKeyData { Id = serviceId, Name = serviceName },
                Timing = new SessionTimingData
                {
                    StartDate = startDate,
                    StartTime = startTime,
                    Duration = duration
                },
                Booking = new SessionBookingData
                {
                    StudentCapacity = studentCapacity,
                    IsOnlineBookable = isOnlineBookable
                },
                Pricing = new SingleSessionPricingData { SessionPrice = sessionPrice },
                Presentation = new PresentationData { Colour = colour }
            };
        }

        private SingleSessionData AddSessionData(Guid businessId, SingleSession session)
        {
            SqlDataReader reader = null;
            try
            {
                var command = new SqlCommand("Session_CreateSession", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@sessionGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@parentGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@locationGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@coachGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@serviceGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@startTime", SqlDbType.Time));
                command.Parameters.Add(new SqlParameter("@duration", SqlDbType.SmallInt));
                command.Parameters.Add(new SqlParameter("@studentCapacity", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@isOnlineBookable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@sessionCount", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@repeatFrequency", SqlDbType.Char));
                command.Parameters.Add(new SqlParameter("@sessionPrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@coursePrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@colour", SqlDbType.Char));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = session.Id;
                command.Parameters[2].Value = session.ParentId;
                command.Parameters[3].Value = session.Location.Id;
                command.Parameters[4].Value = session.Coach.Id;
                command.Parameters[5].Value = session.Service.Id;
                command.Parameters[6].Value = null;                     // Not storing name yet. It's built up from location, coach, service etc.
                command.Parameters[7].Value = session.Timing.StartDate;
                command.Parameters[8].Value = session.Timing.StartTime;
                command.Parameters[9].Value = session.Timing.Duration;
                command.Parameters[10].Value = session.Booking.StudentCapacity;
                command.Parameters[11].Value = session.Booking.IsOnlineBookable;
                command.Parameters[12].Value = 1;       // SessionCount
                command.Parameters[13].Value = null;    // RepeatFrequency
                command.Parameters[14].Value = session.Pricing.SessionPrice;
                command.Parameters[15].Value = null;    // CoursePrice
                 command.Parameters[16].Value = session.Presentation.Colour;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadSessionData(reader);

                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private RepeatedSessionData ReadCourseHeaderData(SqlDataReader reader)
        {
            var id = reader.GetGuid(2);
            var locationId = reader.GetGuid(3);
            var locationName = reader.GetString(4);
            var coachId = reader.GetGuid(5);
            var coachFirstName = reader.GetString(6);
            var coachLastName = reader.GetString(7);
            var serviceId = reader.GetGuid(8);
            var serviceName = reader.GetString(9);
            var name = reader.GetNullableString(10);
            var startDate = reader.GetDateTime(11).ToString("yyyy-MM-dd");
            var startTime = reader.GetTimeSpan(12).ToString(@"h\:mm");
            var duration = reader.GetInt16(13);
            var studentCapacity = reader.GetByte(14);
            var isOnlineBookable = reader.GetBoolean(15);
            var sessionCount = reader.GetByte(16);
            var repeatFrequency = reader.GetNullableStringTrimmed(17);
            var sessionPrice = reader.GetNullableDecimal(18);
            var coursePrice = reader.GetNullableDecimal(19);
            var colour = reader.GetNullableStringTrimmed(20);

            if (sessionCount == 1)
                throw new InvalidOperationException("Course must have more than a single session.");
            if (repeatFrequency == null)
                throw new InvalidOperationException("Course must have a repeatFrequency.");

            return new RepeatedSessionData
            {
                Id = id,
                Location = new LocationKeyData { Id = locationId, Name = locationName },
                Coach = new CoachKeyData { Id = coachId, Name = string.Format("{0} {1}", coachFirstName, coachLastName) },
                Service = new ServiceKeyData { Id = serviceId, Name = serviceName },
                Timing = new SessionTimingData
                {
                    StartDate = startDate,
                    StartTime = startTime,
                    Duration = duration
                },
                Booking = new SessionBookingData
                {
                    StudentCapacity = studentCapacity,
                    IsOnlineBookable = isOnlineBookable
                },
                Repetition = new RepetitionData { SessionCount = sessionCount, RepeatFrequency = repeatFrequency },
                Pricing = new RepeatedSessionPricingData { SessionPrice = sessionPrice, CoursePrice = coursePrice },
                Presentation = new PresentationData { Colour = colour }
            };
        }

        private RepeatedSessionData ReadCourseAndSessionsData(SqlDataReader reader)
        {
            var id = reader.GetGuid(2);
            var parentId = reader.GetNullableGuid(3);
            var locationId = reader.GetGuid(4);
            var locationName = reader.GetString(5);
            var coachId = reader.GetGuid(6);
            var coachFirstName = reader.GetString(7);
            var coachLastName = reader.GetString(8);
            var serviceId = reader.GetGuid(9);
            var serviceName = reader.GetString(10);
            var name = reader.GetNullableString(11);
            var startDate = reader.GetDateTime(12).ToString("yyyy-MM-dd");
            var startTime = reader.GetTimeSpan(13).ToString(@"h\:mm");
            var duration = reader.GetInt16(14);
            var studentCapacity = reader.GetByte(15);
            var isOnlineBookable = reader.GetBoolean(16);
            var sessionCount = reader.GetByte(17);
            var repeatFrequency = reader.GetNullableStringTrimmed(18);
            var sessionPrice = reader.GetNullableDecimal(19);
            var coursePrice = reader.GetNullableDecimal(20);
            var colour = reader.GetNullableStringTrimmed(21);

            if (sessionCount == 1)
                throw new InvalidOperationException("Course must have more than a single session.");
            if (repeatFrequency == null)
                throw new InvalidOperationException("Course must have a repeatFrequency.");

            var sessions = new List<SingleSessionData>();

            while (reader.Read())
                sessions.Add(ReadSessionData(reader));

            return new RepeatedSessionData
            {
                Id = id,
                Location = new LocationKeyData { Id = locationId, Name = locationName },
                Coach = new CoachKeyData { Id = coachId, Name = string.Format("{0} {1}", coachFirstName, coachLastName) },
                Service = new ServiceKeyData { Id = serviceId, Name = serviceName },
                Timing = new SessionTimingData
                {
                    StartDate = startDate,
                    StartTime = startTime,
                    Duration = duration
                },
                Booking = new SessionBookingData
                {
                    StudentCapacity = studentCapacity,
                    IsOnlineBookable = isOnlineBookable
                },
                Repetition = new RepetitionData { SessionCount = sessionCount, RepeatFrequency = repeatFrequency },
                Pricing = new RepeatedSessionPricingData { SessionPrice = sessionPrice, CoursePrice = coursePrice },
                Presentation = new PresentationData { Colour = colour },
                Sessions = sessions
            };
        }

        private CourseBookingData ReadCourseAndSessionsBookingData(SqlDataReader reader)
        {
            var id = reader.GetGuid(2);
            var parentId = reader.GetNullableGuid(3);
            var courseId = reader.GetGuid(4);
            var courseName = reader.GetString(5);
            var customerId = reader.GetGuid(6);
            var customerName = reader.GetString(7);
            var paymentStatus = reader.GetNullableString(8);
            var hasAttended = reader.GetNullableBool(9);
            
            var sessionBookings = new List<SingleSessionBookingData>();

            while (reader.Read())
                sessionBookings.Add(ReadSessionBookingData(reader));

            return new CourseBookingData
            {
                Id = id,
                Customer = new CustomerKeyData { Id = customerId, Name = customerName },
                Course = new SessionKeyData { Id = courseId, Name = courseName },
                SessionBookings = sessionBookings,
                PaymentStatus = paymentStatus,
                HasAttended = hasAttended
            };
        }

        private RepeatedSessionData AddCourseData(Guid businessId, RepeatedSession course)
        {
            SqlDataReader reader = null;
            try
            {
                var command = new SqlCommand("Session_CreateCourse", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@courseGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@locationGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@coachGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@serviceGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@startDate", SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@startTime", SqlDbType.Time));
                command.Parameters.Add(new SqlParameter("@duration", SqlDbType.SmallInt));
                command.Parameters.Add(new SqlParameter("@studentCapacity", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@isOnlineBookable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@sessionCount", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@repeatFrequency", SqlDbType.Char));
                command.Parameters.Add(new SqlParameter("@sessionPrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@coursePrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@colour", SqlDbType.Char));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = course.Id;
                command.Parameters[2].Value = course.Location.Id;
                command.Parameters[3].Value = course.Coach.Id;
                command.Parameters[4].Value = course.Service.Id;
                command.Parameters[5].Value = null; // Not storing name yet. It's built up from location, coach, service etc.
                command.Parameters[6].Value = course.Timing.StartDate;
                command.Parameters[7].Value = course.Timing.StartTime;
                command.Parameters[8].Value = course.Timing.Duration;
                command.Parameters[9].Value = course.Booking.StudentCapacity;
                command.Parameters[10].Value = course.Booking.IsOnlineBookable;
                command.Parameters[11].Value = course.Repetition.SessionCount;
                command.Parameters[12].Value = course.Repetition.RepeatFrequency;
                command.Parameters[13].Value = course.Pricing.SessionPrice;
                command.Parameters[14].Value = course.Pricing.CoursePrice;
                command.Parameters[15].Value = course.Presentation.Colour;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadCourseHeaderData(reader);

                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }


        //private BookingData ReadBookingData(SqlDataReader reader)
        //{
        //    var bookingId = reader.GetGuid(2);
        //    var sessionId = reader.GetGuid(3);
        //    var sessionName = reader.GetString(4);
        //    var customerId = reader.GetGuid(5);
        //    var customerName = reader.GetString(6);
        //    var paymentStatus = reader.GetString(7);
        //    var attended = reader.GetBoolean(8);      
        //      
        //    return new BookingData
        //    {
        //        Id = bookingId,
        //        Session = new SessionKeyData
        //        {
        //            Id = sessionId,
        //            Name = sessionName
        //        },
        //        Customer = new CustomerKeyData
        //        {
        //            Id = customerId,
        //            Name = customerName
        //        },
        //        PaymentStatus = paymentStatus,
        //        Attended = attended
        //    };

        private CustomerBookingData ReadCustomerBookingData(SqlDataReader reader)
        {
            var sessionId = reader.GetGuid(1);
            var customerId = reader.GetGuid(2);
            var bookingId = reader.GetGuid(3);
            var parentBookingId = reader.GetNullableGuid(4);
            var firstName = reader.GetString(5);
            var lastName = reader.GetString(6);
            var email = reader.GetNullableString(7);
            var phone = reader.GetNullableString(8);
            var paymentStatus = reader.GetNullableString(9);
            var hasAttended = reader.GetNullableBool(10);

            return new CustomerBookingData
            {
                Id = bookingId,
                ParentId = parentBookingId,
                SessionId = sessionId,
                Customer = new CustomerData
                {
                    Id = customerId,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Phone = phone                    
                },
                PaymentStatus = paymentStatus,
                HasAttended = hasAttended
            };
        }
    }
}
