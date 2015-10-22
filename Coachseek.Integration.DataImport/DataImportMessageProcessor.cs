using System;
using System.Collections.Generic;
using System.IO;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using Coachseek.Infrastructure.Queueing.Contracts.Import;
using Coachseek.Integration.Contracts.DataImport.Interfaces;
using Environment = CoachSeek.Common.Environment;

namespace Coachseek.Integration.DataImport
{
    public class DataImportMessageProcessor : IDataImportMessageProcessor
    {
        public IDataImportProcessorConfiguration DataImportProcessorConfiguration { get; private set; }
        public IDataAccessFactory DataAccessFactory { get; private set; }
        private ICustomerAddUseCase CustomerAddUseCase { get; set; }

        public DataImportMessageProcessor(IDataImportProcessorConfiguration dataImportProcessorConfiguration,
                                          IDataAccessFactory dataAccessFactory,
                                          ICustomerAddUseCase customerAddUseCase)
        {
            DataImportProcessorConfiguration = dataImportProcessorConfiguration;
            DataAccessFactory = dataAccessFactory;
            CustomerAddUseCase = customerAddUseCase;
        }


        public void ProcessMessage(DataImportMessage message)
        {
            CustomerAddUseCase.Initialise(CreateApplicationContext(message.BusinessId));
            var commands = SplitMessageIntoCustomers(message);
            foreach (var command in commands)
                CustomerAddUseCase.AddCustomerAsync(command).Wait();
        }

        private IList<CustomerAddCommand> SplitMessageIntoCustomers(DataImportMessage message)
        {
            var commands = new List<CustomerAddCommand>();
            using (var reader = new StringReader(message.Contents))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    var cmd = ConvertLineToCustomerAddCommand(line, message.DataFormat);
                    commands.Add(cmd);
                    line = reader.ReadLine();
                }
            }
            return commands;
        }

        private CustomerAddCommand ConvertLineToCustomerAddCommand(string line, string dataFormat)
        {
            // Assume it's CSV for now.
            var parts = line.Split(',');
            return new CustomerAddCommand(parts[0], parts[1], parts[2], parts[3]);
        }

        private DataRepositories GetDataAccess()
        {
            var isTesting = DataImportProcessorConfiguration.Environment != Environment.Production;
            return DataAccessFactory.CreateDataAccess(isTesting);
        }

        private ApplicationContext CreateApplicationContext(Guid businessId)
        {
            var businessContext = new BusinessContext(new Business(businessId), GetDataAccess().BusinessRepository);
            return new ApplicationContext(businessContext);
        }
    }
}
