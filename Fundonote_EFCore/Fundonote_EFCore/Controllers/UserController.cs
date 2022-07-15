// <copyright file="UserController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Fundonote_EFCore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BusinessLayer.Interface;
    using DatabaseLayer.Entities;
    using DatabaseLayer.UserModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NLogger.Interface;
    using RepositoryLayer.Services;
    using RepositoryLayer.Services.Entities;

    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILoggerManager logger;
        private readonly FundoContext fundoContext;
        private readonly IUserBL userBL;

        public UserController(FundoContext fundoContext, IUserBL userBL, ILoggerManager logger)
        {
            this.fundoContext = fundoContext;
            this.userBL = userBL;
            this.logger = logger;
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserPostModel userPostModel)
        {
            try
            {
                this.logger.LogInfo($"User Registration Email : {userPostModel.Email}");
                this.userBL.AddUser(userPostModel);
                return this.Ok(new { success = true, Message = "User Registration Sucessfull" });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"User Registration Failed: {userPostModel.Email}");
                throw ex;
            }
        }

        [HttpGet("GetAllUser")]
        public IActionResult GetAllUser()
        {
            try
            {
                this.logger.LogInfo("Fetched all users");
                List<User> getUsers = new List<User>();
                getUsers = this.userBL.GetAllUsers();
                return this.Ok(new { success = true, Message = "Get all users fetched", data = getUsers });
            }
            catch (Exception ex)
            {
                this.logger.LogError("Failed to fetch all users");
                throw ex;
            }
        }
    }
}
