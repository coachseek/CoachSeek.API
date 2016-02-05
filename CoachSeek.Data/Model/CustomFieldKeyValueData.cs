namespace CoachSeek.Data.Model
{
    public class CustomFieldKeyValueData
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public CustomFieldKeyValueData(string key, string value = null)
        {
            Key = key;
            Value = value;
        }
    }
}
