using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using One.UEditor;
using System.IO;

namespace One.Controllers
{
    [Authorize]
    [Route("api/ueditor")]
    public class UEditorPlusginsController : Controller
    {

        protected string _path;

        public UEditorPlusginsController(IHostingEnvironment env)
        {
            var path = "config.json";
            _path = Path.Combine(env.ContentRootPath, $@"Data{Path.DirectorySeparatorChar}{path}");
        }

        [Route("handler")]
        [HttpGet]
        [HttpPost]
        public object Handler(string action)
        {
            string type = Request.Query["action"];
            IHandler handler = null;
            switch (type)
            {
                case "config":
                    handler = new ConfigHandler(_path);
                    break;
                case "uploadimage":
                    handler = new UploadHandler(new UploadConfig()
                    {
                        AllowExtensions = ConfigHandler.GetStringList("imageAllowFiles"),
                        PathFormat = ConfigHandler.GetString("imagePathFormat"),
                        SizeLimit = ConfigHandler.GetInt("imageMaxSize"),
                        UploadFieldName = ConfigHandler.GetString("imageFieldName")
                    });
                    break;
                //case "uploadscrawl":
                //    handler = new UploadHandler(new UploadConfig()
                //    {
                //        AllowExtensions = new string[] { ".png" },
                //        PathFormat = ConfigHandler.GetString("scrawlPathFormat"),
                //        SizeLimit = ConfigHandler.GetInt("scrawlMaxSize"),
                //        UploadFieldName = ConfigHandler.GetString("scrawlFieldName"),
                //        Base64 = true,
                //        Base64Filename = "scrawl.png"
                //    });
                //    break;
                //case "uploadvideo":
                //    handler = new UploadHandler(new UploadConfig()
                //    {
                //        AllowExtensions = ConfigHandler.GetStringList("videoAllowFiles"),
                //        PathFormat = ConfigHandler.GetString("videoPathFormat"),
                //        SizeLimit = ConfigHandler.GetInt("videoMaxSize"),
                //        UploadFieldName = ConfigHandler.GetString("videoFieldName")
                //    });
                //    break;
                //case "uploadfile":
                //    handler = new UploadHandler(new UploadConfig()
                //    {
                //        AllowExtensions = ConfigHandler.GetStringList("fileAllowFiles"),
                //        PathFormat = ConfigHandler.GetString("filePathFormat"),
                //        SizeLimit = ConfigHandler.GetInt("fileMaxSize"),
                //        UploadFieldName = ConfigHandler.GetString("fileFieldName")
                //    });
                //    break;
                //case "listimage":
                //    handler = new ListFileHandler(ConfigHandler.GetStringList("imageManagerAllowFiles"));
                //    break;
                //case "listfile":
                //    handler = new ListFileHandler(ConfigHandler.GetStringList("fileManagerAllowFiles"));
                //    break;
                case "catchimage":
                    handler = new CrawlerHandler();
                    break;
                default:
                    handler = new NotSupportedHandler();
                    break;
            }
            return handler.Process();
        }



        //GET api/TinyMce/UploadImage
        [Route("UploadImage")]
        [HttpPost]
        public string UploadImage()
        {
            return null;
        }
        //var memberService = ServiceFactory.Get<IMembershipService>();
        //var roleService = ServiceFactory.Get<IRoleService>();
        //var localizationService = ServiceFactory.Get<ILocalizationService>();
        //var uploadService = ServiceFactory.Get<IUploadedFileService>();
        //var unitOfWorkManager = ServiceFactory.Get<IUnitOfWorkManager>();
        //var loggingService = ServiceFactory.Get<ILoggingService>();

        //using (var unitOfWork = unitOfWorkManager.NewUnitOfWork())
        //{
        //    try
        //    {

        //        if (HttpContext.Current.Request.Files.AllKeys.Any())
        //        {
        //            // Get the uploaded image from the Files collection
        //            var httpPostedFile = HttpContext.Current.Request.Files["file"];
        //            if (httpPostedFile != null)
        //            {
        //                HttpPostedFileBase photo = new HttpPostedFileWrapper(httpPostedFile);
        //                var loggedOnReadOnlyUser = memberService.GetUser(HttpContext.Current.User.Identity.Name);
        //                var permissions = roleService.GetPermissions(null, loggedOnReadOnlyUser.Roles.FirstOrDefault());
        //                // Get the permissions for this category, and check they are allowed to update
        //                if (permissions[SiteConstants.Instance.PermissionInsertEditorImages].IsTicked && loggedOnReadOnlyUser.DisableFileUploads != true)
        //                {
        //                    // woot! User has permission and all seems ok
        //                    // Before we save anything, check the user already has an upload folder and if not create one
        //                    var uploadFolderPath = HostingEnvironment.MapPath(string.Concat(SiteConstants.Instance.UploadFolderPath, loggedOnReadOnlyUser.Id));
        //                    if (!Directory.Exists(uploadFolderPath))
        //                    {
        //                        Directory.CreateDirectory(uploadFolderPath);
        //                    }

        //                    // If successful then upload the file
        //                    var uploadResult = AppHelpers.UploadFile(photo, uploadFolderPath, localizationService, true);
        //                    if (!uploadResult.UploadSuccessful)
        //                    {
        //                        return string.Empty;
        //                    }

        //                    // Add the filename to the database
        //                    var uploadedFile = new UploadedFile
        //                    {
        //                        Filename = uploadResult.UploadedFileName,
        //                        MembershipUser = loggedOnReadOnlyUser
        //                    };
        //                    uploadService.Add(uploadedFile);

        //                    // Commit the changes
        //                    unitOfWork.Commit();

        //                    return uploadResult.UploadedFileUrl;
        //                }
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        unitOfWork.Rollback();
        //        loggingService.Error(ex);
        //    }


    }


}
