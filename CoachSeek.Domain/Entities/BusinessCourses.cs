using System.Linq;
using CoachSeek.Data.Model;
using System;
using System.Collections.Generic;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class BusinessCourses
    {
        private List<RepeatedSession> Courses { get; set; }

        public BusinessSessions Sessions { private get; set; }  // Required for overlap session check.


        public BusinessCourses()
        {
            Courses = new List<RepeatedSession>();
        }

        public BusinessCourses(IEnumerable<RepeatedSessionData> courses,
                                BusinessLocations locations, 
                                BusinessCoaches coaches,
                                BusinessServices services)
            : this()
        {
            if (courses == null || locations == null || coaches == null || services == null)
                return;

            foreach (var course in courses)
            {
                var location = locations.GetById(course.Location.Id);
                var coach = coaches.GetById(course.Coach.Id);
                var service = services.GetById(course.Service.Id);

                Append(course, location, coach, service);                
            }
        }


        public Guid Add(SessionAddCommand command, LocationData location, CoachData coach, ServiceData service)
        {
            var newCourse = new RepeatedSession(command, location, coach, service);

            ValidateAdd(newCourse);
            Courses.Add(newCourse);

            return newCourse.Id;
        }

        public void Update(SessionUpdateCommand command, LocationData location, CoachData coach, ServiceData service)
        {
            var newCourse = new RepeatedSession(command, location, coach, service);

            ValidateUpdate(newCourse);
            ReplaceCourseInCourses(newCourse);
        }

        public void Append(RepeatedSessionData course, LocationData location, CoachData coach, ServiceData service)
        {
            // Data is already valid. Eg. It comes from the database.
            Courses.Add(new RepeatedSession(course, location, coach, service));
        }

        public bool IsOverlappingCourses(Session session)
        {
            return Courses.Any(c => c.IsOverlapping(session));
        }

        public IList<RepeatedSessionData> ToData()
        {
            return Courses.Select(session => session.ToData()).ToList();
        }


        private void ReplaceCourseInCourses(RepeatedSession course)
        {
            var updateCourse = Courses.Single(x => x.Id == course.Id);
            var updateIndex = Courses.IndexOf(updateCourse);
            Courses[updateIndex] = course;
        }

        private void ValidateAdd(RepeatedSession newCourse)
        {
            if (IsOverlapping(newCourse))
                throw new ClashingSession();
        }

        private void ValidateUpdate(RepeatedSession existingCourse)
        {
            var isExistingCourse = Courses.Any(x => x.Id == existingCourse.Id);
            if (!isExistingCourse)
                throw new InvalidSession();

            if (IsOverlapping(existingCourse))
                throw new ClashingSession();
        }

        private bool IsOverlapping(RepeatedSession course)
        {
            return IsOverlappingSessions(course) || IsOverlappingCourses(course);
        }

        private bool IsOverlappingSessions(RepeatedSession repeatedSession)
        {
            return Sessions.IsOverlappingSessions(repeatedSession);
        }
    }
}
