using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbServiceRepository : DbRepositoryBase
    {
        public DbServiceRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


        public IList<ServiceData> GetAllServices(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Service_GetAll]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters[0].Value = businessId;

                var services = new List<ServiceData>();
                reader = command.ExecuteReader();
                while (reader.Read())
                    services.Add(ReadServiceData(reader));

                return services;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public ServiceData GetService(Guid businessId, Guid serviceId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

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
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public ServiceData AddService(Guid businessId, Service service)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

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
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public ServiceData UpdateService(Guid businessId, Service service)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

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
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
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
    }
}
