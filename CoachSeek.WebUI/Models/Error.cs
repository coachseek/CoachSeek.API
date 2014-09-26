﻿namespace CoachSeek.WebUI.Models
{
    public class Error
    {
        public int Code { get; private set; }
        public string Message { get; private set; }

        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}