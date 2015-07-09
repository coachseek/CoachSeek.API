using System.Diagnostics;

namespace Coachseek.API.Client.Properties
{
    internal sealed partial class Settings
    {
        public Settings()
        {
            // Each method corrsponds to a build version. We call all four methods, because
            // the conditional compilation will only compile the one indicated:
            this.SetDebugApplicationSettings();
            this.SetTestingApplicationSettings();
            this.SetReleaseApplicationSettings();
        }

        [Conditional("DEBUG")]
        private void SetDebugApplicationSettings()
        {
            this["HttpBaseUrl"] = "http://localhost:5272";
            this["HttpsBaseUrl"] = "https://localhost:44300";
        }

        [Conditional("TESTING")]
        private void SetTestingApplicationSettings()
        {
            // coachseek-api-testing.azurewebsites.net
            //this["HttpBaseUrl"] = "http://coachseek-api-testing.azurewebsites.net";
            //this["HttpsBaseUrl"] = "https://coachseek-api-testing.azurewebsites.net";
            this["HttpBaseUrl"] = "http://api-testing.coachseek.com";
            this["HttpsBaseUrl"] = "https://api-testing.coachseek.com";
        }

        [Conditional("RELEASE")]
        private void SetReleaseApplicationSettings()
        {
            this["HttpBaseUrl"] = "http://api.coachseek.com";
            this["HttpsBaseUrl"] = "https://api.coachseek.com";
        }
    }
}
