using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OneBlog.Configuration;
using OneBlog.Helpers;
using Qiniu.Conf;
using Qiniu.IO;
using Qiniu.RS;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneBlog.Services
{
    public class QiniuService
    {
        private readonly IOptions<QiniuSettings> _qiniuSettings;

        public QiniuService(IOptions<QiniuSettings> qiniuSettings)
        {
            _qiniuSettings = qiniuSettings;
            Config.ACCESS_KEY = _qiniuSettings.Value.AccessKey;
            Config.SECRET_KEY = _qiniuSettings.Value.SecretKey;
            Config.UP_HOST = _qiniuSettings.Value.UPHost;
        }

        public async Task<string> Upload(HttpContent httpContent)
        {
            string fileExtension = "png";
            switch (httpContent.Headers.ContentType.ToString())
            {
                case "image/gif":
                    fileExtension = "gif";
                    break;
                case "image/jpeg":
                    fileExtension = "jpg";
                    break;
                case "image/png":
                    fileExtension = "png";
                    break;
                case "image/bmp":
                    fileExtension = "bmp";
                    break;
            }
            string url = string.Empty;
            using (var stream = httpContent.ReadAsStreamAsync().Result)
            {
                Guid guid = GuidComb.GenerateComb();
                url = await Upload(guid.ToString() + "." + fileExtension, stream);
            }
            return url;
        }

        public async Task<string> Upload(IFormFile file)
        {

            string fileExtension = "png";
            switch (file.ContentType)
            {
                case "image/gif":
                    fileExtension = "gif";
                    break;
                case "image/jpeg":
                    fileExtension = "jpg";
                    break;
                case "image/png":
                    fileExtension = "png";
                    break;
                case "image/bmp":
                    fileExtension = "bmp";
                    break;
            }
            string url = string.Empty;
            using (var stream = file.OpenReadStream())
            {
                Guid guid = GuidComb.GenerateComb();
                url = await Upload(guid.ToString() + "." + fileExtension, stream);
            }
            return url;
        }


        public async Task<string> Upload(string key, byte[] buffer)
        {
            using (Stream stream = new MemoryStream(buffer))
            {
                return await Upload(key, stream);
            }
        }


        public async Task<string> Upload(string key, Stream stream)
        {
            var target = new IOClient();
            var result = await target.PutAsync(new PutPolicy(_qiniuSettings.Value.Bucket).Token(), key, stream, null);
            var url = string.Format("{0}/{1}", _qiniuSettings.Value.Domain, result.key);
            return url;
        }

        public async Task<string> Upload(byte[] buffer)
        {
            string fileExtension = "png";
            Guid guid = GuidComb.GenerateComb();
            return await Upload(guid.ToString() + "." + fileExtension, buffer);
        }
    }
}
