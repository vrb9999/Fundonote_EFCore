namespace DatabaseLayer.UserModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class UserLoginModel
    {
        [Required]
        [DefaultValue("sample@gmail.com")]
        public string Email { get; set; }

        [Required]
        [DefaultValue("Password@123")]
        public string Password { get; set; }
    }
}
