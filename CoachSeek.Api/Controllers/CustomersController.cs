using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class CustomersController : BaseController
    {
        //public ICoachesGetAllUseCase CoachesGetAllUseCase { get; set; }
        //public ICoachGetByIdUseCase CoachGetByIdUseCase { get; set; }
        public ICustomerAddUseCase CustomerAddUseCase { get; set; }
        public ICustomerUpdateUseCase CustomerUpdateUseCase { get; set; }

        public CustomersController()
        { }

        public CustomersController(//ICoachesGetAllUseCase coachesGetAllUseCase,
                                   //ICoachGetByIdUseCase coachGetByIdUseCase,
                                   ICustomerAddUseCase customerAddUseCase,
                                   ICustomerUpdateUseCase customerUpdateUseCase)
        {
            //CoachesGetAllUseCase = coachesGetAllUseCase;
            //CoachGetByIdUseCase = coachGetByIdUseCase;
            CustomerAddUseCase = customerAddUseCase;
            CustomerUpdateUseCase = customerUpdateUseCase;
        }


        //// GET: api/Coaches
        //[BasicAuthentication]
        //[Authorize]
        //public HttpResponseMessage Get()
        //{
        //    CoachesGetAllUseCase.BusinessId = BusinessId;
        //    var response = CoachesGetAllUseCase.GetCoaches();
        //    return CreateGetWebResponse(response);
        //}

        //// GET: api/Coaches/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        //[BasicAuthentication]
        //[Authorize]
        //public HttpResponseMessage Get(Guid id)
        //{
        //    CoachGetByIdUseCase.BusinessId = BusinessId;
        //    var response = CoachGetByIdUseCase.GetCoach(id);
        //    return CreateGetWebResponse(response);
        //}

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
            var command = CustomerAddCommandConverter.Convert(BusinessId, customer);
            var response = CustomerAddUseCase.AddCustomer(command);
            return CreatePostWebResponse(response);
        }

        private HttpResponseMessage UpdateCustomer(ApiCustomerSaveCommand customer)
        {
            var command = CustomerUpdateCommandConverter.Convert(BusinessId, customer);
            var response = CustomerUpdateUseCase.UpdateCustomer(command);
            return CreatePostWebResponse(response);
        }
    }
}
