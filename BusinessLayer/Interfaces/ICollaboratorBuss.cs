using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ICollaboratorBuss
    {
        CollaboratorEntity AddCollaborator(int userId, int notesId, string email);
        bool RemoveCollaborator(int userId, int notesId, string email);
    }
}
