using Microsoft.EntityFrameworkCore;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Context
{
    public class FundooDBContext :DbContext
    {
        public FundooDBContext(DbContextOptions dbContext) : base(dbContext) { }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<ReviewTable> ReviewTable { get; set; }

        public DbSet<NotesEntity>   Notes { get; set; }

        public DbSet<LabelEntity> Labels { get; set; }

        public DbSet<CollaboratorEntity> CollaboratorTable { get; set; }    

    }

}
