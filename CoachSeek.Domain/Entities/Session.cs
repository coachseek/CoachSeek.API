﻿using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public abstract class Session
    {
        protected Service _service;
        protected Location _location;
        protected Coach _coach;

        protected SessionTiming _timing;
        protected SessionBooking _booking;
        protected SessionPresentation _presentation;


        public Guid Id { get; protected set; }
        public ServiceKeyData Service { get { return _service.ToKeyData(); } }
        public LocationKeyData Location { get { return _location.ToKeyData(); } }
        public CoachKeyData Coach { get { return _coach.ToKeyData(); } }
        public SessionTimingData Timing { get { return _timing.ToData(); } }
        public SessionBookingData Booking { get { return _booking.ToData(); } }
        public PresentationData Presentation { get { return _presentation.ToData(); } }


        public abstract bool IsOverlapping(Session otherSession);


        public abstract SessionData ToData();


        protected void ValidateAndCreateLocation(LocationData location, ValidationException errors)
        {
            if (location == null)
            {
                errors.Add("The location field is required.", "session.location");
                return;
            }

            try
            {
                _location = new Location(location);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        protected void ValidateAndCreateCoach(CoachData coach, ValidationException errors)
        {
            if (coach == null)
            {
                errors.Add("The coach field is required.", "session.coach");
                return;
            }

            try
            {
                _coach = new Coach(coach);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        protected void ValidateAndCreateService(ServiceData service, ValidationException errors)
        {
            if (service == null)
            {
                errors.Add("The service field is required.", "session.service");
                return;
            }

            try
            {
                _service = new Service(service);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        protected void ValidateAndCreateSessionTiming(SessionTimingData sessionTiming, ServiceTimingData serviceTiming, ValidationException errors)
        {
            try
            {
                _timing = new SessionTiming(sessionTiming, serviceTiming);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        protected void ValidateAndCreateSessionBooking(SessionBookingData sessionBooking, ServiceBookingData serviceBooking, ValidationException errors)
        {
            try
            {
                _booking = new SessionBooking(sessionBooking, serviceBooking);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        protected void ValidateAndCreateSessionPresentation(PresentationData sessionPresentation, PresentationData servicePresentation, ValidationException errors)
        {
            try
            {
                _presentation = new SessionPresentation(sessionPresentation, servicePresentation);
            }
            catch (ValidationException ex)
            {
                errors.Add(ex);
            }
        }

        protected bool IsNullOrSameSession(Session otherSession)
        {
            return (otherSession == null || otherSession.Equals(this) || otherSession.Id == Id);
        }
    }
}
