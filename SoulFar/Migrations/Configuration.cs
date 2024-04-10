namespace SoulFar.Migrations
{
    using SoulFar.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SoulFar.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SoulFar.Models.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.Roles.AddOrUpdate(new Role()
            {
                RoleID = 1,
                RoleName = "Admin",
                Description = "This is admin role."
            }, new Role()
            {
                RoleID = 2,
                RoleName = "Employee",
                Description = "This is employee role."
            });
            context.admin_Employee.AddOrUpdate(new admin_Employee()
            {
                FirstName = "Admin",
                LastName = "Role",
                Address = "abc",
                DateofBirth = DateTime.Now,
                Email = "admin@soulfar.com",
                EmpID = 1,
                Gender = "male",
                Phone = "938492338847234",
                PicturePath = ""
            });
            context.admin_Login.AddOrUpdate(new admin_Login()
            {
                EmpID = 1,
                Notes = "These are admin credentials.",
                Password = "2024!",
                RoleType = 1,
                UserName = "admin",
                RoleID = 1
            });
            context.PaymentTypes.AddOrUpdate(new PaymentType()
            {
                Description = "Stripe payment gateway.",
                PayTypeID = 1,
                TypeName = "Stripe"
            },new PaymentType()
            {
                Description = "Paypal payment gateway.",
                PayTypeID = 2,
                TypeName = "Paypal"
            });
        }
    }
}
