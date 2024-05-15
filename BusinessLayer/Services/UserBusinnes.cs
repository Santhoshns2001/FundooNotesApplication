using BusinessLayer.Interfaces;
using ModelLayer;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services 
{
    public class UserBusinnes : IUserBuss
    {
        private readonly IUserRepo userRepo;

        public UserBusinnes(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        public UserEntity UserRegistration(RegistrationModel model)
        {
            return userRepo.UserRegistration(model);
        }

        public bool Check(string Email)
        {
            return userRepo.Check(Email);
        }

        public string Login(string Email,string Password)
        {
            return userRepo.LoginUser(Email, Password);
        }

        public ForgetPasswordModel ForgetPassword(string Email)
        {
            return userRepo.ForgetPassword(Email);
        }

       public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel)
        {
            return userRepo.ResetPassword(email,resetPasswordModel);
        }

        public ReviewTable RegisterReview(ReviewModel model)
        {
            return userRepo.RegisterReview(model);
        }

        public ReviewTable FetchReviewById(int reviewId)
        {
            return userRepo.FetchById(reviewId);
        }

    }
}
