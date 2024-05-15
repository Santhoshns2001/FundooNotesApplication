using ModelLayer;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface INotesBuss
    {
        bool AddingBackgroundColour(string colour,int NotesId,int UserId);
        public NotesEntity CreateNotes(int userId, NotesModel model);
        bool SetReminder(int notesId, int userId, DateTime dateTime);
        bool ToggleArchiveNote(int notesId,int userId);
        bool TogglePinNote(int notesId,int UserId);
        bool ToggleTrashNotes(int notesId,int UserId);
    }
}
