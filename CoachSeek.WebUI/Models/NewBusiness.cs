using CoachSeek.WebUI.Contracts.Persistence;

namespace CoachSeek.WebUI.Models
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