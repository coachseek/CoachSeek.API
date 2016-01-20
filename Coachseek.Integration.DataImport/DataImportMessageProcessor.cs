using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.Services.Emailing;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
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
        private IDataImportProcessEmailer DataImportProcessEmailer { get; set; }

        private string EmailSender { get { return DataImportProcessorConfiguration.EmailSender; } }
        private bool IsEmailingEnabled { get { return DataImportProcessorConfiguration.IsEmailingEnabled; } }

        public DataImportMessageProcessor(IDataImportProcessorConfiguration dataImportProcessorConfiguration,
                                          IDataAccessFactory dataAccessFactory,
                                          ICustomerAddUseCase customerAddUseCase,
                                          IDataImportProcessEmailer dataImportProcessEmailer)
        {
            DataImportProcessorConfiguration = dataImportProcessorConfiguration;
            DataAccessFactory = dataAccessFactory;
            CustomerAddUseCase = customerAddUseCase;
            DataImportProcessEmailer = dataImportProcessEmailer;
        }


        public async Task ProcessMessageAsync(DataImportMessage message)
        {
            var errors = new List<string>();

            var context = CreateApplicationContext(message.BusinessId);

            CustomerAddUseCase.Initialise(context);
            var lines = SplitMessageIntoLines(message);
            foreach (var line in lines)
            {
                try
                {
                    var command = ConvertLineToCustomerAddCommand(line, message.DataFormat);
                    var response = await CustomerAddUseCase.AddCustomerAsync(command);
                }
                catch (CoachseekException ex)
                {
                    // TODO: Put into errors collection the original line that errored plus the error message.
                    //errors.Add();
                    throw;
                }
            }


            DataImportProcessEmailer.Initialise(context);
            DataImportProcessEmailer.SendProcessingSuccessfulEmail();
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

        private IList<string> SplitMessageIntoLines(DataImportMessage message)
        {
            var lines = new List<string>();
            using (var reader = new StringReader(message.Contents))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
            }
            return lines;
        }

        private CustomerAddCommand ConvertLineToCustomerAddCommand(string line, string dataFormat)
        {
            // Assume it's CSV for now.
            var parts = line.Split(',');
            if (parts.Length != 4)
                throw new NotEnoughDataParts();
            return new CustomerAddCommand(parts[0], parts[1], parts[2], parts[3]);
        }

        private DataRepositories GetDataAccess()
        {
            var isTesting = DataImportProcessorConfiguration.Environment != Environment.Production;
            return DataAccessFactory.CreateDataAccess(isTesting);
        }

        private ApplicationContext CreateApplicationContext(Guid businessId)
        {
            var userContext = new UserContext(GetDataAccess().UserRepository);
            var businessContext = new BusinessContext(new Business(businessId), 
                                                      GetDataAccess().BusinessRepository,
                                                      GetDataAccess().UserRepository);
            var emailContext = new EmailContext(IsEmailingEnabled, 
                                                false, 
                                                EmailSender, 
                                                GetDataAccess().UnsubscribedEmailAddressRepository);
            return new ApplicationContext(userContext, businessContext, emailContext, GetDataAccess().LogRepository, false);
        }
    }
}
