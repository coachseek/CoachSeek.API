﻿using System.Threading.Tasks;
using CoachSeek.Data.Model;
using Coachseek.Integration.Contracts.UserTracking;

namespace Coachseek.Integration.UserTracking
{
    public class DoNothingUserTracker : IUserTracker
    {
        public DoNothingUserTracker(string credentials)
        { }

        public async Task CreateTrackingUserAsync(UserData user, BusinessData business)
        {
            // Do nothing
            await Task.Delay(100);
        }
    }
}
