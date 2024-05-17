using ModelLayer;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interfaces
{
    public interface ILabelRepo
    {
       public LabelEntity AddLabel(string LabelName, int userId, int notesId);
        LabelEntity FetchByLabelName(int notesId, int userId, string labelName);
        bool RemoveLabel(int userId,int NotesId, string labelName);
        bool RenameLabel(int notesId, int userId, string oldLabelName, string newLabelName);
    }
}
