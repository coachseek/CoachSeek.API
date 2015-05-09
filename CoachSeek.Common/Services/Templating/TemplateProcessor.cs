//using WSuite.Interfaces.Formatting;
//using WSuite.Interfaces.Templating;
using System;
using System.Collections.Generic;


namespace CoachSeek.Common.Services.Templating
{
    /// <summary>
    /// Replaces placeholders in a template with substitute values.
    /// </summary>
    public class TemplateProcessor
    {
        public const string PLACEHOLDER_FORMAT = "<<{0}>>"; 


        public static string ProcessTemplate(string template, IDictionary<string, string> substitutes, Func<string, string> transformation = null)
        {
            foreach (var item in substitutes)
            {
                var placeholder = string.Format(PLACEHOLDER_FORMAT, item.Key);

                // If we want to we can transform (eg. change to uppercase) the placeholder before we replace within the template.
                // Usage (change to uppercase): ProcessTemplate(body, placeholders, x => x.ToUpper());
                if (transformation != null)
                {
                    // Enacts the transform function.
                    placeholder = transformation(placeholder);
                }

                template = template.Replace(placeholder, item.Value);
            }

            return template;
        }
    }
}