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

        [Required(ErrorMessage = "Please enter a name to use on this site.")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$")]
        [EmailAddress(ErrorMessage = "Please enter an email in a valid format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Oops, passwords don't match.")]
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