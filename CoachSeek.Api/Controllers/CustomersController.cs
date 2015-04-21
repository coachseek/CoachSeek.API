using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using System;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.Api.Controllers
{
    public class CustomersController : BaseController
    {
        public ICustomersGetAllUseCase CustomersGetAllUseCase { get; set; }
        public ICustomerGetByIdUseCase CustomerGetByIdUseCase { get; set; }
        public ICustomerAddUseCase CustomerAddUseCase { get; set; }
        public ICustomerUpdateUseCase CustomerUpdateUseCase { get; set; }

        public CustomersController()
        { }

        public CustomersController(ICustomersGetAllUseCase customersGetAllUseCase,
                                   ICustomerGetByIdUseCase customerGetByIdUseCase,
                                   ICustomerAddUseCase customerAddUseCase,
                                   ICustomerUpdateUseCase customerUpdateUseCase)
        {
            CustomersGetAllUseCase = customersGetAllUseCase;
            CustomerGetByIdUseCase = customerGetByIdUseCase;
            CustomerAddUseCase = customerAddUseCase;
            CustomerUpdateUseCase = customerUpdateUseCase;
        }


        // GET: Customers
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get()
        {
            CustomersGetAllUseCase.Initialise(BusinessRepository, BusinessId);
            var response = CustomersGetAllUseCase.GetCustomers();
            return CreateGetWebResponse(response);
        }

        // GET: Customers/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            CustomerGetByIdUseCase.Initialise(BusinessRepository, BusinessId);
            var response = CustomerGetByIdUseCase.GetCustomer(id);
            return CreateGetWebResponse(response);
        }

        // POST: Customers
        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiCustomerSaveCommand customer)
        {
            if (customer.IsNew())
                return AddCustomer(customer);

            return UpdateCustomer(customer);
        }


        private HttpResponseMessage AddCustomer(ApiCustomerSaveCommand customer)
        {
            var command = CustomerAddCommandConverter.Convert(customer);
            CustomerAddUseCase.Initialise(BusinessRepository, BusinessId);
            var response = CustomerAddUseCase.AddCustomer(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateCustomer(ApiCustomerSaveCommand customer)
        {
            var command = CustomerUpdateCommandConverter.Convert(customer);
            CustomerUpdateUseCase.Initialise(BusinessRepository, BusinessId);
            var response = CustomerUpdateUseCase.UpdateCustomer(command);
            return CreatePostWebResponse(response);
        }
    }
}
