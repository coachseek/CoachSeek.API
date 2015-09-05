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

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
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

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@serviceGuid", SqlDbType.UniqueIdentifier));
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

        public void AddService(Guid businessId, Service service)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Service_Create]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@serviceGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@duration", SqlDbType.SmallInt));
                command.Parameters.Add(new SqlParameter("@studentCapacity", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@isOnlineBookable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@sessionCount", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@repeatFrequency", SqlDbType.Char));
                command.Parameters.Add(new SqlParameter("@sessionPrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@coursePrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@colour", SqlDbType.Char));

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

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        public void UpdateService(Guid businessId, Service service)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Service_Update]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@serviceGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@description", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@duration", SqlDbType.SmallInt));
                command.Parameters.Add(new SqlParameter("@studentCapacity", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@isOnlineBookable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@sessionCount", SqlDbType.TinyInt));
                command.Parameters.Add(new SqlParameter("@repeatFrequency", SqlDbType.Char));
                command.Parameters.Add(new SqlParameter("@sessionPrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@coursePrice", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@colour", SqlDbType.Char));

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

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        
        private ServiceData ReadServiceData(SqlDataReader reader)
        {
            var id = reader.GetGuid(1);
            var name = reader.GetString(2);
            var description = reader.GetNullableString(3);
            var duration = reader.GetNullableInt16(4);
            var studentCapacity = reader.GetNullableByte(5);
            var isOnlineBookable = reader.GetNullableBool(6);
            var sessionCount = reader.GetByte(7);
            var repeatFrequency = reader.GetNullableString(8);
            var sessionPrice = reader.GetNullableDecimal(9);
            var coursePrice = reader.GetNullableDecimal(10);
            var colour = reader.GetNullableStringTrimmed(11);

            var service = new ServiceData
            {
                Id = id,
                Name = name,
                Description = description
            };

            if (duration.IsExisting())
                service.Timing = new ServiceTimingData { Duration = duration };

            if (studentCapacity.IsExisting() || isOnlineBookable.IsExisting())
                service.Booking = new ServiceBookingData { StudentCapacity = studentCapacity, IsOnlineBookable = isOnlineBookable };

            service.Repetition = new RepetitionData { SessionCount = sessionCount, RepeatFrequency = repeatFrequency };

            if (sessionPrice.IsExisting() || coursePrice.IsExisting())
                service.Pricing = new RepeatedSessionPricingData { SessionPrice = sessionPrice, CoursePrice = coursePrice };

            if (colour.IsExisting())
                service.Presentation = new PresentationData { Colour = colour };

            return service;
        }
    }
}
