using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbCourseRepository : DbSessionRepository
    {
        public DbCourseRepository(string connectionStringKey)
            : base(connectionStringKey)
        { }


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
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                AddCourseData(businessId, course);
                foreach (var session in course.Sessions)
                    AddSession(businessId, session);

                return GetCourse(businessId, course.Id);
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
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


        private RepeatedSessionData ReadCourseAndSessionsData(SqlDataReader reader)
        {
            var course = ReadCourseData(reader);
            course.Sessions = ReadSessionsData(reader);

            return course;
        }

        private RepeatedSessionData ReadCourseData(SqlDataReader reader)
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
            if (parentId != null)
                throw new InvalidOperationException("Course must have no parentId.");

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

        private List<SingleSessionData> ReadSessionsData(SqlDataReader reader)
        {
            var sessions = new List<SingleSessionData>();

            while (reader.Read())
                sessions.Add(ReadSessionData(reader));

            return sessions;
        }

        private void AddCourseData(Guid businessId, RepeatedSession course)
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

            command.ExecuteNonQuery();
        }
    }
}
