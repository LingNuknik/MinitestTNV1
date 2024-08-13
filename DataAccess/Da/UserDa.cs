using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Da
{
    public class UserDa
    {
        private readonly CourseContext db;
        public UserDa(CourseContext db)
        {
            this.db = db;
        }

        public async Task Insert(ApplicationUser data)
        {
            data.NormalizedUserName = data.UserName.ToUpper();
            data.NormalizedEmail = data.Email.ToUpper();

            data.FirstName = data.FirstName.ToUpper();
            data.LastName = data.LastName.ToUpper();
            data.PhoneNumber = data.PhoneNumber.ToUpper();
            data.UserName = data.UserName.ToLower();
            data.Email = data.Email.ToLower();

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            data.PasswordHash = passwordHasher.HashPassword(data, data.Password);



            await db.AddAsync(data);
            await db.SaveChangesAsync();
        }


    }
}
