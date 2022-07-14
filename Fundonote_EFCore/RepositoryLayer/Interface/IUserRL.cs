// <copyright file="IUserRL.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RepositoryLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using DatabaseLayer.UserModels;

    public interface IUserRL
    {
        public void AddUser(UserPostModel userPostModel);
    }
}
