using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RepoLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration _config;

        public UserRepo(FundooDBContext context, IConfiguration _config)
        {
            this.context = context;
            this._config = _config;
        }

        public UserEntity UserRegistration(RegistrationModel model)
        {
            try
            {
                if (!Check(model.Email))
                {
                    UserEntity userEntity = new UserEntity();
                    userEntity.FirstName = model.FirstName;
                    userEntity.LastName = model.LastName;
                    userEntity.Email = model.Email;
                    userEntity.Password =HashPassword(model.Password);
                    userEntity.Created = DateTime.Now;
                    userEntity.ChangedAt = DateTime.Now;
                    context.Users.Add(userEntity);
                    context.SaveChanges();
                    return userEntity;
                }
                else
                {
                    throw new Exception("Email already Exists");
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Error during user registration: {ex.Message}");
                return null;
            }

        }
       
       public bool Check(string email)
        {
          return  context.Users.Any(a => a.Email == email);
        }   

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public string LoginUser(string Email, string Password)
        {
            var result = context.Users.FirstOrDefault(u => u.Email == Email && u.Password == HashPassword(Password));

            if (result != null)
            {
                var token = GenerateToken(result.Email, result.UserId);
                return token;
            }
            else
            {
                return null;
            }

        }

        private string GenerateToken(string Email ,int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",Email),
                new Claim("userId",userId.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

       public  ForgetPasswordModel ForgetPassword(string email)
        {
          UserEntity user= context.Users.ToList().Find(u=> u.Email==email);
          ForgetPasswordModel forgetPassword=new ForgetPasswordModel();
            forgetPassword.Email = user.Email;
            forgetPassword.UserId = user.UserId;
            forgetPassword.Token=GenerateToken(user.Email, user.UserId);
            return forgetPassword;
        }

        public bool ResetPassword(string email, ResetPasswordModel resetPasswordModel)
        {

            
            //if (resetPasswordModel.Password != resetPasswordModel.ConfirmPassword)
            //{
            //    return false;
            //}

               var record= context.Users.FirstOrDefault(u => u.Email == email);
               if(record != null)
                {
                    record.Password=HashPassword(resetPasswordModel.Password);
                    record.ChangedAt = DateTime.Now;
                    context.SaveChanges();
                return true;
                }
                else
                {
                    return false;
                }

          //var user=context.Users.FirstOrDefault(u=> u.Email== email);
          //  context.Users.Remove(user);

           
            
        }


        // review questions
        //1.
        public ReviewTable RegisterReview(ReviewModel model)
        {
            ReviewTable review = new ReviewTable();

            review.Username = model.Username;
            review.Feedback = model.Feedback;
            context.ReviewTable.Add(review);
            context.SaveChanges();
            return review;
        }

       // 2.
      public ReviewTable FetchById(int reviewId)
        {

            ReviewTable reviewTable = context.ReviewTable.FirstOrDefault(u => u.reviewId == reviewId);

            if (reviewTable != null)
            {
                return reviewTable;
            }
            else
            {
                return null;
            }
        }

    }
}
