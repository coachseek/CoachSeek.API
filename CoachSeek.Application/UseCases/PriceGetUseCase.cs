﻿using System;
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
                var discountCode = await LookupDiscountCodeAsync(command);
                return IsStandaloneSession(firstSession)
                    ? new Response(await CalculateStandaloneSessionPriceAsync(command, firstSession, discountCode))
                    : new Response(await CalculateCoursePriceAsync(command, firstSession.ParentId.GetValueOrDefault(), discountCode));
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task<DiscountCodeData> LookupDiscountCodeAsync(PriceGetCommand command)
        {
            if (command.DiscountCode == null)
                return null;
            var discountCode = await BusinessRepository.GetDiscountCodeAsync(Business.Id, command.DiscountCode);
            if (discountCode.IsNotFound())
                throw new DiscountCodeInvalid(command.DiscountCode);
            if (!discountCode.IsActive)
                throw new DiscountCodeNotActive(command.DiscountCode);
            return discountCode;
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

        private async Task<decimal> CalculateStandaloneSessionPriceAsync(PriceGetCommand command, 
                                                                         SingleSessionData session,
                                                                         DiscountCodeData discountCode)
        {
            await Task.Delay(1);
            ValidateStandaloneSession(command);
            var fullPrice = session.Pricing.SessionPrice.GetValueOrDefault();
            if (discountCode == null)
                return fullPrice;
            return fullPrice.ApplyDiscount(discountCode.DiscountPercent);
        }

        private async Task<decimal> CalculateCoursePriceAsync(PriceGetCommand command, Guid courseId, DiscountCodeData discountCode)
        {
            var course = await LookupCourseAsync(courseId);
            SessionsInCourseValidator.Validate(command.Sessions, course);
            var discountPercent = discountCode != null ? discountCode.DiscountPercent : 0;
            return CourseBookingPriceCalculator.CalculatePrice(command.Sessions.AsReadOnly(), course, Business.UseProRataPricing, discountPercent);
        }
    }
}
