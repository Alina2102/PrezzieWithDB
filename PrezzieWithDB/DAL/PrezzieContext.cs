﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using PrezzieWithDB.Models;

namespace PrezzieWithDB.DAL
{
    public class PrezzieContext : DbContext
    {
        public PrezzieContext() : base("PrezzieContext")
        {
        }

        public DbSet<Customer> customers { get; set; }
        public DbSet<Profile> profiles { get; set; }
        public DbSet<Souvenir> souvenirs { get; set; }
        public DbSet<SouvenirInfo> souvenirInfos { get; set; }
        public DbSet<Request> requests { get; set; }
        public DbSet<Rating> ratings { get; set; }
        public DbSet<CustomerRating> customerRatings { get; set; }

    }
}