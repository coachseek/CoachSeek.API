using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities.EmailTemplating;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbEmailTemplateRepository : DbRepositoryBase
    {
        public DbEmailTemplateRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


        public IList<EmailTemplateData> GetAllEmailTemplates(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[EmailTemplate_GetAll]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var templates = new List<EmailTemplateData>();
                reader = command.ExecuteReader();
                while (reader.Read())
                    templates.Add(ReadEmailTemplateData(reader));

                return templates;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public EmailTemplateData GetEmailTemplate(Guid businessId, string templateType)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[EmailTemplate_GetByType]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@templateType", SqlDbType.NVarChar));
                command.Parameters[1].Value = templateType;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadEmailTemplateData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public void AddEmailTemplate(Guid businessId, EmailTemplate emailTemplate)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[EmailTemplate_Create]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@subject", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@body", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = emailTemplate.Type;
                command.Parameters[2].Value = emailTemplate.Subject;
                command.Parameters[3].Value = emailTemplate.Body;

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        public void UpdateEmailTemplate(Guid businessId, EmailTemplate emailTemplate)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[EmailTemplate_Update]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@subject", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@body", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = emailTemplate.Type;
                command.Parameters[2].Value = emailTemplate.Subject;
                command.Parameters[3].Value = emailTemplate.Body;

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        public void DeleteEmailTemplate(Guid businessId, string templateType)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[EmailTemplate_DeleteByType]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = templateType;

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }


        private EmailTemplateData ReadEmailTemplateData(SqlDataReader reader)
        {
            return new EmailTemplateData
            {
                Type = reader.GetString(1),
                Subject = reader.GetString(2),
                Body = reader.GetNullableString(3),
            };
        }
    }
}
