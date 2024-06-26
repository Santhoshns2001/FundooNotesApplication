﻿using BusinessLayer.Interfaces;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using ModelLayer;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{

    public class NotesBusinnes : INotesBuss
    {
        private readonly INotesRepo notesRepo;

        public NotesBusinnes(INotesRepo notesRepo)
        {
         this. notesRepo=notesRepo;
        }

        public NotesEntity CreateNotes(int userId, NotesModel model)
        {
            return notesRepo.CreateNotes(userId, model);
            
        }

        public bool TogglePinNote(int notesId, int UserId)
        {
            return notesRepo.TogglePinNote(notesId,UserId);
        }

        public  bool ToggleArchiveNote(int notesId, int UserId)
        {
          return  notesRepo.ToggleArchiveNote(notesId,UserId);
        }

       public bool ToggleTrashNotes(int notesId, int UserId)
        {
            return notesRepo.ToggleTrashNotes(notesId, UserId);
        }

        bool INotesBuss.AddingBackgroundColour(string colour, int NotesId, int UserId)
        {
            return notesRepo.AddingBackgroundColour(colour,NotesId,UserId);
        }

       public bool SetReminder(int notesId, int userId, DateTime dateTime)
        {
            return notesRepo.SetReminder(notesId, userId, dateTime);
        }

       public  string AddImageToNotes(int userId, int notesId, string imagePath)
        {
           return notesRepo.AddImageToNotes(userId,notesId,imagePath);
        }

       public ImageUploadResult UploadImage(int userId, int notesId, IFormFile formFile)
        {
            return notesRepo.UploadImage(userId, notesId, formFile);
        }

      public  List<NotesEntity> FetchingNotesByTitleAndDescp(string title, string descrption)
        {
            return notesRepo.FetchingNotesByTitleAndDescp(title, descrption);
        }
    }
}
