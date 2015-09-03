using System;

namespace CoachSeek.Application.Contracts.UseCases.DataImport
{
    public interface ICustomerReceiveDataImportMessageUseCase : IApplicationContextSetter
    {
        void Receive(Guid businessId, string importData);
    }
}
