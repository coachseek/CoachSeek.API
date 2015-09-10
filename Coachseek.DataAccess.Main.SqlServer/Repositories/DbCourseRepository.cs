using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@courseGuid", SqlDbType.UniqueIdentifier));
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

        public IList<RepeatedSessionData> GetAllCourses(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Session_GetAllCourses]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var courses = new List<RepeatedSessionData>();
                reader = command.ExecuteReader();
                while (reader.Read())
                    courses.Add(ReadCourseData(reader));
                 reader.Close();

                var sessionsInCourses = GetAllSessions(businessId).Where(x => x.ParentId != null).OrderBy(x => x.ParentId).ToList();
                foreach (var course in courses)
                    course.Sessions = sessionsInCourses.Where(x => x.ParentId == course.Id).ToList();

                return courses;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }


        public void AddCourse(Guid businessId, RepeatedSession course)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                AddCourseData(businessId, course);
                foreach (var session in course.Sessions)
                    AddSession(businessId, session);
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        public void UpdateCourse(Guid businessId, RepeatedSession course)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                UpdateCourseData(businessId, course);
                foreach (var session in course.Sessions)
                    UpdateSession(businessId, session);
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
            var id = reader.GetGuid(1);
            var parentId = reader.GetNullableGuid(2);
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
            SetCreateOrUpdateParameters(command, businessId, course);
            command.ExecuteNonQuery();
        }

        private void UpdateCourseData(Guid businessId, RepeatedSession course)
        {
            var command = new SqlCommand("[Session_UpdateCourse]", Connection) { CommandType = CommandType.StoredProcedure };
            SetCreateOrUpdateParameters(command, businessId, course);
            command.ExecuteNonQuery();
        }

        private void SetCreateOrUpdateParameters(SqlCommand command, Guid businessId, RepeatedSession course)
        {
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
        }
    }
}
