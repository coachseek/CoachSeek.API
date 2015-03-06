using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using System;
using System.Net.Http;
using System.Web.Http;
using CoachSeek.Application.UseCases;

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


        // GET: api/Customers
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get()
        {
            CustomersGetAllUseCase.BusinessId = BusinessId;
            CustomersGetAllUseCase.BusinessRepository = BusinessRepository;

            var response = CustomersGetAllUseCase.GetCustomers();
            return CreateGetWebResponse(response);
        }

        // GET: api/Customers/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            CustomerGetByIdUseCase.BusinessId = BusinessId;
            CustomerGetByIdUseCase.BusinessRepository = BusinessRepository;

            var response = CustomerGetByIdUseCase.GetCustomer(id);
            return CreateGetWebResponse(response);
        }

        // POST: api/Customers
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
            CustomerAddUseCase.BusinessId = BusinessId;
            CustomerAddUseCase.BusinessRepository = BusinessRepository;

            var response = CustomerAddUseCase.AddCustomer(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateCustomer(ApiCustomerSaveCommand customer)
        {
            var command = CustomerUpdateCommandConverter.Convert(customer);
            CustomerUpdateUseCase.BusinessId = BusinessId;
            CustomerUpdateUseCase.BusinessRepository = BusinessRepository;

            var response = CustomerUpdateUseCase.UpdateCustomer(command);
            return CreatePostWebResponse(response);
        }
    }
}
