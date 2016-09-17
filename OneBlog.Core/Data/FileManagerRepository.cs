using OneBlog.Core.Data.Contracts;
using OneBlog.Core.Data.Models;
using OneBlog.Core.FileSystem;
using OneBlog.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Security;

namespace OneBlog.Core.Data
{
    public class FileManagerRepository : IFileManagerRepository
    {

        public IEnumerable<FileInstance> Find(int take = 10, int skip = 0, string path = "", string order = "")
        {
            if (!Security.IsAuthorizedTo(Rights.EditOwnPosts))
                throw new UnauthorizedAccessException();

            var list = new List<FileInstance>();
            var rwr = WebUtils.RelativeWebRoot;
            var responsePath = "root";

            if(string.IsNullOrEmpty(path))
                path = Blog.CurrentInstance.StorageLocation + WebUtils.FilesFolder;

            var directory = BlogService.GetDirectory(path);

            if (!directory.IsRoot)
            {
                list.Add(new FileInstance()
                {
                    FileSize = "",
                    FileType = FileType.Directory,
                    Created = DateTime.Now.ToString(),
                    FullPath = directory.Parent.FullPath,
                    Name = "..."
                });
                responsePath = "root" + directory.FullPath;
            }

            foreach (var dir in directory.Directories)
                list.Add(new FileInstance()
                {
                    FileSize = "",
                    FileType = FileType.Directory,
                    Created = dir.DateCreated.ToString(),
                    FullPath = dir.FullPath,
                    Name = dir.Name.Replace("/", "")
                });


            foreach (var file in directory.Files)
                list.Add(new FileInstance()
                {
                    FileSize = file.FileSizeFormat,
                    Created = file.DateCreated.ToString(),
                    FileType = file.IsImage ? FileType.Image : FileType.File,
                    FullPath = file.FilePath,
                    Name = file.Name
                });

            for (int i = 0; i < list.Count; i++)
            {
                list[i].SortOrder = i;
            }

            return list;
        }
    }
}
