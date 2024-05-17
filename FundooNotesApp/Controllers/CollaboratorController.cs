using BusinessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer;
using RepoLayer.Entity;
using System;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBuss collaboratorBuss;
        private readonly ILogger<CollaboratorController> logger;

        public CollaboratorController(ICollaboratorBuss collaboratorBuss,ILogger<CollaboratorController> logger)
        {
            this.collaboratorBuss = collaboratorBuss;
            this.logger = logger;
        }

        [Authorize]
        [HttpPut]
        [Route("AddingCollabration")]

        public ActionResult AddCollaborator(int NotesId, string Email)
        {
            try
            {

                int UserId = int.Parse(User.FindFirst("UserId").Value);

                CollaboratorEntity response = collaboratorBuss.AddCollaborator(UserId, NotesId, Email);

                if (response != null)
                {
                    return Ok(new ResponseModel<CollaboratorEntity>() { IsSuccuss = true, Message = "collaborator added to the notes is succuss", Data = response });
                    throw new Exception("error occured");
                }
                else
                {
                    return BadRequest(new ResponseModel<CollaboratorEntity>() { IsSuccuss = false, Message = "Unable to add collaborator", Data = response });
                    throw new Exception("error occured");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return BadRequest(new ResponseModel<bool>() { IsSuccuss = false, Message = ex.Message, Data = false });
            }
        }


        [Authorize]
        [HttpPut]
        [Route("RemoveCollaborator")]
        public ActionResult RemoveCollaborator(int NotesId,string Email)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);

            try
            {

                var response = collaboratorBuss.RemoveCollaborator(UserId, NotesId, Email);

                if (response)
                {
                    return Ok(new ResponseModel<bool>() { IsSuccuss = true, Message = "collaborator removing is succuss ", Data = response });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool>() { IsSuccuss = false, Message = "Unable to remove collaborator", Data = false });
                }
            }catch(Exception ex)
            {
                return BadRequest(new ResponseModel<bool>() { IsSuccuss = false, Message = ex.Message, Data = false });
            }


        }

    }
}
