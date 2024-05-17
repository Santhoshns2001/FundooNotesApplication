using BusinessLayer.Interfaces;
using ModelLayer;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBusiness : ILabelBuss
    {
        private readonly ILabelRepo labelRepo;
        public LabelBusiness(ILabelRepo labelRepo)
        {
            this.labelRepo = labelRepo;   
        }

        public LabelEntity AddLabel(string LabelName, int userId, int notesId)
        {
            return labelRepo.AddLabel(LabelName, userId, notesId);
        }

       public bool RenameLabel(int notesId, int userId, string oldLabelName, string newLabelName)
        {
           return labelRepo.RenameLabel(notesId,userId,oldLabelName, newLabelName);
        }

       public bool RemoveLabel(int userId,int NotesId, string labelName)
        {
            return labelRepo.RemoveLabel(userId, NotesId,labelName);
        }

       public LabelEntity FetchLabelByName(int notesId, int userId, string labelName)
        {

            return labelRepo.FetchByLabelName(notesId, userId, labelName);
        }

       
    }
}
