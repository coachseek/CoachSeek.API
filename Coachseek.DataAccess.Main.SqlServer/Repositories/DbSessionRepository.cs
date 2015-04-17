﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbSessionRepository : DbRepositoryBase
    {
        public DbSessionRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


        public IList<SingleSessionData> GetAllStandaloneSessions(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Session_GetAllStandaloneSessions]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var sessions = new List<SingleSessionData>();
                reader = command.ExecuteReader();
                while (reader.Read())
                    sessions.Add(ReadSessionData(reader));

                return sessions;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public IList<SingleSessionData> GetAllSessions(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Session_GetAllSessions]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var sessions = new List<SingleSessionData>();
                reader = command.ExecuteReader();
                while (reader.Read())
                    sessions.Add(ReadSessionData(reader));

                return sessions;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public SingleSessionData GetSession(Guid businessId, Guid sessionId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Session_GetSessionByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@sessionGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = sessionId;

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

        public SingleSessionData AddSession(Guid businessId, SingleSession session)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("Session_CreateSession", Connection) { CommandType = CommandType.StoredProcedure };
                SetCreateOrUpdateParameters(command, businessId, session);
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

        public SingleSessionData UpdateSession(Guid businessId, SingleSession session)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("Session_UpdateSession", Connection) { CommandType = CommandType.StoredProcedure };
                SetCreateOrUpdateParameters(command, businessId, session);
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

        public void DeleteSession(Guid businessId, Guid sessionId)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Session_DeleteByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@sessionGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = sessionId;

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }


        protected SingleSessionData ReadSessionData(SqlDataReader reader)
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
            var sessionPrice = reader.GetNullableDecimal(19);   // Nullable because for a course session this can be null.
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
                Pricing = new SingleSessionPricingData { SessionPrice = sessionPrice },
                Presentation = new PresentationData { Colour = colour }
            };
        }

        private void SetCreateOrUpdateParameters(SqlCommand command, Guid businessId, SingleSession session)
        {
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
        }
    }
}
