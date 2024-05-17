using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using ModelLayer;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interfaces
{
    public interface INotesRepo
    {
        NotesEntity CreateNotes(int userId, NotesModel model);

        public bool TogglePinNote(int notesId,int UserId);

        public bool ToggleArchiveNote(int NotesId, int UserId);

        public bool ToggleTrashNotes(int NotesId, int UserId);
        bool AddingBackgroundColour(string colour, int NotesId,int UserId);
        bool SetReminder(int notesId, int userId, DateTime dateTime);
        string AddImageToNotes(int userId, int notesId, string imagePath);
        ImageUploadResult UploadImage(int userId, int notesId, IFormFile formFile);
    }
}
