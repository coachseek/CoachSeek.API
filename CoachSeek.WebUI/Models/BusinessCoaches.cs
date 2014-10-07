using CoachSeek.WebUI.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.WebUI.Models
{
    public class BusinessCoaches
    {
        public List<Coach> Coaches { get; private set; }

        public BusinessCoaches()
        {
            Coaches = new List<Coach>();
        }

        public void Add(Coach coach)
        {
            ValidateAdd(coach);

            Coaches.Add(coach);
        }

        public void Update(Coach coach)
        {
            ValidateUpdate(coach);

            var updateCoach = Coaches.Single(x => x.Id == coach.Id);
            var updateIndex = Coaches.IndexOf(updateCoach);
            Coaches[updateIndex] = coach;
        }

        private void ValidateAdd(Coach coach)
        {
            var isExistingCoach = Coaches.Any(x => x.Name.ToLower() == coach.Name.ToLower());
            if (isExistingCoach)
                throw new DuplicateCoachException();
        }

        private void ValidateUpdate(Coach coach)
        {
            var isExistingCoach = Coaches.Any(x => x.Id == coach.Id);
            if (!isExistingCoach)
                throw new InvalidCoachException();

            var existingCoach = Coaches.FirstOrDefault(x => x.Name.ToLower() == coach.Name.ToLower());
            if (existingCoach != null && existingCoach.Id != coach.Id)
                throw new DuplicateCoachException();
        }
    }
}