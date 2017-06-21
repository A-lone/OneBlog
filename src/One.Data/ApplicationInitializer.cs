using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace One.Data
{
    public class ApplicationInitializer
    {
        private ApplicationContext _ctx;
        private UserManager<ApplicationUser> _userMgr;
        private RoleManager<IdentityRole> _roleMgr;
        public ApplicationInitializer(ApplicationContext ctx, UserManager<ApplicationUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _ctx = ctx;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        public async Task SeedAsync()
        {
            // Seed User
            if (await _userMgr.FindByNameAsync("chenrensong@outlook.com") == null)
            {

                // 添加系统角色
                if (!await _roleMgr.RoleExistsAsync("Administrator"))
                {
                    await _roleMgr.CreateAsync(new IdentityRole("Administrator"));
                }
                if (!await _roleMgr.RoleExistsAsync("Anonymous"))
                {
                    await _roleMgr.CreateAsync(new IdentityRole("Anonymous"));
                }
                if (!await _roleMgr.RoleExistsAsync("Editor"))
                {
                    await _roleMgr.CreateAsync(new IdentityRole("Editor"));
                }
                // 创建账号

                var user_crs = new ApplicationUser()
                {
                    Email = "chenrensong@outlook.com",
                    UserName = "chenrensong@outlook.com",
                    DisplayName = "陈仁松",
                    Signature = "根据当地的法律政策，该用户名不予显示",
                    EmailConfirmed = true
                };

                var userResult = await _userMgr.CreateAsync(user_crs, "19890501Boycrs.."); // Temp Password

                if (!userResult.Succeeded) throw new InvalidProgramException("Failed to create seed user");

                // 为账号分配角色
                var roleResult = await _userMgr.AddToRoleAsync(user_crs, "Administrator");

                if (!roleResult.Succeeded) throw new InvalidProgramException("Failed to create seed role");



                //var user_it = new ApplicationUser()
                //{
                //    Email = "icracker@msn.com",
                //    UserName = "icracker@msn.com",
                //    DisplayName = "IT资讯",
                //    EmailConfirmed = true
                //};

                //userResult = await _userMgr.CreateAsync(user_it, "19890501Boycrs.."); // Temp Password

                //roleResult = await _userMgr.AddToRoleAsync(user_it, "Administrator");




            }
        }
    }
}
