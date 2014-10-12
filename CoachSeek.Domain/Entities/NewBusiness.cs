using CoachSeek.Domain.Data;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities
{
    public class NewBusiness : Business
    {
        public void Register(IBusinessRepository repository)
        {
            Validate(repository);

            repository.Save(this);
        }


        private void Validate(IBusinessRepository repository)
        {
            Admin.Validate(repository);
        }
    }
}