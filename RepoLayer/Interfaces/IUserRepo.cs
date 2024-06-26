﻿using ModelLayer;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interfaces
{
    public interface IUserRepo
    {
        public UserEntity UserRegistration(RegistrationModel model);

        
       public string LoginUser(string Email, string Password);

       public  bool Check(string email);
       public  ForgetPasswordModel ForgetPassword(string email);
       public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel);
        public ReviewTable RegisterReview(ReviewModel model);
        public ReviewTable FetchById(int reviewId);

        public object GetUserNotesCounts();
        public UserEntity FetchUserDetails(int UserId, string firstname, string lastname, string email);
    }
}
