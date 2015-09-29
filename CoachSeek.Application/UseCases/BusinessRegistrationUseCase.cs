using System;
using System.Threading.Tasks;
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


        public async Task<IResponse> RegisterBusinessAsync(BusinessRegistrationCommand command)
        {
            try
            {
                SetRepositoriesOnUseCases();
                var userTask = AddUserAsync(command.Admin);
                var businessTask = AddBusiness(command);
                await Task.WhenAll(userTask, businessTask);
                await AssociateUserWithBusiness(userTask.Result, businessTask.Result);
                await PostProcessingAsync(userTask.Result, businessTask.Result);
                return new Response(new RegistrationData(userTask.Result, businessTask.Result));
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

        private async Task<UserData> AddUserAsync(UserAddCommand command)
        {
            var response = await UserAddUseCase.AddUserAsync(command);
            return response.Data as UserData;
        }

        private async Task<BusinessData> AddBusiness(BusinessRegistrationCommand command)
        {
            var response = await BusinessAddUseCase.AddBusinessAsync(command);
            return response.Data as BusinessData;
        }

        private async Task AssociateUserWithBusiness(UserData user, BusinessData business)
        {
            var command = UserAssociateWithBusinessCommandBuilder.BuildCommand(user, business);
            await UserAssociateWithBusinessUseCase.AssociateUserWithBusinessAsync(command);
        }

        private async Task PostProcessingAsync(UserData user, BusinessData business)
        {
            await CreateTrackingUserAsync(user, business);
        }

        private async Task CreateTrackingUserAsync(UserData user, BusinessData business)
        {
            try
            {
                UserTrackerFactory.Credentials = UserTrackerCredentials;
                var userTracker = UserTrackerFactory.GetUserTracker(IsUserTrackingEnabled, IsTesting);
                userTracker.CreateTrackingUser(user, business);
                //await userTracker.CreateTrackingUserAsync(user, business);
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
