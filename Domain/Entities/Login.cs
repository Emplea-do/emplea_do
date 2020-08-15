using System;
namespace Domain.Entities

{
    public class Login : Entity
    {
        public int UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}
