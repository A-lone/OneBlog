using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBlog.UEditor
{

    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public class ConfigHandler : IHandler
    {
        private static bool noCache = true;
        private static string _path;

        public ConfigHandler(string path)
        {
            _path = path;
        }

        public object Process()
        {
            return ConfigHandler.Items;
        }

        private static JObject BuildItems()
        {
            var json = File.ReadAllText(_path);
            return JObject.Parse(json);
        }

        private static JObject Items
        {
            get
            {
                if (noCache || _Items == null)
                {
                    _Items = BuildItems();
                }
                return _Items;
            }
        }
        private static JObject _Items;


        public static T GetValue<T>(string key)
        {
            return Items[key].Value<T>();
        }

        public static String[] GetStringList(string key)
        {
            return Items[key].Select(x => x.Value<String>()).ToArray();
        }

        public static String GetString(string key)
        {
            return GetValue<String>(key);
        }

        public static int GetInt(string key)
        {
            return GetValue<int>(key);
        }


    }
}
