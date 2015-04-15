using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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


        public IList<CoachData> GetAllCoaches(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Coach_GetAll]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier, 0, "Guid"));
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
                wasAlreadyOpen = OpenConnection();

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
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public CoachData AddCoach(Guid businessId, Coach coach)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("Coach_Create", Connection) { CommandType = CommandType.StoredProcedure };

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
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public CoachData UpdateCoach(Guid businessId, Coach coach)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

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
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
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

    }
}
