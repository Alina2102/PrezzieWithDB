namespace PrezzieWithDB.Migrations
{
    using PrezzieWithDB.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PrezzieWithDB.DAL.PrezzieContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PrezzieWithDB.DAL.PrezzieContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            List<Rating> ratings = new List<Rating>();

            ratings.Add(new Rating()
            {
                ratingID = 1,
                ratingDescription = "bad"
            });
            ratings.Add(new Rating()
            {
                ratingID = 2,
                ratingDescription = "poor"
            });
            ratings.Add(new Rating()
            {
                ratingID = 3,
                ratingDescription = "okay"
            });
            ratings.Add(new Rating()
            {
                ratingID = 4,
                ratingDescription = "good"
            });
            ratings.Add(new Rating()
            {
                ratingID = 5,
                ratingDescription = "super"
            });

            foreach (Rating rating in ratings)
                context.ratings.Add(rating);
            base.Seed(context);
        }
    }
}
