using System.Collections;
using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases.DataImport;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases.DataImport
{
    public class ProcessDataImportUseCase : IProcessDataImportUseCase
    {
        public void Process()
        {
            // Read import data from queue

            // Split data into entities

            // Save entity data to the database.
        }
    }
}
