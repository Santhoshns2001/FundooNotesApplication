using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepoLayer.Entity;
using System;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {

        private readonly ILabelBuss labelBuss;

        public LabelController(ILabelBuss labelBuss)
        {
            this.labelBuss = labelBuss;
        }

        [Authorize]
        [HttpPost]
        [Route("AddLabel")]
        public ActionResult AddLabel(int NotesId, string LabelName)
        {
            try
            {

                var UserId = int.Parse(User.FindFirst("UserId").Value);

                var response = labelBuss.AddLabel(LabelName, UserId, NotesId);

                if (response != null)
                {
                    return Ok(new ResponseModel<bool>() { IsSuccuss = true, Message = "label created succussfully", Data = true });

                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { IsSuccuss = false, Message = " failed to insert", Data = false });
                }

               
            }catch(Exception ex)
            { 
                    return BadRequest(new ResponseModel<bool> { IsSuccuss = false,Message= ex.Message, Data = false });
                
            }
        }

        [Authorize]
        [HttpPut]
        [Route("RenameLabel")]
        public ActionResult RenameLabel(int NotesId, string OldLabelName,string NewLabelName)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);

          var response=  labelBuss.RenameLabel(NotesId, UserId, OldLabelName, NewLabelName);
            if (response )
            {
                return Ok(new ResponseModel<bool>() { IsSuccuss = true, Message = "label Renamed succuss", Data = true });
            }
            else
            {
                return BadRequest(new ResponseModel<bool>() { IsSuccuss = false, Message = "label rename Unsuccuss", Data = false });
            }

        }

        [Authorize]
        [HttpGet]
        [Route("FetchLabel")]
        public ActionResult FetchLabelByName(int NotesId, string LabelName)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);

            var response = labelBuss.FetchLabelByName(NotesId, UserId, LabelName);

            try {
            if (response != null)
            {
                return Ok(new ResponseModel<LabelEntity>() { IsSuccuss = true, Message = "label entity fetched sucuss", Data = response });
            }
            else
            {
                return BadRequest(new ResponseModel<bool>() { IsSuccuss = false, Message = "Failed to fetch labels by labelname ", Data = false });
            }
        }catch(Exception ex)
            {
                return BadRequest(new ResponseModel<bool>() { IsSuccuss = false, Message = ex.Message, Data = false });
            }

        }

        [Authorize]
        [HttpPut]
        [Route("RemoveLabel")]
        public ActionResult RemoveLabel(string LabelName,int NotesId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);
            var response=labelBuss.RemoveLabel(UserId,NotesId, LabelName);

            try
            {

                if (response)
                {
                    return Ok(new ResponseModel<bool>() { IsSuccuss = true, Message = "label removed  succussfully", Data = true });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool>() { IsSuccuss = false, Message = "unable to remove  label ", Data = false });
                }

            }catch(Exception e)
            {
                return BadRequest(new ResponseModel<bool>() { IsSuccuss = false, Message = e.Message, Data = false });
            }
        }

    }
}
