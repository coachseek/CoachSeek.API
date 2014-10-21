using System.Collections.Generic;

namespace CoachSeek.Data.Model
{
    public class ErrorData
    {
        private string _originalField;
        private string _field;
        private string _message;

        public string Field
        {
            get { return _field; }
            set
            {
                _originalField = value;
                _field = camelCaseComponentFields(value); 
            }
        }

        public string Message
        {
            get { return _message; }
            set { _message = reformatMessage(value); }
        }

        //public int Code { get; set; }

        public ErrorData()
        { }

        public ErrorData(string message)
        {
            Message = message;
        }

        //public ErrorData(int code, string message, string field = null)
        //{
        //    Code = code;
        //    Message = message;
        //    Field = field;
        //}

        public ErrorData(string field, string message)
        {
            // Temporal coupling! Field needs to be set before Message!
            Field = field;
            Message = message;
        }

        private string camelCaseComponentFields(string field)
        {
            if (string.IsNullOrWhiteSpace(field))
                return field;
            var components = field.Split('.');
            var camelcaseFields = new List<string>();
            foreach (var component in components)
            {
                var firstLetter = component.Substring(0, 1);
                var remainingLetters = component.Substring(1);
                var camelcaseField = string.Format("{0}{1}", firstLetter.ToLower(), remainingLetters);
                camelcaseFields.Add(camelcaseField);
            }

            if (camelcaseFields.Count == 1)
                return camelcaseFields[0];

            var response = "";
            foreach (var camelcaseField in camelcaseFields)
            {
                response += camelcaseField;
                response += ".";
            }

            return response.Substring(0, response.Length - 1);
        }

        private string reformatMessage(string message)
        {
            if (message == null || _originalField == null)
                return message;

            var originalFields = _originalField.Split('.');
            var lastSubField = originalFields[originalFields.GetLength(0) - 1];

            if (message.Contains(lastSubField))
                return message.Replace(lastSubField, camelCaseComponentFields(lastSubField));

            return message;
        }
    }
}
