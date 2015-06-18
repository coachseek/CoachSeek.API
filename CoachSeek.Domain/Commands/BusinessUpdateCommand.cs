﻿namespace CoachSeek.Domain.Commands
{
    public class BusinessUpdateCommand
    {
        // Business Id is not included because we will be in a business context anyway.

        public string Name { get; set; }
        public BusinessPaymentCommand Payment { get; set; }
    }
}
