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
                ratingValue = 1,
                ratingValueDescription = "bad"
            });
            ratings.Add(new Rating()
            {
                ratingValue = 2,
                ratingValueDescription = "poor"
            });
            ratings.Add(new Rating()
            {
                ratingValue = 3,
                ratingValueDescription = "okay"
            });
            ratings.Add(new Rating()
            {
                ratingValue = 4,
                ratingValueDescription = "good"
            });
            ratings.Add(new Rating()
            {
                ratingValue = 5,
                ratingValueDescription = "super"
            });

            foreach (Rating rating in ratings)
                context.ratings.Add(rating);
            base.Seed(context);
        }
    }
}
