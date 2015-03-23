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
                _field = CamelCaseComponentFields(value); 
            }
        }

        public string Message
        {
            get { return _message; }
            set { _message = ReformatMessage(value); }
        }

        public string Code { get; set; }
        public string Data { get; set; }

        public ErrorData()
        { }

        public ErrorData(string message)
        {
            Message = message;
        }

        public ErrorData(string field, string message, string code = null, string data = null)
        {
            // Temporal coupling! Field needs to be set before Message!
            Field = field;
            Message = message;
            Code = code;
            Data = data;
        }

        private string CamelCaseComponentFields(string field)
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

        private string ReformatMessage(string message)
        {
            if (message == null || _originalField == null)
                return message;

            var originalFields = _originalField.Split('.');
            var lastSubField = originalFields[originalFields.GetLength(0) - 1];

            if (message.Contains(lastSubField))
                return message.Replace(lastSubField, CamelCaseComponentFields(lastSubField));

            return message;
        }
    }
}
