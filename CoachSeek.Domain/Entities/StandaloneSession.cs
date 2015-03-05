using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class StandaloneSession : SingleSession
    {
        public StandaloneSession(SessionAddCommand command, CoreData coreData)
            : base(command, coreData)
        {
            ParentId = null;
        }

        public StandaloneSession(SessionUpdateCommand command, CoreData coreData)
            : base(command, coreData)
        {
            ParentId = null;
        }

        public StandaloneSession(SingleSessionData data, CoreData coreData)
            : this(data, coreData.Location, coreData.Coach, coreData.Service)
        { }

        public StandaloneSession(SingleSessionData data, LocationData location, CoachData coach, ServiceData service)
            : this(data.Id, location, coach, service, data.Timing, data.Booking, data.Presentation, data.Pricing)
        {
            // Fatal error! There is a defect in the system.
            if (data.ParentId.HasValue)
                throw new InvalidOperationException("We have a ParentId for a Standalone Session!");
        }

        public StandaloneSession(Guid id,
                       LocationData location,
                       CoachData coach,
                       ServiceData service,
                       SessionTimingData timing,
                       SessionBookingData booking,
                       PresentationData presentation,
                       SingleSessionPricingData pricing)
            : base(id, location, coach, service, timing, booking, presentation, pricing)
        { }


        //public void Apply(SessionUpdateCommand command, CoreData updateCoreData)
        //{
        //    if (updateCoreData.Service.IsExisting())
        //        _service = new Service(updateCoreData.Service);
        //    if (updateCoreData.Location.IsExisting())
        //        _location = new Location(updateCoreData.Location);
        //    if (updateCoreData.Coach.IsExisting())
        //        _coach = new Coach(updateCoreData.Coach);

        //    if (command.Timing.IsExisting())
        //    {
        //        var newTiming = new SessionTiming
        //        {

        //        };

        //        if (command.Timing.StartDate.IsExisting())
        //            _timing.StartDate = command.Timing.StartDate;
        //        if (command.Timing.StartTime.IsExisting())
        //            _timing.StartTime = command.Timing.StartTime;
        //    }

        ////public LocationKeyData Location { get { return _location.ToKeyData(); } }
        ////public CoachKeyData Coach { get { return _coach.ToKeyData(); } }
        ////public SessionTimingData Timing { get { return _timing.ToData(); } }
        ////public SessionBookingData Booking { get { return _booking.ToData(); } }
        ////public PresentationData Presentation { get { return _presentation.ToData(); } }

        //}

        protected override void ValidateAdditional(SessionAddCommand command,
                                                   CoreData coreData,
                                                   ValidationException errors)
        {
            ValidateRepetition(command);
            ValidateAndCreateSessionPricing(command.Pricing, errors);
        }


        private void ValidateRepetition(SessionAddCommand command)
        { }

        private void ValidateAndCreateSessionPricing(PricingCommand sessionPricing, ValidationException errors)
        {
            try
            {
                _pricing = new StandaloneSessionPricing(sessionPricing);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }
    }
}
