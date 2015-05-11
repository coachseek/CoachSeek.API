﻿using System;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class ApplicationContext
    {
        public Guid? BusinessId { get; set; }
        public bool IsTesting { get; set; }
        public bool ForceEmail { get; set; }
        public string EmailSender { get; set; }

        public IBusinessRepository BusinessRepository { get; set; }
    }
}