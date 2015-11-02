using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Services;

namespace CoachSeek.Application.UseCases
{
    public class PriceGetUseCase : SessionBaseUseCase, IPriceGetUseCase
    {
        public async Task<IResponse> GetPriceAsync(PriceGetCommand command)
        {
            try
            {
                ValidateCommand(command);
                var firstSession = await LookupSessionAsync(command);
                return IsStandaloneSession(firstSession)
                    ? new Response(await CalculateStandaloneSessionPriceAsync(command, firstSession)) 
                    : new Response(await CalculateCoursePriceAsync(command, firstSession.ParentId.GetValueOrDefault()));
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private void ValidateCommand(PriceGetCommand command)
        {
            if (!command.Sessions.Any())
                throw new PricingEnquirySessionRequired();
        }

        private async Task<SingleSessionData> LookupSessionAsync(PriceGetCommand command)
        {
            var firstSessionId = command.Sessions.First().Id;
            var firstSession = await LookupSessionAsync(firstSessionId);
            if (firstSession.IsNotFound())
                throw new SessionInvalid(firstSessionId);
            return firstSession;
        }

        private void ValidateStandaloneSession(PriceGetCommand command)
        {
            if (command.Sessions.Count > 1)
                throw new StandaloneSessionMustBeBookedOneAtATime();
        }

        private async Task<decimal> CalculateStandaloneSessionPriceAsync(PriceGetCommand command, SingleSessionData session)
        {
            await Task.Delay(1);
            ValidateStandaloneSession(command);
            return session.Pricing.SessionPrice.GetValueOrDefault();
        }

        private async Task<decimal> CalculateCoursePriceAsync(PriceGetCommand command, Guid courseId)
        {
            var course = await LookupCourseAsync(courseId);
            SessionsInCourseValidator.Validate(command.Sessions, course);
            return CourseBookingPriceCalculator.CalculatePrice(command.Sessions, course);
        }
    }
}
