namespace DatabaseLayer.UserModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class PasswordModel
    {
        [Required]
        [DefaultValue("Password@123")]
        [RegularExpression("(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$_])[a-zA-Z0-9@#$_]{8,}", ErrorMessage = "Please Enter Atleast 8 character with Alteast one numeric,special character")]
        public string Password { get; set; }

        [Required]
        [DefaultValue("Password@123")]
        [RegularExpression("(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$_])[a-zA-Z0-9@#$_]{8,}", ErrorMessage = "Confirm Password should match Password")]
        public string ConfirmPassword { get; set; }
    }
}
