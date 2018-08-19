using System;
using System.Collections.Generic;
using Domain.Framework.Dto;

namespace Domain.Framework
{
    public class LoginResult
    {
        public UserLimited User { get; set; }

        public bool ExecutedSuccesfully { get; set; } = false;

        public string Message
        {
            get
            {
                var result = "";
                if (Exception != null)
                {
                    AddErrorMessage("There was an issue with this action");
                    AddErrorMessage(Exception.ToString());
                    if (Exception.InnerException != null)
                    {
                        AddErrorMessage(Exception.InnerException.ToString());
                    }
                }
                if (Messages.Count == 1)
                {
                    return Messages[0];
                }
                if (Messages.Count > 0)
                {
                    result = string.Join(",", Messages);
                    if (result[result.Length - 1] == ',')
                        result = result.Remove(result.Length - 1);
                }
                else
                {
                    result = "";
                }
                return result;
            }
        }

        public IList<string> Messages { get; } = new List<string>();

        public Exception Exception { get; set; }

        public void AddErrorMessage(string errorMessage)
        {
            ExecutedSuccesfully = false;
            Messages.Add(errorMessage);
        }
        public void AddMessage(string message)
        {
            Messages.Add(message);
        }
    }
}
