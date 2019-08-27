using System;
using System.Collections.Generic;

namespace AppServices.Framework
{
    public class TaskResult
    {
        private bool _success;

        private List<string> _messages;

        public TaskResult()
        {
            _success = true;
            _messages = new List<string>();
        }

        public bool Success { get { return _success; } }

        public void AddErrorMessage(string message)
        {
            _success = false;
            _messages.Add(message);
        }

        public void AddMessage(string message)
        {
            _messages.Add(message);
        }
    }
}
