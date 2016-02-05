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
using CoachSeek.Common;
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
        public ICustomerCustomFieldValuesUpdateUseCase CustomerCustomFieldValuesUpdateUseCase { get; set; }
        public ICustomerReceiveDataImportMessageUseCase CustomerReceiveDataImportMessageUseCase { get; set; }

        public CustomersController()
        { }

        public CustomersController(ICustomersGetAllUseCase customersGetAllUseCase,
                                   ICustomerGetByIdUseCase customerGetByIdUseCase,
                                   ICustomerAddUseCase customerAddUseCase,
                                   ICustomerUpdateUseCase customerUpdateUseCase,
                                   ICustomerOnlineBookingAddUseCase customerOnlineBookingAddUseCase,
                                   ICustomerCustomFieldValuesUpdateUseCase customerCustomFieldValuesUpdateUseCase,
                                   ICustomerReceiveDataImportMessageUseCase customerReceiveDataImportMessageUseCase)
        {
            CustomersGetAllUseCase = customersGetAllUseCase;
            CustomerGetByIdUseCase = customerGetByIdUseCase;
            CustomerAddUseCase = customerAddUseCase;
            CustomerUpdateUseCase = customerUpdateUseCase;
            CustomerOnlineBookingAddUseCase = customerOnlineBookingAddUseCase;
            CustomerCustomFieldValuesUpdateUseCase = customerCustomFieldValuesUpdateUseCase;
            CustomerReceiveDataImportMessageUseCase = customerReceiveDataImportMessageUseCase;
        }


        // GET: Customers
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        public async Task<HttpResponseMessage> GetAsync()
        {
            CustomersGetAllUseCase.Initialise(Context);
            var response = await CustomersGetAllUseCase.GetCustomersAsync();
            return CreateGetWebResponse(response);
        }

        // GET: Customers/D65BA9FE-D2C9-4C05-8E1A-326B1476DE08
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        public async Task<HttpResponseMessage> GetAsync(Guid id)
        {
            CustomerGetByIdUseCase.Initialise(Context);
            var response = await CustomerGetByIdUseCase.GetCustomerAsync(id);
            return CreateGetWebResponse(response);
        }

        // POST: Customers
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> PostAsync([FromBody]ApiCustomerSaveCommand customer)
        {
            return customer.IsNew() ? await AddCustomerAsync(customer) : await UpdateCustomerAsync(customer);
        }

        // POST: OnlineBooking/Customers
        [Route("OnlineBooking/Customers")]
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> PostOnlineBookingAsync([FromBody]ApiCustomerSaveCommand customer)
        {
            if (customer.IsExisting())
                return CreateWebErrorResponse(new UseExistingCustomerForOnlineBookingNotSupported());

            return await AddOnlineBookingCustomerAsync(customer);
        }

        [Route("OnlineBooking/Customers/{id}")]
        [BasicAuthenticationOrAnonymous]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> PostOnlineBookingCustomFieldValuesAsync(Guid id, [FromBody]ApiCustomFieldValueListSaveCommand values)
        {
            return await UpdateCustomFieldValuesAsync(id, values);
        }

        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        [CheckModelForNull]
        [ValidateModelState]
        public async Task<HttpResponseMessage> PostCustomFieldValuesAsync(Guid id, [FromBody]ApiCustomFieldValueListSaveCommand values)
        {
            return await UpdateCustomFieldValuesAsync(id, values);
        }

        //// POST: OnlineBooking/Customers
        //[Route("OnlineBooking/Customers/{id}")]
        //[BasicAuthenticationOrAnonymous]
        //[Authorize]
        //[CheckModelForNull]
        //[ValidateModelState]
        //public async Task<HttpResponseMessage> PostOnlineBookingCustomFieldValuesAsync(Guid id, [FromBody] dynamic apiCommand)
        //{
        // TODO: A better data format for receiving custom fields.
        //    var command = apiCommand.ToObject<Dictionary<string, string>>();

        //    return null;
        //}

        // POST: Customers/Upload
        [Route("Customers/Upload")]
        [BasicAuthentication]
        [BusinessAuthorize(Role.BusinessAdmin)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> ImportDataAsync()
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
                    await CustomerReceiveDataImportMessageUseCase.ReceiveAsync(Business.Id, file.Content);
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

        private async Task<HttpResponseMessage> AddCustomerAsync(ApiCustomerSaveCommand customer)
        {
            var command = CustomerAddCommandConverter.Convert(customer);
            CustomerAddUseCase.Initialise(Context);
            var response = await CustomerAddUseCase.AddCustomerAsync(command);
            return CreatePostWebResponse(response);
        }

        private async Task<HttpResponseMessage> UpdateCustomerAsync(ApiCustomerSaveCommand customer)
        {
            var command = CustomerUpdateCommandConverter.Convert(customer);
            CustomerUpdateUseCase.Initialise(Context);
            var response = await CustomerUpdateUseCase.UpdateCustomerAsync(command);
            return CreatePostWebResponse(response);
        }

        private async Task<HttpResponseMessage> AddOnlineBookingCustomerAsync(ApiCustomerSaveCommand customer)
        {
            var command = CustomerAddCommandConverter.Convert(customer);
            CustomerOnlineBookingAddUseCase.Initialise(Context);
            var response = await CustomerOnlineBookingAddUseCase.AddCustomerAsync(command);
            return CreatePostWebResponse(response);
        }

        private async Task<HttpResponseMessage> UpdateCustomFieldValuesAsync(Guid customerId, ApiCustomFieldValueListSaveCommand values)
        {
            var command = CustomFieldValuesUpdateCommandConverter.Convert(Constants.CUSTOM_FIELD_TYPE_CUSTOMER, customerId, values);
            CustomerCustomFieldValuesUpdateUseCase.Initialise(Context);
            var response = await CustomerCustomFieldValuesUpdateUseCase.UpdateCustomerCustomFieldValuesAsync(command);
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
