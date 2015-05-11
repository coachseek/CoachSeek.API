//using CoachSeek.Application.Contracts.Services.Emailing;
//using CoachSeek.Data.Model;

//namespace CoachSeek.Application.Tests.Unit.Fakes
//{
//    public class StubBusinessRegistrationEmailer : IBusinessRegistrationEmailer
//    {
//        public bool WasSendEmailCalled;
//        public RegistrationData PassedInRegistrationData;

//        public void SendEmail(RegistrationData registration)
//        {
//            WasSendEmailCalled = true;
//            PassedInRegistrationData = registration;
//        }

//        public bool IsTesting
//        {
//            get
//            {
//                throw new System.NotImplementedException();
//            }
//            set
//            {
//                throw new System.NotImplementedException();
//            }
//        }

//        public bool ForceEmail
//        {
//            get
//            {
//                throw new System.NotImplementedException();
//            }
//            set
//            {
//                throw new System.NotImplementedException();
//            }
//        }


//        public string Sender
//        {
//            set { throw new System.NotImplementedException(); }
//        }
//    }
//}