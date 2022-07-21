// <copyright file="FundoContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DatabaseLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using RepositoryLayer.Services.Entities;

    public class FundoContext : DbContext
    {
        public FundoContext(DbContextOptions options)
            : base(options)
        {
        }

        // Users - table name ; User - Entity class
        public DbSet<User> Users { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Label> Labels { get; set; }
    }
}
