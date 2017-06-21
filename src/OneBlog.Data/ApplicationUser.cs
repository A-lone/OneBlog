using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace One.Data
{
    /// <summary>
    /// UserName = Email 
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 用户签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public string SiteUrl { get; set; }



        public virtual IList<Posts> Posts { get; set; }

        public virtual IList<Comments> Comments { get; set; }



    }
}