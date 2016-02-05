using System;
using System.Collections.Generic;

namespace CoachSeek.Domain.Commands
{
    public class CustomFieldValueListUpdateCommand
    {
        public string Type { get; set; }
        public Guid TypeId { get; set; }

        public List<CustomFieldValueUpdateCommand> CustomFields { get; set; }

        //public string Type
        //{
        //    set
        //    {
        //        foreach (var fieldValue in Values)
        //            fieldValue.Type = value;
        //    }
        //}

        //public Guid TypeId
        //{
        //    set 
        //    { 
        //        foreach (var fieldValue in Values)
        //            fieldValue.TypeId = value;
        //    }
        //}

        CustomFieldValueListUpdateCommand()
        {
            CustomFields = new List<CustomFieldValueUpdateCommand>();
        }
    }
}
