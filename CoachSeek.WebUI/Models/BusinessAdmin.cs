using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Exceptions;
using CoachSeek.WebUI.Extensions;
using CoachSeek.WebUI.Properties;

namespace CoachSeek.WebUI.Models
{
    public class BusinessAdmin
    {
        public Identifier Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public BusinessAdmin()
        {
            Id = new Identifier();
        }


        public void Validate(IBusinessRepository repository)
        {
            var admin = repository.GetByAdminEmail(Email);
            if (admin.IsExisting())
                ThrowBusinessAdminDuplicateEmailValiationException();
        }


        private void ThrowBusinessAdminDuplicateEmailValiationException()
        {
            throw new ValidationException((int)ErrorCodes.ErrorBusinessAdminDuplicateEmail,
                                          Resources.ErrorBusinessAdminDuplicateEmail,
                                          FormField.Email);
        }
    }
}