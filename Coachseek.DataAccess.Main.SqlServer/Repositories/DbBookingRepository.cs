using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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


        public async Task<SingleSessionBookingData> GetSessionBookingAsync(Guid businessId, Guid sessionBookingId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Booking_GetSessionBookingByGuid]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = sessionBookingId;

                reader = await command.ExecuteReaderAsync();

                if (reader.HasRows && reader.Read())
                    return ReadSessionBookingData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public SingleSessionBookingData GetSessionBooking(Guid businessId, Guid sessionBookingId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                connection = OpenConnection();

                var command = new SqlCommand("[Booking_GetSessionBookingByGuid]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = sessionBookingId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadSessionBookingData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public SingleSessionBookingData AddSessionBooking(Guid businessId, SingleSessionBookingData booking)
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

        public void AppendToCourseBooking(Guid businessId, CourseBooking courseBooking)
        {
            try
            {
                Connection.Open();

                AddCourseBookingData(businessId, courseBooking);

                foreach (var sessionBooking in courseBooking.SessionBookings)
                    AddSessionBookingData(businessId, sessionBooking);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }

        public async Task<CourseBookingData> GetCourseBookingAsync(Guid businessId, Guid courseBookingId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Booking_GetCourseBookingByGuid]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = courseBookingId;

                reader = await command.ExecuteReaderAsync();

                if (reader.HasRows && reader.Read())
                    return ReadCourseAndSessionsBookingData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task<IList<CourseBookingData>> GetCourseBookingsAsync(Guid businessId, Guid courseId, Guid customerId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Booking_GetCourseBookingByCourseAndCustomer]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@courseGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = courseId;
                command.Parameters[2].Value = customerId;

                var courseBookings = new List<CourseBookingData>();
                reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    courseBookings.Add(ReadCourseAndSessionsBookingData(reader));

                return courseBookings;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public CourseBookingData GetCourseBooking(Guid businessId, Guid courseBookingId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("[Booking_GetCourseBookingByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = courseBookingId;

                reader = command.ExecuteReader();
                var courseBookings = ReadCourseBookingsData(reader);
                return courseBookings.FirstOrDefault();
                //if (reader.HasRows && reader.Read())
                //    return ReadCourseAndSessionsBookingData(reader);
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public IList<CourseBookingData> GetCourseBookings(Guid businessId, Guid courseId, Guid customerId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("[Booking_GetCourseBookingsByCourseAndCustomer]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@courseGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = courseId;
                command.Parameters[2].Value = customerId;

                reader = command.ExecuteReader();
                return ReadCourseBookingsData(reader);
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

        public async Task SetBookingPaymentStatusAsync(Guid businessId, Guid bookingId, string paymentStatus)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Booking_UpdatePaymentStatus]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@paymentStatus", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = bookingId;
                command.Parameters[2].Value = paymentStatus;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
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

        public async Task<CustomerBookingData> GetCustomerBookingAsync(Guid businessId, Guid bookingId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Booking_GetCustomerBookingByBookingId]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = bookingId;

                reader = await command.ExecuteReaderAsync();
                if (reader.HasRows && reader.Read())
                    return ReadCustomerBookingData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
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

        public IList<CustomerBookingData> GetAllCustomerSessionBookingsByCustomerId(Guid businessId, Guid customerId)
        {
            SqlDataReader reader = null;
            try
            {
                Connection.Open();

                var command = new SqlCommand("[Booking_GetAllCustomerSessionBookingsByCustomerId]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = customerId;

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

        public async Task<IList<CustomerBookingData>> GetCustomerBookingsBySessionIdAsync(Guid businessId, Guid sessionId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Booking_GetCustomerBookingsBySessionId]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@sessionGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = sessionId;

                reader = await command.ExecuteReaderAsync();

                var customerBookings = new List<CustomerBookingData>();

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

        public async Task<IList<CustomerBookingData>> GetCustomerBookingsByCourseIdAsync(Guid businessId, Guid courseId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Booking_GetCustomerBookingsByCourseId]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@courseGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = courseId;

                reader = await command.ExecuteReaderAsync();

                var customerBookings = new List<CustomerBookingData>();

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
                Customer = new CustomerKeyData(customerId, customerName),
                Course = new SessionKeyData(courseId, courseName),
                SessionBookings = sessionBookings,
                PaymentStatus = paymentStatus
            };
        }

        private IList<CourseBookingData> ReadCourseBookingsData(SqlDataReader reader)
        {
            var bookings = ReadCourseOrSessionsBookingsData(reader);
            var courseBookings = new List<CourseBookingData>();
            foreach (var booking in bookings)
            {
                if (booking.IsCourseBooking)
                    courseBookings.Add(ConvertToCourseBooking(booking));
                else
                {
                    var sessionBooking = ConvertToSessionBooking(booking);
                    var courseBooking = courseBookings.Single(x => x.Id == sessionBooking.ParentId);
                    courseBooking.SessionBookings.Add(sessionBooking);
                }
            }
            return courseBookings;
        }

        private IList<CourseOrSessionBookingData> ReadCourseOrSessionsBookingsData(SqlDataReader reader)
        {
            var bookings = new List<CourseOrSessionBookingData>();
            while (reader.Read())
                bookings.Add(ReadCourseOrSessionsBookingData(reader));
            return bookings;
        }

        private CourseBookingData ConvertToCourseBooking(CourseOrSessionBookingData booking)
        {
            return new CourseBookingData
            {
                Id = booking.Id,
                Course = new SessionKeyData(booking.CourseOrSessionId, booking.CourseOrSessionName),
                Customer = new CustomerKeyData(booking.CustomerId, booking.CustomerName),
                DiscountPercent = booking.DiscountPercent
            };
        }

        private SingleSessionBookingData ConvertToSessionBooking(CourseOrSessionBookingData booking)
        {
            return new SingleSessionBookingData
            {
                Id = booking.Id,
                ParentId = booking.ParentId,
                Session = new BookingSessionData { Id = booking.CourseOrSessionId, Name = booking.CourseOrSessionName },
                Customer = new CustomerKeyData(booking.CustomerId, booking.CustomerName),
                PaymentStatus = booking.PaymentStatus,
                HasAttended = booking.HasAttended,
                IsOnlineBooking = booking.IsOnlineBooking
            };
        }

        private CourseOrSessionBookingData ReadCourseOrSessionsBookingData(SqlDataReader reader)
        {
            var id = reader.GetGuid(1);
            var parentId = reader.GetNullableGuid(2);
            var courseOrSessionId = reader.GetGuid(3);
            var courseOrSessionName = reader.GetString(4);
            var customerId = reader.GetGuid(5);
            var customerName = reader.GetString(6);
            var paymentStatus = reader.GetNullableString(7);
            var hasAttended = reader.GetNullableBool(8);
            var isOnlineBooking = reader.GetNullableBool(9);
            var discountPercent = reader.GetInt32(10);

            return new CourseOrSessionBookingData
            {
                Id = id,
                ParentId = parentId,
                CourseOrSessionId = courseOrSessionId,
                CourseOrSessionName = courseOrSessionName,
                CustomerId = customerId,
                CustomerName = customerName,
                PaymentStatus = paymentStatus,
                HasAttended = hasAttended,
                IsOnlineBooking = isOnlineBooking,
                DiscountPercent = discountPercent
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
            var isOnlineBooking = reader.GetBoolean(11);

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
                HasAttended = hasAttended,
                IsOnlineBooking = isOnlineBooking
            };
        }

        private SingleSessionBookingData AddSessionBookingData(Guid businessId, SingleSessionBookingData booking)
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
                command.Parameters.Add(new SqlParameter("@isOnlineBooking", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@discountPercent", SqlDbType.Int));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = booking.Id;
                command.Parameters[2].Value = booking.ParentId;
                command.Parameters[3].Value = booking.Session.Id;
                command.Parameters[4].Value = booking.Customer.Id;
                command.Parameters[5].Value = booking.PaymentStatus;
                command.Parameters[6].Value = booking.HasAttended;
                command.Parameters[7].Value = booking.IsOnlineBooking;
                command.Parameters[8].Value = booking.DiscountPercent;

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
            command.Parameters.Add(new SqlParameter("@isOnlineBooking", SqlDbType.Bit));
            command.Parameters.Add(new SqlParameter("@discountPercent", SqlDbType.Int));

            command.Parameters[0].Value = businessId;
            command.Parameters[1].Value = courseBooking.Id;
            command.Parameters[2].Value = courseBooking.Course.Id;
            command.Parameters[3].Value = courseBooking.Customer.Id;
            command.Parameters[4].Value = courseBooking.PaymentStatus;
            command.Parameters[5].Value = courseBooking.IsOnlineBooking;
            command.Parameters[6].Value = courseBooking.DiscountPercent;

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
            var isOnlineBooking = reader.GetNullableBool(9);
            var discountPercent = reader.GetInt32(10);

            return new SingleSessionBookingData
            {
                Id = id,
                ParentId = parentId,
                Session = new BookingSessionData { Id = sessionId, Name = sessionName },
                Customer = new CustomerKeyData(customerId, customerName),
                PaymentStatus = paymentStatus,
                HasAttended = hasAttended,
                IsOnlineBooking = isOnlineBooking,
                DiscountPercent = discountPercent
            };
        }


        private class CourseOrSessionBookingData
        {
            public Guid Id;
            public Guid? ParentId;
            public Guid CourseOrSessionId;
            public string CourseOrSessionName;
            public Guid CustomerId;
            public string CustomerName;
            public string PaymentStatus;
            public bool? HasAttended;
            public bool? IsOnlineBooking;

            public bool IsCourseBooking
            {
                get { return ParentId == null; }
            }

            public int DiscountPercent;
        }
    }
}
