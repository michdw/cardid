using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cardid.Models
{
    public class User
    {
        public string UserID { get; set; }
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$")]
        [EmailAddress(ErrorMessage = "Please enter email in a valid format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public User TrimValues()
        {
            DisplayName = DisplayName.Trim();
            Email = Email.Trim();
            Password = Password.Trim();
            return this;
        }

    }
}