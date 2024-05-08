using ModelLayer;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IUserBuss
    {
        public UserEntity UserRegistration(RegistrationModel model);

        public bool Check(string Email);
       public string Login(string email, string password);
    }
}
