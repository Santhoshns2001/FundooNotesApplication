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
       
       public ForgetPasswordModel ForgetPassword(string Email);
       public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel);
        public ReviewTable RegisterReview(ReviewModel model);
        public ReviewTable FetchReviewById(int reviewId);

        public object GetUserNotesCounts();

        public UserEntity FetchUserDetails(int UserId, string firstname, string lastname, string email);
    }
}
