using BusinessLayer.Interfaces;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class CollaboratorBusiness : ICollaboratorBuss
    {
        private readonly ICollaboratorRepo collaboratorRepo;

        public CollaboratorBusiness(ICollaboratorRepo collaboratorRepo)
        {
            this.collaboratorRepo=collaboratorRepo;
        }

        public  CollaboratorEntity AddCollaborator(int userId, int notesId, string email)
        {
           return collaboratorRepo.AddCollaborator(userId, notesId, email);
        }

       public bool RemoveCollaborator(int userId, int notesId, string email)
        {
            return collaboratorRepo.RemoveCollaborator(userId , notesId, email);
        }
    }
}
