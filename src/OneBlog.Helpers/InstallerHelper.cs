using System;

namespace OneBlog.Helpers
{
    public static class InstallerHelper
    {

        public static void TouchWebConfig()
        {
            //var webConfigPath = FileUtils.MapPath("~/web.config");
            //var xDoc = new XmlDocument();
            //xDoc.Load(webConfigPath);
            //xDoc.Save(webConfigPath);
        }

        public static string GetMainDatabaseFilePath(string appVersion)
        {
            return String.Format("~/Installer/Db/{0}/database.sql", appVersion);
        }

        public static string GetUpdateDatabaseFilePath(string appVersion)
        {
            return String.Format("~/Installer/Db/{0}/Upgrade/upgrade.sql", appVersion);
        }
    }

}