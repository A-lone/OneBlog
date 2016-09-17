using OneBlog.Core.Data.Models;
using OneBlog.Core.FileSystem;
using System;
using System.Collections.Generic;

namespace OneBlog.Core.Data.Contracts
{
    public interface IFileManagerRepository
    {
        IEnumerable<FileInstance> Find(int take = 10, int skip = 0, string path = "", string order = "");
    }
}
