// <copyright file="UserPostModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DatabaseLayer.UserModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class UserPostModel
    {
        [Required]
        [DefaultValue("Firstname")]
        [RegularExpression("[A-Z]{1}[a-z]{3,20}", ErrorMessage = "Please Enter for Firstname Atleast 3 character with first letter capital")]
        public string FirstName { get; set; }

        [Required]
        [DefaultValue("Lastname")]
        [RegularExpression("[A-Z]{1}[a-z]{3,20}", ErrorMessage = "Please Enter for LastName Atleast 3 character with first letter capital")]
        public string LastName { get; set; }

        [Required]
        [DefaultValue("sample@gmail.com")]
        [RegularExpression("^([A-Za-z0-9]{3,20})([.][A-Za-z0-9]{1,10})*([@][A-Za-z]{2,5})+[.][A-Za-z]{2,3}([.][A-Za-z]{2,3})?$", ErrorMessage = "Please Enter Valid Email")]
        public string Email { get; set; }

        [Required]
        [DefaultValue("Password@123")]
        [RegularExpression("(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$_])[a-zA-Z0-9@#$_]{8,}", ErrorMessage = "Please Enter Atleast 8 character with Alteast one numeric,special character")]
        public string Password { get; set; }
    }
}
