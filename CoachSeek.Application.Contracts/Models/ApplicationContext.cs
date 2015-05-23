using System;
using System.Runtime.InteropServices;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class ApplicationContext
    {
        public BusinessContext Business { get; private set; }
        public EmailContext Email { get; private set; }
        public bool IsTesting { get; private set; }
        //public IUserRepository UserRepository { get; set; }


        public ApplicationContext(BusinessContext businessContext, EmailContext emailContext, bool isTesting)
        {
            Business = businessContext;
            Email = emailContext;
            IsTesting = isTesting;
        }
    }
}