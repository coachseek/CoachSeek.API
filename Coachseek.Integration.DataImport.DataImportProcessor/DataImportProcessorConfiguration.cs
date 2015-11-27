using System;
using CoachSeek.Common.Extensions;
using Coachseek.Integration.Contracts.DataImport.Interfaces;
using Environment = CoachSeek.Common.Environment;

namespace Coachseek.Integration.DataImport.DataImportProcessor
{
    public class DataImportProcessorConfiguration : IDataImportProcessorConfiguration
    {
        public Environment Environment
        {
            get
            {
                var configEnvironment = AppSettings.Environment;
                foreach (Environment environment in Enum.GetValues(typeof (Environment)))
                {
                    var environmentName = Enum.GetName(typeof (Environment), environment);
                    if (configEnvironment == environmentName)
                        return environment;
                }

                return Environment.Debug;
            }
        }


        public string EmailSender
        {
            get { return AppSettings.EmailSender; }
        }

        public bool IsEmailingEnabled
        {
            get { return AppSettings.IsEmailingEnabled.Parse(true); }
        }
    }
}
