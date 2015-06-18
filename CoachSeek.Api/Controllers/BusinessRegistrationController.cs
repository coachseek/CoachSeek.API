﻿using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.Services.Emailing;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Controllers
{
    public class BusinessRegistrationController : BaseController
    {
        public IUserAddUseCase UserAddUseCase { get; set; }
        public IBusinessAddUseCase BusinessAddUseCase { get; set; }
        public IUserAssociateWithBusinessUseCase UserAssociateWithBusinessUseCase { get; set; }
        public IBusinessRegistrationEmailer BusinessRegistrationEmailer { get; set; }


        public BusinessRegistrationController()
        { }

        public BusinessRegistrationController(IUserAddUseCase userAddUseCase,
                                              IBusinessAddUseCase businessAddUseCase,
                                              IUserAssociateWithBusinessUseCase userAssociateWithBusinessUseCase,
                                              IBusinessRegistrationEmailer businessRegistrationEmailer)
        {
            UserAddUseCase = userAddUseCase;
            BusinessAddUseCase = businessAddUseCase;
            UserAssociateWithBusinessUseCase = userAssociateWithBusinessUseCase;
            BusinessRegistrationEmailer = businessRegistrationEmailer;
        }


        // POST: BusinessRegistration
        [AllowAnonymous]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiBusinessRegistrationCommand registration)
        {
            var userAddResponse = AddUser(registration.Admin);
            if (!userAddResponse.IsSuccessful)
                return CreateWebErrorResponse(userAddResponse);

            var businessAddResponse = AddBusiness(registration.Business);
            if (!businessAddResponse.IsSuccessful)
                return CreateWebErrorResponse(businessAddResponse);

            var businessData = (BusinessData)businessAddResponse.Data;
            Business = ConvertToBusinessDetails(businessData);
            var userData = (UserData)userAddResponse.Data;

            var userUpdateResponse = AssociateUserWithBusiness(userData, businessData);
            if (!userUpdateResponse.IsSuccessful)
                return CreateWebErrorResponse(userUpdateResponse);

            var registrationData = new RegistrationData(userData, businessData);
            SendRegistrationEmail(registrationData);

            return CreateWebSuccessResponse(new Response(registrationData));
        }

        private Response AddUser(ApiBusinessAdminCommand command)
        {
            var userAddCommand = UserAddCommandConverter.Convert(command);
            UserAddUseCase.UserRepository = UserRepository;

            return UserAddUseCase.AddUser(userAddCommand);
        }

        private Response AddBusiness(ApiBusinessCommand command)
        {
            var businessAddCommand = BusinessAddCommandConverter.Convert(command);
            BusinessAddUseCase.Initialise(Context);
            return BusinessAddUseCase.AddBusiness(businessAddCommand);
        }

        private Response AssociateUserWithBusiness(UserData user, BusinessData business)
        {
            var userBusinessCommand = UserAssociateWithBusinessCommandBuilder.BuildCommand(user, business);
            UserAssociateWithBusinessUseCase.UserRepository = UserRepository;
            return UserAssociateWithBusinessUseCase.AssociateUserWithBusiness(userBusinessCommand);
        }

        private void SendRegistrationEmail(RegistrationData registration)
        {
            BusinessRegistrationEmailer.Initialise(Context);
            BusinessRegistrationEmailer.SendEmail(registration);
        }

        protected BusinessDetails ConvertToBusinessDetails(BusinessData business)
        {
            return new BusinessDetails(business.Id, business.Name, business.Domain);
        }

        protected CurrencyDetails ConvertToCurrencyDetails(CurrencyData currency)
        {
            return new CurrencyDetails(currency.Code, currency.Symbol);
        }
    }
}
