using Microsoft.EntityFrameworkCore.Internal;
using ModelLayer;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoLayer.Services
{
    public class LabelRepo : ILabelRepo
    {
        private readonly FundooDBContext context;
        public LabelRepo(FundooDBContext context)
        {
            this.context = context;
        }

        public LabelEntity AddLabel(string LabelName, int userId, int notesId)
        {
            bool result = GetNotesById(userId, notesId);

            if (result)
            {
                bool res = context.Labels.Any(n => n.LabelName == LabelName && n.NotesId == notesId && n.UserId == userId);

                if (!res)
                {
                    LabelEntity label = new LabelEntity();
                    label.UserId = userId;
                    label.NotesId = notesId;
                    label.LabelName = LabelName;
                    context.Add(label);
                    context.SaveChanges();
                    return label;
                }
                else
                {
                    throw new Exception("Already label exists and Same label connot be added for the Notes");
                }
            }
            else
            {
                throw new Exception("Notes is not present for the given id..");
            }
        }

        public bool GetNotesById(int UserId, int NotesId)
        {
            bool result = context.Notes.Any(u => u.UserId == UserId && u.NotesId == NotesId);

            return result;
        }

        public bool RenameLabel(int notesId, int userId, string oldLabelName, string newLabelName)
        {
            var notes = (from n in context.Notes where n.UserId == userId && n.NotesId == notesId select n).Any();

            if (notes)
            {

                var res = (from i in context.Labels where i.LabelName == newLabelName select i).Any();

                if (!res)
                {
                    var replace = from i in context.Labels where i.LabelName == oldLabelName && i.UserId == userId && i.NotesId == notesId select i;
                    foreach (var i in replace)
                    {
                        i.LabelName = newLabelName;
                    }
                    return true;
                }
                else
                {
                    throw new Exception("label name cannot be same always should be unique");
                }
            }
            else
            {
                throw new Exception("note is not present for the given id");
            }
        }

        public bool RemoveLabel(int userId,int NotesId, string labelName)
        {
            var labelEntity = context.Labels.FirstOrDefault(n => n.LabelName == labelName && userId == n.UserId&& n.UserId==userId);

            if (labelEntity != null)
            {
                context.Remove(labelEntity);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

       public LabelEntity FetchByLabelName(int notesId, int userId, string labelName)
        {

           LabelEntity labelEntity=(from i in context.Labels where i.NotesId == notesId && i.UserId == userId && i.LabelName == labelName select i).FirstOrDefault();

            if (labelEntity != null)
            {
                return labelEntity;
            }
            else
            {
                throw new Exception("Incorrect label name or id given ");
            }



        }

       
    }
}