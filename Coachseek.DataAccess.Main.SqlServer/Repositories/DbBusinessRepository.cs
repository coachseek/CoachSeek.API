using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CoachSeek.Common.DataAccess;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbBusinessRepository : IBusinessRepository
    {
        private SqlConnection _connection;

        private SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new SqlConnection("Server=REDDWARF;Database=Coachseek;User Id=sa;Password=C0@ch5eek;");

                return _connection;
            }
        }


        public Business2Data GetBusiness(Guid businessId)
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
                    return new Business2Data
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

        public Business2Data AddBusiness(Business2 business)
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
                    return new Business2Data
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

        public Business Save(NewBusiness newBusiness)
        {
            try
            {
                Connection.Open();

                var command = new SqlCommand("Business_Create", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 100, "Name"));
                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar, 100, "Domain"));

                command.Parameters[0].Value = newBusiness.Id;
                command.Parameters[1].Value = newBusiness.Name;
                command.Parameters[2].Value = newBusiness.Domain;

                command.ExecuteNonQuery();
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }

            return newBusiness;
        }

        public Business Save(Business business)
        {
            throw new NotImplementedException();
        }

        public Business Get(Guid id)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Business_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = id;

                reader = command.ExecuteReader();

                if (reader.HasRows)
                    reader.Read();

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
                Description = reader.GetString(4)
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
                Email = reader.GetString(5),
                Phone = reader.GetString(6)
            };
        }
    }
}
