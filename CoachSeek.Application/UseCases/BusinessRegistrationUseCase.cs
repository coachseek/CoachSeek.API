using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Services;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class BusinessRegistrationUseCase : IBusinessRegistrationUseCase
    {
        public IUserRepository UserRepository { set; get; }
        public IBusinessRepository BusinessRepository { set; get; }
        public ISupportedCurrencyRepository SupportedCurrencyRepository { set; get; }

        public IUserAddUseCase UserAddUseCase { get; private set; }
        public IBusinessAddUseCase BusinessAddUseCase { get; private set; }
        public IUserAssociateWithBusinessUseCase UserAssociateWithBusinessUseCase { get; private set; }


        public BusinessRegistrationUseCase(IUserAddUseCase userAddUseCase,
                                           IBusinessAddUseCase businessAddUseCase,
                                           IUserAssociateWithBusinessUseCase userAssociateWithBusinessUseCase)
        {
            UserAddUseCase = userAddUseCase;
            BusinessAddUseCase = businessAddUseCase;
            UserAssociateWithBusinessUseCase = userAssociateWithBusinessUseCase;
        }


        public IResponse RegisterBusiness(BusinessRegistrationCommand command)
        {
            try
            {
                SetRepositoriesOnUseCases();
                var user = AddUser(command.Admin);
                var business = AddBusiness(command.Business);
                AssociateUserWithBusiness(user, business);
                return new Response(new RegistrationData(user, business));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        private void SetRepositoriesOnUseCases()
        {
            UserAddUseCase.UserRepository = UserRepository;

            BusinessAddUseCase.BusinessRepository = BusinessRepository;
            BusinessAddUseCase.SupportedCurrencyRepository = SupportedCurrencyRepository;

            UserAssociateWithBusinessUseCase.UserRepository = UserRepository;
        }

        private ErrorResponse HandleException(Exception ex)
        {
            if (ex is SingleErrorException)
                return new ErrorResponse(ex as SingleErrorException);
            if (ex is ValidationException)
                return new ErrorResponse(ex as ValidationException);

            throw ex;
        }

        private UserData AddUser(UserAddCommand command)
        {
            var response = UserAddUseCase.AddUser(command);
            return response.Data as UserData;
        }

        private BusinessData AddBusiness(BusinessAddCommand command)
        {
            var response = BusinessAddUseCase.AddBusiness(command);
            return response.Data as BusinessData;
        }

        private void AssociateUserWithBusiness(UserData user, BusinessData business)
        {
            var command = UserAssociateWithBusinessCommandBuilder.BuildCommand(user, business);
            UserAssociateWithBusinessUseCase.AssociateUserWithBusiness(command);
        }

        //protected BusinessDetails ConvertToBusinessDetails(BusinessData business)
        //{
        //    return new BusinessDetails(business.Id, business.Name, business.Domain);
        //}

        //protected CurrencyDetails ConvertToCurrencyDetails(CurrencyData currency)
        //{
        //    return new CurrencyDetails(currency.Code, currency.Symbol);
        //}
    }
}
