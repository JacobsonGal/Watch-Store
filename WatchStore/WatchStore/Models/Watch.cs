using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchStore.Models
{
    public class Watch
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Watch ID")]
        public int WatchID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Watch Name")]
        public string WatchName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Type")]
        public string WatchType { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Water Resistant")]
        public bool Resistant { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int price { get; set; }


        public virtual ICollection<Deal> Deals { get; set; }

    }
}