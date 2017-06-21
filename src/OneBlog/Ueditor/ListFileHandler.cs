using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace One.UEditor
{
    /// <summary>
    /// FileManager 的摘要说明
    /// </summary>
    public class ListFileHandler : IHandler
    {
        enum ResultState
        {
            Success,
            InvalidParam,
            AuthorizError,
            IOError,
            PathNotFound
        }

        protected string Username => System.Web.HttpContext.Current.User.Identity.IsAuthenticated ? System.Web.HttpContext.Current.User.Identity.Name : null;

        private int Start;
        private int Size;
        private int Total;
        private ResultState State;
        private String[] FileList;
        private String[] SearchExtensions;

        public ListFileHandler(string[] searchExtensions)
        {
            this.SearchExtensions = searchExtensions.Select(x => x.ToLower()).ToArray();
        }

        public object Process()
        {
            var memberService = ServiceFactory.Get<IMembershipService>();
            var localizationService = ServiceFactory.Get<ILocalizationService>();
            var LoggedOnReadOnlyUser = !string.IsNullOrEmpty(Username) ? memberService.GetUser(Username, true) : null;
            if (LoggedOnReadOnlyUser == null)
            {
                State = ResultState.AuthorizError;
                return WriteResult();
            }

            try
            {
                Start = String.IsNullOrEmpty(HttpContext.Current.Request["start"]) ? 0 : Convert.ToInt32(HttpContext.Current.Request["start"]);
                Size = String.IsNullOrEmpty(HttpContext.Current.Request["size"]) ?
                    ConfigHandler.GetInt("imageManagerListSize") : Convert.ToInt32(HttpContext.Current.Request["size"]);
            }
            catch (FormatException)
            {
                State = ResultState.InvalidParam;
                return WriteResult();
            }

            var uploadFolderPath = HostingEnvironment.MapPath(string.Concat(SiteConstants.Instance.UploadFolderPath, LoggedOnReadOnlyUser.Id));

            var buildingList = new List<String>();
            try
            {
                var hostingRoot = HostingEnvironment.MapPath("~/") ?? "";
                buildingList.AddRange(Directory.GetFiles(uploadFolderPath, "*", SearchOption.AllDirectories)
                    .Where(x => SearchExtensions.Contains(Path.GetExtension(x).ToLower()))
                    .Select(x =>  x.Substring(hostingRoot.Length).Replace("\\", "/").Insert(0, "/")));
                Total = buildingList.Count;
                FileList = buildingList.OrderBy(x => x).Skip(Start).Take(Size).ToArray();
            }
            catch (UnauthorizedAccessException)
            {
                State = ResultState.AuthorizError;
            }
            catch (DirectoryNotFoundException)
            {
                State = ResultState.PathNotFound;
            }
            catch (IOException)
            {
                State = ResultState.IOError;
            }

            return WriteResult();
        }

        private object WriteResult()
        {
            return (new
            {
                state = GetStateString(),
                list = FileList == null ? null : FileList.Select(x => new { url = x }),
                start = Start,
                size = Size,
                total = Total
            });
        }

        private string GetStateString()
        {
            switch (State)
            {
                case ResultState.Success:
                    return "SUCCESS";
                case ResultState.InvalidParam:
                    return "参数不正确";
                case ResultState.PathNotFound:
                    return "路径不存在";
                case ResultState.AuthorizError:
                    return "文件系统权限不足";
                case ResultState.IOError:
                    return "文件系统读取错误";
            }
            return "未知错误";
        }
    }
}