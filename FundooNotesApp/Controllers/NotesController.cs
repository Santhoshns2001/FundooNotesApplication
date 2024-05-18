using BusinessLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using Newtonsoft.Json;
using RepoLayer.Context;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBuss notesBuss;
        private readonly IDistributedCache _cache;
        private readonly FundooDBContext context;

        public NotesController(INotesBuss notesBuss,IDistributedCache _cache, FundooDBContext context)
        {
            this.notesBuss = notesBuss;
            this._cache = _cache;
            this.context = context;
        }


        [Authorize]
        [HttpPost]
        [Route("CreateNotes")]
        public ActionResult CreateNotes(NotesModel model)
        {

            try
            {
               int UserId= int.Parse(User.FindFirst("UserId").Value);
               NotesEntity notesEntity= notesBuss.CreateNotes(UserId,model);
                if (notesEntity != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { IsSuccuss = true, Message = "process succuss", Data = notesEntity });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { IsSuccuss = false, Message = " failed to insert", Data = false });
                }

            }
            catch(Exception ex) {

                throw ex;
            }
           

        }
        [Authorize]
        [HttpPut]
        [Route("PinNotes")]
        public ActionResult TogglePinNote(int NotesId)
        {

         int userId=int.Parse(User.FindFirst("UserId").Value) ;

           var response= notesBuss.TogglePinNote(NotesId,userId);

            if (response )
            {
                return Ok(new ResponseModel<bool> { IsSuccuss = true, Message = "Toggled pin succussfully", Data = true });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccuss = false, Message = "Toggled pin Unsuccussfull", Data = false });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("ArchiveNotes")]
        public ActionResult ToggleArchiveNote(int  NotesId)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            var response= notesBuss.ToggleArchiveNote(NotesId,userId);
            if (response)
            {
                return Ok(new ResponseModel<bool> { IsSuccuss = true, Message = "Toggle pin Archived  succussfully", Data = true });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccuss = false, Message = "Toggle pin unarchived ", Data = false });
            }
        }

        
        [Authorize]
        [HttpDelete]
        [Route("TrashNotes")]
        public ActionResult ToggleTrashNotes(int NotesId)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            var  response=  notesBuss.ToggleTrashNotes(NotesId,userId);

            if (response)
            {
                return Ok(new ResponseModel<bool> { IsSuccuss = true, Message = " notes been pushed to trash is   succuss", Data = true });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccuss = false, Message = " notes has not been trashed. Unsucuss!!  ", Data = false });
            }


        }
        [Authorize]
        [HttpPut]
        [Route("Backgroundcolour")]
        public ActionResult AddingBackgroundColour(string colour,int NotesId)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            var response= notesBuss.AddingBackgroundColour(colour.ToString (),NotesId,userId);

            if (response)
            {
                return Ok(new ResponseModel<bool> { IsSuccuss = true, Message = "background colour has been succussfully added ", Data = true });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { IsSuccuss = false, Message = " not added background colour !! ", Data = false });
            }

        }

        [Authorize]
        [HttpPut]
        [Route("Reminder")]

        public ActionResult SetReminder(int notesId, DateTime  dateTime) {

            int UserId = int.Parse(User.FindFirst("UserId").Value);

          var response=  notesBuss.SetReminder(notesId,UserId,dateTime);


            if (response)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "Succussfully reminder has been set ", Data = "succuss" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " Failed to set the Reminder. Please Enter valid date and time   ", Data = "Unsucuss!!" });
            }

        }
        [Authorize]
        [HttpPut]
        [Route("AddImage")]

        public ActionResult AddImageToNote(string ImagePath, int NotesId)
        {
            int UserId= int.Parse(User.FindFirst("UserId").Value);

            var response=notesBuss.AddImageToNotes(UserId, NotesId, ImagePath);
            if (response!=null)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "Succussfully Image  has been Added ", Data = "succuss" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " Failed to Add the Image ", Data = "Unsuccuss!!" });
            }

        }
        [Authorize]
        [HttpPut]
        [Route("UploadImage")]
        public ActionResult UploadImage (IFormFile formFile,int NotesId)
        {
            int UserId = int.Parse(User.FindFirst("UserId").Value);

            var response=notesBuss.UploadImage(UserId, NotesId, formFile);

            if (response != null)
            {
                return Ok(new ResponseModel<string> { IsSuccuss = true, Message = "Succussfully Image  has been Added ", Data = "succuss" });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " Failed to Add the Image ", Data = "Unsuccuss!!" });
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "NotesList";
            string serailzedNotesList;
            var NotesList=new List<NotesEntity>();
            var redisNotesList =await _cache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serailzedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList =JsonConvert.DeserializeObject<List<NotesEntity>>(serailzedNotesList);

            }
            else
            {
                NotesList = context.Notes.ToList();
                serailzedNotesList = JsonConvert.SerializeObject(NotesList);
                redisNotesList=Encoding.UTF8.GetBytes(serailzedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await _cache.SetAsync(cacheKey,redisNotesList,options);

            }
            return Ok(NotesList);

        }



        /* 3) find the notes on the basis of title and description, if its a single note show single
         data else if more than one note is found, show the list of notes
        */

        [HttpGet]
        [Route("Fetchlistofnotes")]
        public IActionResult FetchingNotesByTitleAndDescp(string Title,string Descrption)
        {
            var response = notesBuss.FetchingNotesByTitleAndDescp(Title, Descrption);

            if (response != null)
            {
                return Ok(new ResponseModel<List<NotesEntity>> { IsSuccuss = true, Message = "Succussfully notes with given title and description is fetched  ", Data = response });
            }
            else
            {
                return BadRequest(new ResponseModel<string> { IsSuccuss = false, Message = " Failed to fetch the notes list  ", Data = "Unsuccuss!!" });
            }



        }

    }
}
