using System;

namespace CoachSeek.Application.Contracts.UseCases.Import
{
    public interface ICustomerReceiveDataImportMessageUseCase
    {
        void Receive(Guid businessId, string importData);
    }
}
