﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BouncedEmailProcessor
{
    class AmazonSqsNotification
    {
        public string Type { get; set; }
        public string Message { get; set; }
    }

    /// <summary>Represents an Amazon SES bounce notification.</summary>
    class AmazonSesBounceNotification
    {
        public string NotificationType { get; set; }
        public AmazonSesBounce Bounce { get; set; }
    }

    /// <summary>Represents meta data for the bounce notification from Amazon SES.</summary>
    class AmazonSesBounce
    {
        public string BounceType { get; set; }
        public string BounceSubType { get; set; }
        public DateTime Timestamp { get; set; }
        public List<AmazonSesBouncedRecipient> BouncedRecipients { get; set; }
    }

    /// <summary>Represents the email address of recipients that bounced
    /// when sending from Amazon SES.</summary>
    class AmazonSesBouncedRecipient
    {
        public string EmailAddress { get; set; }
    }
}
