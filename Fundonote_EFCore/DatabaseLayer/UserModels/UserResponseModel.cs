namespace DatabaseLayer.UserModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserResponseModel
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
