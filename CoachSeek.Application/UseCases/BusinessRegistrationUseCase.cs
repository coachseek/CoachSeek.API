using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Services;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using Coachseek.Integration.Contracts.UserTracking;

namespace CoachSeek.Application.UseCases
{
    public class BusinessRegistrationUseCase : IBusinessRegistrationUseCase
    {
        public bool IsTesting { set; get; }
        public bool IsUserTrackingEnabled { set; get; }
        public string UserTrackerCredentials { set; get; }
        public IUserRepository UserRepository { set; get; }
        public IBusinessRepository BusinessRepository { set; get; }
        public ISupportedCurrencyRepository SupportedCurrencyRepository { set; get; }

        public IUserAddUseCase UserAddUseCase { get; private set; }
        public IBusinessAddUseCase BusinessAddUseCase { get; private set; }
        public IUserAssociateWithBusinessUseCase UserAssociateWithBusinessUseCase { get; private set; }
        public IUserTrackerFactory UserTrackerFactory { get; private set; }


        public BusinessRegistrationUseCase(IUserAddUseCase userAddUseCase,
                                           IBusinessAddUseCase businessAddUseCase,
                                           IUserAssociateWithBusinessUseCase userAssociateWithBusinessUseCase,
                                           IUserTrackerFactory userTrackerFactory)
        {
            UserAddUseCase = userAddUseCase;
            BusinessAddUseCase = businessAddUseCase;
            UserAssociateWithBusinessUseCase = userAssociateWithBusinessUseCase;
            UserTrackerFactory = userTrackerFactory;
        }


        public IResponse RegisterBusiness(BusinessRegistrationCommand command)
        {
            try
            {
                SetRepositoriesOnUseCases();
                var user = AddUser(command.Admin);
                var business = AddBusiness(command);
                AssociateUserWithBusiness(user, business);
                PostProcessing(user);
                return new Response(new RegistrationData(user, business));
            }
            catch (CoachseekException ex)
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

        private ErrorResponse HandleException(CoachseekException ex)
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

        private BusinessData AddBusiness(BusinessRegistrationCommand command)
        {
            var response = BusinessAddUseCase.AddBusiness(command);
            return response.Data as BusinessData;
        }

        private void AssociateUserWithBusiness(UserData user, BusinessData business)
        {
            var command = UserAssociateWithBusinessCommandBuilder.BuildCommand(user, business);
            UserAssociateWithBusinessUseCase.AssociateUserWithBusiness(command);
        }

        private void PostProcessing(UserData user)
        {
            CreateTrackingUser(user);
        }

        private void CreateTrackingUser(UserData user)
        {
            try
            {
                UserTrackerFactory.Credentials = UserTrackerCredentials;
                var userTracker = UserTrackerFactory.GetUserTracker(IsUserTrackingEnabled, IsTesting);
                userTracker.CreateTrackingUser(user);
            }
            catch (Exception)
            {
                // User tracking is not that important. If there is a fatal error 
                // then swallow it and do not fail the call.
                // TODO: Log error
            }
        }
    }
}
