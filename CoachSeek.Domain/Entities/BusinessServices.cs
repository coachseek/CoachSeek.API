using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace CoachSeek.Domain.Entities
{
    public class BusinessServices
    {
        private List<Service> Services { get; set; }

        public BusinessServices()
        {
            Services = new List<Service>();
        }

        public BusinessServices(IEnumerable<ServiceData> services)
            : this()
        {
            if (services == null)
                return;

            foreach (var service in services)
                Append(service);
        }


        public ServiceData GetById(Guid id)
        {
            var service = Services.FirstOrDefault(x => x.Id == id);
            if (service == null)
                return null;
            return service.ToData();
        }

        //public Guid Add(NewServiceData newServiceData)
        //{
        //    var newService = new NewService(newServiceData);
        //    ValidateAdd(newService);
        //    Services.Add(newService);

        //    return newService.Id;
        //}

        public void Append(ServiceData serviceData)
        {
            // Data is already valid. Eg. It comes from the database.
            Services.Add(new Service(serviceData));
        }

        public void Update(ServiceData serviceData)
        {
            var service = new Service(serviceData);
            ValidateUpdate(service);
            ReplaceServiceInServices(service);
        }

        public IList<ServiceData> ToData()
        {
            return Services.Select(service => service.ToData()).ToList();
        }


        private void ReplaceServiceInServices(Service service)
        {
            var updateService = Services.Single(x => x.Id == service.Id);
            var updateIndex = Services.IndexOf(updateService);
            Services[updateIndex] = service;
        }

        //private void ValidateAdd(NewService newService)
        //{
        //    var isExistingService = Services.Any(x => x.Name.ToLower() == newService.Name.ToLower());
        //    if (isExistingService)
        //        throw new DuplicateService();
        //}

        private void ValidateUpdate(Service service)
        {
            var isExistingService = Services.Any(x => x.Id == service.Id);
            if (!isExistingService)
                throw new InvalidService();

            var existingService = Services.FirstOrDefault(x => x.Name.ToLower() == service.Name.ToLower());
            if (existingService != null && existingService.Id != service.Id)
                throw new DuplicateService();
        }
    }
}
