using System;

namespace Coachseek.Infrastructure.Queueing.Contracts.Import
{
    public class DataImportMessage : IMessage
    {
        public string Id { get; private set; }
        public Guid BusinessId { get; private set; }
        public string EntityType { get; private set; }
        public string DataFormat { get; private set; }
        public string Contents { get; private set; }


        private DataImportMessage(string id, Guid businessId, string data, string entityType = "Customer", string dataFormat = "CSV")
        {
            Id = id;
            BusinessId = businessId;
            EntityType = entityType;
            DataFormat = dataFormat;
            Contents = data;
        }

        public DataImportMessage(string id, string payloadString)
        {
            if (payloadString == null)
                throw new ArgumentNullException("payloadString");

            Id = id;
            var parts = payloadString.Split('|');
            BusinessId = new Guid(parts[0].Split(':')[1]);
            EntityType = parts[1].Split(':')[1];
            DataFormat = parts[2].Split(':')[1];
            Contents = parts[3].Split(':')[1];
        }

        public static DataImportMessage Create(Guid businessId, string data, string entityType = "Customer", string dataFormat = "CSV")
        {
            return new DataImportMessage(null, businessId, data, entityType, dataFormat);
        }

        public override string ToString()
        {
            return string.Format("BusinessId:{0}|EntityType:{1}|DataFormat:{2}|Contents:{3}", 
                                 BusinessId, EntityType, DataFormat, Contents);
        }
    }
}
