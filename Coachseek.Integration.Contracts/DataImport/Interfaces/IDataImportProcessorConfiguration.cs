﻿using CoachSeek.Common;

namespace Coachseek.Integration.Contracts.DataImport.Interfaces
{
    public interface IDataImportProcessorConfiguration
    {
        Environment Environment { get; }
        string EmailSender { get; }
        bool IsEmailingEnabled { get; }
    }
}
