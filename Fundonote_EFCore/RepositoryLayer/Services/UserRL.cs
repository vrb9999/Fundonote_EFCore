// <copyright file="UserRL.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RepositoryLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using DatabaseLayer.Entities;
    using DatabaseLayer.UserModels;
    using Experimental.System.Messaging;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using RepositoryLayer.Interface;
    using RepositoryLayer.Services.Entities;

    public class UserRL : IUserRL
    {
        private readonly FundoContext fundoContext;
        private readonly IConfiguration configuration;

        public UserRL(FundoContext fundoContext, IConfiguration configuration)
        {
            this.fundoContext = fundoContext;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public void AddUser(UserPostModel userPostModel)
        {
            try
            {
                User user = new User();
                user.FirstName = userPostModel.FirstName;
                user.LastName = userPostModel.LastName;
                user.Email = userPostModel.Email;
                user.Password = userPostModel.Password;
                user.CreatedDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;

                this.fundoContext.Add(user);
                this.fundoContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return this.fundoContext.Users.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string LoginUser(UserLoginModel loginUser)
        {
            try
            {
                var user = this.fundoContext.Users.Where(x => x.Email == loginUser.Email && x.Password == loginUser.Password).FirstOrDefault();
                if (user == null)
                {
                    return null;
                }

                return this.GenerateJWTToken(loginUser.Email, user.UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerateJWTToken(string email, int UserId)
        {
            try
            {
                // generate token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("Email", email),
                    new Claim("UserId",UserId.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddHours(2),

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ForgetPasswordUser(string email)
        {
            try
            {
                var user = this.fundoContext.Users.Where(x => x.Email == email).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }

                MessageQueue messageQueue;

                // ADD MESSAGE TO QUEUE
                if (MessageQueue.Exists(@".\Private$\FundooQueue"))
                {
                    messageQueue = new MessageQueue(@".\Private$\FundooQueue");
                }
                else
                {
                    messageQueue = MessageQueue.Create(@".\Private$\FundooQueue");
                }

                Message myMessage = new Message();
                myMessage.Formatter = new BinaryMessageFormatter();
                myMessage.Body = this.GenerateJWTToken(email, user.UserId);
                myMessage.Label = "Forget Password Email";
                messageQueue.Send(myMessage);
                Message msg = messageQueue.Receive();
                msg.Formatter = new BinaryMessageFormatter();
                EmailService.SendEmail(email, msg.Body.ToString(), user.FirstName);
                messageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(this.MsmqQueue_ReceiveCompleted);

                messageQueue.BeginReceive();
                messageQueue.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MsmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                EmailService.SendEmail(e.Message.ToString(), this.GenerateToken(e.Message.ToString()), e.Message.ToString());
                queue.BeginReceive();
            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode ==
                    MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied. " + "Queue might be a system queue.");
                }
            }
        }

        private string GenerateToken(string email)
        {
            try
            {
                // generate token
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("Email", email),

                    }),
                    Expires = DateTime.UtcNow.AddHours(2),

                    SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature),
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
