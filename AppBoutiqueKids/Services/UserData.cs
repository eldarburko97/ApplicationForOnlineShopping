using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface IUserData
    {
        User Get(int id);
        void AddToRole(int id, string role);
    }

    public class UserData : IUserData
    {
        private readonly ApplicationDbContext context;

        public UserData(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddToRole(int id, string role)
        {
            if (id == 1)
            {
                return;
            }

            Role roleObj = context.Roles.Where(x => x.Name == role).FirstOrDefault();
            if (roleObj == null)
            {
                return;
            }

            var userRole = context.UserRoles.Where(z => z.UserId == id).FirstOrDefault();
            if (userRole == null)
            {
                context.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<int>
                {
                    RoleId = roleObj.Id,
                    UserId = id
                });
            }
            else // 
            {
                userRole.RoleId = roleObj.Id;
            }

            context.SaveChanges();
        }

        public User Get(int id)
        {
            return context.Users.Find(id);
        }
    }
}
