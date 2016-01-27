using System.Collections.Generic;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiCustomFieldValueListSaveCommand
    {
        public List<ApiCustomFieldValueSaveCommand> Values { get; set; }
    }
}