using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interfaces
{
    public interface ICollaboratorRepo
    {
        CollaboratorEntity AddCollaborator(int userId, int notesId, string email);
        bool RemoveCollaborator(int userId, int notesId, string email);
    }
}
