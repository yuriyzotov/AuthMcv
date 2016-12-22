namespace Auth.Net.Migrations
{
    
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Auth.Net.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Auth.Net.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
                context.Users.AddOrUpdate(
                  u=> u.Id,
                  new ApplicationUser { Id= "ed9397e4-fc17-440f-b9fd-b0b246090bba", UserName= "User1", PasswordHash= "AHakmHS57LpZcykMTyp3XaxV4eD7NRXywbUvY5zol1bd1qWDkABeK9OieeRhckt7fQ==", SecurityStamp=Guid.NewGuid().ToString() },
                  new ApplicationUser { Id = "c1e418d9-f4d8-4623-9519-74f85b5ba55d", UserName = "User2", PasswordHash = "AKyOodPHCWeYEZMOq4svfy3Cd2wuUcornMRBORQIyL1Wc2LeVHQvkVlHr/vZQuEPbA==", SecurityStamp = Guid.NewGuid().ToString() },
                  new ApplicationUser { Id = "12e877ab-c41b-491e-b160-ad957bc486aa", UserName = StringConstants.MasterUserName, PasswordHash = "AFGfo8dYXTbxyrfttkH6p/78v4BeVSqtznf1J/Zwqix1ybWW+jKjtKCKWEhu8EzahA==", SecurityStamp = Guid.NewGuid().ToString() }
                );
            
        }
    }
}
