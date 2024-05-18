using BusinessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepoLayer.Context;
using RepoLayer.Entity;
using System;
using System.Threading.Tasks;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBuss userBuss;
        private readonly IBus bus;
        private readonly FundooDBContext context;

        public UserController(IUserBuss userBuss,IBus bus, FundooDBContext context)
        {
            this.userBuss = userBuss;
            this.bus=bus;
            this.context = context;
        }

        [HttpPost ("user")]
                 //or
       // [Route("user")]
        public IActionResult Register(RegistrationModel model)
        {
            var response=userBuss.UserRegistration(model);
            if(response!=null)
            {
                return Ok(new ResponseModel<UserEntity> { IsSuccuss =true,Message="process succuss",Data=response});
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { IsSuccuss=false,Message=" failed to insert",Data=response});
            }
        }
        [HttpGet("login")]
        public IActionResult Login(string Email,string password)
        {
            var response = userBuss.Login( Email, password);
            if (response!=null)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "login succuss", Data = response });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to login", Data = response });
            }
        }

        [HttpGet("check")]
        public bool Check(string Email)
        {
            return userBuss.Check(Email);

        }


       //[Route("forgetPassword")]
       [HttpGet("forgetPassword")]
        public async Task<IActionResult> ForgetPassword(string Email )
        {
           
                if (userBuss.Check(Email))
                {
                    Send send = new Send();
                    ForgetPasswordModel forgetPasswordModel = userBuss.ForgetPassword(Email);
                    send.SendMail(forgetPasswordModel.Email, forgetPasswordModel.Token);
                    Uri uri = new Uri("rabbitmq://localhost/FunDooNotesEmailQueue");

                    var endPoint = await bus.GetSendEndpoint(uri);
                    await endPoint.Send(forgetPasswordModel);


                    return Ok(new ResponseModel<string>() { IsSuccuss = true, Message = "mail sent succussfully", Data = "succuss" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string>() { IsSuccuss = false, Message = "Mail Not sent", Data =  "not succuss"});
                }

            
        }
        [Authorize]  // -> it will lock this particular API  to acess this method we have to provide proper authentication(token)
        [HttpPost]
        [Route("resetpassword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {


            try
            {
                if (resetPasswordModel.Password == resetPasswordModel.ConfirmPassword)
                {
                    string Email = User.FindFirst("Email").Value;    
                    if (userBuss.ResetPassword(Email, resetPasswordModel))
                    {
                        return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "password reset succussfull", Data = "password matched and succussfully changed " });
                    }
                    else
                    {
                        return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = "password reset unsuccessfull ", Data = "password unmtched please check the password " });
                    }
                }
                else
                {
                    return BadRequest(new ResponseModel<string>() { IsSuccuss = false, Message = "password and confirm password does not match", Data = "please check the password and confirm password " });
                }
            } 
            
            catch(Exception ex)
            {
                throw ex;
            }

           

        }
        // Review Questions 
        [HttpPost("register")]
        public ActionResult RegisterReview(ReviewModel model)
        {
            var response = userBuss.RegisterReview(model);

            if (response != null)
            {
                return Ok(new ResponseModel<ReviewTable> { IsSuccuss = true, Message = "insertion succuss", Data = response });
            }
            else
            {
                return BadRequest(new ResponseModel<ReviewTable> { IsSuccuss = false, Message = " failed to insert", Data = response });
            }
        }


        // fetch by review id
        [HttpGet]
        public ActionResult FetchReviewById(int reviewId)
        {
            var result = userBuss.FetchReviewById(reviewId);

            if (result != null)
            {
                return Ok(new ResponseModel<ReviewTable> { IsSuccuss = true, Message = "insertion succuss", Data = result });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " failed to insert", Data = "wroong review id is provided " });
            }

        }

        [HttpGet]
        [Route("GetUserNotesCount")]
        public ActionResult GetUserNotesCounts()
        {
            var response=userBuss.GetUserNotesCounts();

            if (response != null)
            {
                return Ok(new ResponseModel<object> { IsSuccuss = true, Message = "fetched count of notes of a user with username ", Data = response });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccuss = false, Message ="Unable to fetch user notes and user name ", Data = false }); ;
            }
        }

        [HttpGet]
        [Route("FetchAndUpdate")]
        public ActionResult FetchUserDetails(int UserId, string firstname, string lastname, string email)
        {
           var response= userBuss.FetchUserDetails(UserId, firstname, lastname, email);

            try
            {

                if (response != null)
                {
                    return Ok(new ResponseModel<UserEntity> { IsSuccuss = true, Message = "user data fetched succussfully  ", Data = response });
                }
                else
                {
                    return BadRequest(new ResponseModel<UserEntity> { IsSuccuss = false, Message = "Unable to fetch user details", Data = response }); ;
                }
            }catch(Exception ex)
            {
                return BadRequest(new ResponseModel<UserEntity> { IsSuccuss = false, Message = ex.Message, Data = response }); ;
            }

        }

    }
}
