using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchStore.Models
{
    public class Manager
    {
        [Required]
        public int ManagerID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Manager Name")]
        public string ManagerName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Password")]
        public string ManagerPassword { get; set; }

    }
}