using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeWork3.DataAnotations.Attributes;

namespace HomeWork3.Models
{
    [ModelBinder(typeof(ModelBinder.DateTimeBinder))]
    public class UserViewModel
    {
        public int UId { get; set; }

        [Required]
        [Remote("IsCorrectLogin", "User", ErrorMessage = "This Login already exists.")]
        public string Login { get; set; }
        //public Nullable<int> Age { get; set; }

        [Required]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Please enter proper contact details.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone must be 10 characters long.")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Email", ErrorMessage = "Enter Emails are diffirent")]
        public string ConfirmedEmail { get; set; }

        [RequiredIf(OtherProperty = "IsActive", OtherPropertyValue = false, ErrorMessage = "BlockDescription cant be empty")]
        public string BlockDescription { get; set; }
        public System.DateTime BirthDay { get; set; }
        public DateTime DateCreated { get; set; }
        public int ImageID { get; set; }

        public virtual HttpPostedFileBase Image { get; set; }
    }
}