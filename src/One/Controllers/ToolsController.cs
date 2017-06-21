using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using One.Models.ToolsViewModels;
using One.Helpers;
using System.Net.Http;
using System.Xml.Linq;
using One.Data;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace One.Controllers
{
    [Route("tools")]
    public class ToolsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ToolsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        //http://122.114.253.34:1024/ck/api/api.php?xml=http://v.qq.com/x/cover/jwplwx9ootoigud/x0023kp0ue1.html&type=auto&hd=gq&wap=0&siteuser=
        //http://vip.pucms.com/index.php?sort=list&id=dianying&cs=all
        [HttpGet("videofetch")]
        public IActionResult VideoFetch()
        {
            var vm = new VideoFetchViewModel();
            return View(vm);
        }

        [HttpPost("videofetch")]
        public async Task<IActionResult> VideoFetch(VideoFetchViewModel vm)
        {

            vm.Prompt = string.Empty;

            var user = await GetCurrentUserAsync();

            if (user == null)
            {
                vm.Prompt = "无法获取下载地址，请登录后使用该功能~";
                return View(vm);
            }

            if (string.IsNullOrEmpty(vm.Url))
            {
                vm.Prompt = "请输入播放地址Url~";
                return View(vm);
            }

            var url = "http://122.114.253.34:1024/ck/api/api.php?xml=" + vm.Url + "&type=auto&hd=cq&wap=0&siteuser=";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            vm.VideoUrls.Clear();
            vm.Url = url;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    var result = await response.Content.ReadAsStringAsync();
                    XDocument document = XDocument.Parse(result);
                    var videos = from item in document.Descendants("video")  //找到所有Student元素
                                 select new VideoUrl()
                                 {
                                     File = item.Element("file").Value,
                                     Size = item.Element("size").Value,
                                     Seconds = item.Element("seconds").Value
                                 };

                    if (videos != null && videos.Count() > 0)
                    {
                        vm.VideoUrls.AddRange(videos);
                    }
                }
                catch (Exception ex)
                {
                    vm.Prompt = "格式解析错误,请联系管理员~";
                    if (user.UserName == "chenrensong@outlook.com")
                    {
                        vm.Prompt += "\n" + ex.Message;
                    }
                }
            }
            else
            {
                vm.Prompt = "请求错误,请联系管理员~";
            }
            return View(vm);
        }


        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
