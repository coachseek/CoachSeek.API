//using CoachSeek.Domain;
//using CoachSeek.WebUI.Models.UseCases.Requests;

//namespace CoachSeek.WebUI.Conversion
//{
//    public class CoachConverter
//    {
//        public static Coach Convert(CoachAddRequest request)
//        {
//            return new Coach(request.FirstName, request.LastName, 
//                             request.Email, request.Phone);
//        }

//        public static Coach Convert(CoachUpdateRequest request)
//        {
//            return new Coach(request.CoachId, 
//                             request.FirstName, request.LastName, 
//                             request.Email, request.Phone);
//        }
//    }
//}