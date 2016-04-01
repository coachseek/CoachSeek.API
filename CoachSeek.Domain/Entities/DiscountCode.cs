using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class DiscountCode
    {
        public Guid Id { get; private set; }

        public string Code { get; private set; }
        public int DiscountPercent { get; private set; }
        public bool IsActive { get; private set; }


        public DiscountCode(DiscountCodeAddCommand command)
        {
            Id = Guid.NewGuid();
            Code = command.Code.ToUpperInvariant();
            DiscountPercent = command.DiscountPercent;
            IsActive = command.IsActive.GetValueOrDefault();
        }

        public DiscountCode(DiscountCodeUpdateCommand command)
        {
            Id = command.Id;
            Code = command.Code.ToUpperInvariant();
            DiscountPercent = command.DiscountPercent;
            IsActive = command.IsActive.GetValueOrDefault();
        }

        public DiscountCodeData ToData()
        {
            return Mapper.Map<DiscountCode, DiscountCodeData>(this);
        }
    }
}
