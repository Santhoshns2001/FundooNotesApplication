using BusinessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
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

        public UserController(IUserBuss userBuss,IBus bus)
        {
            this.userBuss = userBuss;
            this.bus=bus;
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
        [Route("login")]
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

        [Route("check")]
        public bool Check(string Email)
        {
            return userBuss.Check(Email);

        }


       [Route("forgetPassword")]
        //[HttpPost("forgetPassword")]
        public async Task<IActionResult> ForgetPassword(string Email )
        {
            try
            {
                if (userBuss.Check(Email))
                {
                    Send send = new Send();
                    ForgetPasswordModel forgetPasswordModel = userBuss.ForgetPassword(Email);
                    send.SendMail(forgetPasswordModel.Email, forgetPasswordModel.Token);
                    Uri uri = new Uri("rabbitmq://localhost/FunDooNotesEmailQueue");
                    var endPoint = await bus.GetSendEndpoint(uri);
                    await endPoint.Send(forgetPasswordModel);


                    return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "mail sent succussfully", Data = "succuss" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = "Mail Not sent", Data =  "not succuss"});
                }

            }catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("resetpassword")]
        public IActionResult ResetPassword(string Email,string Password,string ConfirmPassword)
        {
            var response = userBuss.ResetPassword(Email,Password,ConfirmPassword);

            if(response!=null)
            {
                return Ok(new ResponseModel<string> { IsSuccuss=true, Message ="password reset succussfull",Data =response});
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false,Message="password reset unsuccessfull ",Data=response});
            }

        }

    }
}
