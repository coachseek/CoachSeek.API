using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Domain.Entities
{
    public class BusinessCoaches
    {
        public List<Coach> Coaches { get; private set; }

        public BusinessCoaches()
        {
            Coaches = new List<Coach>();
        }

        public BusinessCoaches(IEnumerable<CoachData> coaches) 
            : this()
        {
            if (coaches == null)
                return;

            foreach (var coach in coaches)
                Append(coach);
        }

        public void Add(NewCoachData newCoachData)
        {
            var newCoach = new NewCoach(newCoachData);
            ValidateAdd(newCoach);
            Coaches.Add(newCoach);
        }

        public void Append(CoachData coachData)
        {
            // Data is not Validated. Eg. It comes from the database.
            Coaches.Add(new Coach(coachData));
        }

        public void Update(CoachData coachData)
        {
            var coach = new Coach(coachData);
            ValidateUpdate(coach);
            ReplaceCoachInCoaches(coach);
        }

        private void ReplaceCoachInCoaches(Coach coach)
        {
            var updateCoach = Coaches.Single(x => x.Id == coach.Id);
            var updateIndex = Coaches.IndexOf(updateCoach);
            Coaches[updateIndex] = coach;
        }

        private void ValidateAdd(NewCoach newCoach)
        {
            var isExistingCoach = Coaches.Any(x => x.Name.ToLower() == newCoach.Name.ToLower());
            if (isExistingCoach)
                throw new DuplicateCoach();
        }

        private void ValidateUpdate(Coach coach)
        {
            var isExistingCoach = Coaches.Any(x => x.Id == coach.Id);
            if (!isExistingCoach)
                throw new InvalidCoach();

            var existingCoach = Coaches.FirstOrDefault(x => x.Name.ToLower() == coach.Name.ToLower());
            if (existingCoach != null && existingCoach.Id != coach.Id)
                throw new DuplicateCoach();
        }
    }
}