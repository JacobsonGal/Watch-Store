using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchStore.Models
{
    public class Client
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Client Name")]
        public int ClientID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string ClientFirstName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string ClientLastName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "PhoneNumber")]
        public int PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }

    }
}