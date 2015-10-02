using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbBookingRepository : DbRepositoryBase
    {
        public DbBookingRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


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

        public void SetBookingPaymentStatus(Guid businessId, Guid bookingId, string paymentStatus)
        {
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Booking_UpdatePaymentStatus]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = bookingId;
                command.Parameters.Add(new SqlParameter("@paymentStatus", SqlDbType.NVarChar));
                command.Parameters[2].Value = paymentStatus;

                command.ExecuteNonQuery();
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }

        public void SetBookingAttendance(Guid businessId, Guid sessionBookingId, bool? hasAttended)
        {
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Booking_UpdateHasAttended]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = sessionBookingId;
                command.Parameters.Add(new SqlParameter("@hasAttended", SqlDbType.Bit));
                command.Parameters[2].Value = hasAttended;

                command.ExecuteNonQuery();
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }

        public async Task<IList<CustomerBookingData>> GetAllCustomerBookingsAsync(Guid businessId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Booking_GetAllCustomerBookings]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var customerBookings = new List<CustomerBookingData>();
                reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    customerBookings.Add(ReadCustomerBookingData(reader));

                return customerBookings;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public IList<CustomerBookingData> GetAllCustomerBookings(Guid businessId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Booking_GetAllCustomerBookings]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var customerBookings = new List<CustomerBookingData>();
                reader = command.ExecuteReader();
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


        private CourseBookingData ReadCourseAndSessionsBookingData(SqlDataReader reader)
        {
            var id = reader.GetGuid(1);
            var parentId = reader.GetNullableGuid(2);
            var courseId = reader.GetGuid(3);
            var courseName = reader.GetString(4);
            var customerId = reader.GetGuid(5);
            var customerName = reader.GetString(6);
            var paymentStatus = reader.GetNullableString(7);
            var hasAttended = reader.GetNullableBool(8);

            var sessionBookings = new List<SingleSessionBookingData>();

            while (reader.Read())
                sessionBookings.Add(ReadSessionBookingData(reader));

            return new CourseBookingData
            {
                Id = id,
                Customer = new CustomerKeyData { Id = customerId, Name = customerName },
                Course = new SessionKeyData { Id = courseId, Name = courseName },
                SessionBookings = sessionBookings,
                PaymentStatus = paymentStatus
            };
        }

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

            command.Parameters[0].Value = businessId;
            command.Parameters[1].Value = courseBooking.Id;
            command.Parameters[2].Value = courseBooking.Course.Id;
            command.Parameters[3].Value = courseBooking.Customer.Id;
            command.Parameters[4].Value = courseBooking.PaymentStatus;

            command.ExecuteNonQuery();
        }

        private SingleSessionBookingData ReadSessionBookingData(SqlDataReader reader)
        {
            var id = reader.GetGuid(1);
            var parentId = reader.GetNullableGuid(2);
            var sessionId = reader.GetGuid(3);
            var sessionName = reader.GetString(4);
            var customerId = reader.GetGuid(5);
            var customerName = reader.GetString(6);
            var paymentStatus = reader.GetNullableString(7);
            var hasAttended = reader.GetNullableBool(8);

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
    }
}
