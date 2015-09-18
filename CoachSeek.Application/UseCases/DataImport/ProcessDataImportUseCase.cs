using System.Collections;
using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.DataImport;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases.DataImport
{
    public class ProcessDataImportUseCase : IProcessDataImportUseCase
    {
        private ICustomerAddUseCase CustomerAddUseCase { get; set; }


        public ProcessDataImportUseCase(ICustomerAddUseCase customerAddUseCase)
        {
            CustomerAddUseCase = customerAddUseCase;
        }

        public void Process()
        {
            // Read import data from queue
            string fileContents = "";

            // Split data into entities
            IList<CustomerAddCommand> commands = SplitFileIntoCustomer(fileContents);

            foreach(var command in commands)
                CustomerAddUseCase.AddCustomer(command);
        }

        private IList<CustomerAddCommand> SplitFileIntoCustomer(string fileContents)
        {
            throw new System.NotImplementedException();
        }
    }
}
