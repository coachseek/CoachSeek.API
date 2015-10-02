﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbLocationRepository : DbRepositoryBase
    {
        public DbLocationRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


        public async Task<IList<LocationData>> GetAllLocationsAsync(Guid businessId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Location_GetAll]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var locations = new List<LocationData>();
                reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    locations.Add(ReadLocationData(reader));

                return locations;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public IList<LocationData> GetAllLocations(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Location_GetAll]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var locations = new List<LocationData>();
                reader = command.ExecuteReader();
                while (reader.Read())
                    locations.Add(ReadLocationData(reader));

                return locations;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }


        public LocationData GetLocation(Guid businessId, Guid locationId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Location_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@locationGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = locationId;

                reader = command.ExecuteReader();
                if (reader.HasRows && reader.Read())
                    return ReadLocationData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public void AddLocation(Guid businessId, Location location)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("Location_Create", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@locationGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = location.Id;
                command.Parameters[2].Value = location.Name;

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        public void UpdateLocation(Guid businessId, Location location)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("Location_Update", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@locationGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = location.Id;
                command.Parameters[2].Value = location.Name;

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }


        private LocationData ReadLocationData(SqlDataReader reader)
        {
            return new LocationData
            {
                Id = reader.GetGuid(1),
                Name = reader.GetString(2)
            };
        }
    }
}
