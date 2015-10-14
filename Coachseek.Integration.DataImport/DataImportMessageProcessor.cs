using System.Collections.Generic;
using System.IO;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using Coachseek.Infrastructure.Queueing.Contracts.Import;

namespace Coachseek.Integration.DataImport
{
    public class DataImportMessageProcessor : IDataImportMessageProcessor
    {
        private ICustomerAddUseCase CustomerAddUseCase { get; set; }

        public DataImportMessageProcessor(ICustomerAddUseCase customerAddUseCase)
        {
            CustomerAddUseCase = customerAddUseCase;
        }


        public void ProcessMessage(DataImportMessage message)
        {
            CustomerAddUseCase.Initialise(null);

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
    }
}
