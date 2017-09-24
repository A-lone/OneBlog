using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OneBlog.Configuration;
using OneBlog.UEditor;

namespace OneBlog.Controllers
{
    [Authorize]
    [Route("api/ueditor")]
    public class UEditorPlusginsController : Controller
    {

        private IOptions<EditorSettings> _editorSettings;


        public UEditorPlusginsController(IOptions<EditorSettings> editorSettings)
        {
            _editorSettings = editorSettings;
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
                    handler = new ConfigHandler(_editorSettings);
                    break;
                case "uploadimage":
                    handler = new UploadHandler(new UploadConfig()
                    {
                        AllowExtensions = _editorSettings.Value.imageAllowFiles,
                        PathFormat = _editorSettings.Value.imagePathFormat,
                        SizeLimit = _editorSettings.Value.imageMaxSize,
                        UploadFieldName = _editorSettings.Value.imageFieldName
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
