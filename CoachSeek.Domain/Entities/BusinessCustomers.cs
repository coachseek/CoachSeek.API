using System;
using CoachSeek.Data.Model;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class BusinessCustomers
    {
        private List<Customer> Customers { get; set; }

        public BusinessCustomers()
        {
            Customers = new List<Customer>();
        }

        public BusinessCustomers(IEnumerable<CustomerData> customers) 
            : this()
        {
            if (customers == null)
                return;

            foreach (var customer in customers)
                Append(customer);
        }


        public CustomerData GetById(Guid id)
        {
            var customer = Customers.FirstOrDefault(x => x.Id == id);
            if (customer == null)
                return null;
            return customer.ToData();
        }

        //public Guid Add(NewCustomerData newCustomerData)
        //{
        //    var newCustomer = new NewCustomer(newCustomerData);
        //    ValidateAdd(newCustomer);
        //    Customers.Add(newCustomer);

        //    return newCustomer.Id;
        //}

        public void Append(CustomerData customerData)
        {
            // Data is not Validated. Eg. It comes from the database.
            Customers.Add(new Customer(customerData));
        }

        public void Update(CustomerData customerData)
        {
            var customer = new Customer(customerData);
            ValidateUpdate(customer);
            ReplaceCustomerInCustomers(customer);
        }

        public IList<CustomerData> ToData()
        {
            return Customers.Select(customer => customer.ToData()).ToList();
        }


        private void ReplaceCustomerInCustomers(Customer customer)
        {
            var updateCustomer = Customers.Single(x => x.Id == customer.Id);
            var updateIndex = Customers.IndexOf(updateCustomer);
            Customers[updateIndex] = customer;
        }

        //private void ValidateAdd(NewCustomer newCustomer)
        //{
        //    //var isExistingCoach = Coaches.Any(x => x.Name.ToLower() == newCoach.Name.ToLower());
        //    //if (isExistingCoach)
        //    //    throw new DuplicateCoach();
        //}

        private void ValidateUpdate(Customer customer)
        {
            var isExistingCustomer = Customers.Any(x => x.Id == customer.Id);
            if (!isExistingCustomer)
                throw new InvalidCustomer();

        //    var existingCoach = Coaches.FirstOrDefault(x => x.Name.ToLower() == coach.Name.ToLower());
        //    if (existingCoach != null && existingCoach.Id != coach.Id)
        //        throw new DuplicateCoach();
        }
    }
}