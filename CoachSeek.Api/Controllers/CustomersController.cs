using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Exceptions;
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
        public ICustomerOnlineBookingAddUseCase CustomerOnlineBookingAddUseCase { get; set; }

        public CustomersController()
        { }

        public CustomersController(ICustomersGetAllUseCase customersGetAllUseCase,
                                   ICustomerGetByIdUseCase customerGetByIdUseCase,
                                   ICustomerAddUseCase customerAddUseCase,
                                   ICustomerUpdateUseCase customerUpdateUseCase,
                                   ICustomerOnlineBookingAddUseCase customerOnlineBookingAddUseCase)
        {
            CustomersGetAllUseCase = customersGetAllUseCase;
            CustomerGetByIdUseCase = customerGetByIdUseCase;
            CustomerAddUseCase = customerAddUseCase;
            CustomerUpdateUseCase = customerUpdateUseCase;
            CustomerOnlineBookingAddUseCase = customerOnlineBookingAddUseCase;
        }


        // GET: Customers
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get()
        {
            CustomersGetAllUseCase.Initialise(Context);
            var response = CustomersGetAllUseCase.GetCustomers();
            return CreateGetWebResponse(response);
        }

        // GET: Customers/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [Authorize]
        public HttpResponseMessage Get(Guid id)
        {
            CustomerGetByIdUseCase.Initialise(Context);
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

        // POST: OnlineBooking/Customers
        [Route("OnlineBooking/Customers")]
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage PostOnlineBooking([FromBody]ApiCustomerSaveCommand customer)
        {
            if (customer.IsExisting())
                return CreateWebErrorResponse(new UseExistingCustomerForOnlineBookingNotSupported());

            return AddOnlineBookingCustomer(customer);
        }

        private HttpResponseMessage AddCustomer(ApiCustomerSaveCommand customer)
        {
            var command = CustomerAddCommandConverter.Convert(customer);
            CustomerAddUseCase.Initialise(Context);
            var response = CustomerAddUseCase.AddCustomer(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateCustomer(ApiCustomerSaveCommand customer)
        {
            var command = CustomerUpdateCommandConverter.Convert(customer);
            CustomerUpdateUseCase.Initialise(Context);
            var response = CustomerUpdateUseCase.UpdateCustomer(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage AddOnlineBookingCustomer(ApiCustomerSaveCommand customer)
        {
            var command = CustomerAddCommandConverter.Convert(customer);
            CustomerOnlineBookingAddUseCase.Initialise(Context);
            var response = CustomerOnlineBookingAddUseCase.AddCustomer(command);
            return CreatePostWebResponse(response);
        }
    }
}
