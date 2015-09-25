using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using Coachseek.Infrastructure.Queueing.Contracts.Import;

namespace Coachseek.Integration.DataImport
{
    public class DataImportMessageProcessor : IDataImportMessageProcessor
    {
        private ICustomerAddUseCase CustomerAddUseCase { get; set; }

        public void ProcessMessage(DataImportMessage message)
        {
            string fileContents = "";

            // Split data into entities
            IList<CustomerAddCommand> commands = SplitFileIntoCustomer(fileContents);

            foreach (var command in commands)
                CustomerAddUseCase.AddCustomer(command);
        }

        private IList<CustomerAddCommand> SplitFileIntoCustomer(string fileContents)
        {
            throw new System.NotImplementedException();
        }
    }
}
