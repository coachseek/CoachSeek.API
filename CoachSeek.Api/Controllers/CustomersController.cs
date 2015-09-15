using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.DataImport;
using CoachSeek.Domain.Exceptions;
using System;
using System.Net.Http;
using System.Web.Http;
using Coachseek.Integration.Contracts.DataImport;

namespace CoachSeek.Api.Controllers
{
    public class CustomersController : BaseController
    {
        public ICustomersGetAllUseCase CustomersGetAllUseCase { get; set; }
        public ICustomerGetByIdUseCase CustomerGetByIdUseCase { get; set; }
        public ICustomerAddUseCase CustomerAddUseCase { get; set; }
        public ICustomerUpdateUseCase CustomerUpdateUseCase { get; set; }
        public ICustomerOnlineBookingAddUseCase CustomerOnlineBookingAddUseCase { get; set; }
        public ICustomerReceiveDataImportMessageUseCase CustomerReceiveDataImportMessageUseCase { get; set; }

        public CustomersController()
        { }

        public CustomersController(ICustomersGetAllUseCase customersGetAllUseCase,
                                   ICustomerGetByIdUseCase customerGetByIdUseCase,
                                   ICustomerAddUseCase customerAddUseCase,
                                   ICustomerUpdateUseCase customerUpdateUseCase,
                                   ICustomerOnlineBookingAddUseCase customerOnlineBookingAddUseCase,
                                   ICustomerReceiveDataImportMessageUseCase customerReceiveDataImportMessageUseCase)
        {
            CustomersGetAllUseCase = customersGetAllUseCase;
            CustomerGetByIdUseCase = customerGetByIdUseCase;
            CustomerAddUseCase = customerAddUseCase;
            CustomerUpdateUseCase = customerUpdateUseCase;
            CustomerOnlineBookingAddUseCase = customerOnlineBookingAddUseCase;
            CustomerReceiveDataImportMessageUseCase = customerReceiveDataImportMessageUseCase;
        }


        // GET: Customers
        [BasicAuthentication]
        [BusinessAuthorize]
        public HttpResponseMessage Get()
        {
            CustomersGetAllUseCase.Initialise(Context);
            var response = CustomersGetAllUseCase.GetCustomers();
            return CreateGetWebResponse(response);
        }

        // GET: Customers/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [BusinessAuthorize]
        public HttpResponseMessage Get(Guid id)
        {
            CustomerGetByIdUseCase.Initialise(Context);
            var response = CustomerGetByIdUseCase.GetCustomer(id);
            return CreateGetWebResponse(response);
        }

        // POST: Customers
        [BasicAuthentication]
        [BusinessAuthorize]
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
 
        // POST: Customers/Upload
        [Route("Customers/Upload")]
        [BasicAuthentication]
        [BusinessAuthorize]
        [HttpPost]
        public async Task<IHttpActionResult> ImportData()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            CustomerReceiveDataImportMessageUseCase.Initialise(Context);
            File importFile = null;
            try
            {
                foreach (var file in await ReadDataImportFiles())
                {
                    importFile = file;
                    CustomerReceiveDataImportMessageUseCase.Receive(Business.Id, file.Content);
                }
            }
            catch (DataImportException ex)
            {
                if (importFile != null)
                    LogRepository.LogError(ex, importFile.Name);
            }

            return Ok();
        }


        private async Task<List<File>> ReadDataImportFiles()
        {
            var files = new List<File>();

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var fileBytes = await file.ReadAsByteArrayAsync();
                var fileContent = Encoding.Default.GetString(fileBytes);
                files.Add(new File(filename, fileContent));
            }

            return files;
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


        private class File
        {
            public string Name { get; private set; }
            public string Content { get; private set; }

            public File(string name, string content)
            {
                Name = name;
                Content = content;
            }
        }
    }
}
