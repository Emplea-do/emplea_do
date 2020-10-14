using System;
using System.Collections.Generic;

namespace AppServices.Framework
{
    public class TaskResult
    {
        private bool _success;
        public bool Success { get { return _success; } }

        private List<string> _messages;
        public string Messages { get { return string.Join(", ", _messages); } }

        public TaskResult()
        {
            _success = true;
            _messages = new List<string>();
        }

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

    public class TaskResult<T> : TaskResult
    {
        public T Data { get; set; }
    }
}
