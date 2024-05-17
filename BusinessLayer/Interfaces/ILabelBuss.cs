using ModelLayer;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ILabelBuss
    {
        public LabelEntity AddLabel(string LebelName, int userId, int notesId);
        LabelEntity FetchLabelByName(int notesId, int userId, string labelName);
        bool RemoveLabel(int userId,int NotesId, string labelName);
        bool RenameLabel(int notesId, int userId, string oldLabelName, string newLabelName);
    }
}
