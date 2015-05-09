using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachSeek.Common.Services.Templating
{
    public class NameValuePlaceholderFormatProvider
    {
        public string GetPlaceholderFormat()
        {
            return "{0}:{1}";
        }
    }
}
