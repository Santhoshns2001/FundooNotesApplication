using Microsoft.EntityFrameworkCore.Internal;
using ModelLayer;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Enums;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoLayer.Services
{
    public class NotesRepo :INotesRepo
    {
        private readonly FundooDBContext context;

        public NotesRepo(FundooDBContext context)
        {
            this.context = context;
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

            var result = GetNoteById(NotesId, UserId);
            NotesEntity notesEntity = new NotesEntity();


            if (result == null)
            {
                throw new Exception("Note not found, please Enter valid note and user id ");
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
           var result= GetNoteById(notesId, userId);

            NotesEntity notesEntity = new NotesEntity();

            if(result!=null&& dateTime> DateTime.Now)
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
    }
}


