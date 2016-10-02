using System;
using System.Collections.Generic;
using AutoMapper;

namespace CoachSeek.Data.Model
{
    public class CustomerData
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Sex { get; set; }
        public string DateOfBirth { get; set; }
        public List<CustomFieldKeyValueData> CustomFields { get; set; }
        public IList<CustomerBookingData> CustomerBookings { get; set; }

        public CustomerData()
        {
            CustomFields = new List<CustomFieldKeyValueData>();
        }


        public CustomerKeyData ToKeyData()
        {
            return Mapper.Map<CustomerData, CustomerKeyData>(this);
        }
    }
}