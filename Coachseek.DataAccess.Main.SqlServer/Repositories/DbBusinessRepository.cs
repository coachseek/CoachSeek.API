using System;
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


        private DbLocationRepository LocationRepository { get; set; }
        private DbCoachRepository CoachRepository { get; set; }
        private DbServiceRepository ServiceRepository { get; set; }
        private DbCustomerRepository CustomerRepository { get; set; }
        private DbSessionRepository SessionRepository { get; set; }
        private DbCourseRepository CourseRepository { get; set; }


        public DbBusinessRepository()
        {
            LocationRepository = new DbLocationRepository(ConnectionStringKey);
            CoachRepository = new DbCoachRepository(ConnectionStringKey);
            ServiceRepository = new DbServiceRepository(ConnectionStringKey);
            CustomerRepository = new DbCustomerRepository(ConnectionStringKey);
            SessionRepository = new DbSessionRepository(ConnectionStringKey);
            CourseRepository = new DbCourseRepository(ConnectionStringKey);
        }



        public BusinessData GetBusiness(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Business_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadBusinessData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public BusinessData GetBusiness(string domain)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Business_GetByDomain]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar));
                command.Parameters[0].Value = domain;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadBusinessData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public BusinessData AddBusiness(Business business)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("Business_Create", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@currency", SqlDbType.NChar));

                command.Parameters[0].Value = business.Id;
                command.Parameters[1].Value = business.Name;
                command.Parameters[2].Value = business.Domain;
                command.Parameters[3].Value = business.Currency.Code;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadBusinessData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public BusinessData UpdateBusiness(Business business)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("Business_Update", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@currency", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@isOnlinePaymentEnabled", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@forceOnlinePayment", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@paymentProvider", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@merchantAccountIdentifier", SqlDbType.NVarChar));

                command.Parameters[0].Value = business.Id;
                command.Parameters[1].Value = business.Name;
                command.Parameters[2].Value = business.Currency.Code;
                command.Parameters[3].Value = business.Payment.IsOnlinePaymentEnabled;
                command.Parameters[4].Value = business.Payment.ForceOnlinePayment;
                command.Parameters[5].Value = business.Payment.Provider;
                command.Parameters[6].Value = business.Payment.MerchantAccountIdentifier;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadBusinessData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }


        public IList<LocationData> GetAllLocations(Guid businessId)
        {
            return LocationRepository.GetAllLocations(businessId);
        }

        public LocationData GetLocation(Guid businessId, Guid locationId)
        {
            return LocationRepository.GetLocation(businessId, locationId);
        }

        public LocationData AddLocation(Guid businessId, Location location)
        {
            return LocationRepository.AddLocation(businessId, location);
        }

        public LocationData UpdateLocation(Guid businessId, Location location)
        {
            return LocationRepository.UpdateLocation(businessId, location);
        }


        public IList<CoachData> GetAllCoaches(Guid businessId)
        {
            return CoachRepository.GetAllCoaches(businessId);
        }

        public CoachData GetCoach(Guid businessId, Guid coachId)
        {
            return CoachRepository.GetCoach(businessId, coachId);
        }

        public CoachData AddCoach(Guid businessId, Coach coach)
        {
            return CoachRepository.AddCoach(businessId, coach);
        }

        public CoachData UpdateCoach(Guid businessId, Coach coach)
        {
            return CoachRepository.UpdateCoach(businessId, coach);
        }


        public IList<ServiceData> GetAllServices(Guid businessId)
        {
            return ServiceRepository.GetAllServices(businessId);
        }

        public ServiceData GetService(Guid businessId, Guid serviceId)
        {
            return ServiceRepository.GetService(businessId, serviceId);
        }

        public ServiceData AddService(Guid businessId, Service service)
        {
            return ServiceRepository.AddService(businessId, service);
        }

        public ServiceData UpdateService(Guid businessId, Service service)
        {
            return ServiceRepository.UpdateService(businessId, service);
        }


        public IList<CustomerData> GetAllCustomers(Guid businessId)
        {
            return CustomerRepository.GetAllCustomers(businessId);
        }

        public CustomerData GetCustomer(Guid businessId, Guid customerId)
        {
            return CustomerRepository.GetCustomer(businessId, customerId);
        }

        public CustomerData AddCustomer(Guid businessId, Customer customer)
        {
            return CustomerRepository.AddCustomer(businessId, customer);
        }

        public CustomerData UpdateCustomer(Guid businessId, Customer customer)
        {
            return CustomerRepository.UpdateCustomer(businessId, customer);
        }


        public IList<SingleSessionData> GetAllStandaloneSessions(Guid businessId)
        {
            return SessionRepository.GetAllStandaloneSessions(businessId);
        }

        public IList<SingleSessionData> GetAllSessions(Guid businessId)
        {
            return SessionRepository.GetAllSessions(businessId);
        }

        public SingleSessionData GetSession(Guid businessId, Guid sessionId)
        {
            return SessionRepository.GetSession(businessId, sessionId);
        }

        public SingleSessionData AddSession(Guid businessId, StandaloneSession session)
        {
            return SessionRepository.AddSession(businessId, session);
        }

        public SingleSessionData UpdateSession(Guid businessId, SingleSession session)
        {
            return SessionRepository.UpdateSession(businessId, session);
        }

        public void DeleteSession(Guid businessId, Guid sessionId)
        {
            SessionRepository.DeleteSession(businessId, sessionId);
        }


        public IList<RepeatedSessionData> GetAllCourses(Guid businessId)
        {
            return CourseRepository.GetAllCourses(businessId);
        }

        public RepeatedSessionData GetCourse(Guid businessId, Guid courseId)
        {
            return CourseRepository.GetCourse(businessId, courseId);
        }

        public RepeatedSessionData AddCourse(Guid businessId, RepeatedSession course)
        {
            return CourseRepository.AddCourse(businessId, course);
        }

        public RepeatedSessionData UpdateCourse(Guid businessId, RepeatedSession course)
        {
            return CourseRepository.UpdateCourse(businessId, course);
        }

        public void DeleteCourse(Guid businessId, Guid courseId)
        {
            CourseRepository.DeleteCourse(businessId, courseId);
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

        public void UpdateBooking(Guid businessId, BookingData booking)
        {
            try
            {
                Connection.Open();

                UpdateSessionBookingData(businessId, booking);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
            }
        }

        private void UpdateSessionBookingData(Guid businessId, BookingData booking)
        {
            var command = new SqlCommand("Booking_Update", Connection) { CommandType = CommandType.StoredProcedure };

            command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@bookingGuid", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@paymentStatus", SqlDbType.NVarChar));
            command.Parameters.Add(new SqlParameter("@hasAttended", SqlDbType.Bit));

            command.Parameters[0].Value = businessId;
            command.Parameters[1].Value = booking.Id;
            command.Parameters[2].Value = booking.PaymentStatus.ConvertNullToDbNull();
            command.Parameters[3].Value = booking.HasAttended.ConvertNullToDbNull();

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

        private BusinessData ReadBusinessData(SqlDataReader reader)
        {
            return new BusinessData
            {
                Id = reader.GetGuid(1),
                Name = reader.GetString(2),
                Domain = reader.GetString(3),
                Currency = reader.GetNullableString(4),
                IsOnlinePaymentEnabled = reader.GetBoolean(5),
                ForceOnlinePayment = reader.GetBoolean(6),
                PaymentProvider = reader.GetNullableString(7),
                MerchantAccountIdentifier = reader.GetNullableString(8)
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
