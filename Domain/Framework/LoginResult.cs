using System;
namespace Domain.Framework
{
    public class LoginResult
    {
        public User User { get; set; }

        public bool ExecutedSuccessfully { get; set; } = false;
    }
}
