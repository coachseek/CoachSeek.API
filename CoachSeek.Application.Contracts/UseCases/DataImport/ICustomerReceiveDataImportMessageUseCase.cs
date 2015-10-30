using System;
using System.Threading.Tasks;

namespace CoachSeek.Application.Contracts.UseCases.DataImport
{
    public interface ICustomerReceiveDataImportMessageUseCase : IApplicationContextSetter
    {
        Task ReceiveAsync(Guid businessId, string importData);
    }
}
