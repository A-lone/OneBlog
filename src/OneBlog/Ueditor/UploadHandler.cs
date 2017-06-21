using Microsoft.Extensions.DependencyInjection;
using OneBlog.Helpers;
using OneBlog.Services;
using System;
using System.IO;
using System.Linq;

namespace OneBlog.UEditor
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : IHandler
    {
        public UploadConfig UploadConfig { get; private set; }

        public UploadHandler(UploadConfig config)
        {
            this.UploadConfig = config;
        }

        public object Process()
        {

            //var memberService = ServiceFactory.Get<IMembershipService>();
            //var localizationService = ServiceFactory.Get<ILocalizationService>();
            //var LoggedOnReadOnlyUser = !string.IsNullOrEmpty(Username) ? memberService.GetUser(Username, true) : null;
            //if (LoggedOnReadOnlyUser == null)
            //{
            //    return new
            //    {
            //        state = "授权错误",
            //        error = "授权错误"
            //    };
            //}

            var qiniuService = DI.ServiceProvider.GetRequiredService<QiniuService>();
            byte[] uploadFileBytes = null;
            string uploadFileName = null;
            var url = string.Empty;

            if (UploadConfig.Base64)
            {
                uploadFileName = UploadConfig.Base64Filename;
                uploadFileBytes = Convert.FromBase64String(AspNetCoreHelper.HttpContext.Request.Form[UploadConfig.UploadFieldName]);
                url = qiniuService.Upload(uploadFileBytes).Result;
            }
            else
            {
                var file = AspNetCoreHelper.HttpContext.Request.Form.Files[UploadConfig.UploadFieldName];
                using (var stream = file.OpenReadStream())
                {
                    url = qiniuService.Upload(file).Result;
                }
            }

            return new
            {
                state = !string.IsNullOrEmpty(url) ? "SUCCESS" : "Error",
                url = url,
                title = "",
                original = "",
                error = ""
            };

        }

        private bool CheckFileType(string filename)
        {
            var fileExtension = Path.GetExtension(filename).ToLower();
            return UploadConfig.AllowExtensions.Select(x => x.ToLower()).Contains(fileExtension);
        }

        private bool CheckFileSize(int size)
        {
            return size < UploadConfig.SizeLimit;
        }
    }

    public class UploadConfig
    {
        /// <summary>
        /// 文件命名规则
        /// </summary>
        public string PathFormat { get; set; }

        /// <summary>
        /// 上传表单域名称
        /// </summary>
        public string UploadFieldName { get; set; }

        /// <summary>
        /// 上传大小限制
        /// </summary>
        public int SizeLimit { get; set; }

        /// <summary>
        /// 上传允许的文件格式
        /// </summary>
        public string[] AllowExtensions { get; set; }

        /// <summary>
        /// 文件是否以 Base64 的形式上传
        /// </summary>
        public bool Base64 { get; set; }

        /// <summary>
        /// Base64 字符串所表示的文件名
        /// </summary>
        public string Base64Filename { get; set; }
    }


}