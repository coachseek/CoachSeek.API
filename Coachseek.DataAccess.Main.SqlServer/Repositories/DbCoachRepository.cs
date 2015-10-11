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
    public class DbCoachRepository : DbRepositoryBase
    {
        public DbCoachRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


        public async Task<IList<CoachData>> GetAllCoachesAsync(Guid businessId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Coach_GetAll]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var coaches = new List<CoachData>();
                reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    coaches.Add(ReadCoachData(reader));

                return coaches;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public IList<CoachData> GetAllCoaches(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("[Coach_GetAll]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var coaches = new List<CoachData>();
                reader = command.ExecuteReader();
                while (reader.Read())
                    coaches.Add(ReadCoachData(reader));

                return coaches;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public CoachData GetCoach(Guid businessId, Guid coachId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("[Coach_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@coachGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = coachId;

                reader = command.ExecuteReader();
                if (reader.HasRows && reader.Read())
                    return ReadCoachData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public void AddCoach(Guid businessId, Coach coach)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("[Coach_Create]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@coachGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@mondayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@mondayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@mondayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@tuesdayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@tuesdayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@tuesdayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@wednesdayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@wednesdayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@wednesdayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@thursdayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@thursdayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@thursdayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@fridayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@fridayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@fridayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@saturdayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@saturdayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@saturdayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@sundayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@sundayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@sundayFinishTime", SqlDbType.NChar));

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

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        public void UpdateCoach(Guid businessId, Coach coach)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("[Coach_Update]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@coachGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@mondayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@mondayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@mondayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@tuesdayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@tuesdayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@tuesdayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@wednesdayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@wednesdayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@wednesdayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@thursdayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@thursdayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@thursdayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@fridayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@fridayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@fridayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@saturdayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@saturdayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@saturdayFinishTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@sundayIsAvailable", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@sundayStartTime", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@sundayFinishTime", SqlDbType.NChar));

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

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }


        private CoachData ReadCoachData(SqlDataReader reader)
        {
            return new CoachData
            {
                Id = reader.GetGuid(1),
                FirstName = reader.GetString(2),
                LastName = reader.GetString(3),
                Email = reader.GetString(4),
                Phone = reader.GetString(5),
                WorkingHours = new WeeklyWorkingHoursData
                {
                    Monday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(6),
                        StartTime = reader.GetNullableStringTrimmed(7),
                        FinishTime = reader.GetNullableStringTrimmed(8)
                    },
                    Tuesday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(9),
                        StartTime = reader.GetNullableStringTrimmed(10),
                        FinishTime = reader.GetNullableStringTrimmed(11)
                    },
                    Wednesday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(12),
                        StartTime = reader.GetNullableStringTrimmed(13),
                        FinishTime = reader.GetNullableStringTrimmed(14)
                    },
                    Thursday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(15),
                        StartTime = reader.GetNullableStringTrimmed(16),
                        FinishTime = reader.GetNullableStringTrimmed(17)
                    },
                    Friday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(18),
                        StartTime = reader.GetNullableStringTrimmed(19),
                       FinishTime = reader.GetNullableStringTrimmed(20)
                    },
                    Saturday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(21),
                        StartTime = reader.GetNullableStringTrimmed(22),
                        FinishTime = reader.GetNullableStringTrimmed(23)
                    },
                    Sunday = new DailyWorkingHoursData
                    {
                        IsAvailable = reader.GetBoolean(24),
                        StartTime = reader.GetNullableStringTrimmed(25),
                        FinishTime = reader.GetNullableStringTrimmed(26)
                    }
                }
            };
        }
    }
}
