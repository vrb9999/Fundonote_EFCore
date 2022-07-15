// <copyright file="IUserBL.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BusinessLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DatabaseLayer.UserModels;
    using RepositoryLayer.Services.Entities;

    public interface IUserBL
    {
        public void AddUser(UserPostModel userPostModel);

        public List<User> GetAllUsers();
    }
}
