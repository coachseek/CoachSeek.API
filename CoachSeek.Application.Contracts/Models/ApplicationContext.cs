using System;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class ApplicationContext
    {
        public Guid? BusinessId { get; set; }
        public string BusinessName { get; set; }
        public bool IsTesting { get; set; }
        public bool ForceEmail { get; set; }
        public string EmailSender { get; set; }
        public bool IsEmailingEnabled { get; set; }

        public IBusinessRepository BusinessRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IUnsubscribedEmailAddressRepository UnsubscribedEmailAddressRepository { get; set; }
    }
}