using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchStore.Models
{
    public class Deal
    {
        public int DealID { get; set; }
        public int ClientID { get; set; }
        public int WatchID { get; set; }
        public virtual Client Client { get; set; }
        public virtual Watch Watch { get; set; }
    }
}