using OneBlog.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OneBlog.Data
{

    public class RolesRepository : IRolesRepository
    {
        private RoleManager<IdentityRole> _roleMgr;
        ApplicationContext _context;

        public RolesRepository(RoleManager<IdentityRole> roleMgr, ApplicationContext context)
        {
            _roleMgr = roleMgr;
            _context = context;
        }

        public RoleItem Add(RoleItem role)
        {
            var task = _roleMgr.CreateAsync(new IdentityRole(role.RoleName));
            var result = task.Result;
            if (result.Succeeded)
            {
                return role;
            }
            return null;
        }

        public IEnumerable<RoleItem> Find(int take = 10, int skip = 0, string filter = "", string order = "")
        {
            var userRoles = new List<RoleItem>();

            if (take == 0)
            {
                take = _context.Roles.Count();
            }

            var roles = _context.Roles.Skip(skip)
                     .Take(take)
                     .ToList();

            foreach (var m in roles)
            {
                userRoles.Add(new RoleItem
                {
                    IsChecked = false,
                    RoleName = m.Name
                });
            }
            return userRoles;
        }

        public RoleItem FindById(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Group> GetRoleRights(string role)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RoleItem> GetUserRoles(string id)
        {
            var roles = new List<Data.Models.RoleItem>();
            roles.AddRange(_context.Roles.Include(m => m.Users)
                .Select(r => new Data.Models.RoleItem
                {
                    RoleName = r.Name,
                    IsChecked = r.Users.FirstOrDefault(m => m.UserId == id) != null
                }));

            roles.Sort((r1, r2) => string.Compare(r1.RoleName, r2.RoleName));
            return roles;
        }

        public bool Remove(string id)
        {
            throw new NotImplementedException();
        }

        public bool SaveRights(List<Group> rights, string id)
        {
            throw new NotImplementedException();
        }
    }
}
