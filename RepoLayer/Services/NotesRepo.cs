using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using ModelLayer;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Enums;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace RepoLayer.Services
{
    public class NotesRepo :INotesRepo
    {
        private readonly FundooDBContext context;
        private readonly IConfiguration configuration;
        public NotesRepo(FundooDBContext context, IConfiguration configuration)
        {
            this.context = context;
           this.configuration = configuration;
        }

      public  NotesEntity CreateNotes(int userId, NotesModel model)
        {
            
            NotesEntity notesEntity = new NotesEntity();

            notesEntity.Title= model.Title;
            notesEntity.Description= model.Description;
            notesEntity.UserId= userId;
            notesEntity.CreatedAt= DateTime.Now;
            notesEntity.UpdatedAt= DateTime.Now;

            context.Notes.Add(notesEntity);
            context.SaveChanges();
            return notesEntity;
        }


        public NotesEntity GetNoteById(int noteId,int UserId)
        {
            NotesEntity result = context.Notes.FirstOrDefault(n => n.NotesId==noteId && n.UserId==UserId);

            if (result != null)
            {
                return result;
            }
               
            else
            {
              return  null;
            }
               
        }

        public bool TogglePinNote( int notesId,int userId)
        {
            var note=GetNoteById(notesId,userId);

            if(note!=null)
            {
                note.IsPin = !note.IsPin;
                note.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ToggleArchiveNote(int NotesId,int UserId)
        {
            var note=GetNoteById(NotesId,UserId);
            if(note!=null)
            {
                if (note.IsPin)
                    note.IsPin = false;
                note.IsArchive=!note.IsArchive;
                note.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("Not  found for requested id :" + NotesId);
            }
        }


        public bool ToggleTrashNotes(int NotesId,int UserId)
        {
            var note = GetNoteById(NotesId,UserId);
            if(note != null)
            {
                note.IsTrash= !note.IsTrash;
                note.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("Note is not found for a requested id : "+NotesId);
            }

        }

        public bool AddingBackgroundColour(string colour, int NotesId, int UserId)
        {

            var notesEntity = GetNoteById(NotesId, UserId);
          


            if (notesEntity == null)
            {
                throw new Exception("Note not found, please Enter valid note id ");
            }

            if (Enum.TryParse<BackgroudColour>(colour, out var chosenColour))
            {
                notesEntity.Colour = chosenColour.ToString();
                notesEntity.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("Invalid colour. please choose valid colour ");
            }

        }

        public bool SetReminder(int notesId, int userId, DateTime dateTime)
        {
           var notesEntity= GetNoteById(notesId, userId);

            if(notesEntity!=null&& dateTime> DateTime.Now)
            {
                notesEntity.Reminder = dateTime.ToString();
                context.SaveChanges();
                return true;
            }
            else
            {
                throw new Exception("Invalid date and time");
            }


        }

       public string AddImageToNotes(int userId, int notesId, string filePath)
        {
            var notes =GetNoteById(notesId, userId);
            if (notes != null)
            {
                Account account = new Account("dv7snxvoi", "563851866817452", "Vre-2kSbyW05eGeshTe5pgRZKpI");
                Cloudinary cloudinary= new Cloudinary(account);
                ImageUploadParams uploadParams=new ImageUploadParams()
                {
                    File=new FileDescription(filePath),
                    PublicId =notes.Title
                };

                ImageUploadResult uploadResult=cloudinary.Upload(uploadParams);
                notes.UpdatedAt = DateTime.Now;
                notes.Image = uploadResult.Url.ToString();
                context.SaveChanges();
                return "Upload Succussfull";
            }
            else
            {
                    
                throw new Exception("Notes is not present by the given id ");
            }


        }

       public ImageUploadResult UploadImage(int userId, int notesId, IFormFile formFile)
        {
           var res= GetNoteById(notesId,userId);
            try
            {
                if (res != null)
                {
                    Account account = new Account(configuration["Cloudinary:cloudName"], configuration["Cloudinary:APIKey"], configuration["Cloudinary:APISecret"]);
                    Cloudinary cloudinary = new Cloudinary(account);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(formFile.FileName, formFile.OpenReadStream()),
                    };

                    var uploadImages = cloudinary.Upload(uploadParams);
                    if (uploadImages != null)
                    {
                        return uploadImages;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    throw new Exception("Incorrect id given unable to find the notes");
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}


