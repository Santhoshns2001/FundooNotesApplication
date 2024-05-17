using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using RepoLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoLayer.Services
{
    public class CollaboratorRepo : ICollaboratorRepo
    {
        private readonly FundooDBContext context;
        public CollaboratorRepo(FundooDBContext context)
        {
            this.context = context;
        }

        public CollaboratorEntity AddCollaborator(int userId, int notesId, string email)
        {
           bool res= (from i in context.Users where i.Email == email  select i).Any();

            if (res)
            {
                if (GetNotesById(userId, notesId) && res == true)
                {
                    var result = (from n in context.CollaboratorTable where n.Email == email  && n.UserId==userId && n.NotesId==notesId select n).Any();
                    if (!result)
                    {
                        CollaboratorEntity collaborator = new CollaboratorEntity();
                        collaborator.NotesId = notesId;
                        collaborator.Email = email;
                        collaborator.UserId = userId;
                        context.CollaboratorTable.Add(collaborator);
                        context.SaveChanges();
                        return collaborator;
                    }
                    else
                    {
                        throw new Exception("Cannot collaborate with same person Again!!!");
                    }
                }
                else
                {
                    throw new Exception("Unable to find  notes by the given id, ");
                }
            }
            else
            {
                throw new Exception("Email is not present  in the database ");
            }
        }


        public bool GetNotesById(int UserId,int NotesId) {

            var result = context.Notes.Any(a => a.NotesId == NotesId&& a.UserId==UserId);
            return result;

        }

       public  bool RemoveCollaborator(int userId, int notesId, string email)
        {
        CollaboratorEntity collaboratoryEntity=  ( from i in context.CollaboratorTable where i.UserId==userId && i.NotesId==notesId && i.Email==email select i).FirstOrDefault();
            if (collaboratoryEntity!=null)
            {
                context.Remove(collaboratoryEntity);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
                throw new Exception("collaboratories details not found to remove");
                
            }
        }
    }
}
