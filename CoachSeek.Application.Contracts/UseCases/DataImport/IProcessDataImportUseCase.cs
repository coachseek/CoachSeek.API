using System.Threading.Tasks;

namespace CoachSeek.Application.Contracts.UseCases.DataImport
{
    public interface IProcessDataImportUseCase
    {
        Task ProcessAsync();
    }
}
