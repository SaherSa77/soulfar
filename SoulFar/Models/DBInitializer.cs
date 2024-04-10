using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

namespace SoulFar.Models
{
    public class DBInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            Role role1 = new Role()
            {
                RoleID = 1,
                RoleName = "Admin",
                Description = "This is admin role."
            };
            Role role2 = new Role()
            {
                RoleID = 2,
                RoleName = "Employee",
                Description = "This is employee role."
            };
            context.Roles.Add(role1);
            context.Roles.Add(role2);
            admin_Login login = new admin_Login()
            {
                EmpID = 1,
                Notes = "These are admin credentials.",
                Password = "2024!",
                RoleType = 1,
                UserName = "admin",
                RoleID = 1
            };
            context.admin_Login.Add(login);
            var emp = new admin_Employee()
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
            };
            context.admin_Employee.Add(emp);
            base.Seed(context);
        }
    }
}