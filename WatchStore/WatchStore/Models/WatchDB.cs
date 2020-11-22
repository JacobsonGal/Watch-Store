using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WatchStore.Models
{
    public class WatchDB : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Watch> Watches { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Deal> Deals { get; set; }    }
}