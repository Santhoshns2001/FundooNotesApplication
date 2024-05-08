using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepoLayer.Entity;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBuss userBuss;

        public UserController(IUserBuss userBuss)
        {
            this.userBuss = userBuss;
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

    }
}
